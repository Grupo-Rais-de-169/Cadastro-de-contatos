using Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infra.Context
{
    public class MainContext : DbContext
    {
        private readonly IDbContextFactory<MainContext> _context;
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Permissao> Permissao { get; set; }
        public DbSet<Contatos> Contatos { get; set; }
        public DbSet<CodigoDeArea> CodigosDeArea { get; set; }

        public MainContext(DbContextOptions<MainContext> options, ILogger<MainContext> logger, IDbContextFactory<MainContext> context) : base(options)
        {
            Database.SetCommandTimeout(1000);
            _context = context;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var extension = optionsBuilder.Options.FindExtension<Microsoft.EntityFrameworkCore.Infrastructure.RelationalOptionsExtension>();
            var connectionString = extension?.ConnectionString;

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
