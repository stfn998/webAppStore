using DataAcceess.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAcceess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DataAccessContext context;
        protected DbSet<T> table;

        public GenericRepository()
        {
            this.context = new DataAccessContext();
            table = context.Set<T>();
        }

        public GenericRepository(DataAccessContext context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public async Task Delete(T obj)
        {
            table.Remove(obj);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.ToListAsync();
        }

        public async Task<T> GetByObject(object obj)
        {
            return await table.FindAsync(obj);
        }

        public async Task Insert(T obj)
        {
            await table.AddAsync(obj);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(T obj)
        {
            table.Attach(obj);
            context.Entry(obj).State = EntityState.Modified;
        }
    }
}