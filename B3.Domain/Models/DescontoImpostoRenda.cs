using System.ComponentModel.DataAnnotations;

namespace B3.Domain.Models
{
    public class DescontoImpostoRenda
    {
        [Key]
        [Required]
        public Guid IdDescontoImpostoRenda { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Descricao { get; set; }
        [Required]
        public int QtdeMesesInicio { get; set; }
        
        public int? QtdeMesesFim { get; set; }
        [Required]
        public float PercentualDesconto { get; set; }
    }
}
