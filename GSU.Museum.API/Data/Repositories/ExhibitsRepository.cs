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

        public async Task<string> CreateAsync(string hallId, string standId, Exhibit exhibit)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            var stand = hall?.Stands.FirstOrDefault(stand => stand.Id.Equals(standId));
            
            if (stand != null)
            {
                if (exhibit.Photos != null)
                {
                    foreach (var photo in exhibit.Photos)
                    {
                        ObjectId id = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), photo?.Photo);
                        photo.Id = id.ToString();
                        photo.Photo = null;
                    }
                }
                else
                {
                    exhibit.Photos = new List<PhotoInfo>();
                }

                var filter = Builders<Hall>.Filter.And(
                Builders<Hall>.Filter.Where(hall => hall.Id.Equals(hallId)),
                Builders<Hall>.Filter.Eq("Stands.Id", standId));
                exhibit.Id = ObjectId.GenerateNewId().ToString();
                var update = Builders<Hall>.Update.Push("Stands.$.Exhibits", exhibit);
                await _halls.FindOneAndUpdateAsync(filter, update);
                return exhibit.Id;
            }
            return null;
        }

        public async Task<List<Exhibit>> GetAllAsync(string hallId, string standId)
        {
            var hall = await _halls.Find(h => h.Id.Equals(hallId)).FirstOrDefaultAsync();
            var exhibits = hall?.Stands?.FirstOrDefault(s => s.Id.Equals(standId))?.Exhibits?.ToList();
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

        public async Task UpdateAsync(string hallId, string standId, string id, Exhibit exhibit)
        {
            for (int i = 0; i < exhibit.Photos?.Count; i++)
            {
                if (!string.IsNullOrEmpty(exhibit.Photos[i]?.Id))
                {
                    await _gridFS.DeleteAsync(ObjectId.Parse(exhibit.Photos[i].Id));
                }
                if (exhibit.Photos[i]?.Photo != null)
                {
                    ObjectId photoId = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), exhibit.Photos[i].Photo);
                    exhibit.Photos[i].Id = photoId.ToString();
                    exhibit.Photos[i].Photo = null;
                }
                else
                {
                    exhibit.Photos.RemoveAt(i--);
                }
            }

            int index1 = 0, index2 = 0;
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            for (int i = 0; i < hall.Stands.Count; i++)
            {
                if (hall?.Stands[i]?.Id == standId)
                {
                    index1 = i;
                    break;
                }
            }

            for (int i = 0; i < hall.Stands[index1].Exhibits.Count; i++)
            {
                if (hall.Stands[index1]?.Exhibits[i]?.Id == id)
                {
                    index2 = i;
                    break;
                }
            }

            var arrayFilter = Builders<Hall>.Filter.Where(hall => hall.Id.Equals(hallId));

            var update = Builders<Hall>.Update.Set($"Stands.{index1}.Exhibits.{index2}", exhibit);

            await _halls.FindOneAndUpdateAsync(arrayFilter, update);
        }
    }
}
