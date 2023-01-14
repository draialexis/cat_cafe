using AutoMapper;
using cat_cafe.Dto;
using cat_cafe.Entities;

namespace cat_cafe.Mappers
{
    public class CatMapper : Profile
    {
        public CatMapper()
        {
            CreateMap<Cat, CatDto>().ReverseMap();
        }
    }
}
