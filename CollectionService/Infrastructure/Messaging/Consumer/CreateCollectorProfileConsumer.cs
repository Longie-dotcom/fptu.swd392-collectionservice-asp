using Application.Helper;
using Application.Interface.IService;
using MassTransit;
using SWD392.MessageBroker;

namespace Infrastructure.Messaging.Consumer
{
    public class CreateCollectorProfileConsumer : IConsumer<CollectorProfileDTO>
    {
        private readonly ICollectorProfileService collectorProfileService;

        public CreateCollectorProfileConsumer(
            ICollectorProfileService collectorProfileService)
        {
            this.collectorProfileService = collectorProfileService;
        }

        public async Task Consume(ConsumeContext<CollectorProfileDTO> context)
        {
            try
            {
                var message = context.Message;
                ServiceLogger.Logging(
                    Level.Infrastructure, $"Create collector profile data: {message.UserID}");
                await collectorProfileService.CreateCollectorProfileAsync(message);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(
                    Level.Infrastructure, $"Failed when create collector profile data: {ex.Message}");
            }
        }


    }
}
