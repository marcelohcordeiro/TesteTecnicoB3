using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B3.Infra.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DescontoImpostoRenda",
                columns: table => new
                {
                    IdDescontoImpostoRenda = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QtdeMesesInicio = table.Column<int>(type: "int", nullable: false),
                    QtdeMesesFim = table.Column<int>(type: "int", nullable: true),
                    PercentualDesconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescontoImpostoRenda", x => x.IdDescontoImpostoRenda);
                });

            migrationBuilder.CreateTable(
                name: "IndiceTipo",
                columns: table => new
                {
                    IdIndiceTipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaxaAtual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndiceTipo", x => x.IdIndiceTipo);
                });

            migrationBuilder.CreateTable(
                name: "TituloTipo",
                columns: table => new
                {
                    IdTipoTitulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RendaFixa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TituloTipo", x => x.IdTipoTitulo);
                });

            migrationBuilder.CreateTable(
                name: "Titulo",
                columns: table => new
                {
                    IdTitulo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeTitulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdTipoTitulo = table.Column<int>(type: "int", nullable: false),
                    PosFixado = table.Column<bool>(type: "bit", nullable: false),
                    IdIndiceTipo = table.Column<int>(type: "int", nullable: false),
                    TaxaRendimento = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titulo", x => x.IdTitulo);
                    table.ForeignKey(
                        name: "FK_Titulo_IndiceTipo_IdIndiceTipo",
                        column: x => x.IdIndiceTipo,
                        principalTable: "IndiceTipo",
                        principalColumn: "IdIndiceTipo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Titulo_TituloTipo_IdTipoTitulo",
                        column: x => x.IdTipoTitulo,
                        principalTable: "TituloTipo",
                        principalColumn: "IdTipoTitulo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Titulo_IdIndiceTipo",
                table: "Titulo",
                column: "IdIndiceTipo");

            migrationBuilder.CreateIndex(
                name: "IX_Titulo_IdTipoTitulo",
                table: "Titulo",
                column: "IdTipoTitulo");

            migrationBuilder.Sql(
                $@"
                INSERT INTO IndiceTipo(Descricao,TaxaAtual,DataUltimaAtualizacao) VALUES('SELIC', '0.9', getdate())
                INSERT INTO IndiceTipo(Descricao,TaxaAtual,DataUltimaAtualizacao) VALUES('CDI', '0.9', getdate())
                INSERT INTO IndiceTipo(Descricao,TaxaAtual,DataUltimaAtualizacao) VALUES('IPCA', '0.9', getdate())
                INSERT INTO IndiceTipo(Descricao,TaxaAtual,DataUltimaAtualizacao) VALUES('IGP-M', '0.9', getdate())


                INSERT INTO TituloTipo(Descricao, RendaFixa) VALUES('Tesouro Direto',1)
                INSERT INTO TituloTipo(Descricao, RendaFixa) VALUES('Tesouro IPCA',1)
                INSERT INTO TituloTipo(Descricao, RendaFixa) VALUES('CDB',1)
                INSERT INTO TituloTipo(Descricao, RendaFixa) VALUES('CRI',1)
                INSERT INTO TituloTipo(Descricao, RendaFixa) VALUES('CRA',1)
                INSERT INTO TituloTipo(Descricao, RendaFixa) VALUES('LCI',1)
                INSERT INTO TituloTipo(Descricao, RendaFixa) VALUES('LCA',1)

                INSERT INTO Titulo(IdTitulo,NomeTitulo,IdTipoTitulo,PosFixado,IdIndiceTipo,TaxaRendimento) 
	                VALUES (newid(), 'CDB Teste', 3, 1, 2, 108)


                INSERT INTO DescontoImpostoRenda(IdDescontoImpostoRenda,Descricao, QtdeMesesInicio, QtdeMesesFim, PercentualDesconto) 
	                VALUES(newid(), 'Até 06 meses', 0, 6, 22.5) 
                INSERT INTO DescontoImpostoRenda(IdDescontoImpostoRenda,Descricao, QtdeMesesInicio, QtdeMesesFim, PercentualDesconto) 
	                VALUES(newid(), 'Até 12 meses', 7, 12, 20) 
                INSERT INTO DescontoImpostoRenda(IdDescontoImpostoRenda,Descricao, QtdeMesesInicio, QtdeMesesFim, PercentualDesconto) 
	                VALUES(newid(), 'Até 24 meses', 12, 24, 17.5) 
                INSERT INTO DescontoImpostoRenda(IdDescontoImpostoRenda,Descricao, QtdeMesesInicio, QtdeMesesFim, PercentualDesconto) 
	                VALUES(newid(), 'Acima de 24 meses', 25, null, 15) 

                "
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescontoImpostoRenda");

            migrationBuilder.DropTable(
                name: "Titulo");

            migrationBuilder.DropTable(
                name: "IndiceTipo");

            migrationBuilder.DropTable(
                name: "TituloTipo");
        }
    }
}
