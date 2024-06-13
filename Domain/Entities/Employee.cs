using Domain.Enums;

namespace Domain.Entities;

public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string FatherName { get; set; }=null!;
    public Position Position { get; set; }

    public string? Photo { get; set; }

    public List<Shift>? Shifts { get; set; }

}
