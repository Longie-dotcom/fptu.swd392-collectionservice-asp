using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Application.DTO
{
    public class CollectorProfileDTO
    {
        public Guid CollectorProfileID { get; private set; }
        public Guid UserID { get; private set; }
        public string ContactInfo { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public List<CollectionTask> collectionTasks { get; set; } = new();

    }
}
