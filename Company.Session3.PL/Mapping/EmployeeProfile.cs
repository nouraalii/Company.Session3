using AutoMapper;
using Company.Session3.DAL.Models;
using Company.Session3.PL.Dtos;

namespace Company.Session3.PL.Mapping
{
    //CLR
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<CreateEmployeeDto, Employee>();
            //CreateMap<Employee,CreateDepartmentDto>();
        }
    }
}
