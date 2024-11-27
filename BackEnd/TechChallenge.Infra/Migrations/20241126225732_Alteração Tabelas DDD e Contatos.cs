using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlteraçãoTabelasDDDeContatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_CodigosDeArea_DddId",
                table: "Contatos");


            migrationBuilder.DropColumn(
                name: "DddId",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "Ddd",
                table: "CodigosDeArea");

            migrationBuilder.DropColumn(
                name: "Uf",
                table: "CodigosDeArea");

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Contatos",
                type: "VARCHAR(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Contatos",
                type: "VARCHAR(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Contatos",
                type: "VARCHAR(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Contatos",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "IdDDD",
                table: "Contatos",
                type: "INT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CodigosDeArea",
                type: "INT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Regiao",
                table: "CodigosDeArea",
                type: "VARCHAR(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_IdDDD",
                table: "Contatos",
                column: "IdDDD");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_CodigosDeArea_IdDDD",
                table: "Contatos",
                column: "IdDDD",
                principalTable: "CodigosDeArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_CodigosDeArea_IdDDD",
                table: "Contatos");

            migrationBuilder.DropIndex(
                name: "IX_Contatos_IdDDD",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "IdDDD",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "Regiao",
                table: "CodigosDeArea");

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Contatos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Contatos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Contatos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Contatos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "DddId",
                table: "Contatos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CodigosDeArea",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INT")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Ddd",
                table: "CodigosDeArea",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Uf",
                table: "CodigosDeArea",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_DddId",
                table: "Contatos",
                column: "DddId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_CodigosDeArea_DddId",
                table: "Contatos",
                column: "DddId",
                principalTable: "CodigosDeArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
