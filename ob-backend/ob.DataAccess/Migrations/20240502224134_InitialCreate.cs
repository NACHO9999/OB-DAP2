using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ob.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Nombre);
                });

            migrationBuilder.CreateTable(
                name: "Constructoras",
                columns: table => new
                {
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constructoras", x => x.Nombre);
                });

            migrationBuilder.CreateTable(
                name: "Duenos",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duenos", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Invitaciones",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitaciones", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Edificios",
                columns: table => new
                {
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ubicación = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GastosComunes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EncargadoEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edificios", x => new { x.Nombre, x.Direccion });
                    table.ForeignKey(
                        name: "FK_Edificios_Constructoras_Nombre",
                        column: x => x.Nombre,
                        principalTable: "Constructoras",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Edificios_Usuarios_EncargadoEmail",
                        column: x => x.EncargadoEmail,
                        principalTable: "Usuarios",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Usuarios_UsuarioEmail",
                        column: x => x.UsuarioEmail,
                        principalTable: "Usuarios",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deptos",
                columns: table => new
                {
                    Numero = table.Column<int>(type: "int", nullable: false),
                    EdificioNombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EdificioDireccion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Piso = table.Column<int>(type: "int", nullable: false),
                    DuenoEmail = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CantidadCuartos = table.Column<int>(type: "int", nullable: false),
                    CantidadBanos = table.Column<int>(type: "int", nullable: false),
                    ConTerraza = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deptos", x => new { x.Numero, x.EdificioNombre, x.EdificioDireccion });
                    table.ForeignKey(
                        name: "FK_Deptos_Duenos_DuenoEmail",
                        column: x => x.DuenoEmail,
                        principalTable: "Duenos",
                        principalColumn: "Email");
                    table.ForeignKey(
                        name: "FK_Deptos_Edificios_EdificioNombre_EdificioDireccion",
                        columns: x => new { x.EdificioNombre, x.EdificioDireccion },
                        principalTable: "Edificios",
                        principalColumns: new[] { "Nombre", "Direccion" });
                });

            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeptoNumero = table.Column<int>(type: "int", nullable: false),
                    DeptoEdificioNombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeptoEdificioDireccion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoriaNombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PerManEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Categorias_CategoriaNombre",
                        column: x => x.CategoriaNombre,
                        principalTable: "Categorias",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Deptos_DeptoNumero_DeptoEdificioNombre_DeptoEdificioDireccion",
                        columns: x => new { x.DeptoNumero, x.DeptoEdificioNombre, x.DeptoEdificioDireccion },
                        principalTable: "Deptos",
                        principalColumns: new[] { "Numero", "EdificioNombre", "EdificioDireccion" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Usuarios_PerManEmail",
                        column: x => x.PerManEmail,
                        principalTable: "Usuarios",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deptos_DuenoEmail",
                table: "Deptos",
                column: "DuenoEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Deptos_EdificioNombre_EdificioDireccion",
                table: "Deptos",
                columns: new[] { "EdificioNombre", "EdificioDireccion" });

            migrationBuilder.CreateIndex(
                name: "IX_Edificios_EncargadoEmail",
                table: "Edificios",
                column: "EncargadoEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UsuarioEmail",
                table: "Sessions",
                column: "UsuarioEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_CategoriaNombre",
                table: "Solicitudes",
                column: "CategoriaNombre");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_DeptoNumero_DeptoEdificioNombre_DeptoEdificioDireccion",
                table: "Solicitudes",
                columns: new[] { "DeptoNumero", "DeptoEdificioNombre", "DeptoEdificioDireccion" });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_PerManEmail",
                table: "Solicitudes",
                column: "PerManEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitaciones");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Solicitudes");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Deptos");

            migrationBuilder.DropTable(
                name: "Duenos");

            migrationBuilder.DropTable(
                name: "Edificios");

            migrationBuilder.DropTable(
                name: "Constructoras");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
