using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using TrackerEmployeeHours.Models;
namespace TrackerEmployeeHours.Services
{
    public class TimeDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TimeDataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<TimeData>> GetTimeEntriesAsync()
        {
            string apiKey = _configuration["ApiSettings:Key"]
                ?? throw new InvalidOperationException("Cannot find API key.");

            string url = $"https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code={Uri.EscapeDataString(apiKey)}";

            var employees = await _httpClient.GetFromJsonAsync<List<TimeData>>(url);

            return employees ?? new List<TimeData>();
        }

        public async Task<List<EmployeeHoursSum>> GetEmployeeHoursAsync()
        {
            var timeEntries = await GetTimeEntriesAsync();

            var employeeHours = timeEntries
                .Where(e => !string.IsNullOrWhiteSpace(e.EmployeeName)
                            && e.StartTimeUtc.HasValue
                            && e.EndTimeUtc.HasValue
                            && e.DeletedOn == null
                            && e.EndTimeUtc.Value > e.StartTimeUtc.Value)
                .GroupBy(e => e.EmployeeName!)
                .Select(group => new EmployeeHoursSum
                {
                    EmployeeName = group.Key,
                    TotalHours = group.Sum(e =>
                        (e.EndTimeUtc!.Value - e.StartTimeUtc!.Value).TotalHours)
                })
                .OrderByDescending(e => e.TotalHours)
                .ToList();

            return employeeHours;
        }
    }
}
