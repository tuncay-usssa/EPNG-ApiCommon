using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using EPNG_ApiCommon.Entities;
using EPNG_ApiCommon.Messages;
using EPNG_ApiCommon.Repositories;
using Nelibur.ObjectMapper;

namespace EPNG_ApiCommon.Services
{
    public class Service<TElement, TEntity> : IService<TElement, TEntity> where TElement : class where TEntity : Entity
    {
        protected readonly IRepository<TEntity> _genericRepository;


        public Service(IRepository<TEntity> repository)
        {
            _genericRepository = repository;
            ConfigureMapping();
        }

        public virtual TElement GetById(int id)
        {
            TEntity e = _genericRepository.GetById(id);

            if (e is null)
            {
                return null;
            }

            return EntityToElementMapper(e);
        }

        public virtual List<TElement> GetAll()
        {
            List<TEntity> entities = _genericRepository.GetAll();
            List<TElement> elements = new List<TElement>();

            foreach (TEntity entity in entities)
            {
                elements.Add(EntityToElementMapper(entity));
            }

            return elements;
        }


        public virtual List<TElement> Find(Expression<Func<TEntity, bool>> predicate)
        {
            List<TEntity> entities = _genericRepository.Find(predicate);
            List<TElement> elements = new List<TElement>();

            foreach (TEntity entity in entities)
            {
                elements.Add(EntityToElementMapper(entity));
            }

            return elements;
        }

        public virtual void Insert(TElement el)
        {
            var type = typeof(TEntity);

            TEntity entity = (TEntity)Activator.CreateInstance(type);
            _genericRepository.Insert(ElementToEntityMapper(el, ref entity));
        }

        public virtual void Insert(IEnumerable<TElement> elements)
        {
            foreach (TElement el in elements)
            {
                TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                _genericRepository.Insert(ElementToEntityMapper(el, ref entity));
            }
        }

        public virtual void Delete(int id)
        {
            _genericRepository.Delete(id);
        }

        public virtual void Delete(IEnumerable<TElement> elements)
        {
            //PKL - TODO - complete this//
            //foreach (TElement el in elements)
            //{
            //    _genericRepository.Delete(el)
            //}
        }

        public virtual void Update(TElement el, int id)
        {
            TEntity entity = _genericRepository.GetById(id);
            //TEntity entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            entity = ElementToEntityMapper(el, ref entity);
            if (!(entity is null))
            {
                entity.Id = id;
            }
            //_genericRepository.CommitAll();
            _genericRepository.Update(entity, id);

        }


        public virtual void ConfigureMapping()
        {
            TinyMapper.Bind<TEntity, TElement>();
            TinyMapper.Bind<TElement, TEntity>();
        }

        public virtual TElement EntityToElementMapper(TEntity e)
        {
            if (e?.Id == null)
            {
                return null;
            }
            else
            {
                return TinyMapper.Map<TElement>(e);
            }
            //Default implementation requires element and entity to have the same properties
        }

        public virtual TEntity ElementToEntityMapper(TElement te, ref TEntity e)
        {
            if (te is null)
            {
                return default;
            }
            else
            {
                e = TinyMapper.Map<TEntity>(te);
                return e;
            }
            //Default implementation requires element and entity to have the same properties
        }

        public SearchResults<TElement> Filter<TFilter>(TFilter filter) where TFilter : Filter
        {
            return _genericRepository.Filter(filter).Transform(EntityToElementMapper);

        }
    }
}
