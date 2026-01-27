using Application.DTO;
using AutoMapper;
using Domain.Aggregate;
using Domain.Entity;

namespace Application.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Collector Profile
            // Entity
            CreateMap<CollectionTask, CollectionTaskDTO>();

            // Aggregate
            CreateMap<CollectorProfile, CollectorProfileDetailDTO>()
                .ForMember(dest => dest.CollectionTasks, opt => opt.MapFrom(src => src.CollectionTasks));
            CreateMap<CollectorProfile, CollectorProfileDTO>();
            #endregion
        }
    }
}
