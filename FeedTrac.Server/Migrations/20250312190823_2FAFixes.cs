using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class _2FAFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                schema: "feedtrac",
                table: "AspNetUsers",
                type: "char(32)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                schema: "feedtrac",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
