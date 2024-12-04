using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedFirstandLastname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "feedtrac",
                table: "AspNetUsers",
                type: "character varying(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "feedtrac",
                table: "AspNetUsers",
                type: "character varying(255)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "feedtrac",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "feedtrac",
                table: "AspNetUsers");
        }
    }
}
