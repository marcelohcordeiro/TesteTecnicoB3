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
        private ConfigureTestDbContext _testDbContext;
        /*private static async Task<AppDbContext> getDatabaseDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

            var x = new DbContextOptionsBuilder<AppDbContext>().IsConfigured;

            var databaseContext = new AppDbContext(options);            
            await databaseContext.Database.EnsureCreatedAsync();

            x = new DbContextOptionsBuilder<AppDbContext>().IsConfigured;



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
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = new Guid("9c75f7e2-761d-424a-8b54-64cebc73cbb2"), Descricao = "'Até 06 meses'", QtdeMesesInicio = 0, QtdeMesesFim = 6, PercentualDesconto = (decimal)22.5 });
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = new Guid("6f6885df-7235-4243-8359-3baa445d7c10"), Descricao = "'Até 12 meses'", QtdeMesesInicio = 7, QtdeMesesFim = 12, PercentualDesconto = (decimal)20 });
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = new Guid("73c1a667-a080-4a71-955e-1ed34ee4c3f4"), Descricao = "'Até 24 meses'", QtdeMesesInicio = 13, QtdeMesesFim = 24, PercentualDesconto = (decimal)17.5 });
                databaseContext.descontoImpostoRendas!.Add(new DescontoImpostoRenda { IdDescontoImpostoRenda = new Guid("8ce752b2-1469-41c1-96fa-a7488be64744"), Descricao = "'Acima de 24 meses'", QtdeMesesInicio = 25, QtdeMesesFim = null, PercentualDesconto = (decimal)15 });

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
        }*/

       


        



    }
}
