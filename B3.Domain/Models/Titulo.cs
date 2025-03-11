using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace B3.Domain.Models
{
    public class Titulo
    {
        public Titulo()
        {
            
        }

        [Key]
        public Guid IdTitulo { get; set; }
        [Required]
        [MaxLength(250)]
        public string? NomeTitulo { get; set; }
        [Required]
        public int IdTipoTitulo { get; set; }
        public TipoTitulo? TipoTitulo { get; set; }

        [Required]
        public bool PosFixado { get; set; }
        
        public int IdIndexador{ get; set; }
        
        public Indexador? Indexador { get; set; }
      
        public decimal TaxaRendimento { get; set; }
        
        
    }
}
