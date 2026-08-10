using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dias_usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "platos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Ingredientes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Receta = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_platos_usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dias_platos",
                columns: table => new
                {
                    IdPlato = table.Column<int>(type: "int", nullable: false),
                    IdDia = table.Column<int>(type: "int", nullable: false),
                    TipoComida = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dias_platos", x => new { x.IdDia, x.IdPlato, x.TipoComida });
                    table.ForeignKey(
                        name: "FK_dias_platos_dias_IdDia",
                        column: x => x.IdDia,
                        principalTable: "dias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dias_platos_platos_IdPlato",
                        column: x => x.IdPlato,
                        principalTable: "platos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dias_IdUsuario_Fecha",
                table: "dias",
                columns: new[] { "IdUsuario", "Fecha" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dias_platos_IdPlato",
                table: "dias_platos",
                column: "IdPlato");

            migrationBuilder.CreateIndex(
                name: "IX_platos_IdUsuario",
                table: "platos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_Email",
                table: "usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dias_platos");

            migrationBuilder.DropTable(
                name: "dias");

            migrationBuilder.DropTable(
                name: "platos");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
