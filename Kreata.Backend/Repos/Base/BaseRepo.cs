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
