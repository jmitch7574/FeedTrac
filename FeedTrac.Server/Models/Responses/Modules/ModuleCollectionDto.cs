using FeedTrac.Server.Database;
using System.Text.Json.Serialization;

namespace FeedTrac.Server.Models.Responses.Modules;

/// <summary>
/// Data Transfer Object containing a collection of modules
/// </summary>
public class ModuleCollectionDto
{
    /// <summary>
    /// The module Dto's
    /// </summary>
    [JsonPropertyName("modules")]
    public List<ModuleDto> Modules { get; set; }
    
    /// <summary>
    /// Constructor for Module Collection DTO
    /// </summary>
    /// <param name="modules"></param>
    public ModuleCollectionDto(List<Module> modules)
    {
        Modules = modules.Select(x => new ModuleDto(x)).ToList();
    }
}

