using System.Collections;
using Domain.Abstraction;
using Domain.Enum;

namespace Domain.Entity
{
    public class CollectionTask : SoftDeletedEntity
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
            Guid collectorProfileId,
            string note,
            string imageName,
            double amountEstimated)
        {
            CollectionTaskID = collectionTaskId;
            CollectionReportID = collectionReportId;
            CollectorProfileID = collectorProfileId;
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

        public (bool Success, string? ErrorMessage) CheckDeleted()
        {
            if (StartedAt != default(DateTime))
            {
                return (false, "Collection Task is already started and cannot be deleted.");
            }

            if (Status == CollectionReportStatus.Collected || Status == CollectionReportStatus.Failed)
            {
                return (false, "Collection Task is already completed or failed and cannot be deleted.");
            }

            if(CollectionReportID != Guid.Empty)
            {
                return (false, "Collection Task is associated with a Collection Report and cannot be deleted.");
            }

            return (true, null);
        } 


        public void UpdateDetails(string? note, string? imageName, double? amountEstimated, Guid? collectorProfileId)
        {
            if(StartedAt != default(DateTime) || CompletedAt.HasValue)
                throw new InvalidOperationException("Cannot update task that has started or completed.");
            if(!string.IsNullOrWhiteSpace(note)) 
                Note = note;
            if(!string.IsNullOrWhiteSpace(imageName))
                ImageName = imageName;
            if(amountEstimated.HasValue && amountEstimated.Value > 0)
                AmountEstimated = amountEstimated.Value;
            if(collectorProfileId.HasValue && collectorProfileId.Value != Guid.Empty)
                CollectorProfileID = collectorProfileId.Value;
        }

        public void AssignCollector(Guid collectorProfileId)
        {
            if(StartedAt != default(DateTime))
                throw new InvalidOperationException("Cannot reassign collector after task started");

            if (collectorProfileId == Guid.Empty)
                throw new ArgumentException("Invalid collector profile ID");

            CollectorProfileID = collectorProfileId;
        }
        #endregion
    }
}
