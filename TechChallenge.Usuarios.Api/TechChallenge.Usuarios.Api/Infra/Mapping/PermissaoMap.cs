using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Usuarios.Api.Domain.Entities;

namespace TechChallenge.Usuarios.Api.Infra.Mapping
{
    public class PermissaoMap : IEntityTypeConfiguration<Permissao>
    {
        public void Configure(EntityTypeBuilder<Permissao> builder)
        {
            builder.ToTable("Permissao");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("Id").HasColumnType("INT").UseIdentityColumn();
            builder.Property(u => u.Funcao).HasColumnType("VARCHAR(100)").IsRequired();
        }
    }
}
