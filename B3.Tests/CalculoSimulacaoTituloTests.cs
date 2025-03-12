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
        public async Task ShouldReturnValorBruto()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);

            //Act + Assert  - Mes 1
            var simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 1);
            Assert.Equal(Math.Round((decimal)1009.72, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalBruto, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 2
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 2);
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
        public async Task ShouldReturnValorLiquido()
        {
            //AAA
            //Arrange
            AppDbContext appDbContext = await getDatabaseDbContext();
            TituloRepository tituloRepository = new TituloRepository(appDbContext);
            DescontoImpostoRendaRepository descontoImpostoRendaRepository = new DescontoImpostoRendaRepository(appDbContext);
            DescontoImpostoRendaService descontoImpostoRendaService = new DescontoImpostoRendaService(descontoImpostoRendaRepository);
            TituloService tituloService = new TituloService(tituloRepository, descontoImpostoRendaService);

            //Act + Assert  - Mes 1
            var simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 1);   
            Assert.Equal(Math.Round((decimal)1007.53, 2, MidpointRounding.ToZero), Math.Round(simulacao.ValorTotalLiquido, 2, MidpointRounding.ToZero), 2);

            //Act + Assert  - Mes 2
            simulacao = await tituloService.GetSimularTitulo(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), 1000, 2);            
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
            Assert.Equal("Quantidade de meses investidos deve ser no minimo igual a um.", exception.Message);

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