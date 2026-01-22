using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UpdateCollectionTaskDTO
    {
        public string? Note { get; set; }
        public string? ImageName { get; set; }
        public double? AmountEstimated { get; set; }
        public Guid? CollectorProfileID { get; set; }
    }
}
