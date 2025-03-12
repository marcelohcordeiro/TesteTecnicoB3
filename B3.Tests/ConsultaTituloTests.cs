using B3.Application.Services;
using B3.Domain.Models;
using B3.Infra.Data.Context;
using B3.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3.Tests
{
    public class ConsultaTituloTests
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
                databaseContext.Indexadores!.Add(new Indexador { IdIndexador = 1, Descricao = "SELIC", TaxaAtual = (decimal)0.9, DataUltimaAtualizacao = new DateTime(2025, 03, 11, 00, 00, 00, DateTimeKind.Unspecified) });
                databaseContext.Indexadores!.Add(new Indexador { IdIndexador = 2, Descricao = "CDI", TaxaAtual = (decimal)0.9, DataUltimaAtualizacao = new DateTime(2025, 03, 11, 00, 00, 00, DateTimeKind.Unspecified) });
                databaseContext.Indexadores!.Add(new Indexador { IdIndexador = 3, Descricao = "IPCA", TaxaAtual = (decimal)0.9, DataUltimaAtualizacao = new DateTime(2025, 03, 11, 00, 00, 00, DateTimeKind.Unspecified) });

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
                databaseContext.Titulos!.Add(new Titulo { IdTitulo = new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), NomeTitulo = "Tesouro Pré-Fixado Teste", IdTipoTitulo = 1, PosFixado = false, IdIndexador = 2, TaxaRendimento = 1 });
                databaseContext.Titulos!.Add(new Titulo { IdTitulo = new Guid("4baa7a4a-6ec3-4e52-b011-a82824830686"), NomeTitulo = "FII Teste", IdTipoTitulo = 4, PosFixado = false, IdIndexador = 2, TaxaRendimento = 1 });

                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

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
            Assert.Equal(new Guid("4baa7a4a-6ec3-4e52-b011-a82824830686"), titulos[0].IdTitulo);
            Assert.Equal(new Guid("34d7d18b-27f8-4ed7-bfa7-5aa976d7a8e3"), titulos[1].IdTitulo);            
            Assert.Equal(new Guid("C2CCD2C3-2A9E-45A9-B407-3FDFE5D95FED"), titulos[2].IdTitulo);


        }


        



    }
}
