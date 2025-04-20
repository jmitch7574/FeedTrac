using FeedTrac.Server.Database;
using System.Text.Json.Serialization;

namespace FeedTrac.Server.Models.Responses.Modules;

/// <summary>
/// Data Transfer Object for a module
/// </summary>
public class ModuleDto
{
    /// <summary>
    /// The module's ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    /// <summary>
    /// The module's name
    /// </summary>

    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// The module's join code
    /// </summary>

    [JsonPropertyName("joinCode")]
    public string JoinCode { get; set; }

    /// <summary>
    /// Module response constructor
    /// </summary>
    /// <param name="module">The module to use to construct this response</param>
    public ModuleDto(Module module)
    {
        Id = module.Id;
        Name = module.Name;
        JoinCode = module.JoinCode;
    }
}
