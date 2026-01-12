namespace Domain.DomainException
{
    public class CollectorProfileAggregateException : Exception
    {
        public CollectorProfileAggregateException(string message) : base(message) { }
    }
}
