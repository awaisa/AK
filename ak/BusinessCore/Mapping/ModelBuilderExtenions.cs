using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessCore.EntityMappings
{
    public static class ModelBuilderExtenions
    {
        public static void AddEntityConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var implementedConfigTypes = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract
            && !t.IsGenericTypeDefinition
            && t.GetTypeInfo().ImplementedInterfaces.Any(i =>
                i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));
            foreach (var configType in implementedConfigTypes)
            {
                dynamic config = Activator.CreateInstance(configType);
                modelBuilder.ApplyConfiguration(config);
            }
        }
    }
}
