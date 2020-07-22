using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Repositories
{
    public class StandsRepository : IStandsRepository
    {
        private readonly IMongoCollection<HallViewModel> _halls;
        private readonly IGridFSBucket _gridFS;

        public StandsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<HallViewModel>(settings.CollectionName);
            _gridFS = new GridFSBucket(database);
        }

        public async Task CreateAsync(string hallId, StandViewModel stand)
        {
            if (stand.Photo?.Photo != null)
            {
                ObjectId id = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), stand.Photo.Photo);
                stand.Photo.Id = id.ToString();
                stand.Photo.Photo = null;
            }

            stand.Id = ObjectId.GenerateNewId().ToString();
            var filter = Builders<HallViewModel>.Filter.Eq("Id", hallId);
            var update = Builders<HallViewModel>.Update.Push("Stands", stand);
            await _halls.UpdateOneAsync(filter, update);
        }

        public async Task<List<StandViewModel>> GetAllAsync(string hallId)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            if (hall != null)
            {
                foreach (var stand in hall.Stands)
                {
                    if (!string.IsNullOrEmpty(stand.Photo?.Id))
                    {
                        stand.Photo.Photo = await _gridFS.DownloadAsBytesAsync(ObjectId.Parse(stand.Photo.Id));
                    }
                }
            }
            return hall?.Stands.ToList();
        }

        public async Task<StandViewModel> GetAsync(string hallId, string id)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            var stand = hall?.Stands.FirstOrDefault(stand => stand.Id.Equals(id));
            if (stand != null)
            {
                if (!string.IsNullOrEmpty(stand.Photo?.Id))
                {
                    stand.Photo.Photo = await _gridFS.DownloadAsBytesAsync(ObjectId.Parse(stand.Photo.Id));
                }
            }
            return stand;
        }

        public async Task RemoveAsync(string hallId, string id)
        {
            var stand = await GetAsync(hallId, id);
            if (stand.Photo?.Id != null)
            {
                await _gridFS.DeleteAsync(ObjectId.Parse(stand.Photo.Id));
            }
            var update = Builders<HallViewModel>.Update.PullFilter(hall => hall.Stands,
                                                stand => stand.Id.Equals(id));
            var result = await _halls
                .FindOneAndUpdateAsync(hall => hall.Id.Equals(hallId), update);
        }

        public async Task UpdateAsync(string hallId, string id, StandViewModel stand)
        {
            if (!string.IsNullOrEmpty(stand.Photo?.Id))
            {
                await _gridFS.DeleteAsync(ObjectId.Parse(stand.Photo.Id));
            }
            if (stand.Photo?.Photo != null)
            {
                ObjectId photoId = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), stand.Photo.Photo);
                stand.Photo.Id = photoId.ToString();
                stand.Photo.Photo = null;
            }
            else
            {
                stand.Photo = null;
            }
            
            var arrayFilter = Builders<HallViewModel>.Filter.And(
                Builders<HallViewModel>.Filter.Where(hall => hall.Id.Equals(hallId)),
                Builders<HallViewModel>.Filter.Eq("Stands.Id", id));
            var update = Builders<HallViewModel>.Update.Set("Stands.$", stand);

            await _halls.UpdateOneAsync(arrayFilter, update);
        }
    }
}
