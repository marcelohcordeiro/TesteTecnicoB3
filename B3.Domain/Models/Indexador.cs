using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace B3.Domain.Models
{
    public class Indexador
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdIndexador { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Descricao { get; set; }

        [Required]
        public float TaxaAtual {get;set;}
        public DateTime DataUltimaAtualizacao { get; set; }
        
        public ICollection<Titulo> Titulos { get; set; }
    }
}
