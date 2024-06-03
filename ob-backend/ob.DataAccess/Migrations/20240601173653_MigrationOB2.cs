using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ob.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationOB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Usuarios",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<Guid>(
                name: "ConstructoraId",
                table: "Usuarios",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rol",
                table: "Invitaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ConstructoraId",
                table: "Usuarios",
                column: "ConstructoraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Constructoras_ConstructoraId",
                table: "Usuarios",
                column: "ConstructoraId",
                principalTable: "Constructoras",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Constructoras_ConstructoraId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_ConstructoraId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ConstructoraId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Invitaciones");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Usuarios",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);
        }
    }
}
