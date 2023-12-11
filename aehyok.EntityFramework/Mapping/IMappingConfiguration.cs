using aehyok.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aehyok.EntityFramework.Mapping
{
    public interface IMappingConfiguration
    {
        void ApplyConfiguration(ModelBuilder modelBuilder);
    }

    public interface IMappingConfiguration<TEntity> : IMappingConfiguration where TEntity : class, IEntity
    {
        void ApplyConfiguration(ModelBuilder modelBuilder, TEntity entity);
    }
}
