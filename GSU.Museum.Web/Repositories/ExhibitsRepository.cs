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
    public class ExhibitsRepository : IExhibitsRepository
    {
        private readonly IMongoCollection<HallViewModel> _halls;
        private readonly IGridFSBucket _gridFS;

        public ExhibitsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<HallViewModel>(settings.CollectionName);
            _gridFS = new GridFSBucket(database);
        }

        public async Task CreateAsync(string hallId, string standId, ExhibitViewModel entity)
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

        public async Task<ExhibitViewModel> GetAsync(string hallId, string standId, string id)
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

            var filter = Builders<HallViewModel>.Filter.Eq(hall => hall.Id, hallId);
            var update = Builders<HallViewModel>.Update.PullFilter("Stands.$[].Exhibits",
                                        Builders<ExhibitViewModel>.Filter.Eq(x => x.Id, id));

            var result = await _halls
                .FindOneAndUpdateAsync(filter, update);
        }

        public async Task UpdateAsync(string hallId, string standId, string id, ExhibitViewModel entity)
        {
            for (int i = 0; i <  entity.Photos?.Count; i++)
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
                    entity.Photos.RemoveAt(i--);
                }
            }

            int index1 = 0, index2 = 0;
            var hall = await _halls.Find(hall => hall.Id.Equals(hallId)).FirstOrDefaultAsync();
            for (int i = 0; i < hall.Stands.Count; i++)
            {
                if (hall.Stands[i].Id == standId)
                {
                    index1 = i;
                    break;
                }
            }

            for (int i = 0; i < hall.Stands[index1].Exhibits.Count; i++)
            {
                if (hall.Stands[index1].Exhibits[i].Id == id)
                {
                    index2 = i;
                    break;
                }
            }

            var arrayFilter = Builders<HallViewModel>.Filter.Where(hall => hall.Id.Equals(hallId));

            var update = Builders<HallViewModel>.Update.Set($"Stands.{index1}.Exhibits.{index2}", entity);

            await _halls.FindOneAndUpdateAsync(arrayFilter, update);
        }
    }
}
