using System.Diagnostics.Metrics;
using B3.Application.Services;
using B3.Domain.Models;
using B3.Infra.Data.Context;
using B3.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace B3.Tests
{
    public class CalculoSimulacaoTituloTests
    {
        private static async Task<AppDbContext> getDatabaseDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

            

            var databaseContext = new AppDbContext(options);           
            await databaseContext.Database.EnsureCreatedAsync();

            


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
                databaseContext.TipoTitulos!.Add(new TipoTitulo { IdTipoTitulo = 4, Descricao = "FII", RendaFixa = false });

                await databaseContext.SaveChangesAsync();
            }



            if (!await databaseContext.descontoImpostoRendas!.AnyAsync())
            {
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = new Guid("08207917-5781-49bf-a06d-99d6d2d068d9"), Descricao = "Até 06 meses", QtdeMesesInicio = 0, QtdeMesesFim = 6, PercentualDesconto = (decimal)22.5 });
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = new Guid("eca52168-16a6-475b-ba15-b8bbe20568c2"), Descricao = "Até 12 meses", QtdeMesesInicio = 7, QtdeMesesFim = 12, PercentualDesconto = (decimal)20 });
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = new Guid("8ef1eec5-601b-467a-8d9d-e6f61d603520"), Descricao = "Até 24 meses", QtdeMesesInicio = 13, QtdeMesesFim = 24, PercentualDesconto = (decimal)17.5 });
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = new Guid("aee56080-6375-437e-b815-4df776d2ac7f"), Descricao = "Acima de 24 meses", QtdeMesesInicio = 25, QtdeMesesFim = null, PercentualDesconto = (decimal)15 });

                await databaseContext.SaveChangesAsync();
            }


            // Adiciona dados iniciais para testar
            if (!await databaseContext.Titulos!.AnyAsync())
            {


                databaseContext.Titulos!.Add(new Titulo { IdTitulo = new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), NomeTitulo = "CDB Teste", IdTipoTitulo = 3, PosFixado = true, IdIndexador = 2, TaxaRendimento = 108 });
                databaseContext.Titulos!.Add(new Titulo { IdTitulo = new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), NomeTitulo = "Tesouro Pré-Fixado Teste", IdTipoTitulo = 1, PosFixado = false, IdIndexador = 2, TaxaRendimento = 1 });
                databaseContext.Titulos!.Add(new Titulo { IdTitulo = new Guid("4baa7a4a-6ec3-4e52-b011-a82824830686"), NomeTitulo = "FII Teste", IdTipoTitulo = 4, PosFixado = false, IdIndexador = 2, TaxaRendimento = 1 });

                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
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
            Assert.Equal("Valor Inicial deve ser maior que zero", exception.Message);

        }

        [Fact]
        public async Task ShouldReturnErrorWhenQtdeMesesLowerThanOne()
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
            Assert.Equal("Quantidade de meses investidos deve ser maior que um", exception.Message);

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



            int qtdMes = 2;
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

        [Fact]
        public async Task ShouldReturnSimulacaoWithMaximum1200MonthsWhenItsExcedeed()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);

            int maxQtdeMesesInvestimento = 1200;
            int qtdeMesInvestimento = 1250;
            

            //Act
            var simulacaoMesMax = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, maxQtdeMesesInvestimento);
            var simulacaoMes = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, qtdeMesInvestimento);

            //Assert
            Assert.Equal(simulacaoMesMax.ValorTotalBruto, simulacaoMes.ValorTotalBruto);
            Assert.Equal(simulacaoMesMax.ValorTotalInvestido, simulacaoMes.ValorTotalInvestido);
            Assert.Equal(simulacaoMesMax.ValorRendimento, simulacaoMes.ValorRendimento);
            Assert.Equal(simulacaoMesMax.ValorDescontoImpostoRenda, simulacaoMes.ValorDescontoImpostoRenda);
            Assert.Equal(simulacaoMesMax.ValorTotalLiquido, simulacaoMes.ValorTotalLiquido);

        }

        #region PosFixado
        [Fact]
        public async Task ShouldReturnValorBrutoPosFixado()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);
            

            //Act + Assert  - Mes 2
            var simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 2);
            Assert.Equal(Math.Round((decimal)1019.53, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 3
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 3);
            Assert.Equal(Math.Round((decimal)1029.44, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 4
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 4);
            Assert.Equal(Math.Round((decimal)1039.45, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 5
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 5);
            Assert.Equal(Math.Round((decimal)1049.55, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 6
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 6);
            Assert.Equal(Math.Round((decimal)1059.75, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 7
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 7);
            Assert.Equal(Math.Round((decimal)1070.05, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 8
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 8);
            Assert.Equal(Math.Round((decimal)1080.45, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 9
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 9);
            Assert.Equal(Math.Round((decimal)1090.95, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 10
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 10);
            Assert.Equal(Math.Round((decimal)1101.56, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 11
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 11);
            Assert.Equal(Math.Round((decimal)1112.27, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 12
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 12);
            Assert.Equal(Math.Round((decimal)1123.08, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 13
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 13);
            Assert.Equal(Math.Round((decimal)1133.99, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 14
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 14);
            Assert.Equal(Math.Round((decimal)1145.02, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 15
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 15);
            Assert.Equal(Math.Round((decimal)1156.15, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 16
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 16);
            Assert.Equal(Math.Round((decimal)1167.38, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 17
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 17);
            Assert.Equal(Math.Round((decimal)1178.73, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 18
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 18);
            Assert.Equal(Math.Round((decimal)1190.19, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 19
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 19);
            Assert.Equal(Math.Round((decimal)1201.76, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 20
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 20);
            Assert.Equal(Math.Round((decimal)1213.44, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 21
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 21);
            Assert.Equal(Math.Round((decimal)1225.23, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 22
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 22);
            Assert.Equal(Math.Round((decimal)1237.14, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 23
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 23);
            Assert.Equal(Math.Round((decimal)1249.17, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 24
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 24);
            Assert.Equal(Math.Round((decimal)1261.31, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 25
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 25);
            Assert.Equal(Math.Round((decimal)1273.57, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 26
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 26);
            Assert.Equal(Math.Round((decimal)1285.95, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 27
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 27);
            Assert.Equal(Math.Round((decimal)1298.45, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 28
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 28);
            Assert.Equal(Math.Round((decimal)1311.07, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 29
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 29);
            Assert.Equal(Math.Round((decimal)1323.81, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 30
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 30);
            Assert.Equal(Math.Round((decimal)1336.68, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

        }

        [Fact]
        public async Task ShouldReturnValorLiquidoPosFixado()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);

            

            //Act + Assert  - Mes 2
            var simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 2);            
            Assert.Equal(Math.Round((decimal)1015.13, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 3
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 3);
            Assert.Equal(Math.Round((decimal)1022.81, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 4
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 4);
            Assert.Equal(Math.Round((decimal)1030.57, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 5
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 5);
            Assert.Equal(Math.Round((decimal)1038.40, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 6
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 6);
            Assert.Equal(Math.Round((decimal)1046.31, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 7
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 7);
            Assert.Equal(Math.Round((decimal)1056.04, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 8
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 8);
            Assert.Equal(Math.Round((decimal)1064.36, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 9
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 9);
            Assert.Equal(Math.Round((decimal)1072.76, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 10
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 10);
            Assert.Equal(Math.Round((decimal)1081.25, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 11
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 11);
            Assert.Equal(Math.Round((decimal)1089.81, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 12
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 12);
            Assert.Equal(Math.Round((decimal)1098.46, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 13
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 13);
            Assert.Equal(Math.Round((decimal)1110.54, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 14
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 14);
            Assert.Equal(Math.Round((decimal)1119.64, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 15
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 15);
            Assert.Equal(Math.Round((decimal)1128.82, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 16
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 16);
            Assert.Equal(Math.Round((decimal)1138.09, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 17
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 17);
            Assert.Equal(Math.Round((decimal)1147.45, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 18
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 18);
            Assert.Equal(Math.Round((decimal)1156.90, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 19
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 19);
            Assert.Equal(Math.Round((decimal)1166.45, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 20
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 20);
            Assert.Equal(Math.Round((decimal)1176.08, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 21
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 21);
            Assert.Equal(Math.Round((decimal)1185.82, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 22
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 22);
            Assert.Equal(Math.Round((decimal)1195.64, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 23
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 23);
            Assert.Equal(Math.Round((decimal)1205.56, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 24
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 24);
            Assert.Equal(Math.Round((decimal)1215.58, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 25
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 25);
            Assert.Equal(Math.Round((decimal)1232.53, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 26
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 26);
            Assert.Equal(Math.Round((decimal)1243.05, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 27
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 27);
            Assert.Equal(Math.Round((decimal)1253.68, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 28
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 28);
            Assert.Equal(Math.Round((decimal)1264.41, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 29
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 29);
            Assert.Equal(Math.Round((decimal)1275.24, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 30
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 30);
            Assert.Equal(Math.Round((decimal)1286.18, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);
        }

        #endregion


        #region PreFixado
        [Fact]
        public async Task ShouldReturnValorBrutoPreFixado()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);

            
            //Act + Assert  - Mes 2
            var simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 2);
            Assert.Equal(Math.Round((decimal)1020.10, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 3
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 3);
            Assert.Equal(Math.Round((decimal)1030.30, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 4
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 4);
            Assert.Equal(Math.Round((decimal)1040.60, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 5
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 5);
            Assert.Equal(Math.Round((decimal)1051.01, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 6
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 6);
            Assert.Equal(Math.Round((decimal)1061.52, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 7
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 7);
            Assert.Equal(Math.Round((decimal)1072.13, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 8
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 8);
            Assert.Equal(Math.Round((decimal)1082.85, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 9
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 9);
            Assert.Equal(Math.Round((decimal)1093.68, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 10
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 10);
            Assert.Equal(Math.Round((decimal)1104.62, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 11
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 11);
            Assert.Equal(Math.Round((decimal)1115.66, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 12
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 12);
            Assert.Equal(Math.Round((decimal)1126.82, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 13
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 13);
            Assert.Equal(Math.Round((decimal)1138.09, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 14
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 14);
            Assert.Equal(Math.Round((decimal)1149.47, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 15
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 15);
            Assert.Equal(Math.Round((decimal)1160.96, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 16
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 16);
            Assert.Equal(Math.Round((decimal)1172.57, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 17
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 17);
            Assert.Equal(Math.Round((decimal)1184.30, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 18
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 18);
            Assert.Equal(Math.Round((decimal)1196.14, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 19
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 19);
            Assert.Equal(Math.Round((decimal)1208.10, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 20
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 20);
            Assert.Equal(Math.Round((decimal)1220.19, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 21
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 21);
            Assert.Equal(Math.Round((decimal)1232.39, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 22
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 22);
            Assert.Equal(Math.Round((decimal)1244.71, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 23
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 23);
            Assert.Equal(Math.Round((decimal)1257.16, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 24
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 24);
            Assert.Equal(Math.Round((decimal)1269.73, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 25
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 25);
            Assert.Equal(Math.Round((decimal)1282.43, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 26
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 26);
            Assert.Equal(Math.Round((decimal)1295.25, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 27
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 27);
            Assert.Equal(Math.Round((decimal)1308.20, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 28
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 28);
            Assert.Equal(Math.Round((decimal)1321.29, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 29
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 29);
            Assert.Equal(Math.Round((decimal)1334.50, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 30
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 30);
            Assert.Equal(Math.Round((decimal)1347.84, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

        }

        [Fact]
        public async Task ShouldReturnValorLiquidoPreFixado()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);

            
            //Act + Assert  - Mes 2
            var simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 2);
            Assert.Equal(Math.Round((decimal)1015.57, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 3
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 3);
            Assert.Equal(Math.Round((decimal)1023.48, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 4
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 4);
            Assert.Equal(Math.Round((decimal)1031.46, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 5
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 5);
            Assert.Equal(Math.Round((decimal)1039.53, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 6
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 6);
            Assert.Equal(Math.Round((decimal)1047.67, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 7
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 7);
            Assert.Equal(Math.Round((decimal)1057.70, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 8
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 8);
            Assert.Equal(Math.Round((decimal)1066.28, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 9
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 9);
            Assert.Equal(Math.Round((decimal)1074.94, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 10
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 10);
            Assert.Equal(Math.Round((decimal)1083.69, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 11
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 11);
            Assert.Equal(Math.Round((decimal)1092.53, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 12
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 12);
            Assert.Equal(Math.Round((decimal)1101.46, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 13
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 13);
            Assert.Equal(Math.Round((decimal)1113.92, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 14
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 14);
            Assert.Equal(Math.Round((decimal)1123.31, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 15
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 15);
            Assert.Equal(Math.Round((decimal)1132.79, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 16
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 16);
            Assert.Equal(Math.Round((decimal)1142.37, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 17
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 17);
            Assert.Equal(Math.Round((decimal)1152.05, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 18
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 18);
            Assert.Equal(Math.Round((decimal)1161.82, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 19
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 19);
            Assert.Equal(Math.Round((decimal)1171.68, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 20
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 20);
            Assert.Equal(Math.Round((decimal)1181.65, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 21
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 21);
            Assert.Equal(Math.Round((decimal)1191.72, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 22
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 22);
            Assert.Equal(Math.Round((decimal)1201.89, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 23
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 23);
            Assert.Equal(Math.Round((decimal)1212.15, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 24
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 24);
            Assert.Equal(Math.Round((decimal)1222.53, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 25
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 25);
            Assert.Equal(Math.Round((decimal)1240.06, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 26
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 26);
            Assert.Equal(Math.Round((decimal)1250.96, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 27
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 27);
            Assert.Equal(Math.Round((decimal)1261.97, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 28
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 28);
            Assert.Equal(Math.Round((decimal)1273.09, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 29
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 29);
            Assert.Equal(Math.Round((decimal)1284.32, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 30
            simulacao = await tituloService.GetSimularTitulo(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), 1000, 30);
            Assert.Equal(Math.Round((decimal)1295.67, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);
        }

        #endregion

        #region Consultas
        [Fact]
        public async Task ShouldReturnTituloById()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);


            //Act 
            var retTituloFII = await tituloService.GetTituloById(new Guid("4baa7a4a-6ec3-4e52-b011-a82824830686"));
            Titulo tituloFII = new Titulo { IdTitulo = new Guid("4baa7a4a-6ec3-4e52-b011-a82824830686"), NomeTitulo = "FII Teste", IdTipoTitulo = 4, PosFixado = false, IdIndexador = 2, TaxaRendimento = 1 };

            //Assert
            Assert.Equal(tituloFII.IdTitulo, retTituloFII.IdTitulo);
            Assert.Equal(tituloFII.NomeTitulo, retTituloFII.NomeTitulo);
            Assert.Equal(tituloFII.TaxaRendimento, retTituloFII.TaxaRendimento);
            Assert.Equal(tituloFII.IdIndexador, retTituloFII.IdIndexador);
            Assert.Equal(tituloFII.IdTitulo, retTituloFII.IdTitulo);
            Assert.Equal(tituloFII.PosFixado, retTituloFII.PosFixado);

            //Act 
            var retTesouro = await tituloService.GetTituloById(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"));
            Titulo tituloTesouro = new Titulo { IdTitulo = new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), NomeTitulo = "Tesouro Pré-Fixado Teste", IdTipoTitulo = 1, PosFixado = false, IdIndexador = 2, TaxaRendimento = 1 };
            //Assert
            Assert.Equal(tituloTesouro.IdTitulo, retTesouro.IdTitulo);
            Assert.Equal(tituloTesouro.NomeTitulo, retTesouro.NomeTitulo);
            Assert.Equal(tituloTesouro.TaxaRendimento, retTesouro.TaxaRendimento);
            Assert.Equal(tituloTesouro.IdIndexador, retTesouro.IdIndexador);
            Assert.Equal(tituloTesouro.IdTitulo, retTesouro.IdTitulo);
            Assert.Equal(tituloTesouro.PosFixado, retTesouro.PosFixado);


            //Act 
            var retCDB = await tituloService.GetTituloById(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"));
            Titulo tituloCDB = new Titulo { IdTitulo = new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), NomeTitulo = "CDB Teste", IdTipoTitulo = 3, PosFixado = true, IdIndexador = 2, TaxaRendimento = 108 };
            //Assert
            Assert.Equal(tituloCDB.IdTitulo, retCDB.IdTitulo);
            Assert.Equal(tituloCDB.NomeTitulo, retCDB.NomeTitulo);
            Assert.Equal(tituloCDB.TaxaRendimento, retCDB.TaxaRendimento);
            Assert.Equal(tituloCDB.IdIndexador, retCDB.IdIndexador);
            Assert.Equal(tituloCDB.TaxaRendimento, retCDB.TaxaRendimento);
            Assert.Equal(tituloCDB.IdTitulo, retCDB.IdTitulo);
            Assert.Equal(tituloCDB.PosFixado, retCDB.PosFixado);

        }

        [Fact]
        public async Task ShouldReturnListTitulosRendaFixa()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);


            //Act
            var titulos = await tituloService.GetTitulosRendaFixa();

            //Assert
            Assert.Equal(2, titulos.Count);
            Assert.Equal(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), titulos[0].IdTitulo);
            Assert.Equal(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), titulos[1].IdTitulo);


        }

        [Fact]
        public async Task ShouldReturnListAllTitulos()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);


            //Act
            var titulos = await tituloService.GetTitulos();

            //Assert
            Assert.Equal(3, titulos.Count);
            Assert.Equal(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), titulos[0].IdTitulo);
            Assert.Equal(new Guid("4baa7a4a-6ec3-4e52-b011-a82824830686"), titulos[1].IdTitulo);
            Assert.Equal(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), titulos[2].IdTitulo);


        }

        [Fact]
        public async Task ShouldReturnListEntireDescontoImpostoRenda()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();            
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);          


            //Act
            var tabelaIR = await descontoImpostoRendaService.GetDescontoImpostoRendas();
            

            //Assert
            Assert.Equal(4, tabelaIR.Count);
            Assert.Equal("Até 06 meses", tabelaIR[0].Descricao);
            Assert.Equal((decimal)22.5, tabelaIR[0].PercentualDesconto);
            Assert.Equal("Até 12 meses", tabelaIR[1].Descricao);
            Assert.Equal((decimal)20, tabelaIR[1].PercentualDesconto);
            Assert.Equal("Até 24 meses", tabelaIR[2].Descricao);
            Assert.Equal((decimal)17.5, tabelaIR[2].PercentualDesconto);
            Assert.Equal("Acima de 24 meses", tabelaIR[3].Descricao);
            Assert.Equal((decimal)15, tabelaIR[3].PercentualDesconto);


        }
        #endregion



    }
}