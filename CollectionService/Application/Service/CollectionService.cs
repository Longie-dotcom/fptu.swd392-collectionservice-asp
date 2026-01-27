using Application.ApplicationException;
using Application.DTO;
using Application.Interface.IService;
using AutoMapper;
using Domain.Aggregate;
using Domain.IRepository;

namespace Application.Service
{
    public class CollectionService : ICollectionService
    {
        #region Attributes
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public CollectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        #region Methods
        public async Task<IEnumerable<CollectorProfileDTO>> GetCollectorProfiles(
            QueryCollectorProfileDTO dto)
        {
            // Validate collector profile list existence
            var list = await unitOfWork
                .GetRepository<ICollectorProfileRepository>()
                .QueryCollectorProfiles(
                    dto.PageIndex,
                    dto.PageLength,
                    dto.Search,
                    dto.IsActive);

            if (list == null || !list.Any())
                throw new CollectorProfileNotFound(
                    $"Collector profile list is not found or empty");

            return mapper.Map<IEnumerable<CollectorProfileDTO>>(list);
        }

        public async Task<CollectorProfileDetailDTO> GetCollectorProfileDetail(
            Guid collectorProfileId)
        {
            // Validate collector profile existence
            var collectorProfile = await unitOfWork
                .GetRepository<ICollectorProfileRepository>()
                .GetCollectorProfileDetailById(collectorProfileId);

            if (collectorProfile == null)
                throw new CollectorProfileNotFound(
                    $"Collector profile with ID: {collectorProfileId} has not been found");

            return mapper.Map<CollectorProfileDetailDTO>(collectorProfile);
        }


        public Task<IEnumerable<CollectionTaskDTO>> GetCollectionTasks(
            Guid callerId, 
            QueryCollectorTaskDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task CreateCollectionTaskAsync(
            SWD392.MessageBroker.CollectionTaskCreateDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCollectorProfileAsync(
            SWD392.MessageBroker.CollectorProfileDTO request)
        {
            // Validate duplicated contact info
            var profileByContactInfo = await unitOfWork
                .GetRepository<ICollectorProfileRepository>()
                .GetCollectorProfileByContactInfo(request.ContactInfo);

            if (profileByContactInfo != null)
                throw new InvalidOperationException(
                    $"CollectorProfile with ContactInfo '{request.ContactInfo}' already exists and is active.");

            // Apply domain
            var collectorProfile = new CollectorProfile(
                request.UserID,
                request.ContactInfo,
                request.IsActive);

            // Apply persistence
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<ICollectorProfileRepository>()
                .Add(collectorProfile);
            await unitOfWork.CommitAsync("System");
        }

        public async Task UserSyncDeleting(
            SWD392.MessageBroker.UserDeleteDTO dto)
        {
            // Validate citizen profile existence
            var profile = await unitOfWork
                .GetRepository<ICollectorProfileRepository>()
                .GetCollectorProfileByUserId(dto.UserID);

            if (profile == null)
                throw new CollectorProfileNotFound(
                    $"The collector profile with user ID: {dto.UserID} is not found");

            // Apply domain
            profile.Deactivate();

            // Apply persistence;
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<ICollectorProfileRepository>()
                .Update(profile.CollectorProfileID, profile);
            await unitOfWork.CommitAsync();
        }
        #endregion
    }
}
