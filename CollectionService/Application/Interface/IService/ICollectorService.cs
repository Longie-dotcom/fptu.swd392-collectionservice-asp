namespace Application.Interface.IService
{
    public interface ICollectorService
    {
        public Task UserSyncDeleting(
            SWD392.MessageBroker.UserDeleteDTO dto);
    }
}