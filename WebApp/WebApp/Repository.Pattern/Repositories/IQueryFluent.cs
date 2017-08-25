using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Repository.Pattern.Infrastructure;

namespace Repository.Pattern.Repositories
{
    public interface IQueryFluent<TEntity> where TEntity : IObjectState
    {
        IQueryFluent<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IQueryFluent<TEntity> Include(Expression<Func<TEntity, object>> expression);
        Result<IEnumerable<TEntity>> SelectPaged(int page, int pageSize, out int totalCount);
        Result<IEnumerable<TEntity>> Select();
        Task<IEnumerable<TEntity>> SelectAsync();
        Result<IQueryable<TEntity>> SqlQuery(string query, params object[] parameters);
    }
}