using SWD392.MessageBroker;

namespace Application.Interface.IPublisher
{
    public interface ICollectionReportStatusUpdatePublisher
    {
        Task UpdateCollectionReportStatus(CollectionReportStatusUpdateDTO dto);
    }
}
