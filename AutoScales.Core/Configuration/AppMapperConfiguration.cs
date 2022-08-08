using AutoMapper;
using AutoScales.Data.Entities;
using AutoScales.Shared.Dtos;
using AutoScales.Shared.Views;

namespace AutoScales.Core.Configuration
{
    public class AppMapperConfiguration : Profile
    {
        public AppMapperConfiguration()
        {
            CreateMap<Transport, TransportDto>();
            CreateMap<TransportDto, Transport>()
                .ForMember(x => x.Created, _ => _.Ignore())
                .ForMember(x => x.Updated, _ => _.Ignore());

            CreateMap<TransportView, TransportDto>()
                .ForMember(x => x.Id, _ => _.Ignore())
                .ForMember(x => x.IsDeleted, _ => _.Ignore());
            CreateMap<TransportDto, TransportView>();
            CreateMap<Transport, TransportView>();

            CreateMap<Journal, JournalDto>();
            CreateMap<JournalDto, Journal>()
                .ForMember(x => x.Created, _ => _.Ignore())
                .ForMember(x => x.Updated, _ => _.Ignore());

            CreateMap<JournalView, JournalDto>()
                .ForMember(x => x.Id, _ => _.Ignore())
                .ForMember(x => x.IsDeleted, _ => _.Ignore());
            CreateMap<JournalDto, JournalView>();
        }
    }
}
