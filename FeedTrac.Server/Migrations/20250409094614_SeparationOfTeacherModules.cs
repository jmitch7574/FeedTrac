using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeparationOfTeacherModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserModule",
                schema: "feedtrac");

            migrationBuilder.CreateTable(
                name: "StudentModule",
                schema: "feedtrac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentModule_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "feedtrac",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentModule_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "feedtrac",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherModule",
                schema: "feedtrac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherModule_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "feedtrac",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherModule_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "feedtrac",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentModule_ModuleId",
                schema: "feedtrac",
                table: "StudentModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentModule_UserId",
                schema: "feedtrac",
                table: "StudentModule",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherModule_ModuleId",
                schema: "feedtrac",
                table: "TeacherModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherModule_UserId",
                schema: "feedtrac",
                table: "TeacherModule",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentModule",
                schema: "feedtrac");

            migrationBuilder.DropTable(
                name: "TeacherModule",
                schema: "feedtrac");

            migrationBuilder.CreateTable(
                name: "UserModule",
                schema: "feedtrac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ModuleId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserModule_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "feedtrac",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserModule_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "feedtrac",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserModule_Modules_ModuleId1",
                        column: x => x.ModuleId1,
                        principalSchema: "feedtrac",
                        principalTable: "Modules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserModule_ModuleId",
                schema: "feedtrac",
                table: "UserModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModule_ModuleId1",
                schema: "feedtrac",
                table: "UserModule",
                column: "ModuleId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserModule_UserId",
                schema: "feedtrac",
                table: "UserModule",
                column: "UserId");
        }
    }
}
