using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Repositories
{
    public class StandsRepository : IStandsRepository
    {
        private readonly IMongoCollection<HallViewModel> _halls;

        public StandsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<HallViewModel>(settings.CollectionName);
        }

        public async Task CreateAsync(string hallId, StandViewModel entity)
        {
            entity.Id = ObjectId.GenerateNewId().ToString();
            var filter = Builders<HallViewModel>.Filter.Eq("Id", hallId);
            var update = Builders<HallViewModel>.Update.Push("Stands", entity);

            await _halls.UpdateOneAsync(filter, update);
        }

        public async Task<List<StandViewModel>> GetAllAsync(string hallId)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            return hall?.Stands.ToList();
        }

        public async Task<StandViewModel> GetAsync(string hallId, string id)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            return hall?.Stands.FirstOrDefault(stand => stand.Id.Equals(id));
        }

        public async Task RemoveAsync(string hallId, string id)
        {
            var update = Builders<HallViewModel>.Update.PullFilter(hall => hall.Stands,
                                                stand => stand.Id.Equals(id));
            var result = await _halls
                .FindOneAndUpdateAsync(hall => hall.Id.Equals(hallId), update);
        }

        public async Task UpdateAsync(string hallId, string id, StandViewModel entity)
        {
            var arrayFilter = Builders<HallViewModel>.Filter.And(
                Builders<HallViewModel>.Filter.Where(hall => hall.Id.Equals(hallId)),
                Builders<HallViewModel>.Filter.Eq("Stands.Id", id));
            var update = Builders<HallViewModel>.Update.Set("Stands.$", entity);

            await _halls.UpdateOneAsync(arrayFilter, update);
        }
    }
}
