using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using cat_cafe.Dto;
using cat_cafe.Entities;
namespace cat_cafe.Mappers
{
    public class BarMapper : Profile
    {
        public BarMapper(){

            CreateMap<Bar,BarMapper>().ReverseMap();
        }
       
    }
}
