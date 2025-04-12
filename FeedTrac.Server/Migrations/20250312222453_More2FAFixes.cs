using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class More2FAFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TwoFactorSecret",
                schema: "feedtrac",
                table: "AspNetUsers",
                type: "char(16)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(32)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TwoFactorSecret",
                schema: "feedtrac",
                table: "AspNetUsers",
                type: "char(32)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(16)");
        }
    }
}
