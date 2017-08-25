using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Infrastructure;

namespace Repository.Pattern.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IObjectState
    {
        Result<TEntity> Find(params object[] keyValues);
        Result<TEntity> FindAll(params object[] keyValues);
        Result<IQueryable<TEntity>> SelectQuery(string query, params object[] parameters);
        Result<TEntity> Insert(TEntity entity);
        Result<IEnumerable<TEntity>> InsertRange(IEnumerable<TEntity> entities);
        Result<TEntity> InsertOrUpdateGraph(TEntity entity);
        Result<IEnumerable<TEntity>> InsertGraphRange(IEnumerable<TEntity> entities);
        Result<TEntity> Update(TEntity entity);
        Result<TEntity> HardDelete(object id);
        Result<TEntity> HardDelete(TEntity id);
        Result<TEntity> Delete(object id);
        Result<TEntity> Delete(TEntity id);
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity> Query();
        Result<IQueryable<TEntity>> Queryable();
        IQueryable<TEntity> QueryableCustom();
        Result<IEnumerable<TEntity>> ExecWithStoreProcedure(string query, params object[] parameters);
        IRepository<T> GetRepository<T>() where T : class, IObjectState;
    }
}