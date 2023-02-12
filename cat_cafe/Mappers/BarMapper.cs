using AutoMapper;
using cat_cafe.Dto;
using cat_cafe.Entities;

namespace cat_cafe.Mappers
{
    public class BarMapper : Profile
    {
        public BarMapper()
        {
            CreateMap<Bar, BarDto>()
                .ForMember(dest => dest.CatIds, opt => opt.MapFrom(src => src.Cats.Select(c => c.Id)));

            CreateMap<BarDto, Bar>();
        }
    }
}
