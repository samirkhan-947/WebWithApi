using AutoMapper;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.ParkyMapper
{
    public class ParkyMappings:Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParksDtos>().ReverseMap();
            CreateMap<Trail,TrailDtos> ().ReverseMap();
            CreateMap<Trail, TrailCreateDtos>().ReverseMap();
            CreateMap<Trail, TrailUpdateDtos>().ReverseMap();
        }
    }
}
