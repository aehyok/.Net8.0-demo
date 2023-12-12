using aehyok.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aehyok.EntityFramework.Mapping
{
    /// <summary>
    /// EFCore实体映射到数据库表结构的基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class MapBase<TEntity>: MapBase<TEntity, long>, IMappingConfiguration<TEntity> where TEntity : Entity<long>
    {

    }

    public abstract class MapBase<TEntity, TKey> : IMappingConfiguration<TEntity> where TEntity : Entity<TKey> where TKey : struct
    {
        public void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(this);
        }

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ///统一设置每个表的主键都为Id字段
            builder.HasKey(a => a.Id);
        }
    }
}
