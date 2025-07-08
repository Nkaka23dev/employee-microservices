using AutoMapper;
using BenefitSoapService.Contacts;
using Core.Domain.DTOs.Benefits;
using Core.Infrastructure.Repositories;
using CoreWCF;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Entities;

namespace BenefitSoapService.Services;

[ServiceBehavior(Namespace = "http://benefitsoapservice.com/")]
public class BenefitService(
    IRepository<Benefit> benefitRepository,
    IMapper mapper,
    ILogger<BenefitService> logger) : IBenefitService
{
    private readonly IRepository<Benefit> _benefitRepository = benefitRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    public async Task<IEnumerable<GetBenefitResponse>> GetAllBenefits()
    {
        try
        {
            IQueryable<Benefit> query = _benefitRepository.GetQuery();
            var benefits = await query.ToArrayAsync();
            return _mapper.Map<List<GetBenefitResponse>>(benefits);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllBenefits");
            throw;
        }
    }

    public async Task<BenefitContract> CreateBenefit(BenefitContract request)
    {
        var newBenefit = _mapper.Map<Benefit>(request);
        await _benefitRepository.AddAsync(newBenefit);
        return _mapper.Map<BenefitContract>(newBenefit);
    }

    public async Task<GetBenefitResponse> UpdateBenefit(int id, UpdateBenefit request)
    {
        var existingBenefit = await _benefitRepository.GetByIdAsync(id);
        if (existingBenefit == null)
        {
            _logger.LogWarning("Benefit with ID {employeeId} NOT FOUND!", id);
            throw new KeyNotFoundException($"Employee with {id} not found!");
        }
        _mapper.Map(request, existingBenefit);
        await _benefitRepository.UpdateAsync(existingBenefit);
        var benefitResponse = _mapper.Map<GetBenefitResponse>(existingBenefit);
        return benefitResponse;
    }

    public async Task DeleteBenefit(int id)
    {
        await _benefitRepository.DeleteAsync(id);
    }
}
