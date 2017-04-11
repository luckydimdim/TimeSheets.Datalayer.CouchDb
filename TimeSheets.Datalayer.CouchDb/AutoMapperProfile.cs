using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.BusinessLayers.TimeSheets.Entities;
using System;

namespace Cmas.DataLayers.CouchDb.TimeSheets
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TimeSheet, TimeSheetDto>();
            CreateMap<TimeSheetDto, TimeSheet>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src._id))
                .ForMember(
                    dest => dest.RevId,
                    opt => opt.MapFrom(src => src._rev));

            CreateMap<TimeSheetStatus, int>().ConvertUsing(src => (int)src);
            CreateMap<int, TimeSheetStatus>().ConvertUsing(src => (TimeSheetStatus)Enum.Parse(typeof(TimeSheetStatus), src.ToString()));

        }
    }
}
