using System;
using AutoMapper;
using cat_cafe.Entities;

namespace cat_cafe.Mappers
{
	public class BarMapper:Profile
	{
		public BarMapper()
		{
			CreateMap<Bar, BarMapper>().ReverseMap();
		}
	}
}

