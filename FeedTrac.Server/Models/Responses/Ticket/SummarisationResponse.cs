namespace FeedTrac.Server.Models.Responses.Ticket;

/// <summary>
/// Response body for ticket endpoint <see cref="FeedTrac.Server.Controllers.TicketController.Summarize"/>
/// </summary>
public class SummarisationResponse
{
	/// <summary>
	/// A one to two sentence summary of the feedback ticket
	/// </summary>
	public string Summary { get; set; }

	/// <summary>
	/// Constructor for response
	/// </summary>
	/// <param name="summary"></param>
	public SummarisationResponse(string summary)
	{
		Summary = summary;
	}
}