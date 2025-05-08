using System.Text.Json.Serialization;

namespace FeedTrac.Server.Models.Responses;

/// <summary>
/// 
/// </summary>
public class BadResponse
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("message")]
	public string Error { get; set; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="err"></param>
	public BadResponse(string err)
	{
		Error = err;
	}
}