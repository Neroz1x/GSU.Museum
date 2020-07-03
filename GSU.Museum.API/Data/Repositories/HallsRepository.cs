using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Data.Repositories
{
    public class HallsRepository : IHallsRepository
    {
        private readonly IMongoCollection<Hall> _halls;

        public HallsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _halls = database.GetCollection<Hall>(settings.CollectionName);
        }

        public async Task<List<Hall>> GetAllAsync()
        {
            return await _halls.Find(exhibit => true).ToListAsync();
        }

        public async Task<Hall> GetAsync(string id)
        {
            return await _halls.Find(exhibit => exhibit.Id.Equals(id)).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Add record
        /// </summary>
        /// <param name="exhibit">Hall to add</param>
        /// <returns></returns>
        public async Task CreateAsync(Hall exhibit)
        {
            await _halls.InsertOneAsync(exhibit);
        }

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="id">Id of the record to update</param>
        /// <param name="exhibitIn">New record</param>
        /// <returns></returns>
        public async Task UpdateAsync(string id, Hall exhibitIn)
        {
            exhibitIn.Id = id;
            await _halls.ReplaceOneAsync(exhibit => exhibit.Id.Equals(id), exhibitIn);
        }

        /// <summary>
        /// Remove record
        /// </summary>
        /// <param name="exhibitIn">Recod to remove</param>
        /// <returns></returns>
        public async Task RemoveAsync(Hall exhibitIn)
        {
            await _halls.DeleteOneAsync(exhibit => exhibit.Id.Equals(exhibitIn.Id));
        }

        /// <summary>
        /// Remove record
        /// </summary>
        /// <param name="id">Id of the recrod to remove</param>
        /// <returns></returns>
        public async Task RemoveAsync(string id)
        {
            await _halls.DeleteOneAsync(exhibit => exhibit.Id.Equals(id));
        }
    }
}
