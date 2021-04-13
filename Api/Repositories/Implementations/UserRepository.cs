using System.Threading.Tasks;
using Api.Models;
using Api.Repositories.Interfaces;

namespace Api.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        public Task Create(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task Deactivate(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindById(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}