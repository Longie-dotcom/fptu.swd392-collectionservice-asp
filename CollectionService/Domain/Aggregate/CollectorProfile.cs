using Domain.DomainException;
using Domain.Entity;

namespace Domain.Aggregate
{
    public class CollectorProfile
    {
        #region Attributes
        private readonly List<CollectionTask> collectionTasks = new List<CollectionTask>();
        #endregion

        #region Properties
        public Guid CollectorProfileID { get; private set; }
        public Guid UserID { get; private set; }
        public string ContactInfo { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyCollection<CollectionTask> CollectionTasks 
        {
            get { return collectionTasks.AsReadOnly(); }
        }
        #endregion

        protected CollectorProfile() { }

        public CollectorProfile(
            Guid userId,
            string contactInfo, 
            bool isActive = true)
        {
            CollectorProfileID = new Guid();
            UserID = userId;
            ContactInfo = contactInfo;
            IsActive = isActive;
        }

        #region Methods
        public void Deactivate()
        {
            IsActive = false;
        }

        public CollectionTask AssignTask(
            Guid collectionTaskId,
            Guid collectionReportId)
        {
            var collectionTask = new CollectionTask(
                collectionTaskId,
                collectionReportId,
                CollectorProfileID);

            collectionTasks.Add(collectionTask);

            return collectionTask;
        }

        public void FinishTask(
            Guid collectionTaskId,
            string imageName,
            string? note,
            double amountEstimated)
        {
            var task = collectionTasks.FirstOrDefault(
                ct => ct.CollectionTaskID == collectionTaskId);

            if (task == null)
                throw new CollectorProfileAggregateException(
                    $"Collection task with ID: {collectionTaskId} is not found");

            task.Complete(note, amountEstimated, imageName);
        }
        #endregion
    }
}
