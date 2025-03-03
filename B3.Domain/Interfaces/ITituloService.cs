using B3.Domain.Models;
using B3.Domain.ViewModels;

namespace B3.Domain.Interfaces
{
    public interface ITituloService
    {
        Task<List<Titulo>> GetTitulos();
        Task<Titulo> GetTituloById(Guid id);
        Task<SimulacaoTituloViewModel> GetSimularTitulo(Guid id, float valorInicial, float valorAporteMensal, int qtdeMesesInvestimento);
        Task<SimulacaoTituloViewModel> CalcularSimulacaoTitulo(Guid id, float valorInicial, float valorAporteMensal, int qtdeMesesInvestimento);
        Task<List<Titulo>> GetTitulosRendaFixa();
    }
}
