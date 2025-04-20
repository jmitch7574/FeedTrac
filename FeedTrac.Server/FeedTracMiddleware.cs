namespace FeedTrac.Server;

public class FeedTracMiddleware
{
	private readonly RequestDelegate _next;
	
	
	public FeedTracMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context); // Continue down the pipeline
		}
		catch (Exception ex)
		{
			context.Response.StatusCode = 401; // Unauthorized
			context.Response.ContentType = "application/json";
			var response = new { error = ex.Message };
			await context.Response.WriteAsJsonAsync(response);
		}
	}
}

public class NotLoggedInException : Exception
{
	public NotLoggedInException() : base("User is not logged in") {}
}

public class InsufficientRolesException : Exception
{
	public InsufficientRolesException() : base("You do not have the required roles to use this endpoint") {}
}

public class ResourceNotFoundException : Exception
{
	public ResourceNotFoundException() : base("Resource not found") {}
}