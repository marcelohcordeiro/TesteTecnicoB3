using B3.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace B3.Infra.Data.Configuration
{
    public class IndexadorConfiguration : IEntityTypeConfiguration<Indexador>
    {
        public void Configure(EntityTypeBuilder<Indexador> builder)
        {
            builder.ToTable("IndiceTipo");

            builder.Property(x => x.IdIndexador).HasColumnName("IdIndiceTipo").IsRequired();
            builder.Property(x => x.Descricao).IsRequired();
            builder.Property(x => x.TaxaAtual).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.DataUltimaAtualizacao);

            builder.HasKey(x => x.IdIndexador);

            builder.HasMany(f => f.Titulos).WithOne(x => x.Indexador).HasForeignKey(f => f.IdIndexador);
           
        }
    }
}
