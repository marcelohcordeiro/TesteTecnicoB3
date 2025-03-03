using B3.Domain.Models;

namespace B3.Domain.Interfaces
{
    public interface IDescontoImpostoRendaService
    {
        Task<List<DescontoImpostoRenda>> GetDescontoImpostoRendas();
        Task<float> CalcularDescontoIR(float valorRendimento, int qtdeMesesInvestimento);

    }
}
