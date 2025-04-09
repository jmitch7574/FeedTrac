using FeedTrac.Server.Database;
using System.Text.Json.Serialization;

namespace FeedTrac.Server.Models.Responses.Modules
{
    public class ModuleResponse
    {
        [JsonIgnore]
        public Module Module;

        [JsonPropertyName("id")]
        public int Id => Module.Id;

        [JsonPropertyName("name")]
        public string Name => Module.Name;

        [JsonPropertyName("joinCode")]
        public string JoinCode => Module.JoinCode;

        public ModuleResponse(Module module)
        {
            Module = module;
        }
    }
}
