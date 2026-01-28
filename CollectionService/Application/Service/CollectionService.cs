using Application.ApplicationException;
using Application.DTO;
using Application.Interface.IService;
using AutoMapper;
using Domain.Aggregate;
using Domain.Entity;
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


        public async Task<IEnumerable<CollectionTaskDTO>> GetCollectionTasks(
            Guid callerId, 
            QueryCollectorTaskDTO dto)
        {
            // Validate collector profile existence
            var profile = await unitOfWork
                .GetRepository<ICollectorProfileRepository>()
                .GetCollectorProfileByUserId(callerId);

            if (profile == null)
                throw new CollectorProfileNotFound(
                    $"Collector profile with user ID: {callerId} is not found");

            // Validate task list existence
            var list = await unitOfWork
                .GetRepository<ICollectionTaskRepository>()
                .GetCollectionTasksByCollectorIdAsync(
                profile.CollectorProfileID,
                dto.SortBy,
                dto.PageIndex,
                dto.PageLength,
                dto.AssignedAt,
                dto.StartAt,
                dto.Status);

            if(list == null || !list.Any())
                throw new CollectionTaskNotFound(
                    $"Collection task list is not found or empty for collector profile ID: {profile.CollectorProfileID}");

            return mapper.Map<IEnumerable<CollectionTaskDTO>>(list);
        }

        public async Task CreateCollectionTaskAsync(
            SWD392.MessageBroker.CollectionTaskCreateDTO dto)
        {
            // Validate collector profile existence
            var profile = await unitOfWork
                .GetRepository<ICollectorProfileRepository>()
                .GetCollectorProfileByUserId(dto.CollectorUserID);

            if (profile == null)
                throw new CollectorProfileNotFound(
                    $"Collector profile with user ID: {dto.CollectorUserID} is not found");

            // Apply domain
            var collectionTask = new CollectionTask(
                Guid.NewGuid(),
                dto.CollectionReportID,
                profile.CollectorProfileID,
                string.Empty,
                string.Empty,
                0);

            // Apply persistence
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<ICollectionTaskRepository>()
                .Add(collectionTask);
            await unitOfWork.CommitAsync("System");
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
