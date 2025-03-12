using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace B3.Domain.Models
{
    public class TipoTitulo
    {
        public TipoTitulo()
        {
            Descricao = "";
        }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTipoTitulo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Descricao { get; set; }

        [Required]        
        public bool RendaFixa { get; set; }


        public ICollection<Titulo>? Titulos { get; set; }

    }
}
