using Domain.Enum;

namespace Domain.Entity
{
    public class CollectionTask
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid CollectionTaskID { get; private set; }
        public Guid CollectionReportID { get; private set; }
        public string Note { get; private set; }
        public string ImageName { get; private set; }
        public double AmountEstimated { get; private set; }
        public CollectionReportStatus Status { get; private set; }
        public DateTime AssignedAt { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime CompletedAt { get; private set; }

        public Guid CollectorProfileID { get; private set; }
        #endregion

        protected CollectionTask() { }

        public CollectionTask(
            Guid collectionTaskId, 
            Guid collectionRequestId, 
            string note,
            string imageName, 
            double amountEstimated)
        {
            CollectionTaskID = collectionTaskId;
            CollectionReportID = collectionRequestId;
            Note = note;
            ImageName = imageName;
            AmountEstimated = amountEstimated;
            Status = CollectionReportStatus.Assigned;
            AssignedAt = DateTime.UtcNow;
        }

        #region Methods
        public void Start()
        {
            Status = CollectionReportStatus.Proceed;
            StartedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            Status = CollectionReportStatus.Collected;
            CompletedAt = DateTime.UtcNow;
        }

        public void Fail()
        {
            Status = CollectionReportStatus.Failed;
            StartedAt = DateTime.UtcNow;
        }
        #endregion
    }
}
