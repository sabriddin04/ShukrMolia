using AutoMapper;
using Domain.DTOs.EmployeeDTOs;
using Domain.DTOs.ShiftDTOs;
using Domain.Entities;

namespace Infrastructure.Automapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Employee, CreateEmployeeDto>().ReverseMap();
            CreateMap<Employee, GetEmployeeDto>().ReverseMap();
            CreateMap<Employee, UpdateEmployeeDto>().ReverseMap();


            CreateMap<Shift, GetShiftDto>().ReverseMap();
            
        }
    }
}

