using System;
using AutoMapper;
using cat_cafe.Dto;
using cat_cafe.Entities;

namespace cat_cafe.Mappers
{
	public class BarMapper:Profile
	{
		public BarMapper()
		{
             
           // var mapper = config.CreateMapper();
            CreateMap<Bar, BarDto>().ReverseMap();
		}
	}
}

