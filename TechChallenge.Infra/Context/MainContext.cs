using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using TechChallenge.Domain;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Infra.Context
{
    public class MainContext : DbContext
    {
        private readonly IDbContextFactory<MainContext> _context;

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Permissao> Permissao { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<CodigoDeArea> CodigosDeArea { get; set; }

        public MainContext(DbContextOptions<MainContext> options, IDbContextFactory<MainContext> context) : base(options)
        {
            Database.SetCommandTimeout(1000);
            _context = context;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var extension = optionsBuilder.Options.FindExtension<RelationalOptionsExtension>();
            var connectionString = extension?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly);

            // ========================================
            base.OnModelCreating(modelBuilder);

            // Configura a chave primária para EntityBase
            modelBuilder.Entity<EntityBase>().HasKey(e => e.Id);
            modelBuilder.Entity<EntityBase>().ToTable("Entities");
            modelBuilder.Entity<CodigoDeArea>().HasBaseType<EntityBase>();
        }
    }
}
