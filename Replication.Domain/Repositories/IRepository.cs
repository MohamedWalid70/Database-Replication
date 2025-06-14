﻿namespace Replication.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ValueTask AddAsync(TEntity entity);
        IQueryable<TEntity> GetAll();
        ValueTask<TEntity?> GetByIdAsync(Guid id);
        void BeginUpdate(TEntity entity);
        void Delete(TEntity entity);
       
    }
}
