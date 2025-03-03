using B3.Domain.Interfaces;
using B3.Domain.Models;

namespace B3.Application.Services
{
    public class DescontoImpostoRendaService : IDescontoImpostoRendaService
    {
        private readonly IDescontoImpostoRendaRepository _impostoRepository;
        public DescontoImpostoRendaService(IDescontoImpostoRendaRepository impostoRespository)
        {
            _impostoRepository = impostoRespository;
        }
        public async Task<List<DescontoImpostoRenda>> GetDescontoImpostoRendas()
        {
            
            var x = await _impostoRepository.GetDescontoImpostoRendas();
            return x;
        }

        public async Task<float> CalcularDescontoIR(float valorRendimento, int qtdeMesesInvestimento)
        {
            var descontoImpostoRenda = await _impostoRepository.GetDescontoImpostoRendaByMonths(qtdeMesesInvestimento);

            return valorRendimento * (descontoImpostoRenda.PercentualDesconto / 100);
           

        }
    }
}
