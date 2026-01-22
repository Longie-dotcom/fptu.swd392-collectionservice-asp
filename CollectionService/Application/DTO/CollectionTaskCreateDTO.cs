using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.DTO
{
    public class CollectionTaskCreateDTO
    {
        public Guid CollectionReportID { get; private set; }
        public Guid CollectorProfileID { get; private set; }
        public string Note { get; private set; }
        public string ImageName { get; private set; }
        public double AmountEstimated { get; private set; }

        
    }
}
