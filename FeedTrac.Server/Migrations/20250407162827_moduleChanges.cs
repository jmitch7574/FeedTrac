using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class moduleChanges : Migration
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModules",
                schema: "feedtrac",
                table: "UserModules");

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "feedtrac",
                table: "UserModules");

            migrationBuilder.RenameTable(
                name: "UserModules",
                schema: "feedtrac",
                newName: "UserModule",
                newSchema: "feedtrac");

            migrationBuilder.RenameIndex(
                name: "IX_UserModules_UserId",
                schema: "feedtrac",
                table: "UserModule",
                newName: "IX_UserModule_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserModules_ModuleId",
                schema: "feedtrac",
                table: "UserModule",
                newName: "IX_UserModule_ModuleId");

            migrationBuilder.AddColumn<int>(
                name: "ModuleId1",
                schema: "feedtrac",
                table: "UserModule",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModule",
                schema: "feedtrac",
                table: "UserModule",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserModule_ModuleId1",
                schema: "feedtrac",
                table: "UserModule",
                column: "ModuleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserModule_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "UserModule",
                column: "UserId",
                principalSchema: "feedtrac",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModule_Modules_ModuleId",
                schema: "feedtrac",
                table: "UserModule",
                column: "ModuleId",
                principalSchema: "feedtrac",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserModule_Modules_ModuleId1",
                schema: "feedtrac",
                table: "UserModule",
                column: "ModuleId1",
                principalSchema: "feedtrac",
                principalTable: "Modules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserModule_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "UserModule");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModule_Modules_ModuleId",
                schema: "feedtrac",
                table: "UserModule");

            migrationBuilder.DropForeignKey(
                name: "FK_UserModule_Modules_ModuleId1",
                schema: "feedtrac",
                table: "UserModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModule",
                schema: "feedtrac",
                table: "UserModule");

            migrationBuilder.DropIndex(
                name: "IX_UserModule_ModuleId1",
                schema: "feedtrac",
                table: "UserModule");

            migrationBuilder.DropColumn(
                name: "ModuleId1",
                schema: "feedtrac",
                table: "UserModule");

            migrationBuilder.RenameTable(
                name: "UserModule",
                schema: "feedtrac",
                newName: "UserModules",
                newSchema: "feedtrac");

            migrationBuilder.RenameIndex(
                name: "IX_UserModule_UserId",
                schema: "feedtrac",
                table: "UserModules",
                newName: "IX_UserModules_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserModule_ModuleId",
                schema: "feedtrac",
                table: "UserModules",
                newName: "IX_UserModules_ModuleId");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                schema: "feedtrac",
                table: "UserModules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModules",
                schema: "feedtrac",
                table: "UserModules",
                column: "Id");

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
