using Dev.Core;
using Dev.Core.Infrastructure;
using Dev.Data.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Dev.Plugin.Data.EFCore.Data
{
    public class DevDbContext : DbContext, IDevDbContext
    {
        public DevDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        /// <summary>
        /// refer https://docs.microsoft.com/en-us/ef/core/modeling/
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   //get assembly from TypeFinder
            var typeFinder = EngineContext.Current.Resolve<ITypeFinder>();
            //dynamically load all entity and query type configurations
            var typeConfigurations = typeFinder.FindClassesOfType<IBuilderConfiguration>()
                                               .Where(x => !x.IsAbstract)
                                               .ToList();
            foreach (var typeConfiguration in typeConfigurations)
            {
                var configuration = (IBuilderConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }
            base.OnModelCreating(modelBuilder);
        }

        public string GenerateCreateScript()
        {
            return Database.GenerateCreateScript();
        }

        public virtual int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            //set specific command timeout
            var previousTimeout = Database.GetCommandTimeout();
            Database.SetCommandTimeout(timeout);

            var result = 0;
            if (!doNotEnsureTransaction)
            {
                //use with transaction
                using (var transaction = Database.BeginTransaction())
                {
                    result = Database.ExecuteSqlRaw(sql, parameters);
                    transaction.Commit();
                }
            }
            else
                result = Database.ExecuteSqlRaw(sql, parameters);

            //return previous timeout back
            Database.SetCommandTimeout(previousTimeout);

            return result;
        }

        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public bool EnsureCreated()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            return true;
        }
    }
}
