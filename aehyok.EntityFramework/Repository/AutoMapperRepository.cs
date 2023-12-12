using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using X.PagedList;

namespace aehyok.EntityFramework.Repository
{
    public abstract class AutoMapperRepository<TEntity, TKey> : RepositoryBase<TEntity, TKey>, IAutoMapperRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly IMapper Mapper;
        protected readonly AutoMapper.IConfigurationProvider MapperConfig;

        protected AutoMapperRepository(DbContext dbContext, IMapper mapper)
            : this(dbContext, mapper, SpecificationEvaluator.Default)
        {
        }

        public AutoMapperRepository(DbContext dbContext, IMapper mapper, ISpecificationEvaluator specificationEvaluator)
            : base(dbContext, specificationEvaluator)
        {
            this.Mapper = mapper;
            this.MapperConfig = mapper.ConfigurationProvider;
        }

        public Task<TProjectedType> GetAsync<TProjectedType>(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default) where TProjectedType : class
        {
            ArgumentNullException.ThrowIfNull(condition);
            return this.GetQueryable().Where(condition).ProjectTo<TProjectedType>(this.MapperConfig).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<TProjectedType> GetAsync<TProjectedType>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default) where TProjectedType : class
        {
            return this.ApplySpecification(specification).ProjectTo<TProjectedType>(this.MapperConfig).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<TProjectedType> GetByIdAsync<TProjectedType>(TKey id, CancellationToken cancellationToken = default) where TProjectedType : class
        {
            ArgumentNullException.ThrowIfNull(id);

            IEntityType entityType = DbContext.Model.FindEntityType(typeof(TEntity));

            string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
            Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primayKeyValue = null;

            try
            {
                primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            ParameterExpression pe = Expression.Parameter(typeof(TEntity), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<TEntity, bool>> expressionTree = Expression.Lambda<Func<TEntity, bool>>(body, new[] { pe });

            var query = GetQueryable();

            return query.Where(expressionTree).ProjectTo<TProjectedType>(this.MapperConfig).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<List<TProjectedType>> GetListAsync<TProjectedType>(Expression<Func<TEntity, bool>> condition = null,
                                                                       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                                                                       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
                                                                       bool asNoTracking = true,
                                                                       CancellationToken cancellationToken = default) where TProjectedType : class
        {
            var query = GetQueryable();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orders != null)
            {
                query = orders(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.ProjectTo<TProjectedType>(this.MapperConfig).ToListAsync(cancellationToken);
        }

        public Task<List<TProjectedType>> GetListAsync<TProjectedType>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default) where TProjectedType : class
        {
            return this.ApplySpecification(specification).ProjectTo<TProjectedType>(this.MapperConfig).ToListAsync(cancellationToken);
        }

        public Task<IPagedList<TProjectedType>> GetPagedListAsync<TProjectedType>(int pageIndex = 1,
                                                                                                    int pageSize = 10,
                                                                                                    Expression<Func<TEntity, bool>> condition = null,
                                                                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                                                                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
                                                                                                    bool asNoTracking = true,
                                                                                                    CancellationToken cancellationToken = default) where TProjectedType : class
        {
            var query = GetQueryable();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orders != null)
            {
                query = orders(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.ProjectTo<TProjectedType>(this.MapperConfig).ToPagedListAsync(pageIndex, pageSize, null, cancellationToken);
        }

        public Task<IPagedList<TProjectedType>> GetPagedListAsync<TProjectedType>(ISpecification<TEntity> specification, int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default) where TProjectedType : class
        {
            return this.ApplySpecification(specification).ProjectTo<TProjectedType>(this.MapperConfig).ToPagedListAsync(pageIndex, pageSize, null, cancellationToken);
        }
    }
}
