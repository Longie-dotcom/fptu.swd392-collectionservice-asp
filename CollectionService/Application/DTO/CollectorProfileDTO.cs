using Domain.Enum;

namespace Application.DTO
{
    public class CollectorProfileDTO
    {
        public Guid CollectorProfileID { get; set; }
        public Guid UserID { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
        public bool IsActive { get; set; }

    }

    public class CollectorProfileDetailDTO
    {
        public Guid CollectorProfileID { get; set; }
        public Guid UserID { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<CollectionTaskDTO> CollectionTasks { get; set; } = new();
    }

    public class QueryCollectorProfileDTO
    {
        public int PageIndex { get; set; } = 1;
        public int PageLength { get; set; } = 10;
        public string? Search { get; set; } = string.Empty;
        public bool? IsActive { get; set; } = true;
    }

    public class CollectionTaskDTO
    {
        public Guid CollectionTaskID { get; set; }
        public Guid CollectionReportID { get; set; }
        public string? Note { get; set; }
        public string? ImageName { get; set; }
        public double AmountEstimated { get; set; }
        public CollectionReportStatus Status { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid CollectorProfileID { get; set; }
    }

    public class QueryMyCollectionTaskDTO
    {
        public string? SortBy { get; set; } = string.Empty;
        public int PageIndex { get; set; } = 1;
        public int PageLength { get; set; } = 10;
        public DateTime? AssignedAt { get; set; }
        public DateTime? StartAt { get; set; }
        public CollectionReportStatus? Status { get; set; } = CollectionReportStatus.Pending;
    }

    public class SubmitProofDTO
    {
        public Guid CollectionTaskID { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public double AmountEstimated { get; set; }
        public string? Note { get; set; }
    }
}
