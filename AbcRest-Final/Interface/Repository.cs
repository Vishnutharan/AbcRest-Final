using AbcRest_Final.Database_Context;
using Microsoft.EntityFrameworkCore;

namespace AbcRest_Final.Interface
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly ApplicationDbContext _context;
        public readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateAsync(T entity)
        //{
        //    _dbSet.Attach(entity);
        //    _context.Entry(entity).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}
        public async Task UpdateAsync(T entity)
        {
            // Detach any already tracked entity with the same primary key
            var entityKey = _context.Entry(entity).Property("Id").CurrentValue;
            var existingEntity = await _dbSet.FindAsync(entityKey);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            // Attach the updated entity and mark it as modified
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
