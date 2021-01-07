using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using UserCRUDApi.Domain.Entities;
using UserCRUDApi.Infra.Data.Mapping;

namespace UserCRUDApi.Infra.Data.Context
{
    public class UserCRUDContext : DbContext
    {
        private static bool dbCreated = false;
        
        public DbSet<Usuario> Usuarios { get; set; }      

        public UserCRUDContext()
        {
            if (!dbCreated)
            {
                dbCreated = true;                
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnectionSqlServer"));               
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UsuarioMap()).Entity<Usuario>();                             
        }

        public override int SaveChanges()
        {
            foreach (var entidade in ChangeTracker.Entries().Where(a => a.Entity.GetType().GetProperty("DataInclusao") != null))
            {
                if (entidade.State == EntityState.Added)
                {
                    entidade.Property("DataInclusao").CurrentValue = DateTime.Now;
                }

                if (entidade.State == EntityState.Modified)
                {
                    entidade.Property("DataInclusao").IsModified = false;
                }
            }

            return base.SaveChanges();
        }
    }
}
