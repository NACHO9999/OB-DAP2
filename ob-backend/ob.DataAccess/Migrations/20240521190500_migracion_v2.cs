using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ob.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migracion_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Edificios_Constructoras_Nombre",
                table: "Edificios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Constructoras",
                table: "Constructoras");

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaConstructoraId",
                table: "Edificios",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Constructoras",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Constructoras",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Constructoras",
                table: "Constructoras",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Edificios_EmpresaConstructoraId",
                table: "Edificios",
                column: "EmpresaConstructoraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Edificios_Constructoras_EmpresaConstructoraId",
                table: "Edificios",
                column: "EmpresaConstructoraId",
                principalTable: "Constructoras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Edificios_Constructoras_EmpresaConstructoraId",
                table: "Edificios");

            migrationBuilder.DropIndex(
                name: "IX_Edificios_EmpresaConstructoraId",
                table: "Edificios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Constructoras",
                table: "Constructoras");

            migrationBuilder.DropColumn(
                name: "EmpresaConstructoraId",
                table: "Edificios");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Constructoras");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Constructoras",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Constructoras",
                table: "Constructoras",
                column: "Nombre");

            migrationBuilder.AddForeignKey(
                name: "FK_Edificios_Constructoras_Nombre",
                table: "Edificios",
                column: "Nombre",
                principalTable: "Constructoras",
                principalColumn: "Nombre",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
