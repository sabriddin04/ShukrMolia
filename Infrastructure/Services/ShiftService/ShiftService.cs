using System.Net;
using AutoMapper;
using Domain.DTOs.EmployeeDTOs;
using Domain.DTOs.ShiftDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ShiftService;
public class ShiftService(DataContext _context,IMapper mapper) : IShiftService
{
 public async Task<Response<string>> StartShiftAsync(int employeeId)
        {
            try
            {
                var employee = await _context.Employees.Include(e => e.Shifts).FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
                if (employee == null)
                {
                    return new Response<string>(HttpStatusCode.BadRequest, "Not Found Employee");
                }

                if (employee.Shifts!.Any(s => s.EndTime == DateTime.MinValue))
                {
                    return new Response<string>(HttpStatusCode.BadRequest, "Previous shift not ended.");
                }

                var shift = new Shift
                {
                    StartTime = DateTime.UtcNow.AddHours(5),
                    EmployeeId = employeeId
                };

                _context.Shifts.Add(shift);
                await _context.SaveChangesAsync();

                return new Response<string>("Succesfully Start Shift");
            }
            catch (Exception e)
            {
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<string>> EndShiftAsync(int employeeId)
        {
            try
            {
                var employee = await _context.Employees.Include(e => e.Shifts).FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
                if (employee == null)
                {
                    return new Response<string>(HttpStatusCode.BadRequest, "Not Found Employee");
                }

                var shift = employee.Shifts?.LastOrDefault(s => s.EndTime == DateTime.MinValue);
                if (shift == null)
                {
                    return new Response<string>(HttpStatusCode.BadRequest, "No active shift found.");
                }

                shift.EndTime = DateTime.UtcNow.AddHours(5);
                shift.HoursWorked = shift.EndTime - shift.StartTime;

                await _context.SaveChangesAsync();

                return new Response<string>("Succesfully End Shift");
            }
            catch (Exception e)
            {
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }



      public async Task<PagedResponse<List<GetShiftDto>>> GetShiftsAsync(ShiftFilter filter)
    {
        try
        {
            var shifts = _context.Shifts.AsQueryable();
            if (filter.EmployeeId != 0)
                shifts = shifts.Where(x => x.EmployeeId==filter.EmployeeId);

            var result = await shifts.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .OrderBy(x => x.EmployeeId)
                .ToListAsync();

            var total = await shifts.CountAsync();
            var response = mapper.Map<List<GetShiftDto>>(result);
            return new PagedResponse<List<GetShiftDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetShiftDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}
