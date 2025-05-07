using System.Text.Json.Serialization;

namespace FeedTrac.Server.Models.Gemini;

/// <summary>
/// The body that makes up a request to the gemini api
/// </summary>
public class GeminiRequest
{
	/// <summary>
	/// The contents of the request
	/// </summary>
	[JsonPropertyName("contents")]
	public List<GeminiContent> Contents { get; set; }
}

/// <summary>
/// The body that contains the parts of a request
/// </summary>
public class GeminiContent
{
	/// <summary>
	/// The parts that make up this part of the request
	/// </summary>
	[JsonPropertyName("parts")]
	public List<GeminiPart> Parts { get; set; }
}

/// <summary>
/// A message that is given to the gemini api
/// </summary>
public class GeminiPart
{
	/// <summary>
	/// The text that the gemini api is given
	/// </summary>
	[JsonPropertyName("text")]
	public string Text { get; set; }
}