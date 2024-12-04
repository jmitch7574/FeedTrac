using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModuleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JoinModules",
                schema: "feedtrac");

            migrationBuilder.CreateTable(
                name: "ApplicationUserModule",
                schema: "feedtrac",
                columns: table => new
                {
                    ModulesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserModule", x => new { x.ModulesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserModule_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "feedtrac",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserModule_Modules_ModulesId",
                        column: x => x.ModulesId,
                        principalSchema: "feedtrac",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserModule_UsersId",
                schema: "feedtrac",
                table: "ApplicationUserModule",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserModule",
                schema: "feedtrac");

            migrationBuilder.CreateTable(
                name: "JoinModules",
                schema: "feedtrac",
                columns: table => new
                {
                    moduleId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "text", nullable: false)
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
    }
}
