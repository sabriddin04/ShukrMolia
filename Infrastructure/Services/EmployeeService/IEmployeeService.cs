
using Domain.DTOs.EmployeeDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.EmployeeService;

public interface IEmployeeService
{ 
    Task<PagedResponse<List<GetEmployeeDto>>> GetEmployeesAsync(EmployeeFilter filter);
    Task<Response<GetEmployeeDto>> GetEmployeeByIdAsync(int employeeId);
    Task<Response<string>> CreateEmployeeAsync(CreateEmployeeDto Employee);
    Task<Response<bool>> RemoveEmployeeAsync(int EmployeeId);
    Task<Response<string>> UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto);
    Task<Response<List<ViolationReport>>> GetMonthlyViolationReportAsync(uint month, uint year);


}
