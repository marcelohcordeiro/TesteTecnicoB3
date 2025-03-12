
using B3.Application.Exceptions;
using B3.Domain.Interfaces;
using B3.Domain.Models;
using B3.Domain.ViewModels;


namespace B3.Application.Services
{
    public class TituloService : ITituloService
    {
        private readonly ITituloRepository _tituloRepository;
        private readonly IDescontoImpostoRendaService _impostoService;
        private readonly int MaximoQtdeMeses = 1200;
        public readonly int MinimoQtdeMeses = 1;
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

        public async Task<SimulacaoTituloViewModel> GetSimularTitulo(Guid idTitulo, decimal valorInicial, int qtdeMesesInvestimento)
        {
            if (valorInicial == 0)
                throw new MinimumValueException("Valor Inicial deve ser maior que zero");

            if (qtdeMesesInvestimento < 1)
                throw new MinimumValueException("Quantidade de meses investidos deve ser no minimo igual a um.");


            var simulacao = await CalcularSimulacaoTitulo(idTitulo, valorInicial, qtdeMesesInvestimento);

            simulacao.ValorDescontoImpostoRenda = await _impostoService.CalcularDescontoIR(simulacao.ValorRendimento, qtdeMesesInvestimento);


            simulacao.ValorTotalLiquido = simulacao.ValorTotalBruto - simulacao.ValorDescontoImpostoRenda;
            

            return simulacao;
        }

        public async Task<SimulacaoTituloViewModel> CalcularSimulacaoTitulo(Guid idTitulo, decimal valorInicial, int qtdeMesesInvestimento)
        {

            decimal valorTaxaCalculada;
            decimal valorTotal = valorInicial;

            /*Ajuste para limitar o looping - SonarQube Issues*/
            if(MinimoQtdeMeses > qtdeMesesInvestimento)
            {
                qtdeMesesInvestimento = MinimoQtdeMeses;
            } 
            else if(qtdeMesesInvestimento > MaximoQtdeMeses)
            {   
                qtdeMesesInvestimento = MaximoQtdeMeses;
            }

            SimulacaoTituloViewModel simulacao = new SimulacaoTituloViewModel();    

            //Pegar Taxas do Titulo
            var titulo = await _tituloRepository.GetTituloById(idTitulo);

            valorTaxaCalculada =  (titulo.PosFixado ? titulo.Indexador!.TaxaAtual/100 * titulo.TaxaRendimento/100: titulo.TaxaRendimento/100);       
                        

            //Calculando o juro composto mes a mes
            for (int mes = 1; mes <= qtdeMesesInvestimento; mes++)
            {
               valorTotal = valorTotal  * (1 + valorTaxaCalculada);                
            }

            simulacao.ValorTotalBruto = valorTotal;
            simulacao.ValorTotalInvestido = valorInicial;
            simulacao.ValorRendimento = simulacao.ValorTotalBruto - simulacao.ValorTotalInvestido;

            return simulacao;
        }

        public async Task<List<Titulo>> GetTitulosRendaFixa()
        {
            return await _tituloRepository.GetTitulosRendaFixa();
        }
    }
}
