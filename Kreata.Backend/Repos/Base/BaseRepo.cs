using Kreta.Shared.Models;
using Kreta.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kreata.Backend.Repos.Base
{
    public class BaseRepo<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class, IDbEntity<TEntity>, new()
    {
        private readonly DbContext? _dbContext;
        protected readonly DbSet<TEntity>? _dbSet;

        public BaseRepo(TDbContext? dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext, $"A {nameof(TEntity)} adatbázis tábla nem elérhető!");
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>() ?? throw new ArgumentException($"A {nameof(TEntity)} adatbázis tábla nem elérhető!");
        }

        public async Task<List<TEntity>> GetAllAsync() => await _dbSet!.ToListAsync();
        public async Task<List<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression) => await _dbSet!.Where(expression).ToListAsync();
        public async Task<Response> UpdateAsync(TEntity entity)
        {
            Response response = new();
            try
            {
                if (_dbContext is not null)
                {
                    _dbContext.ChangeTracker.Clear();
                    _dbContext.Entry(entity).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    return response;
                }
            }
            catch (Exception e)
            {
                return HandleExceptionOrError(nameof(UpdateAsync), e, $"{entity} frissítése nem sikerült!");
            }
            return response;
        }

        private static Response HandleExceptionOrError(string methodName, Exception? exception, string additionalMessage = "")
        {
            Response response = new();
            if (!string.IsNullOrEmpty(methodName))
                response.AppendNewError($"{methodName} metódusban hiba történt.");
            if (exception is not null)
                response.AppendNewError(exception.Message);
            if (!string.IsNullOrWhiteSpace(additionalMessage))
                response.AppendNewError(additionalMessage);
            return response;
        }


    }
}
