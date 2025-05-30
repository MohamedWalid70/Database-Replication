using Microsoft.EntityFrameworkCore;
using Replication.Domain.Repositories;
using Replication.Infrastructure.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Infrastructure.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        AppDbContext _appDbContext;
        DbSet<TEntity> _dbSet;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<TEntity>();
        }

        public async ValueTask AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void BeginUpdate(TEntity entity)
        {
            _dbSet.Attach(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> GetAllAsync()
        {
            return _dbSet.AsNoTracking();
        }

        public ValueTask<TEntity?> GetByIdAsync(Guid id)
        {
            return _dbSet.FindAsync(id);
        }

    }
}
