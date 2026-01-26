using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public async Task<GenericResult<Application.DTO.CollectorProfileDTO>> CreateCollectorProfileAsync(string createdBy, CollectorProfileCreateDTO dto)
        {
            var duplicatedExists = await unitOfWork.GetRepository<ICollectorProfileRepository>()
                .GetAllCollectorProfiles()
                .AnyAsync(x => x.ContactInfo.ToLower() == dto.ContactInfo.ToLower() && x.IsActive);

            if(duplicatedExists)
                return GenericResult<Application.DTO.CollectorProfileDTO>.Failure("Collector Profile with the same contact infor already exists.");

            var collectorProfile = new CollectorProfile(dto.UserID, dto.ContactInfo, dto.IsActive);
            await unitOfWork.BeginTransactionAsync();
            unitOfWork.GetRepository<ICollectorProfileRepository>().Add(collectorProfile);

            int result = await unitOfWork.CommitAsync(createdBy);
            if(result <= 0)
                return GenericResult<Application.DTO.CollectorProfileDTO>.Failure("Failed to create Collector Profile.");

            var savedProfile = await unitOfWork.GetRepository<ICollectorProfileRepository>().GetByIdAsync(collectorProfile.CollectorProfileID);

            return GenericResult <Application.DTO.CollectorProfileDTO>.Success(
                mapper.Map<Application.DTO.CollectorProfileDTO>(savedProfile),
                "Collector Profile created successfully.");
        }

        public async Task<GenericResult<IEnumerable<Application.DTO.CollectorProfileDTO>>> GetAllCollectorProfile(string sortBy, QueryCollectorProfileDTO dto)
        {
            var collectorProfiles = await GetCollectorProfileWithFilterAsync(
                pageIndex: dto.PageIndex,
                pageLength: dto.PageLength,
                search: dto.Search,
                isActive: dto.isActive,
                sortBy: sortBy
                );

            return GenericResult<IEnumerable<Application.DTO.CollectorProfileDTO>>.Success(collectorProfiles);
        }

        public async Task<IEnumerable<Application.DTO.CollectorProfileDTO>> GetCollectorProfileWithFilterAsync(
            int pageIndex,
            int pageLength,
            string? search,
            bool? isActive,
            string? sortBy)
        {
            var query = unitOfWork.GetRepository<ICollectorProfileRepository>().GetAllCollectorProfiles();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.Trim().ToLower();
                query = query.Where(x => x.ContactInfo.ToLower().Contains(searchLower));
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            query = ApplySorting(query, sortBy);

            query = query.Skip((pageIndex - 1) * pageLength).Take(pageLength);

            var entities = await query.ToListAsync();
            return mapper.Map<IEnumerable<Application.DTO.CollectorProfileDTO>>(entities);
        }

        private IQueryable<CollectorProfile> ApplySorting(IQueryable<CollectorProfile> query, string? sortBy)
        {
            return sortBy?.Trim().ToLower() switch
            {
                "contactinfor_desc" => query.OrderByDescending(cp => cp.ContactInfo),
                _ => query.OrderBy(cp => cp.ContactInfo)
            };
        }

        public async Task<GenericResult<Application.DTO.CollectorProfileDTO>> GetCollectorProfileByIdAsync(Guid collectorProfileId)
        {
            var entity = await unitOfWork.GetRepository<ICollectorProfileRepository>()
                .GetByIdAsync(collectorProfileId);
            if (entity == null)
            {
                return GenericResult<Application.DTO.CollectorProfileDTO>.Failure("Collector Profile not found");
            }
            return GenericResult<Application.DTO.CollectorProfileDTO>.Success(
                    mapper.Map<Application.DTO.CollectorProfileDTO>(entity));
        }

        public async Task<GenericResult<Application.DTO.CollectorProfileDTO>> UpdateCollectorProfileAsync(Guid collectorProfileId, CollectorProfileUpdateDTO dto, string updatedBy)
        {
            var entity = await unitOfWork.GetRepository<ICollectorProfileRepository>().GetByIdAsync(collectorProfileId);

            if (entity == null)
                return GenericResult<Application.DTO.CollectorProfileDTO>.Failure("Collector Profile not found");

            var duplicatedExists = await unitOfWork.GetRepository<ICollectorProfileRepository>().GetAllCollectorProfiles()
                .AnyAsync(x => x.ContactInfo.ToLower() == dto.ContactInfo.ToLower()
                && x.CollectorProfileID != collectorProfileId
                && x.IsActive);

            if (duplicatedExists)
                return GenericResult<Application.DTO.CollectorProfileDTO>.Failure("Contact infor already exists in another active profile.");

            await unitOfWork.BeginTransactionAsync();
            unitOfWork.GetRepository<ICollectorProfileRepository>().Update(collectorProfileId, entity);

            int result = await unitOfWork.CommitAsync(updatedBy);
            if(result <= 0)
            {
                await unitOfWork.RollbackAsync();
                return GenericResult<Application.DTO.CollectorProfileDTO>.Failure("Failed to update Collector Profile.");
            }

            return GenericResult<Application.DTO.CollectorProfileDTO>.Success(mapper.Map<Application.DTO.CollectorProfileDTO>(entity));
        }

        public async Task<(bool Success, string? Error)> DeleteCollectorProfileAsync(Guid collectorProfileId, string deletedBy)
        {
            await unitOfWork.BeginTransactionAsync();

            var entity = await unitOfWork.GetRepository<ICollectorProfileRepository>()
                .GetByIdAsync(collectorProfileId);

            if (entity == null)
                return (false, "Collector Profile not found");

            if (!entity.IsActive)
            {
                return (false, "COllector Profile is already deleted");
            }

            entity.Deactivate();
            unitOfWork.GetRepository<ICollectorProfileRepository>().Update(collectorProfileId, entity);

            int result = await unitOfWork.CommitAsync();
            return (result > 0, null);
        }

        public async Task<GenericResult<IEnumerable<Application.DTO.CollectorProfileDTO>>> GetActiveCollectorsAsync(string? wardCode)
        {
            var query = unitOfWork.GetRepository<ICollectorProfileRepository>()
                .GetAllCollectorProfiles()
                .Where(x => x.IsActive);

            var entities = await query.OrderBy(x => x.ContactInfo).ToListAsync();

            return GenericResult<IEnumerable<Application.DTO.CollectorProfileDTO>>.Success(mapper.Map<IEnumerable<Application.DTO.CollectorProfileDTO>>(entities));
        }

        public async Task<GenericResult<Application.DTO.CollectorProfileDTO>> GetCollectorProfileByUserIdAsync(string userId)
        {
            var entity = await unitOfWork.GetRepository<ICollectorProfileRepository>()
                .GetAllCollectorProfiles()
                .FirstOrDefaultAsync(x => x.UserID == Guid.Parse(userId) && x.IsActive);

            if (entity == null)
                return GenericResult<Application.DTO.CollectorProfileDTO>.Failure("Collector profile not found for this user.");

            return GenericResult<Application.DTO.CollectorProfileDTO>.Success(mapper.Map<Application.DTO.CollectorProfileDTO>(entity));

        }
    }
}
