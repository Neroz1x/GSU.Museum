using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Data.Repositories
{
    public class HallsRepository : IHallsRepository
    {
        private readonly IMongoCollection<Hall> _halls;
        private readonly IGridFSBucket _gridFS;

        public HallsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<Hall>(settings.CollectionName);
            _gridFS = new GridFSBucket(database);
        }

        public async Task<List<Hall>> GetAllAsync()
        {
            var halls = await _halls.Find(hall => true).ToListAsync();
            foreach(var hall in halls)
            {
                if (!string.IsNullOrEmpty(hall.Photo.Id))
                {
                    hall.Photo.Photo = await _gridFS.DownloadAsBytesAsync(ObjectId.Parse(hall.Photo.Id));
                }
            }
            return halls;
        }

        public async Task<Hall> GetAsync(string id)
        {
            var hall = await _halls.Find(hall => hall.Id.Equals(id)).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(hall.Photo.Id))
            {
                hall.Photo.Photo = await _gridFS.DownloadAsBytesAsync(ObjectId.Parse(hall.Photo.Id));
            }
            return hall;
        }

        public async Task CreateAsync(Hall hall)
        {
            if(hall.Photo.Photo != null)
            {
                ObjectId id = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), hall.Photo.Photo);
                hall.Photo.Id = id.ToString();
            }
            hall.Photo.Photo = null;
            await _halls.InsertOneAsync(hall);
        }

        public async Task UpdateAsync(string id, Hall hallIn)
        {
            hallIn.Id = id;
            if (hallIn.Photo.Id != null)
            {
                await _gridFS.DeleteAsync(ObjectId.Parse(hallIn.Photo.Id));
            }
            if (hallIn.Photo.Photo != null)
            {
                ObjectId photoId = await _gridFS.UploadFromBytesAsync(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), hallIn.Photo.Photo);
                hallIn.Photo.Id = photoId.ToString();
            }
            hallIn.Photo.Photo = null;
            await _halls.ReplaceOneAsync(hall => hall.Id.Equals(id), hallIn);
        }

        public async Task RemoveAsync(Hall hallIn)
        {
            if(hallIn.Photo.Id != null)
            {
                await _gridFS.DeleteAsync(ObjectId.Parse(hallIn.Photo.Id));
            }
            await _halls.DeleteOneAsync(hall => hall.Id.Equals(hallIn.Id));
        }

        public async Task RemoveAsync(string id)
        {
            var hall = await GetAsync(id);
            await RemoveAsync(hall);
        }
    }
}
