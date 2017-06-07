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
            CreateMap<TimeSheet, TimeSheetDto>()
                .ForMember(
                    dest => dest._id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest._attachments,
                    opt => opt.MapFrom(src => src.Attachments));

            CreateMap<TimeSheetDto, TimeSheet>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src._id))
                .ForMember(
                    dest => dest.RevId,
                    opt => opt.MapFrom(src => src._rev))
                .ForMember(
                    dest => dest.Attachments,
                    opt => opt.MapFrom(src => src._attachments));

            CreateMap<AttachmentDto, Attachment>();
            CreateMap<Attachment, AttachmentDto>();

            CreateMap<TimeSheetStatus, int>().ConvertUsing(src => (int) src);
            CreateMap<int, TimeSheetStatus>()
                .ConvertUsing(src => (TimeSheetStatus) Enum.Parse(typeof(TimeSheetStatus), src.ToString()));
        }
    }
}