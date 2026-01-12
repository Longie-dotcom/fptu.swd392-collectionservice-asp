using SWD392.MessageBroker;

namespace Application.Interface.IPublisher
{
    public interface IEmailSendPublisher
    {
        Task SendEmail(EmailMessageDTO dto);
    }
}
