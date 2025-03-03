using B3.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace B3.Infra.Data.Configuration
{
    public class DescontoImpostoRendaConfiguration : IEntityTypeConfiguration<DescontoImpostoRenda>
    {
        public void Configure(EntityTypeBuilder<DescontoImpostoRenda> builder)
        {
            builder.ToTable("DescontoImpostoRenda");

            builder.Property(x => x.IdDescontoImpostoRenda).IsRequired();
            builder.Property(x => x.Descricao).HasMaxLength(100).IsRequired();
            builder.Property(x => x.QtdeMesesInicio).IsRequired();
            builder.Property(x => x.QtdeMesesFim);            
            builder.Property(x => x.PercentualDesconto).HasColumnType("decimal(18,2)").IsRequired();         
            

            builder.HasKey(x => x.IdDescontoImpostoRenda);

        }
    }
}
