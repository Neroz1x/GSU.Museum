using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Repositories
{
    public class ExhibitsRepository : IExhibitsRepository
    {
        private readonly IMongoCollection<HallViewModel> _halls;

        public ExhibitsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<HallViewModel>(settings.CollectionName);
        }

        public async Task CreateAsync(string hallId, string standId, ExhibitViewModel entity)
        {
            var filter = Builders<HallViewModel>.Filter.And(
            Builders<HallViewModel>.Filter.Where(hall => hall.Id.Equals(hallId)),
            Builders<HallViewModel>.Filter.Eq("Stands.Id", standId));
            entity.Id = ObjectId.GenerateNewId().ToString();
            var update = Builders<HallViewModel>.Update.Push("Stands.$.Exhibits", entity);
            await _halls.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<List<ExhibitViewModel>> GetAllAsync(string hallId, string standId)
        {
            var hall = await _halls.Find(h => h.Id.Equals(hallId)).FirstOrDefaultAsync();
            return hall.Stands.FirstOrDefault(s => s.Id.Equals(standId))?.Exhibits?.ToList();
        }

        public async Task<ExhibitViewModel> GetAsync(string hallId, string standId, string id)
        {
            var hall = await _halls.Find(h => h.Id.Equals(hallId)).FirstOrDefaultAsync();
            return hall?.Stands?.FirstOrDefault(s => s.Id.Equals(standId))?.Exhibits?.FirstOrDefault(e => e.Id.Equals(id));
        }

        public async Task RemoveAsync(string hallId, string standId, string id)
        {
            var filter = Builders<HallViewModel>.Filter.Eq(hall => hall.Id, hallId);
            var update = Builders<HallViewModel>.Update.PullFilter("Stands.$[].Exhibits",
                                        Builders<ExhibitViewModel>.Filter.Eq(x => x.Id, id));

            var result = await _halls
                .FindOneAndUpdateAsync(filter, update);
        }

        public async Task UpdateAsync(string hallId, string standId, string id, ExhibitViewModel entity)
        {
            var arrayFilter = Builders<HallViewModel>.Filter.And(
                Builders<HallViewModel>.Filter.Where(hall => hall.Id.Equals(hallId)),
                Builders<HallViewModel>.Filter.Eq("Stands.Id", standId),
                Builders<HallViewModel>.Filter.Eq("Stands.Exhibits.Id", id));

             var update = Builders<HallViewModel>.Update.Set("Stands.$[].Exhibits.$", entity);

            await _halls.FindOneAndUpdateAsync(arrayFilter, update);
        }
    }
}
