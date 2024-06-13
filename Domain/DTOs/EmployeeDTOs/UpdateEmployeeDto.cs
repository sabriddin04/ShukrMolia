using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.EmployeeDTOs;

public class UpdateEmployeeDto
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string FatherName { get; set; }=null!;
    public Position Position { get; set; }
    public IFormFile? Photo { get; set; }
}
