using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAcceess.IRepository
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonByEmail(string email);

        Task<IEnumerable<Person>> GetPersonByType(Enums.PersonType type);

        Task<Person> GetPersonByUserId(int id);

        Task<Person> GetByUserName(string userName);
    }
}