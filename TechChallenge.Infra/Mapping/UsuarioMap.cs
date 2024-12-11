using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain;

namespace TechChallenge.Infra.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("Id").HasColumnType("INT").UseIdentityColumn();
            builder.Property(u => u.Login).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(u => u.Senha).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(u => u.PermissaoId).HasColumnType("INT").IsRequired();
            builder.HasOne(u => u.Permissao)
                            .WithMany(d => d.Usuarios)
                            .HasForeignKey(c => c.PermissaoId);

        }
    }
}
