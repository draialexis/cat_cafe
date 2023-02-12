using AutoMapper;
using cat_cafe.Dto;
using cat_cafe.Entities;

namespace cat_cafe.Mappers
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
}

