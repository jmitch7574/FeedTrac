using System.Net;
using System.Net.Mail;
using FeedTrac.Server.Database;

namespace FeedTrac.Server.Services;

/// <summary>
/// Email service for sending emails 
/// </summary>
public class EmailService
{
	private const string SmtpServer = "smtp.gmail.com";
	private const int SmtpPort = 587;
	private readonly string _smtpUser;
	private readonly string _smtpPass;
	
	/// <summary>
	/// Constructor and Injector for Email Service
	/// </summary>
	public EmailService()
	{
		_smtpUser = Environment.GetEnvironmentVariable("GMAIL_EMAIL")??"";
		_smtpPass = Environment.GetEnvironmentVariable("GMAIL_PASS")??"";
	}
	

	/// <summary>
	/// Creates a welcome emailer for a just-created teacher account
	/// </summary>
	/// <param name="user">The new application user</param>
	/// <param name="plainTextPass">The user's password</param>
	public async Task TeacherWelcomeEmail(ApplicationUser user, string plainTextPass)
	{
		await SendEmailAsync(
			to: user.Email ?? string.Empty,
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

	/// <summary>
	/// Send an email notifying teachers when a ticket is created in a module they are a teacher for
	/// </summary>
	/// <param name="ticket">Target ticket</param>
	public async Task NotifyTeachersAboutTicket(FeedbackTicket ticket)
	{
		List<TeacherModule> teachers = ticket.Module.TeacherModule;
		foreach (TeacherModule tm in teachers)
		{
			await SendEmailAsync(
				to: tm.User.Email ?? string.Empty,
				subject: "FeedTrac Teacher Onboarding",
				htmlContent: $"""
				              <p>Hello {tm.User.FirstName} {tm.User.LastName}</p>
				              <p>A ticket has been created in the {tm.Module.Name} module</p>
				              <h3>{ticket.Title}</h3>
				              """
			);
		}
	}

	/// <summary>
	/// Update teachers and ticket owner when a ticket they have access to is updated
	/// </summary>
	/// <param name="fm"></param>
	public async Task NotifyTicketMessage(FeedbackMessage fm)
	{
		List<TeacherModule> teachers = fm.Ticket.Module.TeacherModule;
		
		await SendEmailAsync(
			to: fm.Ticket.Owner.Email ?? string.Empty,
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
				to: tm.User.Email ?? string.Empty,
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

	/// <summary>
	/// Email teachers and ticket owner when a ticket is marked as resolved
	/// </summary>
	/// <param name="ticket"></param>
	public async Task TicketResolved(FeedbackTicket ticket)
	{
		List<TeacherModule> teachers = ticket.Module.TeacherModule;
		
		await SendEmailAsync(
			to: ticket.Owner.Email ?? string.Empty,
			subject: "FeedTrac Ticket Update",
			htmlContent: $"""
			              <p>Hello {ticket.Owner.FirstName} {ticket.Owner.LastName}</p>
			              <p>One of your tickets, {ticket.Title}, has been marked as resolved</p>
			              """
		);
		
		foreach (TeacherModule tm in teachers)
		{
			await SendEmailAsync(
				to: tm.User.Email ?? string.Empty,
				subject: "FeedTrac Ticket Update",
				htmlContent: $"""
				              <p>Hello {tm.User.FirstName} {tm.User.LastName}</p>
				              <p>The ticket, {ticket.Title}, in {ticket.Module.Name} has been marked as resolved</p>
				              """
			);
		}
	}
	
	/// <summary>
	/// Function to send an email
	/// </summary>
	/// <param name="to">target inbox</param>
	/// <param name="subject">Email Subject</param>
	/// <param name="htmlContent">HTML content of the email</param>
	async Task SendEmailAsync(string to, string subject, string htmlContent)
	{
		var smtpClient = new SmtpClient(SmtpServer);
		smtpClient.Port = SmtpPort;

		if (string.IsNullOrWhiteSpace(_smtpUser) || string.IsNullOrWhiteSpace(_smtpPass))
			return;
		
		smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
		smtpClient.EnableSsl = true;
		
		var payload = new MailMessage
		{
			From = new MailAddress(_smtpUser, "FeedTrac Notifications"),
			Subject = subject,
			Body = htmlContent +  "<br/><br/>This email is part of a Team Software Engineering Assignment, if you feel like you received this email in error, please contact 27774557@students.lincoln.ac.uk",
			IsBodyHtml = true
		};
		
		payload.To.Add(to);
		
		await smtpClient.SendMailAsync(payload);
	}
}