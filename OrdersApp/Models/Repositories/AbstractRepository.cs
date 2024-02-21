using Microsoft.EntityFrameworkCore;
using OrdersApp.Models.Contexts;
using OrdersApp.Models.Entities;
using System.Diagnostics;

namespace OrdersApp.Models.Repositories
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        public AbstractRepository(AppDbContext context)
        {
            Context = context;
        }

        public AppDbContext Context { get; init; }

        protected DbSet<T> Table = null!;

        public virtual async Task<T?> GetAsync(int id) => await Table.FirstOrDefaultAsync(e => e.Id == id);
            //await Task.Run(() => Table.FirstOrDefault(entity => entity.Id == id));

        public async Task<int> AddAsync(T entity)
        {
            await Table.AddAsync(entity);
            await SaveChangesAsync();
            return entity.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity != null)
            {
                //Table.Remove(entity);
                Context.Entry(entity).State = EntityState.Deleted;
                return await SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return await SaveChangesAsync();
        }


        public virtual async Task<IEnumerable<T>> GetAllAsync() => await Table.ToListAsync();

        public async Task<IEnumerable<int>> AddRangeAsync(IEnumerable<T> entities)
        {
            await Table.AddRangeAsync(entities);
            await SaveChangesAsync();

            var result = new List<int>(entities.Select(e => e.Id));
            return result;
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<int> ids)
        {
            int counter = 0;

            foreach (var id in ids)
            {
                counter += await DeleteAsync(id);
            }

            return counter;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
