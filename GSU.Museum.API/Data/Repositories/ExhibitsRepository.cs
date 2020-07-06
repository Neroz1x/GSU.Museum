using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.API.Data.Repositories
{
    public class ExhibitsRepository : IExhibitsRepository
    {
        private readonly IMongoCollection<Hall> _halls;

        public ExhibitsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<Hall>(settings.CollectionName);
        }

        public async Task CreateAsync(string hallId, string standId, Exhibit entity)
        {
            var filter = Builders<Hall>.Filter.And(
            Builders<Hall>.Filter.Where(hall => hall.Id.Equals(hallId)),
            Builders<Hall>.Filter.Eq("Stands.Id", standId));
            entity.Id = ObjectId.GenerateNewId().ToString();
            var update = Builders<Hall>.Update.Push("Stands.$.Exhibits", entity);
            await _halls.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<List<Exhibit>> GetAllAsync(string hallId, string standId)
        {
            var hall = await _halls.Find(h => h.Id.Equals(hallId)).FirstOrDefaultAsync();
            return hall.Stands.FirstOrDefault(s => s.Id.Equals(standId))?.Exhibits?.ToList();
        }

        public async Task<Exhibit> GetAsync(string hallId, string standId, string id)
        {
            var hall = await _halls.Find(h => h.Id.Equals(hallId)).FirstOrDefaultAsync();
            return hall?.Stands?.FirstOrDefault(s => s.Id.Equals(standId))?.Exhibits?.FirstOrDefault(e => e.Id.Equals(id));
        }

        public async Task RemoveAsync(string hallId, string standId, string id)
        {
            var filter = Builders<Hall>.Filter.Eq(hall => hall.Id, hallId);
            var update = Builders<Hall>.Update.PullFilter("Stands.$[].Exhibits",
                                        Builders<Exhibit>.Filter.Eq(x => x.Id, id));

            var result = await _halls
                .FindOneAndUpdateAsync(filter, update);
        }

        public async Task UpdateAsync(string hallId, string standId, string id, Exhibit entity)
        {
            var arrayFilter = Builders<Hall>.Filter.And(
                Builders<Hall>.Filter.Where(hall => hall.Id.Equals(hallId)),
                Builders<Hall>.Filter.Eq("Stands.Exhibits.Id", id));
            var update = Builders<Hall>.Update.Set("Stands.$.Exhibits", entity);

            await _halls.UpdateOneAsync(arrayFilter, update);
        }
    }
}
