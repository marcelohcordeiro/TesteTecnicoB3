using B3.Domain.Models;
using B3.Domain.ViewModels;

namespace B3.Domain.Interfaces
{
    public interface ITituloService
    {
        Task<List<Titulo>> GetTitulos();
        Task<Titulo> GetTituloById(Guid id);
        Task<SimulacaoTituloViewModel> GetSimularTitulo(Guid idTitulo, decimal valorInicial, int qtdeMesesInvestimento);
        Task<SimulacaoTituloViewModel> CalcularSimulacaoTitulo(Guid idTitulo, decimal valorInicial, int qtdeMesesInvestimento);
        Task<List<Titulo>> GetTitulosRendaFixa();
    }
}
