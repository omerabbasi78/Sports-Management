#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using Repository.Pattern.DataContext;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;

#endregion

namespace Repository.Pattern.Ef6
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        #region Private Fields

        private readonly IDataContextAsync _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorkAsync _unitOfWork;

        #endregion Private Fields

        public Repository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

            // Temporarily for FakeDbContext, Unit Test and Fakes
            var dbContext = context as DbContext;

            if (dbContext != null)
            {
                _dbSet = dbContext.Set<TEntity>();
            }
            else
            {
                var fakeContext = context as FakeDbContext;

                if (fakeContext != null)
                {
                    _dbSet = fakeContext.Set<TEntity>();
                }
            }
        }

        public virtual Result<TEntity> Find(params object[] keyValues)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                TEntity entity = _dbSet.Find(keyValues);
                if (entity != null && entity.IsActive)
                {
                    result.data = entity;
                    result.success = true;
                }
                else
                {
                    result.success = false;
                    result.ErrorMessage = "Data not found";
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public virtual Result<TEntity> FindAll(params object[] keyValues)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                TEntity entity = _dbSet.Find(keyValues);
                if (entity != null)
                {
                    result.data = entity;
                    result.success = true;
                }
                else
                {
                    result.success = false;
                    result.ErrorMessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public virtual Result<IQueryable<TEntity>> SelectQuery(string query, params object[] parameters)
        {
            Result<IQueryable<TEntity>> result = new Result<IQueryable<TEntity>>();
            try
            {
                var data = _dbSet.SqlQuery(query, parameters).Where(w => w.IsActive).AsQueryable();
                if (data != null && data.Count() > 0)
                {
                    result.data = data;
                    result.success = true;
                }
                else
                {
                    result.success = false;
                    result.ErrorMessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public virtual Result<TEntity> Insert(TEntity entity)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                entity.ObjectState = ObjectState.Added;
                _dbSet.Attach(entity);
                _context.SyncObjectState(entity);
                result.success = true;
                result.data = entity;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;

        }

        public virtual Result<IEnumerable<TEntity>> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            Result<IEnumerable<TEntity>> result = new Result<IEnumerable<TEntity>>();
            try
            {
                var context = _context as DbContext;
                IEnumerable<TEntity> data = context.Database.SqlQuery<TEntity>(query, parameters);
                if (data != null && data.Count() > 0)
                {
                    result.success = true;
                }
                else
                {
                    result.success = false;
                    result.ErrorMessage = "Data not found";
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public virtual Result<IEnumerable<TEntity>> InsertRange(IEnumerable<TEntity> entities)
        {
            Result<IEnumerable<TEntity>> result = new Result<IEnumerable<TEntity>>();
            try
            {
                foreach (var entity in entities)
                {
                    Insert(entity);
                }
                result.data = entities;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public virtual Result<IEnumerable<TEntity>> InsertGraphRange(IEnumerable<TEntity> entities)
        {
            Result<IEnumerable<TEntity>> result = new Result<IEnumerable<TEntity>>();
            try
            {
                _dbSet.AddRange(entities);
                result.data = entities;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public virtual Result<TEntity> Update(TEntity entity)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                entity.ObjectState = ObjectState.Modified;
                _dbSet.Attach(entity);
                _context.SyncObjectState(entity);
                result.data = entity;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;

        }

        public virtual Result<TEntity> Delete(object id)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                var entity = _dbSet.Find(id);
                result = Delete(entity);
                result.data = entity;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;

        }

        public virtual Result<TEntity> Delete(TEntity entity)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                entity.ObjectState = ObjectState.Modified;
                entity.IsActive = false;
                _dbSet.Attach(entity);
                _context.SyncObjectState(entity);
                result.data = entity;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;

        }

        public virtual Result<TEntity> HardDelete(object id)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                var entity = _dbSet.Find(id);
                result = HardDelete(entity);
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public virtual Result<TEntity> HardDelete(TEntity entity)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                entity.ObjectState = ObjectState.Deleted;
                _dbSet.Attach(entity);
                _context.SyncObjectState(entity);
                result.data = entity;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public IQueryFluent<TEntity> Query()
        {
            return new QueryFluent<TEntity>(this);
        }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            return new QueryFluent<TEntity>(this, queryObject);
        }

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return new QueryFluent<TEntity>(this, query);
        }

        public Result<IQueryable<TEntity>> Queryable()
        {
            Result<IQueryable<TEntity>> result = new Result<IQueryable<TEntity>>();
            try
            {
                var data = _dbSet.Where(w => w.IsActive);
                if (data != null && data.Count() > 0)
                {
                    result.data = data;
                    result.success = true;
                }
                else
                {
                    result.success = false;
                    result.ErrorMessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public IQueryable<TEntity> QueryableCustom()
        {
            return _dbSet;
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);

            return true;
        }

        internal IQueryable<TEntity> SelectDataAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query.Where(w => w.IsActive), (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal Result<IEnumerable<TEntity>> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null
            )
        {
            Result<IEnumerable<TEntity>> result = new Result<IEnumerable<TEntity>>();
            try
            {

                IQueryable<TEntity> query = _dbSet;

                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }
                if (orderBy != null)
                {
                    query = orderBy(query);
                }
                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }
                if (page != null && pageSize != null)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                query = query.Where(w => w.IsActive);
                if (query != null && query.Count() > 0)
                {
                    result.data = query;
                    result.success = true;
                }
                else
                {
                    result.success = false;
                    result.ErrorMessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        internal IQueryable<TEntity> SelectAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await SelectDataAsync(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        // Insert or Updating an object graph
        [Obsolete("Will be renamed to UpsertGraph(TEntity entity) in next version.")]
        public virtual Result<TEntity> InsertOrUpdateGraph(TEntity entity)
        {
            Result<TEntity> result = new Result<TEntity>();
            try
            {
                result.data = entity;
                Result<object> resultSync = new Result<object>();
                resultSync = SyncObjectGraph(entity);
                if (!resultSync.success)
                {
                    result.success = false;
                    result.ErrorMessage = resultSync.ErrorMessage;
                    return result;
                }
                _entitesChecked = null;
                _dbSet.Attach(entity);
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        // tracking of all processed entities in the object graph when calling SyncObjectGraph
        HashSet<object> _entitesChecked;

        private Result<object> SyncObjectGraph(object entity) // scan object graph for all 
        {
            Result<object> result = new Result<object>();
            try
            {
                // instantiating _entitesChecked so we can keep track of all entities we have scanned, avoid any cyclical issues
                if (_entitesChecked == null)
                    _entitesChecked = new HashSet<object>();

                // if already processed skip
                if (_entitesChecked.Contains(entity))
                    return result;

                // add entity to alreadyChecked collection
                _entitesChecked.Add(entity);

                var objectState = entity as IObjectState;
                // discovered entity with ObjectState.Added, sync this with provider e.g. EF
                if (objectState != null && objectState.ObjectState == ObjectState.Added)
                    _context.SyncObjectState((IObjectState)entity);

                // discovered entity with ObjectState.Detached, sync this with provider e.g. EF By neo
                if (objectState != null && objectState.ObjectState == ObjectState.Detached)
                    _context.SyncObjectState((IObjectState)entity);

                // discovered entity with ObjectState.Unchanged, sync this with provider e.g. EF By neo
                if (objectState != null && objectState.ObjectState == ObjectState.Unchanged)
                    _context.SyncObjectState((IObjectState)entity);

                // Set tracking state for child collections
                foreach (var prop in entity.GetType().GetProperties())
                {
                    // Apply changes to 1-1 and M-1 properties
                    var trackableRef = prop.GetValue(entity, null) as IObjectState;
                    if (trackableRef != null)
                    {
                        // discovered entity with ObjectState.Added, sync this with provider e.g. EF
                        if (trackableRef.ObjectState == ObjectState.Added)
                            _context.SyncObjectState((IObjectState)entity);

                        // discovered entity with ObjectState.Unchanged, sync this with provider e.g. EF By neo
                        //if (trackableRef.ObjectState == ObjectState.Unchanged)
                        //    _context.SyncObjectState((IObjectState)entity);

                        // recursively process the next property
                        SyncObjectGraph(prop.GetValue(entity, null));
                    }

                    // Apply changes to 1-M properties
                    var items = prop.GetValue(entity, null) as IEnumerable<IObjectState>;

                    // collection was empty, nothing to process, continue
                    if (items == null) continue;

                    // collection isn't empty, continue to recursively scan the elements of this collection
                    foreach (var item in items)
                        SyncObjectGraph(item);
                }

                result.success = true;
                result.data = entity;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public IRepository<T> GetRepository<T>() where T : class, IObjectState
        {
            return _unitOfWork.Repository<T>();
        }
    }
}