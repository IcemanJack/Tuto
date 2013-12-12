using System;
using System.Linq;
using System.Linq.Expressions;
using Tuto.DataLayer.Models;

namespace Tuto.DataLayer.Repository
{
    public class EntityRepository : IEntityRepository
    {
        private readonly TutoContext context;

        public EntityRepository()
        {
            this.context = new TutoContext();
        }

        public IQueryable<T> getAll<T>() where T : class
        {
            return this.context.Set<T>().AsQueryable();
        }

        public IQueryable<T> getAll<T>(Expression<Func<T, bool>> filter) where T : class
        {
            return this.context.Set<T>().Where(filter);
        }

        public T single<T>(Expression<Func<T, bool>> filter) where T : class
        {
            try
            {
                T resultToReturn = this.context.Set<T>().Where(filter).FirstOrDefault();
                return resultToReturn;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void delete<T>(int id) where T : Entity
        {
            var entity = this.context.Set<T>().FirstOrDefault(x => x.id == id);
            delete(entity);
        }

        public T getById<T>(int id) where T : Entity
        {
            return this.context.Set<T>().FirstOrDefault(x => x.id == id);
        }

        public void add<T>(T entity) where T : class
        {
            this.context.Set<T>().Add(entity);
            this.context.SaveChanges();
        }

        public void update<T>(T entity) where T : class
        {
            this.context.Set<T>().Attach(entity);
            this.context.Entry(entity).State = System.Data.EntityState.Modified;
            this.context.SaveChanges();
        }

        public void delete<T>(T entity) where T : class
        {
            this.context.Set<T>().Attach(entity);
            this.context.Entry(entity).State = System.Data.EntityState.Deleted;
            this.context.SaveChanges();
        }

        public void addAll<T>(params T[] entities) where T : class
        {
            foreach (var entity in entities)
            {
                this.context.Set<T>().Add(entity);
            }
            this.context.SaveChanges(); 
        }

        public void dispose()
        {
            this.context.Dispose();
        }
    }
}
