using Application.Helper;
using Application.Interface.IService;
using MassTransit;
using SWD392.MessageBroker;

namespace Infrastructure.Messaging.Consumer
{
    public class UserDeleteConsumer : IConsumer<UserDeleteDTO>
    {
        private readonly ICollectionService collectorService;

        public UserDeleteConsumer(
            ICollectionService collectorService)
        {
            this.collectorService = collectorService;
        }

        public async Task Consume(ConsumeContext<UserDeleteDTO> context)
        {
            try
            {
                var message = context.Message;
                ServiceLogger.Logging(
                    Level.Infrastructure, $"Delete user data: {message.UserID}");
                await collectorService.UserSyncDeleting(message);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(
                    Level.Infrastructure, $"Failed when delete user data: {ex.Message}");
            }
        }
    }
}
