using Api.Models;

namespace Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void Create(User user);
        User FindByEmail(string email);
        User FindById(string id);
        void Update(User user);
        void Deactivate(string id);
    }
}