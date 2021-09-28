using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPNG_ApiCommon.Messages
{
    public class SearchResults<TEntity>
    {
        public IEnumerable<TEntity> CurrentPageResults { get; set; }
        public int PageNumber { get; set; }
        public long TotalResultCount { get; set; }

        public SearchResults<TElement> Transform<TElement>(Func<TEntity, TElement> mapper) where TElement : class
        {
            SearchResults<TElement> result = new SearchResults<TElement>();
            result.CurrentPageResults = CurrentPageResults.Select(a => mapper(a));
            result.PageNumber = PageNumber;
            result.TotalResultCount = TotalResultCount;
            return result;
        }
    }
}
