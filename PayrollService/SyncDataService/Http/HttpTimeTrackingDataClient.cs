using System.Text;
using System.Text.Json;
using PayrollService.DTOs;

namespace PayrollService.SyncDataService.Http;

public class HttpTimeTrackingDataClient(HttpClient httpClient, IConfiguration configuration) : IHttpTimeTrackingDataClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    public async Task SendPayrollToTimeTracking(PayrollReadDTOs payroll)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(payroll),
            Encoding.UTF8,
            "application/json"
        );
        Console.WriteLine($"{httpContent}");
        var response = await _httpClient.PostAsync($"{_configuration["TimeTrackingService"]}/api/t/payroll/", httpContent);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Sync POST to Time tracking was OK!");
        }
        else
        {
            Console.WriteLine("Sync POST to Time tracking was FAILED!");
        }
    }
}
