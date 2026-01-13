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
    }
}
