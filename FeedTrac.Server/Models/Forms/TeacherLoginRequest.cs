// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// MODIFIED BY JAKE MITCHELL FOR THE PURPOSES OF THE FEEDTRAC PROJECT
// WOULD HAVE LOVED TO JUST DO AN INHERITANCE THING BUT THE IDENTITY API IS SEALED

using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Routing;

namespace FeedTrac.Server.Models.Forms;

/// <summary>
/// The request type for the "/login" endpoint added by <see cref="IdentityApiEndpointRouteBuilderExtensions.MapIdentityApi"/>.
/// </summary>
public class TeacherLoginRequest
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
    /// The optional two-factor authenticator code. This may be required for users who have enabled two-factor authentication.
    /// This is not required if a <see cref="TwoFactorRecoveryCode"/> is sent.
    /// </summary>
    public required string TwoFactorCode { get; init; }

    /// <summary>
    /// An optional parameter to indicate if the user's session should be remembered across browser restarts.
    /// </summary>
    public bool RememberMe { get; init; } = false;
}
