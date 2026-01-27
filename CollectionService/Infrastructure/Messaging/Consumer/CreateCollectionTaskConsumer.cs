using Application.Helper;
using Application.Interface.IService;
using MassTransit;
using SWD392.MessageBroker;

namespace Infrastructure.Messaging.Consumer
{
    public class CreateCollectionTaskConsumer : IConsumer<CollectionTaskCreateDTO>
    {
        private readonly ICollectionService collectorProfileService;

        public CreateCollectionTaskConsumer(
            ICollectionService collectorProfileService)
        {
            this.collectorProfileService = collectorProfileService;
        }

        public async Task Consume(ConsumeContext<CollectionTaskCreateDTO> context)
        {
            try
            {
                var message = context.Message;
                ServiceLogger.Logging(
                    Level.Infrastructure, $"Create collection task with report ID: {message.CollectionReportID} for assignee: {message.CollectorProfileID}");
                await collectorProfileService.CreateCollectionTaskAsync(message);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(
                    Level.Infrastructure, $"Failed when create collector profile data: {ex.Message}");
            }
        }
    }
}
