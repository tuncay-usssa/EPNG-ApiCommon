using EPNG_ApiCommon.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EPNG_ApiCommon.DataAccess
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        TEntity Find<TEntity>( params object[] keyValues) where TEntity : class;
        object Find(Type entityType, params object[] keyValues);
        EntityEntry Entry([NotNullAttribute] object entity);
        EntityEntry<TEntity> Entry<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;

    }
}
