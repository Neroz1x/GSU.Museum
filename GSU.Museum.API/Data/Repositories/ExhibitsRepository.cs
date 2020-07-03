using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
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
            var update = Builders<Hall>.Update.Push("Stands.$.Exhibits", entity);
            await _halls.FindOneAndUpdateAsync(filter, update);
        }

        public Task<List<Exhibit>> GetAllAsync(string hallId, string standId)
        {
            throw new NotImplementedException();
        }

        public Task<Exhibit> GetAsync(string hallId, string standId, string id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string hallId, string standId, string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string hallId, string standId, string id, Exhibit entity)
        {
            throw new NotImplementedException();
        }
    }
}
