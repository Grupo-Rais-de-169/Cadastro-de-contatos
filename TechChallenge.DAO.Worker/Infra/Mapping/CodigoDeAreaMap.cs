using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TechChallenge.DAO.Worker.Entities;

namespace TechChallenge.DAO.Worker.Infra.Mapping
{
    public class CodigoDeAreaMap : IEntityTypeConfiguration<CodigoDeArea>
    {
        public void Configure(EntityTypeBuilder<CodigoDeArea> builder)
        {
            builder.ToTable("CodigosDeArea");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("Id").HasColumnType("INT").UseIdentityColumn();
            builder.Property(u => u.Regiao).HasColumnType("VARCHAR(100)").IsRequired();
        }
    }
}
