using AutoMapper;

using ParkAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkAPI.Models;

namespace ParkAPI.ParkMapper
{   //mapping using automapper
    public class ParkMappings : Profile
    {
        public ParkMappings()
        {   //both way conversion/mapping NationalPark <=> NationalParkDto
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
        }

    }
}
