namespace Domain.DTOs.EmployeeDTOs;

public class ViolationReport
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }=null!;
    public string Position { get; set; }=null!;
    public int Violations { get; set; }
}

