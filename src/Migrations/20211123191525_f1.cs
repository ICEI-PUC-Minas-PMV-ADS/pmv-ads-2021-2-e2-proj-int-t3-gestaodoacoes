using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Migrations
{
    public partial class f1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Instituicoes",
                columns: table => new
                {
                    CNPJ = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataRegistro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    DescricaoCurta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescricaoDetalhada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaArrecadacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    senha = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicoes", x => x.CNPJ);
                    table.ForeignKey(
                        name: "FK_Instituicoes_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instituicoes_CategoriaId",
                table: "Instituicoes",
                column: "CategoriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instituicoes");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
