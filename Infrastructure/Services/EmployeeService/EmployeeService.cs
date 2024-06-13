using System.Data.Common;
using System.Net;
using AutoMapper;
using Domain.DTOs.EmployeeDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Services.EmployeeService;

public class EmployeeService(IMapper _mapper, DataContext _context, IFileService fileService) : IEmployeeService
{

    public async Task<PagedResponse<List<GetEmployeeDto>>> GetEmployeesAsync(EmployeeFilter filter)
    {
        try
        {
            var employees = _context.Employees.AsQueryable();
            if (filter.Name != null)
                employees = employees.Where(x => x.Name.Contains(filter.Name));
            if (filter.Surname != null)
                employees = employees.Where(x => x.Surname.Contains(filter.Surname));
            if (filter.FatherName != null)
                employees = employees.Where(x => x.FatherName.Contains(filter.FatherName));
            if (filter.Position != 0)
                employees = employees.Where(x => x.Position==filter.Position);

            var result = await employees.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .OrderBy(x => x.EmployeeId)
                .ToListAsync();

            var total = await employees.CountAsync();

            var response = _mapper.Map<List<GetEmployeeDto>>(result);
            return new PagedResponse<List<GetEmployeeDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetEmployeeDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }


    public async Task<Response<GetEmployeeDto>> GetEmployeeByIdAsync(int id)
    {
        try
        {
            var existing = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (existing == null) return new Response<GetEmployeeDto>(HttpStatusCode.BadRequest, "Employee not found");
            var employeeDto = _mapper.Map<GetEmployeeDto>(existing);
            return new Response<GetEmployeeDto>(employeeDto);
        }
        catch (Exception e)
        {
            return new Response<GetEmployeeDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }


    public async Task<Response<string>> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
    {
        try
        {

            var employee = new Employee()
            {
                Name = createEmployeeDto.Name,
                Surname = createEmployeeDto.Surname,
                FatherName = createEmployeeDto.FatherName,
                Position = createEmployeeDto.Position,

                Photo = await fileService.CreateFile(createEmployeeDto.Photo!),
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return new Response<string>("Successfully created Employee");

        }
        catch (DbException e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }


    public async Task<Response<bool>> RemoveEmployeeAsync(int employeeId)
    {
        try
        {
            var existing = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            if (existing == null) return new Response<bool>(HttpStatusCode.BadRequest, "Not Found");
            fileService.DeleteFile(existing.Photo!);
            _context.Employees.Remove(existing);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);

        }
        catch (DbException e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }

    }
    public async Task<Response<string>> UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto)
    {
        try
        {
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == updateEmployeeDto.EmployeeId);
            if (existingEmployee == null)
            {
                return new Response<string>(HttpStatusCode.BadRequest, "Employee not Found!");
            }

            if (updateEmployeeDto.Photo != null)
            {
                fileService.DeleteFile(existingEmployee.Photo!);
                existingEmployee.Photo = await fileService.CreateFile(updateEmployeeDto.Photo);
            }

            _mapper.Map(updateEmployeeDto, existingEmployee);


            await _context.SaveChangesAsync();

            return new Response<string>("Succesfully updated");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

      public async Task<Response<List<ViolationReport>>> GetMonthlyViolationReportAsync(uint month, uint year)
    {

        try
        {
            var employees = await _context.Employees.Include(e => e.Shifts).ToListAsync();
            var reports = new List<ViolationReport>();
            foreach (var employee in employees)
            {
                var violations = 0;
                foreach (var shift in employee.Shifts!.Where(s => s.StartTime.Month == month && s.StartTime.Year == year))
                {
                    if (employee.Position.ToString() == "tester")
                    {
                        if (shift.StartTime.Hour > 9 || shift.EndTime.Hour < 21)
                        {
                            violations++;
                        }
                    }
                    else
                    {
                        if (shift.StartTime.Hour > 9 || shift.EndTime.Hour < 18)
                        {
                            violations++;
                        }
                    }
                }

                reports.Add(new ViolationReport
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = $"{employee.Name} {employee.Surname}",
                    Position = employee.Position.ToString(),
                    Violations = violations
                });
            }
                return new Response<List<ViolationReport>>(reports);
        }
        catch (Exception e)
        {
            return new Response<List<ViolationReport>>(HttpStatusCode.InternalServerError, e.Message);
        }
        
    }
}



