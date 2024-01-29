using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public abstract class ProductBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ProductDbContext _dbContext;

        protected ProductBaseRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entity);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                    return entity;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }

        public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
                if (entity != null)
                {
                    _dbContext.Set<TEntity>().Remove(entity);
                    var result = await _dbContext.SaveChangesAsync();

                    return result > 0;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return false;
        }

        public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity)
        {
            try
            {
                var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(updatedEntity);
                    var result = await _dbContext.SaveChangesAsync();

                    if (result > 0)
                        return updatedEntity;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                var entities = await _dbContext.Set<TEntity>().ToListAsync();
                if (entities != null)
                {
                    return entities;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var found = await _dbContext.Set<TEntity>().AnyAsync(predicate);
                return found;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return false!;
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
                if (entity != null)
                {
                    return entity;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }
    }
}
