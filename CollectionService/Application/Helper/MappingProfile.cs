using System.Security.Cryptography.X509Certificates;
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
            // Aggregate
            CreateMap<CollectionTask, CollectionTaskDTO>();
            CreateMap<CollectorProfile, CollectorProfileDTO>();

            #endregion
        }
    }
}
