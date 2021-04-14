using Api.Models;
using Api.Repositories.Interfaces;

namespace Api.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        public void Create(User user)
        {
            throw new System.NotImplementedException();
        }

        public void Deactivate(string id)
        {
            throw new System.NotImplementedException();
        }

        public User FindByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public User FindById(string id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}