using CreditServiceTest.Data.Repository.Interface;
using CreditServiceTest.Data.Services.Interface;
using CreditServiceTest.Models;
using System.Linq.Expressions;

namespace CreditServiceTest.Data.Services
{
    public class UserService : IUser
    {
        private readonly IMongoRepository<User> _userRepository;

        public UserService(IMongoRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var userQuery = _userRepository.GetQueryable();
            return await _userRepository.FindAllAsync(userQuery);
        }
        public async Task<bool> AddUserAsync(User user)
        {
            var result = await _userRepository.InsertOneAsync(user);
            return result;
        }
        public async Task<bool> DeleteUserAsync(User user)
        {
            Expression<Func<User, bool>> filterExpression = x => x.Id == user.Id;
            return await _userRepository.DeleteOneAsync(filterExpression);
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            Expression<Func<User, bool>> filterExpression = x => x.Id == user.Id;
            return await _userRepository.ReplaceOneAsync(filterExpression, user);
        }
    }
}
