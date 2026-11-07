using System.Text.Json.Serialization;
namespace TrackerEmployeeHours.Models

{
    public class TimeData
    {
        public string? EmployeeName { get; set; }

        [JsonPropertyName("StarTimeUtc")]
        public DateTime? StartTimeUtc { get; set; }

        public DateTime? EndTimeUtc { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
