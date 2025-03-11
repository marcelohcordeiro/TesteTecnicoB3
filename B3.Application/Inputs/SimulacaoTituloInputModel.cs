using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3.Application.Inputs
{
    public class SimulacaoTituloInputModel
    {

        public Guid IdTitulo { get; set; }
        public decimal ValorInicial { get; set; }
        public int QuantidadeMesesInvestimento { get; set; }

    }
}
