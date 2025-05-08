using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FeedTrac.Server.Migrations
{
    /// <inheritdoc />
    public partial class SummaryCaching : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketSummaries",
                schema: "feedtrac",
                columns: table => new
                {
                    SummaryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    MessageCount = table.Column<int>(type: "int", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSummaries", x => x.SummaryId);
                    table.ForeignKey(
                        name: "FK_TicketSummaries_FeedbackTicket_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "feedtrac",
                        principalTable: "FeedbackTicket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketSummaries_TicketId",
                schema: "feedtrac",
                table: "TicketSummaries",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketSummaries",
                schema: "feedtrac");
        }
    }
}
