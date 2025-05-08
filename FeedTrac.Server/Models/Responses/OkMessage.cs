using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FeedTrac.Server.Models.Responses;

/// <summary>
/// 
/// </summary>
public class OkMessage
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("message")]
	public string Message { get; set; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="msg"></param>
	public OkMessage(string msg)
	{
		Message = msg;
	}
}