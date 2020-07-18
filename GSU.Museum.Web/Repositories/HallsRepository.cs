using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Repositories
{
    public class HallsRepository : IHallsRepository
    {
        private readonly IMongoCollection<HallViewModel> _halls;

        public HallsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<HallViewModel>(settings.CollectionName);
        }

        public async Task<List<HallViewModel>> GetAllAsync()
        {
            return await _halls.Find(exhibit => true).ToListAsync();
        }

        public async Task<HallViewModel> GetAsync(string id)
        {
            return await _halls.Find(exhibit => exhibit.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(HallViewModel exhibit)
        {
            await _halls.InsertOneAsync(exhibit);
        }

        public async Task UpdateAsync(string id, HallViewModel exhibitIn)
        {
            exhibitIn.Id = id;
            await _halls.ReplaceOneAsync(exhibit => exhibit.Id.Equals(id), exhibitIn);
        }

        public async Task RemoveAsync(string id)
        {
            await _halls.DeleteOneAsync(exhibit => exhibit.Id.Equals(id));
        }
    }
}
