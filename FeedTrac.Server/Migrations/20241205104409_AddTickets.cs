using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedbackTicket",
                schema: "feedtrac",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<string>(type: "text", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackTicket", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_FeedbackTicket_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "feedtrac",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedbackTicket_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "feedtrac",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackTicket_ModuleId",
                schema: "feedtrac",
                table: "FeedbackTicket",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackTicket_OwnerId",
                schema: "feedtrac",
                table: "FeedbackTicket",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackTicket",
                schema: "feedtrac");
        }
    }
}
