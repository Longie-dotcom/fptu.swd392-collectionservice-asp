using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.DTO
{
    public class CollectionTaskDTO
    {
        public Guid CollectionTaskID { get; set; }
        public Guid CollectionReportID { get; set; }
        public string Note { get; set; }
        public string ImageName { get; set; }
        public double AmountEstimated { get; set; }
        public CollectionReportStatus Status { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid CollectorProfileID { get; set; }
    }
}
