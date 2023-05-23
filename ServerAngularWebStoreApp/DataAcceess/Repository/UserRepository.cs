using Common.Models;
using DataAcceess.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataAcceess.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DataAccessContext context) : base(context)
        {
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await table.Where(u => u.UserName == username)?.FirstOrDefaultAsync();
        }
    }
}