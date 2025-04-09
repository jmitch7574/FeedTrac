using FeedTrac.Server.Database;
using System.Text.Json.Serialization;

namespace FeedTrac.Server.Models.Responses.Modules
{
    public class UserModulesResponse
    {
        /// <summary>
        /// The list of modules that the user is signed up to
        /// </summary>
        [JsonIgnore]
        public List<Module> Modules { get; set; } = new List<Module>();

        [JsonPropertyName("modules")]
        public List<ModuleResponse> ModuleResponses => Modules.Select(m => new ModuleResponse(m)).ToList();

        public UserModulesResponse(List<Module> modules)
        {
            Modules = modules;
        }
    }
}
