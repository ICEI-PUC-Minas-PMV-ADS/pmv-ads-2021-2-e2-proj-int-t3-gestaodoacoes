using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Migrations
{
    public partial class doacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeCartao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroCartao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidadeCartao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodSegurancaCartao = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<double>(type: "float", nullable: false),
                    instituicaoCNPJ = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doacoes_Instituicoes_instituicaoCNPJ",
                        column: x => x.instituicaoCNPJ,
                        principalTable: "Instituicoes",
                        principalColumn: "CNPJ",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doacoes_instituicaoCNPJ",
                table: "Doacoes",
                column: "instituicaoCNPJ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doacoes");
        }
    }
}
