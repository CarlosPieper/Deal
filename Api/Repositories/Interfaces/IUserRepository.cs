using System.Threading.Tasks;
using Api.Models;

namespace Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task<User> FindByEmail(string email);
        Task<User> FindById(string id);
        Task Update(User user);
        Task Deactivate(string id);
    }
}