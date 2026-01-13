using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.DTO
{
    public class QueryCollectionTaskDTO
    {
        public int PageIndex { get; set; } = 1;
        public int PageLength { get; set; } = 10;
        public string Search {  get; set; } = string.Empty;
        public CollectionReportStatus Status { get; set; } = CollectionReportStatus.Pending;
    }
}
