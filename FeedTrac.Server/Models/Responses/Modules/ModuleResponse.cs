using FeedTrac.Server.Database;
using System.Text.Json.Serialization;

namespace FeedTrac.Server.Models.Responses.Modules
{
    public class ModuleResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("joinCode")]
        public string JoinCode { get; set; }

        public ModuleResponse(Module module)
        {
            Id = module.Id;
            Name = module.Name;
            JoinCode = module.JoinCode;
        }
    }
}
