
using B3.Domain.Interfaces;
using B3.Domain.Models;
using B3.Domain.ViewModels;


namespace B3.Application.Services
{
    public class TituloService : ITituloService
    {
        private readonly ITituloRepository _tituloRepository;
        private readonly IDescontoImpostoRendaService _impostoService;
        public TituloService(ITituloRepository tituloRepository, IDescontoImpostoRendaService impostoService)
        {

            _tituloRepository = tituloRepository;
            _impostoService = impostoService;
        }

        public async Task<Titulo> GetTituloById(Guid id)
        {
            return await _tituloRepository.GetTituloById(id);
        }

        public async Task<List<Titulo>> GetTitulos()
        {
            return await _tituloRepository.GetTitulos();
        }

        public async Task<SimulacaoTituloViewModel> GetSimularTitulo(Guid idTitulo, float valorInicial, float valorAporteMensal, int qtdeMesesInvestimento)
        {
            if(valorInicial <= 0)
            {
                throw new Exception("Valor Inicial deve ser maior que zero.");
            }

            if(qtdeMesesInvestimento <= 0)
            {
                throw new Exception("Quantidade de meses investidos deve ser maior que zero.");
            }

            var simulacao = await CalcularSimulacaoTitulo(idTitulo, valorInicial, valorAporteMensal, qtdeMesesInvestimento);

            simulacao.ValorDescontoImpostoRenda = await _impostoService.CalcularDescontoIR(simulacao.ValorRendimento, qtdeMesesInvestimento);


            simulacao.ValorTotalLiquido = simulacao.ValorTotalBruto - simulacao.ValorDescontoImpostoRenda;
            

            return simulacao;
        }

        public async Task<SimulacaoTituloViewModel> CalcularSimulacaoTitulo(Guid id, float valorInicial, float valorAporteMensal, int qtdeMesesInvestimento)
        {
            
            float valorTaxaCalculada;
            float valorTotal = valorInicial;

            SimulacaoTituloViewModel simulacao = new SimulacaoTituloViewModel();    

            //Pegar Taxas do Titulo
            var titulo = await _tituloRepository.GetTituloById(id);

            valorTaxaCalculada =  (titulo.PosFixado ? titulo.Indexador.TaxaAtual/100 * titulo.TaxaRendimento/100: titulo.TaxaRendimento/100);
            valorTaxaCalculada = (float)Math.Round(valorTaxaCalculada, 4);
            

            //Calculando o juro composto mes a mes
            for (int mes = 1; mes <= qtdeMesesInvestimento; mes++)
            {
               valorTotal = (valorTotal + valorAporteMensal) * (1 + valorTaxaCalculada);                
            }

            simulacao.ValorTotalBruto = valorTotal;
            simulacao.ValorTotalInvestido = valorInicial + (valorAporteMensal * qtdeMesesInvestimento);
            simulacao.ValorRendimento = simulacao.ValorTotalBruto - simulacao.ValorTotalInvestido;

            return simulacao;
        }

        public async Task<List<Titulo>> GetTitulosRendaFixa()
        {
            return await _tituloRepository.GetTitulosRendaFixa();
        }
    }
}
