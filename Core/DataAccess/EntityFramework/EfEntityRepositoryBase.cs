using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess.EntityFramework
{
    // Core katmanı, nesne ve veritabanından bağımsızdır
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using (TContext nc = new TContext())
            {
                var add = nc.Entry(entity);
                add.State = EntityState.Added;
                nc.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext nc = new TContext())
            {
                var delete = nc.Entry(entity);
                delete.State = EntityState.Deleted;
                nc.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext nc = new TContext())
            {
                return nc.Set<TEntity>().SingleOrDefault(filter);

            }
        }


        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext nc = new TContext())
            {
                return filter == null
                    ? nc.Set<TEntity>().ToList()
                    : nc.Set<TEntity>().Where(filter).ToList();
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext nc = new TContext())
            {
                var update = nc.Entry(entity);
                update.State = EntityState.Modified;
                nc.SaveChanges();
            }
        }
    }
}
