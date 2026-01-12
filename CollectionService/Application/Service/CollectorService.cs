using Application.ApplicationException;
using Application.DTO;
using Application.Enum;
using Application.Interface.IPublisher;
using Application.Interface.IService;
using AutoMapper;
using Domain.Aggregate;
using Domain.IRepository;
using IAMServer.gRPC;
using SWD392.MessageBroker;

namespace Application.Service
{
    public class CollectorService : ICollectorService
    {
        #region Attributes
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        #endregion

        #region Properties
        #endregion

        public CollectorService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        #region Methods
        public Task UserSyncDeleting(UserDeleteDTO dto)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
