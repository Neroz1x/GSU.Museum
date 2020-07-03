using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.API.Data.Repositories
{
    public class StandsRepository : IStandsRepository
    {
        private readonly IMongoCollection<Hall> _halls;

        public StandsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<Hall>(settings.CollectionName);
        }

        public async Task CreateAsync(string hallId, Stand entity)
        {
            var filter = Builders<Hall>.Filter.Eq("Id", hallId);
            var update = Builders<Hall>.Update.Push("Stands", entity);

            await _halls.UpdateOneAsync(filter, update);
        }

        public async Task<List<Stand>> GetAllAsync(string hallId)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            return hall?.Stands.ToList();
        }

        public async Task<Stand> GetAsync(string hallId, string id)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            return hall?.Stands.FirstOrDefault(stand => stand.Id.Equals(id));
        }

        public async Task RemoveAsync(string hallId, string id)
        {
            var update = Builders<Hall>.Update.PullFilter(hall => hall.Stands,
                                                stand => stand.Id.Equals(id));
            var result = await _halls
                .FindOneAndUpdateAsync(hall => hall.Id.Equals(hallId), update);
        }

        public async Task UpdateAsync(string hallId, string id, Stand entity)
        {
            var arrayFilter = Builders<Hall>.Filter.And(//"Id", hallId)
                Builders<Hall>.Filter.Where(hall => hall.Id.Equals(hallId)),
                Builders<Hall>.Filter.Eq("Stands.Id", id));
            var update = Builders<Hall>.Update.Set("Stands.$", entity);

            await _halls.UpdateOneAsync(arrayFilter, update);
        }
    }
}
