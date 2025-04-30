using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentModule_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "StudentModule");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentModule_Modules_ModuleId",
                schema: "feedtrac",
                table: "StudentModule");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherModule_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "TeacherModule");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherModule_Modules_ModuleId",
                schema: "feedtrac",
                table: "TeacherModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherModule",
                schema: "feedtrac",
                table: "TeacherModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentModule",
                schema: "feedtrac",
                table: "StudentModule");

            migrationBuilder.RenameTable(
                name: "TeacherModule",
                schema: "feedtrac",
                newName: "TeacherModules",
                newSchema: "feedtrac");

            migrationBuilder.RenameTable(
                name: "StudentModule",
                schema: "feedtrac",
                newName: "StudentModules",
                newSchema: "feedtrac");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherModule_UserId",
                schema: "feedtrac",
                table: "TeacherModules",
                newName: "IX_TeacherModules_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherModule_ModuleId",
                schema: "feedtrac",
                table: "TeacherModules",
                newName: "IX_TeacherModules_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentModule_UserId",
                schema: "feedtrac",
                table: "StudentModules",
                newName: "IX_StudentModules_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentModule_ModuleId",
                schema: "feedtrac",
                table: "StudentModules",
                newName: "IX_StudentModules_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherModules",
                schema: "feedtrac",
                table: "TeacherModules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentModules",
                schema: "feedtrac",
                table: "StudentModules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentModules_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "StudentModules",
                column: "UserId",
                principalSchema: "feedtrac",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentModules_Modules_ModuleId",
                schema: "feedtrac",
                table: "StudentModules",
                column: "ModuleId",
                principalSchema: "feedtrac",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherModules_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "TeacherModules",
                column: "UserId",
                principalSchema: "feedtrac",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherModules_Modules_ModuleId",
                schema: "feedtrac",
                table: "TeacherModules",
                column: "ModuleId",
                principalSchema: "feedtrac",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentModules_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "StudentModules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentModules_Modules_ModuleId",
                schema: "feedtrac",
                table: "StudentModules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherModules_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "TeacherModules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherModules_Modules_ModuleId",
                schema: "feedtrac",
                table: "TeacherModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherModules",
                schema: "feedtrac",
                table: "TeacherModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentModules",
                schema: "feedtrac",
                table: "StudentModules");

            migrationBuilder.RenameTable(
                name: "TeacherModules",
                schema: "feedtrac",
                newName: "TeacherModule",
                newSchema: "feedtrac");

            migrationBuilder.RenameTable(
                name: "StudentModules",
                schema: "feedtrac",
                newName: "StudentModule",
                newSchema: "feedtrac");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherModules_UserId",
                schema: "feedtrac",
                table: "TeacherModule",
                newName: "IX_TeacherModule_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherModules_ModuleId",
                schema: "feedtrac",
                table: "TeacherModule",
                newName: "IX_TeacherModule_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentModules_UserId",
                schema: "feedtrac",
                table: "StudentModule",
                newName: "IX_StudentModule_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentModules_ModuleId",
                schema: "feedtrac",
                table: "StudentModule",
                newName: "IX_StudentModule_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherModule",
                schema: "feedtrac",
                table: "TeacherModule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentModule",
                schema: "feedtrac",
                table: "StudentModule",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentModule_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "StudentModule",
                column: "UserId",
                principalSchema: "feedtrac",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentModule_Modules_ModuleId",
                schema: "feedtrac",
                table: "StudentModule",
                column: "ModuleId",
                principalSchema: "feedtrac",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherModule_AspNetUsers_UserId",
                schema: "feedtrac",
                table: "TeacherModule",
                column: "UserId",
                principalSchema: "feedtrac",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherModule_Modules_ModuleId",
                schema: "feedtrac",
                table: "TeacherModule",
                column: "ModuleId",
                principalSchema: "feedtrac",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
