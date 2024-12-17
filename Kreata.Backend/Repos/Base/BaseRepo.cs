using Kreta.Shared.Models;
using Microsoft.EntityFrameworkCore;

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

    }
}
