using B3.Application.Services;
using B3.Domain.Models;
using B3.Infra.Data.Context;
using B3.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace B3.Tests
{
    public class CalculoSimulacaoTituloTests
    {
        private async Task<AppDbContext> getDatabaseDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();


            if (!await databaseContext.Indexadores!.AnyAsync())
            {
                databaseContext.Indexadores!.Add(new Indexador { IdIndexador = 1, Descricao = "SELIC", TaxaAtual = (decimal)0.9, DataUltimaAtualizacao = DateTime.Now });
                databaseContext.Indexadores!.Add(new Indexador { IdIndexador = 2, Descricao = "CDI", TaxaAtual = (decimal)0.9, DataUltimaAtualizacao = DateTime.Now });
                databaseContext.Indexadores!.Add(new Indexador { IdIndexador = 3, Descricao = "IPCA", TaxaAtual = (decimal)0.9, DataUltimaAtualizacao = DateTime.Now });

                await databaseContext.SaveChangesAsync();
            }


            if (!await databaseContext.TipoTitulos!.AnyAsync())
            {
                databaseContext.TipoTitulos!.Add(new TipoTitulo { IdTipoTitulo = 1, Descricao = "Tesouro Direto", RendaFixa = true });
                databaseContext.TipoTitulos!.Add(new TipoTitulo { IdTipoTitulo = 2, Descricao = "Tesouro IPCA", RendaFixa = true });
                databaseContext.TipoTitulos!.Add(new TipoTitulo { IdTipoTitulo = 3, Descricao = "CDB", RendaFixa = true });

                await databaseContext.SaveChangesAsync();
            }



            if (!await databaseContext.descontoImpostoRendas!.AnyAsync())
            {
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = Guid.NewGuid(), Descricao = "'Até 06 meses'", QtdeMesesInicio = 0, QtdeMesesFim = 6, PercentualDesconto = (decimal)22.5 });
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = Guid.NewGuid(), Descricao = "'Até 12 meses'", QtdeMesesInicio = 7, QtdeMesesFim = 12, PercentualDesconto = (decimal)20 });
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = Guid.NewGuid(), Descricao = "'Até 24 meses'", QtdeMesesInicio = 13, QtdeMesesFim = 24, PercentualDesconto = (decimal)17.5 });
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = Guid.NewGuid(), Descricao = "'Acima de 24 meses'", QtdeMesesInicio = 25, QtdeMesesFim = null, PercentualDesconto = (decimal)15 });



                await databaseContext.SaveChangesAsync();
            }


            // Adiciona dados iniciais para testar
            if (!await databaseContext.Titulos!.AnyAsync())
            {


                databaseContext.Titulos!.Add(new Titulo { IdTitulo = new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), NomeTitulo = "CDB Teste", IdTipoTitulo = 3, PosFixado = true, IdIndexador = 2, TaxaRendimento = 108 });

                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        [Fact]
        public async Task ShouldReturnValorLiquido()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);

            //Act


            var simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 2);
            

            //Assert
            Assert.Equal((decimal)1621.90, simulacao.ValorTotalLiquido, 2);

        }

        [Fact]
        public async Task ShouldReturnErrorWhenValorInicialEqualsZero()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);

            //Act


            Exception exception = await Record.ExceptionAsync(() => tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 0, 2));

            //Assert
            Assert.Equal("Valor Inicial deve ser maior que zero.", exception.Message);

        }

        [Fact]
        public async Task ShouldReturnErrorWhenQtdeMesesEqualsZero()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);

            //Act


            Exception exception = await Record.ExceptionAsync(() => tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 0));

            //Assert
            Assert.Equal("Quantidade de meses investidos deve ser maior que zero.", exception.Message);

        }

        [Fact]
        public async Task ShouldReturnImpostoRendaPercentualBasedOnQtdeMeses()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);



            int qtdMes = 1;
            decimal percentual;

            while (qtdMes <= 30)
            {
                if (qtdMes <= 6)
                {
                    percentual = (decimal)22.5;
                }
                else if (qtdMes <= 12)
                {
                    percentual = (decimal)20;

                }
                else if (qtdMes <= 24)
                {
                    percentual = (decimal)17.5;

                }
                else
                {
                    percentual = (decimal)15;
                }

                //Act
                var simulacaoMes = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, qtdMes);

                //Assert
                Assert.Equal(percentual, (simulacaoMes.ValorDescontoImpostoRenda * 100 / simulacaoMes.ValorRendimento), 2);

                qtdMes++;
            }


        }


    }
}