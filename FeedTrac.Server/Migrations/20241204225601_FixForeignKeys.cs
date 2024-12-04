using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserModules_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "UserModules");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModules_Modules_ModuleId",
                schema: "feedtrac",
                table: "UserModules");

            migrationBuilder.AddForeignKey(
                name: "FK_UserModules_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "UserModules",
                column: "UserId",
                principalSchema: "feedtrac",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModules_Modules_ModuleId",
                schema: "feedtrac",
                table: "UserModules",
                column: "ModuleId",
                principalSchema: "feedtrac",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserModules_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "UserModules");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModules_Modules_ModuleId",
                schema: "feedtrac",
                table: "UserModules");

            migrationBuilder.AddForeignKey(
                name: "FK_UserModules_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "UserModules",
                column: "UserId",
                principalSchema: "feedtrac",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModules_Modules_ModuleId",
                schema: "feedtrac",
                table: "UserModules",
                column: "ModuleId",
                principalSchema: "feedtrac",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
