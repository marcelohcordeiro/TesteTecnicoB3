using B3.Domain.Models;

namespace B3.Domain.Interfaces
{
    public interface IDescontoImpostoRendaService
    {
        Task<List<DescontoImpostoRenda>> GetDescontoImpostoRendas();
        Task<decimal> CalcularDescontoIR(decimal valorRendimento, int qtdeMesesInvestimento);

    }
}
