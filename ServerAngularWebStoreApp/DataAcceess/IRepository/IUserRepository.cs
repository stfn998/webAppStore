using Common.Models;
using System.Threading.Tasks;

namespace DataAcceess.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
    }
}