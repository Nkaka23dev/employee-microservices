using PayrollService.DTOs;

namespace PayrollService.SyncDataService.Http;

public interface IHttpTimeTrackingDataClient
{
    Task SendPayrollToTimeTracking(PayrollReadDTOs payroll);
}
