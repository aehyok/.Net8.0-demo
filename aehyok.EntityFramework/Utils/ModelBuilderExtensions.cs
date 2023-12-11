using aehyok.EntityFramework.Mapping;
using aehyok.Infrastructure.TypeFinders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace aehyok.EntityFramework.Utils
{
    public static class ModelBuilderExtensions
    {
        public static void RegisterFromAssembly<TEntity>(this ModelBuilder builder, Func<Type, bool> modelTypePredicate)
        {
            var registedTypes = new HashSet<Type>();

            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("aehyok.")).ToList();

            //foreach ( var assembly in assemblies)
            //{
            //    foreach (var type in assembly.GetTypes())
            //    {
            //        if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(IMappingConfiguration)))
            //        {
            //            var mapping = (IMappingConfiguration)Activator.CreateInstance(type);
            //            mapping.ApplyConfiguration(builder);

            //            var entityType = type.GetTypeInfo().ImplementedInterfaces.First().GetGenericArguments().Single();
            //            registedTypes.Add(entityType);
            //        }

            //        var types = type.ge (a => !a.IsAbstract && a.IsClass).Where(modelTypePredicate).Where(a => !registedTypes.Contains(a)).ToList();

            //        types.ForEach(type =>
            //        {
            //            builder.Entity(type).HasNoDiscriminator();
            //        });
            //    }
            //}

            foreach (var type in TypeFinders.SearchTypes(typeof(IMappingConfiguration)))
            {
                var mapping = (IMappingConfiguration)Activator.CreateInstance(type);
                mapping.ApplyConfiguration(builder);

                var entityType = type.GetTypeInfo().ImplementedInterfaces.First().GetGenericArguments().Single();
                registedTypes.Add(entityType);
            }

            var types = TypeFinders.SearchTypes(typeof(TEntity)).Where(modelTypePredicate).Where(a => !registedTypes.Contains(a)).ToList();
            foreach (var type in types)
            {
                builder.Entity(type).HasNoDiscriminator();
            }
        }
    }
}
