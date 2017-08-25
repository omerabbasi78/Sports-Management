using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern;

namespace Service.Pattern
{
    public interface IService<TEntity> where TEntity : IObjectState
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
        IQueryFluent<TEntity> Query();
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
        Result<IQueryable<TEntity>> Queryable();
        IQueryable<TEntity> QueryableCustom();
    }
}