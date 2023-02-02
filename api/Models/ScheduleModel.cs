using Newtonsoft.Json;

namespace api.Models
{
    public class ScheduleModel
    {
        [JsonProperty("group_name")]
        public string? GroupName { get; set; }

        [JsonProperty("trigger_name")]
        public string? TriggerName { get; set; }

        [JsonProperty("job_name")]
        public string? JobName { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}