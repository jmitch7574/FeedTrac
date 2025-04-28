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
		catch (FeedTracHTTPException ex)
		{
			context.Response.StatusCode = ex.HttpStatusCode;
			context.Response.ContentType = "application/json";
			var response = new { error = ex.Message };
			await context.Response.WriteAsJsonAsync(response);
		}
		catch (Exception ex)
		{
			context.Response.StatusCode = 500;
			context.Response.ContentType = "application/json";
			var response = new { error = ex.Message };
			await context.Response.WriteAsJsonAsync(response);
			Console.WriteLine(ex.Message);
			Console.WriteLine(ex.StackTrace);
		}
	}
}

/// <summary>
/// Represents that can occur in the FeedTrac API
/// </summary>
public class FeedTracHTTPException : Exception
{
	/// <summary>
	/// The HTTP code of the error
	/// </summary>
	public int HttpStatusCode { get; set; }

	/// <summary>
	/// Initializes a FeedTrac HTTP Exception
	/// </summary>
	/// <param name="message">The error message</param>
	/// <param name="httpStatusCode">HTTP status code associated with the error</param>
	public FeedTracHTTPException(string message, int httpStatusCode) : base(message)
	{
		this.HttpStatusCode = httpStatusCode;
	}
}

/// <summary>
/// Represents errors that occur due to user not being logged in
/// </summary>
public class NotLoggedInException : FeedTracHTTPException
{
	/// <summary>
	/// Initializes a new NotLoggedInException
	/// </summary>
	public NotLoggedInException() : base("User is not logged in", 401) {}
}

/// <summary>
/// Represents errors that occur due the user not having sufficient roles 
/// </summary>
public class InsufficientRolesException : FeedTracHTTPException
{
	/// <summary>
	/// Initializes a new Insufficient Roles Exception
	/// </summary>
	public InsufficientRolesException() : base("You do not have the required roles to use this endpoint", 401) {}
}

/// <summary>
/// Represents errors that occur due to the user trying to access a resource they do not have access to
/// </summary>
public class UnauthorizedResourceAccessException : FeedTracHTTPException
{
	/// <summary>
	/// Initialize a Unauthorized Resource Access Exception
	/// </summary>
	public UnauthorizedResourceAccessException() : base("You do not have access to the requested resource", 401) {}
}

/// <summary>
/// Represents errors that occur when the user tries to reference a resource that cannot be found or does not exist
/// </summary>
public class ResourceNotFoundException : FeedTracHTTPException
{
	/// <summary>
	/// Initializes a Resource Not Found Exception
	/// </summary>
	public ResourceNotFoundException() : base("Resource not found", 404) {}
}