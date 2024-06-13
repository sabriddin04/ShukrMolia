using Domain.Enums;

namespace Domain.Filters;

public class EmployeeFilter:PaginationFilter
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? FatherName { get; set; }
    public Position Position { get; set; }

}
