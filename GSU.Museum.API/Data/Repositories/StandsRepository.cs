using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.API.Data.Repositories
{
    public class StandsRepository : IStandsRepository
    {
        private readonly IMongoCollection<Hall> _halls;
        private readonly IGridFSBucket _gridFS;

        public StandsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<Hall>(settings.CollectionName);
            _gridFS = new GridFSBucket(database);
        }

        public async Task<string> CreateAsync(string hallId, Stand stand)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();

            if(hall != null)
            {
                if (stand.Photo?.Photo != null)
                {
                    ObjectId id = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), stand.Photo.Photo);
                    stand.Photo.Id = id.ToString();
                    stand.Photo.Photo = null;
                }

                stand.Id = ObjectId.GenerateNewId().ToString();
                var filter = Builders<Hall>.Filter.Eq("Id", hallId);
                var update = Builders<Hall>.Update.Push("Stands", stand);
                await _halls.UpdateOneAsync(filter, update);
                return stand.Id;
            }
            return null;
        }

        public async Task<List<Stand>> GetAllAsync(string hallId)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            if (hall != null)
            {
                foreach (var stand in hall.Stands)
                {
                    if (!string.IsNullOrEmpty(stand?.Photo?.Id))
                    {
                        stand.Photo.Photo = await _gridFS.DownloadAsBytesAsync(ObjectId.Parse(stand.Photo.Id));
                    }
                }
            }
            return hall?.Stands.ToList();
        }

        public async Task<Stand> GetAsync(string hallId, string id)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            var stand = hall?.Stands.FirstOrDefault(stand => stand.Id.Equals(id));
            if (stand != null)
            {
                if (!string.IsNullOrEmpty(stand?.Photo?.Id))
                {
                    stand.Photo.Photo = await _gridFS.DownloadAsBytesAsync(ObjectId.Parse(stand.Photo.Id));
                }
            }
            return stand;
        }

        public async Task RemoveAsync(string hallId, string id)
        {
            var stand = await GetAsync(hallId, id);
            if (stand?.Photo?.Id != null)
            {
                await _gridFS.DeleteAsync(ObjectId.Parse(stand.Photo.Id));
            }
            var update = Builders<Hall>.Update.PullFilter(hall => hall.Stands,
                                                stand => stand.Id.Equals(id));
            var result = await _halls
                .FindOneAndUpdateAsync(hall => hall.Id.Equals(hallId), update);
        }

        public async Task UpdateAsync(string hallId, string id, Stand stand)
        {
            if (!string.IsNullOrEmpty(stand.Photo?.Id))
            {
                await _gridFS.DeleteAsync(ObjectId.Parse(stand.Photo.Id));
            }
            if (stand?.Photo?.Photo != null)
            {
                ObjectId photoId = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), stand.Photo.Photo);
                stand.Photo.Id = photoId.ToString();
                stand.Photo.Photo = null;
            }
            else
            {
                stand.Photo = null;
            }

            var arrayFilter = Builders<Hall>.Filter.And(
                Builders<Hall>.Filter.Where(hall => hall.Id.Equals(hallId)),
                Builders<Hall>.Filter.Eq("Stands.Id", id));
            var update = Builders<Hall>.Update.Set("Stands.$", stand);

            await _halls.UpdateOneAsync(arrayFilter, update);
        }
    }
}
