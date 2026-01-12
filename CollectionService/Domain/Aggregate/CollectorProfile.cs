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
            Guid collectorProfileID,
            Guid userId,
            string contactInfo, 
            bool isActive = true)
        {
            CollectorProfileID = collectorProfileID;
            UserID = userId;
            ContactInfo = contactInfo;
            IsActive = isActive;
        }

        #region Methods
        public void Deactivate()
        {
            IsActive = false;
        }
        #endregion
    }
}
