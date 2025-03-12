// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// MODIFIED BY JAKE MITCHELL FOR THE PURPOSES OF THE FEEDTRAC PROJECT
// WOULD HAVE LOVED TO JUST DO AN INHERITANCE THING BUT THE IDENTITY API IS SEALED

using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Identity.Data;

/// <summary>
/// The request type for the "/login" endpoint added by <see cref="IdentityApiEndpointRouteBuilderExtensions.MapIdentityApi"/>.
/// </summary>
public class RegisterUserRequest
{
    /// <summary>
    /// The user's email address which acts as a user name.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// The user's First Name
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// The user's Last Name
    /// </summary>
    public required string LastName { get; init; }
}
