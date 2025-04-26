using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace FeedTrac.Server.Services;

public class EmailService
{
	private HttpClient _httpClient;

	public EmailService()
	{
		_httpClient = new HttpClient();
		string apiKey = Environment.GetEnvironmentVariable("RESEND_KEY");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
	}
	
	public async Task TestEmail()
	{
		await SendEmailAsync(
			from: "feedtrac@resend.dev", 
			to: "27774557@students.lincoln.ac.uk",
			subject: "FeedTrac Development Test",
			htmlContent: "<p>Hello Jake! This is a <strong>FeedTrac</strong> test email 🚀</p>"
		);
	}
	
	
	public async Task SendEmailAsync(string from, string to, string subject, string htmlContent)
	{
		var payload = new
		{
			from = from,
			to = new[] { to },
			subject = subject,
			html = htmlContent
		};

		var jsonPayload = JsonConvert.SerializeObject(payload);
		var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync("https://api.resend.com/emails", content);
		response.EnsureSuccessStatusCode();
	}
}