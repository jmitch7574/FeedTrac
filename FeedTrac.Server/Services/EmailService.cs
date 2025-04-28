using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
using Newtonsoft.Json;

namespace FeedTrac.Server.Services;

/// <summary>
/// Email service for sending emails 
/// </summary>
public class EmailService
{
	private HttpClient _httpClient;
	
	private readonly string smtpServer = "smtp.gmail.com";
	private readonly int smtpPort = 587;
	private readonly string smtpUser;
	private readonly string smtpPass;
	public EmailService()
	{
		_httpClient = new HttpClient();
		string apiKey = Environment.GetEnvironmentVariable("RESEND_KEY");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
		
		smtpUser = Environment.GetEnvironmentVariable("GMAIL_EMAIL");
		smtpPass = Environment.GetEnvironmentVariable("GMAIL_PASS");
	}
	
	public async Task TestEmail()
	{
		await SendEmailAsync(
			to: "27774557@students.lincoln.ac.uk",
			subject: "FeedTrac Development Test",
			htmlContent: "<p>Hello Jake! This is a <strong>FeedTrac</strong> test email 🚀</p>"
		);
	}

	public async Task TeacherWelcomeEmail(ApplicationUser user, string plainTextPass)
	{
		await SendEmailAsync(
			to: user.Email,
			subject: "FeedTrac Teacher Onboarding",
			htmlContent: $"""
			              <p>Hello {user.FirstName} {user.LastName}</p>
			              <p>You have been made a teacher for your organization, please use the following details to login:
			              <h3>Email: {user.Email}</h3>
			              <h3>Password: {plainTextPass}</h3>
			              <h3>Two Factor Secret: {user.TwoFactorSecret}</h3>
			              <p>It is recommended you change your password once you've logged in. In addition to this, you will be required to use two-factor authentication to sign in. Please add the provided secret to your authentication app of choice (Google Authenticator / Microsoft Authenticator, etc.)</p>
			              <p>Please do not delete this email, this information will not be resent to you</p>
			              """
		);
	}

	public async Task NotifyTeachersAboutTicket(FeedbackTicket ticket)
	{
		List<TeacherModule> teachers = ticket.Module.TeacherModule;
		foreach (TeacherModule tm in teachers)
		{
			await SendEmailAsync(
				to: tm.User.Email,
				subject: "FeedTrac Teacher Onboarding",
				htmlContent: $"""
				              <p>Hello {tm.User.FirstName} {tm.User.LastName}</p>
				              <p>A ticket has been created in the {tm.Module.Name} module</p>
				              <h3>{ticket.Title}</h3>
				              """
			);
		}
	}

	public async Task NotifyTicketMessage(FeedbackMessage fm)
	{
		List<TeacherModule> teachers = fm.Ticket.Module.TeacherModule;
		
		await SendEmailAsync(
			to: fm.Ticket.Owner.Email,
			subject: "FeedTrac Ticket Update",
			htmlContent: $"""
			              <p>Hello {fm.Ticket.Owner.FirstName} {fm.Ticket.Owner.LastName}</p>
			              <p>One of your tickets, {fm.Ticket.Title}, has been updated</p>
			              <br>
			              <h4>{fm.Author.FirstName} {fm.Author.LastName} Said:</h4>
			              <p>{fm.Content}</p>
			              """
		);
		
		foreach (TeacherModule tm in teachers)
		{
			await SendEmailAsync(
				to: tm.User.Email,
				subject: "FeedTrac Ticket Update",
				htmlContent: $"""
				              <p>Hello {tm.User.FirstName} {tm.User.LastName}</p>
				              <p>A ticket, {fm.Ticket.Title}, in the {fm.Ticket.Module.Name} module has been updated</p>
				              <br>
				              <h4>{fm.Author.FirstName} {fm.Author.LastName} Said:</h4>
				              <p>{fm.Content}</p>
				              """
			);
		}
	}

	public async Task TicketResolved(FeedbackTicket ticket)
	{
		List<TeacherModule> teachers = ticket.Module.TeacherModule;
		
		await SendEmailAsync(
			to: ticket.Owner.Email,
			subject: "FeedTrac Ticket Update",
			htmlContent: $"""
			              <p>Hello {ticket.Owner.FirstName} {ticket.Owner.LastName}</p>
			              <p>One of your tickets, {ticket.Title}, has been marked as resolved</p>
			              """
		);
		
		foreach (TeacherModule tm in teachers)
		{
			await SendEmailAsync(
				to: tm.User.Email,
				subject: "FeedTrac Ticket Update",
				htmlContent: $"""
				              <p>Hello {tm.User.FirstName} {tm.User.LastName}</p>
				              <p>The ticket, {ticket.Title}, in {ticket.Module.Name} has been marked as resolved</p>
				              """
			);
		}
	}
	
	public async Task SendEmailAsync(string to, string subject, string htmlContent)
	{
		var smtpClient = new SmtpClient(smtpServer);
		smtpClient.Port = smtpPort;

		if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPass))
			return;
		
		smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
		smtpClient.EnableSsl = true;
		
		var payload = new MailMessage
		{
			From = new MailAddress(smtpUser, "FeedTrac Notifications"),
			Subject = subject,
			Body = htmlContent +  "<br/><br/>This email is part of a Team Software Engineering Assignment, if you feel like you received this email in error, please contact 27774557@students.lincoln.ac.uk",
			IsBodyHtml = true
		};
		
		payload.To.Add(to);
		
		await smtpClient.SendMailAsync(payload);
	}
}