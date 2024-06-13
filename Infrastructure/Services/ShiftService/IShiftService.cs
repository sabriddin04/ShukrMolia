using Domain.DTOs.ShiftDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.ShiftService;

public interface IShiftService
{
    Task<PagedResponse<List<GetShiftDto>>> GetShiftsAsync(ShiftFilter filter);
    
    Task<Response<string>> StartShiftAsync(int employeeId);
    Task<Response<string>> EndShiftAsync(int employeeId);
}