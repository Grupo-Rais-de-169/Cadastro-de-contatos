using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallenge.DAO.Api.Entities;

namespace TechChallenge.DAO.Api.Infra.Mapping
{
    public class ContatoMap : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.ToTable("Contatos");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("Id").HasColumnType("INT").UseIdentityColumn();
            builder.Property(u => u.Nome).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(u => u.Email).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(u => u.Telefone).HasColumnType("VARCHAR(20)").IsRequired();
            builder.Property(u => u.IdDDD).HasColumnType("INT").IsRequired();
            builder.HasOne(u => u.Ddd)
                            .WithMany(d => d.Contatos)
                            .HasForeignKey(c => c.IdDDD);

        }
    }
}
