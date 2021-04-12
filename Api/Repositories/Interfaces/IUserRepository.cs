using Api.Models;

namespace Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void Create(User user);
    }
}