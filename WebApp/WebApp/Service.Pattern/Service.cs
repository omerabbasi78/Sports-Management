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
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class, IObjectState
    {
        #region Private Fields
        private readonly IRepositoryAsync<TEntity> _repository;
        #endregion Private Fields

        #region Constructor
        protected Service(IRepositoryAsync<TEntity> repository) { _repository = repository; }
        #endregion Constructor

        public virtual Result<TEntity> Find(params object[] keyValues) { return _repository.Find(keyValues); }

        public virtual Result<TEntity> FindAll(params object[] keyValues) { return _repository.FindAll(keyValues); }

        public virtual Result<IQueryable<TEntity>> SelectQuery(string query, params object[] parameters) { return _repository.SelectQuery(query, parameters); }

        public virtual Result<TEntity> Insert(TEntity entity) { return _repository.Insert(entity); }

        public virtual Result<IEnumerable<TEntity>> InsertRange(IEnumerable<TEntity> entities) { return _repository.InsertRange(entities); }

        public virtual Result<TEntity> InsertOrUpdateGraph(TEntity entity) { return _repository.InsertOrUpdateGraph(entity); }

        public virtual Result<IEnumerable<TEntity>> InsertGraphRange(IEnumerable<TEntity> entities) { return _repository.InsertGraphRange(entities); }

        public virtual Result<TEntity> Update(TEntity entity) { return _repository.Update(entity); }

        public virtual Result<TEntity> HardDelete(object id) { return _repository.HardDelete(id); }

        public virtual Result<TEntity> HardDelete(TEntity entity) { return _repository.HardDelete(entity); }
        public virtual Result<TEntity> Delete(object id) { return _repository.Delete(id); }
        public virtual Result<TEntity> Delete(TEntity entity) { return _repository.Delete(entity); }

        public IQueryFluent<TEntity> Query() { return _repository.Query(); }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject) { return _repository.Query(queryObject); }

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query) { return _repository.Query(query); }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues) { return await _repository.FindAsync(keyValues); }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues) { return await _repository.FindAsync(cancellationToken, keyValues); }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues) { return await DeleteAsync(CancellationToken.None, keyValues); }

        //IF 04/08/2014 - Before: return await DeleteAsync(cancellationToken, keyValues);
        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues) { return await _repository.DeleteAsync(cancellationToken, keyValues); }

        public Result<IQueryable<TEntity>> Queryable() { return _repository.Queryable(); }
        public IQueryable<TEntity> QueryableCustom() { return _repository.QueryableCustom(); }
    }
}