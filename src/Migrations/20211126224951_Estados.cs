using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Migrations
{
    public partial class Estados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "senha",
                table: "Instituicoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Instituicoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Instituicoes");

            migrationBuilder.AlterColumn<string>(
                name: "senha",
                table: "Instituicoes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
