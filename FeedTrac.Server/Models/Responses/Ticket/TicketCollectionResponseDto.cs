using FeedTrac.Server.Database;

namespace FeedTrac.Server.Models.Responses.Ticket;

public class TicketCollectionResponseDto
{
	public List<TicketResponseDto> Tickets { get; set; }

	public TicketCollectionResponseDto(List<FeedbackTicket> tickets)
	{
		Tickets = tickets.Select(x => new TicketResponseDto(x)).ToList();
	}
}