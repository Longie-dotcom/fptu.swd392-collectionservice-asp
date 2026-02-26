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
        public string? Note { get; private set; }
        public string? ImageName { get; private set; }
        public double AmountEstimated { get; private set; }
        public CollectionReportStatus Status { get; private set; }
        public DateTime AssignedAt { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        public Guid CollectorProfileID { get; private set; }
        #endregion

        protected CollectionTask() { }

        public CollectionTask(
            Guid collectionTaskId,
            Guid collectionReportId,
            Guid collectorProfileId)
        {
            CollectionTaskID = collectionTaskId;
            CollectionReportID = collectionReportId;
            CollectorProfileID = collectorProfileId;
            Status = CollectionReportStatus.Assigned;
            AssignedAt = DateTime.UtcNow;
        }

        #region Methods
        public void Complete(
            string? note,
            double amountEstimated,
            string imageName)
        {
            Note = note;
            AmountEstimated = amountEstimated;
            ImageName = imageName;

            Status = CollectionReportStatus.Collected;
            CompletedAt = DateTime.UtcNow;
        }
        #endregion
    }
}
