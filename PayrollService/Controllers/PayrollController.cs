using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PayrollService.Data;
using PayrollService.DTOs;
using PayrollService.Models;
using PayrollService.SyncDataService.Http;

namespace PayrollService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PayrollController(
    IPayrollRepo repository,
    IMapper mapper,
    IHttpTimeTrackingDataClient httpTimeTrackingDataClient) : ControllerBase
{
    private readonly IPayrollRepo _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IHttpTimeTrackingDataClient _httpTimeTrackingDataClient = httpTimeTrackingDataClient;

    [HttpGet]
    public ActionResult<IEnumerable<Payroll>> GetPayroll()
    {
        Console.WriteLine("---> Getting payroll....");
        var payrollItems = _repository.GetPayrolls();
        return Ok(_mapper.Map<IEnumerable<PayrollReadDTOs>>(payrollItems));
    }
    [HttpGet("{id}", Name = "GetPayrollById")]
    public ActionResult<Payroll> GetPayrollById(int id)
    {
        Console.WriteLine("---> Getting payroll by ID....");
        var payrollItem = _repository.GetPayrollById(id);
        if (payrollItem != null)
        {
            return Ok(_mapper.Map<PayrollReadDTOs>(payrollItem));
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<ActionResult<PayrollReadDTOs>> CreatePayroll(PayrollCreateDTO payrollCreateDTO)
    {
        Console.WriteLine("--> Creatign a payroll...");
        var payroll = _mapper.Map<Payroll>(payrollCreateDTO);
        _repository.CreatePayroll(payroll);
        _repository.SaveChanges();
        var payrollReadDTO = _mapper.Map<PayrollReadDTOs>(payroll);

        //Communicating with TimeTracking service inside Payroll service
        try
        {
            await _httpTimeTrackingDataClient.SendPayrollToTimeTracking(payrollReadDTO);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message} Could not send synchronously");
        }

        return CreatedAtRoute(nameof(GetPayrollById), new { payrollReadDTO.Id }, payrollReadDTO);
    }
}