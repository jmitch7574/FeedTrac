namespace FeedTrac.Server;

/// <summary>
/// Custom middleware used to manage FeedTrac API calls
/// </summary>
public class FeedTracMiddleware
{
	private readonly RequestDelegate _next;
	
	/// <summary>
	/// Middleware Constructor
	/// </summary>
	/// <param name="next"></param>
	public FeedTracMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	/// <summary>
	/// Custom middleware logic for API Calls
	/// </summary>
	/// <param name="context">The API call being middleware'd</param>
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context); // Continue down the pipeline
		}
		catch (Exception ex) // Safely catch any errors and parse them into json responses
		{
			context.Response.StatusCode = 401; // Unauthorized
			context.Response.ContentType = "application/json";
			var response = new { error = ex.Message };
			await context.Response.WriteAsJsonAsync(response);
		}
	}
}

/// <summary>
/// Represents errors that occur due to user not being logged in
/// </summary>
public class NotLoggedInException : Exception
{
	/// <summary>
	/// Initializes a new NotLoggedInException
	/// </summary>
	public NotLoggedInException() : base("User is not logged in") {}
}

/// <summary>
/// Represents errors that occur due the user not having sufficient roles 
/// </summary>
public class InsufficientRolesException : Exception
{
	/// <summary>
	/// Initializes a new Insufficient Roles Exception
	/// </summary>
	public InsufficientRolesException() : base("You do not have the required roles to use this endpoint") {}
}

/// <summary>
/// Represents errors that occur when the user tries to reference a resource that cannot be found or does not exist
/// </summary>
public class ResourceNotFoundException : Exception
{
	/// <summary>
	/// Initializes a Resource Not Found Exception
	/// </summary>
	public ResourceNotFoundException() : base("Resource not found") {}
}