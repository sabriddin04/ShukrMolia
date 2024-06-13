using Domain.DTOs.EmployeeDTOs;
using Domain.Filters;
using Infrastructure.Services.EmployeeService;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]


public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService employeeService;
    public EmployeeController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    [HttpGet("get-Employees")]
    public async Task<IActionResult> GetEmployeesAsync([FromQuery] EmployeeFilter filter)
    {
        var response= await employeeService.GetEmployeesAsync(filter);
         return StatusCode((int)response.StatusCode!, response);
    }

    [HttpGet("get-EmployeeId/{id:int}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(int id)
    {

        var response = await employeeService.GetEmployeeByIdAsync(id);
        return StatusCode((int)response.StatusCode!, response);

    }


    [HttpPost("add-Employee")]
    public async Task<IActionResult> AddEmployeeAsync([FromForm] CreateEmployeeDto addEmployeeDto)
    {
        var response = await employeeService.CreateEmployeeAsync(addEmployeeDto);
        return StatusCode((int)response.StatusCode!, response);
    }

    [HttpPut("update-Employee")]

    public async Task<IActionResult> UpdateEmployeeAsync([FromForm] UpdateEmployeeDto updateEmployeeDto)
    {
        var response = await employeeService.UpdateEmployeeAsync(updateEmployeeDto);
        return StatusCode((int)response.StatusCode!, response);
    }

    [HttpDelete("delete-EmployeeId/{id:int}")]
    public async Task<IActionResult> DeleteEmployeeAsync(int id)
    {
        var response = await employeeService.RemoveEmployeeAsync(id);
        return StatusCode((int)response.StatusCode!, response);
    }

    [HttpGet("get-Employee-with-Count-Violation/{month:int}/{year:int}")]
    public async Task<IActionResult> GetMonthlyViolationReportAsync(uint month, uint year)
    {
        var response= await employeeService.GetMonthlyViolationReportAsync(month, year);
        return StatusCode((int)response.StatusCode!,response);
    }
}


