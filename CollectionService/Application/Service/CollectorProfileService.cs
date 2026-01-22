using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Helper;
using Application.Interface.IService;
using AutoMapper;
using Domain.Aggregate;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using SWD392.MessageBroker;

namespace Application.Service
{
    public class CollectorProfileService : ICollectorProfileService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CollectorProfileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task CreateCollectorProfileAsync(SWD392.MessageBroker.CollectorProfileDTO request)
        {
            var duplicatedExists = await unitOfWork.GetRepository<ICollectorProfileRepository>()
                                .GetAllCollectorProfiles()
                .AnyAsync(x => x.ContactInfo.ToLower() == request.ContactInfo.ToLower() && x.IsActive);

            var collectorProfile = new CollectorProfile(request.UserID, request.ContactInfo, request.IsActive);
            await unitOfWork.BeginTransactionAsync();
            unitOfWork.GetRepository<ICollectorProfileRepository>().Add(collectorProfile);
        }
        public async Task<GenericResult<Application.DTO.CollectorProfileDTO>> AddCollectorProfileAsync(string performedBy, CollectorProfileCreateDTO request)
        {
            var duplicatedExists = await unitOfWork.GetRepository<ICollectorProfileRepository>()
                                .GetAllCollectorProfiles()
                .AnyAsync(x => x.ContactInfo.ToLower() == request.ContactInfo.ToLower() && x.IsActive);
            if (duplicatedExists)
                return GenericResult<Application.DTO.CollectorProfileDTO>.Failure("Collector Profile with the same contact info already exists.");

            var collectorProfile = new CollectorProfile(request.UserID, request.ContactInfo, request.IsActive);
            await unitOfWork.BeginTransactionAsync();
            unitOfWork.GetRepository<ICollectorProfileRepository>().Add(collectorProfile);

            int result = await unitOfWork.CommitAsync(performedBy);
            if (result <= 0)
                return GenericResult<Application.DTO.CollectorProfileDTO>.Failure("Failed to add Collector Profile.");

            return GenericResult<Application.DTO.CollectorProfileDTO>.Success(
                mapper.Map<Application.DTO.CollectorProfileDTO>(collectorProfile),
                "Collector Profile added successfully.");
        }

        public async Task<GenericResult<IEnumerable<Application.DTO.CollectorProfileDTO>>> GetAllCollectorProfile(string sortBy, QueryCollectorProfileDTO dto)
        {
            var collectorProfiles = await GetCollectorProfileWithFilterAsync(
                pageIndex: dto.PageIndex,
                pageLength: dto.PageLength,
                search: dto.Search,
                isActive: dto.isActive
                );

            var res = mapper.Map<IEnumerable<Application.DTO.CollectorProfileDTO>>(collectorProfiles);
            return GenericResult<IEnumerable<Application.DTO.CollectorProfileDTO>>.Success(res);
        }

        public async Task<IEnumerable<Application.DTO.CollectorProfileDTO>> GetCollectorProfileWithFilterAsync(
            int pageIndex,
            int pageLength,
            string? search,
            bool isActive)
        {
            var query = unitOfWork.GetRepository<ICollectorProfileRepository>().GetAllCollectorProfiles().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.Trim().ToLower();
                query = query.Where(x => (!string.IsNullOrEmpty(x.ContactInfo) && x.ContactInfo.ToLower().Contains(searchLower)));
            }

            query = query.Where(x => x.IsActive == isActive);
            query = query.Skip((pageIndex - 1) * pageLength).Take(pageLength);

            var entities = await query.ToListAsync();
            return mapper.Map<IEnumerable<Application.DTO.CollectorProfileDTO>>(entities);
        }
    }
}
