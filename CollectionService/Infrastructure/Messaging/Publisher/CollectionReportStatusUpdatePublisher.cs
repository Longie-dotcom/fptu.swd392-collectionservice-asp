using Application.Helper;
using Application.Interface.IPublisher;
using MassTransit;
using SWD392.MessageBroker;

namespace Infrastructure.Messaging.Publisher
{
    public class CollectionReportStatusUpdatePublisher : ICollectionReportStatusUpdatePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public CollectionReportStatusUpdatePublisher(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task UpdateCollectionReportStatus(CollectionReportStatusUpdateDTO dto)
        {
            ServiceLogger.Logging(
                Level.Infrastructure, $"Publishing updated status: {dto.Status} for collection report with ID: {dto.CollectionReportID}");
            await _publishEndpoint.Publish(dto);
        }
    }
}