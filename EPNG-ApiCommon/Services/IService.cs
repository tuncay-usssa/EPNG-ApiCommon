using EPNG_ApiCommon.Messages;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPNG_ApiCommon.Services
{
    public interface IService<TElement, TEntity> where TElement : class where TEntity : class
    {
        TElement GetById(int id);
        List<TElement> GetAll();
        List<TElement> Find(Expression<Func<TEntity, bool>> predicate);

        void Insert(TElement element);
        void Insert(IEnumerable<TElement> elements);

        void Delete(int id);
        void Delete(IEnumerable<TElement> elements);

        void Update(TElement element, int id);

        TElement EntityToElementMapper(TEntity entity);
        TEntity ElementToEntityMapper(TElement element, ref TEntity entity);
        SearchResults<TElement> Filter<TFilter>(TFilter filter) where TFilter : Filter;

        void ConfigureMapping();
    }
}
