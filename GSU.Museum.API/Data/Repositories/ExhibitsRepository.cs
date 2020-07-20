using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.API.Data.Repositories
{
    public class ExhibitsRepository : IExhibitsRepository
    {
        private readonly IMongoCollection<Hall> _halls;
        private readonly IGridFSBucket _gridFS;

        public ExhibitsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<Hall>(settings.CollectionName);
            _gridFS = new GridFSBucket(database);
        }

        public async Task CreateAsync(string hallId, string standId, Exhibit entity)
        {
            if (entity.Photos != null)
            {
                foreach (var photo in entity.Photos)
                {
                    ObjectId id = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), photo?.Photo);
                    photo.Id = id.ToString();
                    photo.Photo = null;
                }
            }
            else
            {
                entity.Photos = new List<PhotoInfo>();
            }

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
            var exhibits = hall.Stands.FirstOrDefault(s => s.Id.Equals(standId))?.Exhibits?.ToList();
            if (exhibits != null)
            {
                foreach (var exhibit in exhibits)
                {
                    if (exhibit?.Photos != null)
                    {
                        foreach (var photo in exhibit.Photos)
                        {
                            if (!string.IsNullOrEmpty(photo?.Id))
                            {
                                photo.Photo = await _gridFS.DownloadAsBytesAsync(ObjectId.Parse(photo.Id));
                            }
                        }
                    }
                }
            }
            return exhibits;
        }

        public async Task<Exhibit> GetAsync(string hallId, string standId, string id)
        {
            var hall = await _halls.Find(h => h.Id.Equals(hallId)).FirstOrDefaultAsync();
            var exhibit = hall?.Stands?.FirstOrDefault(s => s.Id.Equals(standId))?.Exhibits?.FirstOrDefault(e => e.Id.Equals(id));
            if (exhibit?.Photos != null)
            {
                foreach (var photo in exhibit.Photos)
                {
                    if (!string.IsNullOrEmpty(photo?.Id))
                    {
                        photo.Photo = await _gridFS.DownloadAsBytesAsync(ObjectId.Parse(photo.Id));
                    }
                }
            }
            return exhibit;
        }

        public async Task RemoveAsync(string hallId, string standId, string id)
        {
            var exhibit = await GetAsync(hallId, standId, id);
            if (exhibit?.Photos != null)
            {
                foreach (var photo in exhibit.Photos)
                {
                    if (!string.IsNullOrEmpty(photo?.Id))
                    {
                        await _gridFS.DeleteAsync(ObjectId.Parse(photo.Id));
                    }
                }
            }

            var filter = Builders<Hall>.Filter.Eq(hall => hall.Id, hallId);
            var update = Builders<Hall>.Update.PullFilter("Stands.$[].Exhibits",
                                        Builders<Exhibit>.Filter.Eq(x => x.Id, id));

            var result = await _halls
                .FindOneAndUpdateAsync(filter, update);
        }

        public async Task UpdateAsync(string hallId, string standId, string id, Exhibit entity)
        {
            for (int i = 0; i < entity.Photos?.Count; i++)
            {
                if (!string.IsNullOrEmpty(entity.Photos[i]?.Id))
                {
                    await _gridFS.DeleteAsync(ObjectId.Parse(entity.Photos[i].Id));
                }
                if (entity.Photos[i]?.Photo != null)
                {
                    ObjectId photoId = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), entity.Photos[i].Photo);
                    entity.Photos[i].Id = photoId.ToString();
                    entity.Photos[i].Photo = null;
                }
                else
                {
                    entity.Photos.RemoveAt(i);
                }
            }

            var arrayFilter = Builders<Hall>.Filter.And(
                Builders<Hall>.Filter.Where(hall => hall.Id.Equals(hallId)),
                Builders<Hall>.Filter.Eq("Stands.Id", standId),
                Builders<Hall>.Filter.Eq("Stands.Exhibits.Id", id));

            var update = Builders<Hall>.Update.Set("Stands.$[].Exhibits.$", entity);

            await _halls.UpdateOneAsync(arrayFilter, update);
        }
    }
}
