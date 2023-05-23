using Common.Models;
using DataAcceess.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAcceess.Repository
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(DataAccessContext context) : base(context)
        {
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            return await table.Where(p => p.Email == email)?.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Person>> GetPersonByType(Enums.PersonType type)
        {
            return table.Where(p => p.PersonType == type);
        }

        public async Task<Person> GetPersonByUserId(int id)
        {
            return await table.Where(p => p.IdUser == id)?.FirstOrDefaultAsync();
        }
    }
}