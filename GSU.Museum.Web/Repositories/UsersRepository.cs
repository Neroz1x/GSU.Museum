using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IMongoCollection<User> _users;

        public UsersRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public async Task<string> CreateAsync(User user)
        {
            user.Id = ObjectId.GenerateNewId().ToString();
            await _users.InsertOneAsync(user);
            return user.Id;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _users.Find(user => true).ToListAsync();
        }

        public async Task<User> GetAsync(string login)
        {
            return await _users.Find(user => user.Login.Equals(login)).FirstOrDefaultAsync();
        }
        
        public async Task<User> GetByIdAsync(string id)
        {
            return await _users.Find(user => user.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await _users.DeleteOneAsync(user => user.Id.Equals(id));
        }

        public async Task UpdateAsync(string id, User user)
        {
            user.Id = id;
            await _users.ReplaceOneAsync(user => user.Id.Equals(id), user);
        }
    }
}
