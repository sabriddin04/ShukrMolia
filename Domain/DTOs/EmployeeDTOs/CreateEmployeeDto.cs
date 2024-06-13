using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.EmployeeDTOs;


public class CreateEmployeeDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string FatherName { get; set; }=null!;
    public Position Position { get; set; }
    public IFormFile?  Photo { get; set; }
    
}
