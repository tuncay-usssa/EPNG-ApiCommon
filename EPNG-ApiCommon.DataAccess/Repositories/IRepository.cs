using EPNG_ApiCommon.Messages;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPNG_ApiCommon.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {

        TEntity GetById(int id);
        List<TEntity> GetAll();
        List<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Insert(TEntity element);
        void Insert(IEnumerable<TEntity> elements);
        TEntity InsertAndReturn(TEntity entity);

        void Delete(int id);
        void Delete(IEnumerable<TEntity> elements);

        void Update(TEntity element, int id);

        void CommitAll();
        TEntity GetLastItem();
        void DeleteUpToAndIncludingId(int id);
        SearchResults<TEntity> Filter<TFilter>(TFilter filter) where TFilter : Filter;
        ICollection<TEntity> Search(Expression<Func<TEntity, bool>> predicate);

        //List<THistEntity> UpdateHistoryItems<THistEntity>(THistEntity newItem) where THistEntity : AuditableHistory;

    }
}
