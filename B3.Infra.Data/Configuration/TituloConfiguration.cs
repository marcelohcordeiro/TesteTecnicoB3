using B3.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace B3.Infra.Data.Configuration
{
    public class TituloConfiguration : IEntityTypeConfiguration<Titulo>
    {
        public void Configure(EntityTypeBuilder<Titulo> builder)
        {
            builder.ToTable("Titulo");

            builder.Property(x => x.IdTitulo).IsRequired();
            builder.Property(x => x.NomeTitulo).HasMaxLength(255).IsRequired();
            builder.Property(x => x.IdTipoTitulo).IsRequired();
            builder.Property(x => x.IdIndexador).HasColumnName("IdIndiceTipo").IsRequired();
            builder.Property(x => x.PosFixado).IsRequired();
            builder.Property(x => x.TaxaRendimento).HasColumnType("decimal(18,2)").IsRequired();
           

            builder.HasKey(x => x.IdTitulo);

            builder.Navigation(x => x.Indexador).AutoInclude(false);
        }
    }
}
