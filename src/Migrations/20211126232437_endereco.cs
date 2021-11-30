using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Migrations
{
    public partial class endereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CEP",
                table: "Instituicoes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Instituicoes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Instituicoes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CEP",
                table: "Instituicoes");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Instituicoes");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Instituicoes");
        }
    }
}
