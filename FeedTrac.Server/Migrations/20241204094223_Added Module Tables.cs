using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedModuleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "feedtrac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JoinCode = table.Column<string>(type: "char(6)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JoinModules",
                schema: "feedtrac",
                columns: table => new
                {
                    userId = table.Column<string>(type: "text", nullable: false),
                    moduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_JoinModules_AspNetUsers_userId",
                        column: x => x.userId,
                        principalSchema: "feedtrac",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinModules_Modules_moduleId",
                        column: x => x.moduleId,
                        principalSchema: "feedtrac",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JoinModules_moduleId",
                schema: "feedtrac",
                table: "JoinModules",
                column: "moduleId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinModules_userId",
                schema: "feedtrac",
                table: "JoinModules",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JoinModules",
                schema: "feedtrac");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "feedtrac");
        }
    }
}
