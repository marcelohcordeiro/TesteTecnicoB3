using B3.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace B3.Infra.Data.Configuration
{
    public class TipoTituloConfiguration : IEntityTypeConfiguration<TipoTitulo>
    {
        public void Configure(EntityTypeBuilder<TipoTitulo> builder)
        {
            builder.ToTable("TituloTipo");

            builder.Property(x => x.IdTipoTitulo).IsRequired();
            builder.Property(x => x.Descricao).IsRequired();
            builder.Property(x => x.RendaFixa).HasDefaultValue(true).IsRequired();


            builder.HasKey(x => x.IdTipoTitulo);
            builder.HasMany(x => x.Titulos).WithOne(f => f.TipoTitulo).HasForeignKey(f => f.IdTipoTitulo);

        }
    }
}
