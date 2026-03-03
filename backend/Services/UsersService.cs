using Backend.Models;
using MongoDB.Driver;

namespace Backend.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMongoCollection<User> _users;

        public UsersService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateUserPasswordAsync(string id, string passwordHash)
        {
            var update = Builders<User>.Update.Set(u => u.PasswordHash, passwordHash);
            await _users.UpdateOneAsync(u => u.Id == id, update);
        }
    }
}
