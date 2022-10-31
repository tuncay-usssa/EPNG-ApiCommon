using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using EPNG_ApiCommon.Entities;
using EPNG_ApiCommon.Messages;

namespace EPNG_ApiCommon.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbContext _context;
        //private readonly ICoreLogger _logger;

        public Repository(DbContext context)
        {
            _context = context;
            //_logger = logger;
        }

        public virtual TEntity GetById(int id)
        {
            return ScopedItems().FirstOrDefaultAsync(x => x.Id == id).Result;
        }

        public virtual List<TEntity> GetAll()
        {
            return ScopedItems()?.ToList();
        }


        public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return ScopedItems().Where(predicate).ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add((TEntity)entity);
            _context.SaveChanges();
        }

        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public virtual TEntity InsertAndReturn(TEntity entity)
        {
            TEntity e = _context.Set<TEntity>().Add((TEntity)entity).Entity;
            _context.SaveChanges();
            return e;
        }

        public virtual void Delete(int id)
        {

            try
            {
                TEntity oldEntity = (TEntity)_context.Find(typeof(TEntity), id);

                if (oldEntity is IAuditable auditableE)
                {
                    auditableE.IsActive = false;
                    Update(oldEntity, id);
                }

                else
                {
                    _context.Set<TEntity>().Remove((TEntity)oldEntity);
                    _context.SaveChanges(); //not sure this is needed
                }
            }

            catch (Exception ex)
            {
                //_logger.LogException(ex);
            }
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            //PKL - This probably does not work
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual void DeleteUpToAndIncludingId(int id)
        {
            _context.Set<TEntity>().RemoveRange(_context.Set<TEntity>().Where(a => a.Id <= id));
        }


        public virtual void Update(TEntity entity, int id)
        {
            if (entity is IAuditable auditableE)
            {
                auditableE.ModifiedBy = "coming soon"; //TODO - add user
                auditableE.ModifiedDate = DateTime.Now;
            }

            try
            {
                TEntity oldEntity = (TEntity)_context.Find(typeof(TEntity), id);
                ((dynamic)entity).Id = id;
                _context.Entry(oldEntity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                _context.Set<TEntity>().Attach((TEntity)entity);
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //_logger.LogException(ex);
            }
        }

        public void CommitAll()
        {
            _context.SaveChanges();
        }

        protected virtual IQueryable<TEntity> ScopedItems()
        {
            return _context.Set<TEntity>();
        }

        public virtual TEntity GetLastItem()
        {
            return ScopedItems().OrderByDescending(x => x.Id).FirstOrDefault();
        }

        public virtual SearchResults<TEntity> Filter<TFilter>(TFilter filter) where TFilter : Filter
        {
            return null;
        }

        protected SearchResults<TEntity> ApplyFilter<TFilter>(TFilter filter, ExpressionStarter<TEntity> predicate, bool sortDescending = false) where TFilter : Filter
        {
            return ApplyFilter<TFilter, object>(filter, predicate, null, sortDescending);
        }

        protected SearchResults<TEntity> ApplyFilter<TFilter, TSort>(TFilter filter, ExpressionStarter<TEntity> predicate, Expression<Func<TEntity, TSort>>? orderBy, bool sortDescending = false) where TFilter : Filter
        {
            if (filter.PageIndex == 0)
            {
                filter.PageIndex = 1;
            }

            var results = ScopedItems()
                .AsExpandable()
                .Where(predicate);

            // TODO(jpr): use the FilterItem logic somehow
            if (orderBy != null) {
                if (sortDescending) {
                    results = results.OrderByDescending(orderBy);
                } else {
                    results = results.OrderBy(orderBy);
                }
            }
            var currentResults = results
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new SearchResults<TEntity>
            {
                TotalResultCount = results.Count(),
                PageNumber = filter.PageIndex,
                CurrentPageResults = currentResults,
            };
        }

        public ICollection<TEntity> Search(Expression<Func<TEntity, bool>> predicate)
        {
            //return _dbContext.Set<TEntity>().Find(predicate, new FindOptions
            //{
            //    Collation = new Collation("en", strength: CollationStrength.Primary)
            //}).ToList();
            return null;
        }

        //public List<THistEntity> UpdateHistoryItems<THistEntity>(THistEntity newItem) where THistEntity : AuditableHistory
        //{
        //    bool auditableItemsChanged = false;
        //    THistEntity lastRevision = _context.Set<THistEntity>().Where(a => a.ParentId == newItem.ParentId && a.EndDate == null).FirstOrDefault();

        //    Type targetType = typeof(THistEntity);
        //    Type baseType = typeof(AuditableHistory);

        //    PropertyInfo[] properties = targetType.GetProperties();
        //    PropertyInfo[] baseProperties = baseType.GetProperties();

        //    foreach (PropertyInfo p in properties)
        //    {
        //        if (!baseProperties.Any(a => a.Name == p.Name))
        //        {
        //            string test1 = p.GetValue(newItem)?.ToString();
        //            string test2 = p.GetValue(lastRevision)?.ToString();

        //            if (!Object.Equals(test1, test2) && p.PropertyType.Name != typeof(TEntity).Name)
        //            {
        //                lastRevision.EndDate = DateTime.Now;
        //                newItem.StartDate = DateTime.Now;
        //                newItem.SetBy = "system"; //PKL-TODO will need to use user ID
        //                _context.Set<THistEntity>().Add(newItem);
        //                break;
        //            }
        //        }
        //    }

        //    return _context.Set<THistEntity>().Where(a => a.ParentId == newItem.ParentId).ToList();
        //}


        //private bool _isAuditable()
        //{
        //    return typeof(TEntity).IsSubclassOf(typeof(Auditable));
        //}
    }
    }
