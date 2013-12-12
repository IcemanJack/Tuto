using System;
using System.Linq;
using System.Linq.Expressions;
using Tuto.DataLayer.Models;

namespace Tuto.DataLayer.Repository
{
    public interface IEntityRepository
    {
        IQueryable<T> getAll<T>() where T : class;
        IQueryable<T> getAll<T>(Expression<Func<T, bool>> filter) where T : class;
        T getById<T>(int id) where T : Entity;
        T single<T>(Expression<Func<T, bool>> filter) where T : class;


        void delete<T>(int id) where T : Entity;
        void add<T>(T entity) where T : class;
        void update<T>(T entity) where T : class;
    }
}
