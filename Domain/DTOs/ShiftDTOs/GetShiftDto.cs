namespace Domain.DTOs.ShiftDTOs;

public class GetShiftDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan HoursWorked { get; set; }
    public int EmployeeId { get; set; }
}
