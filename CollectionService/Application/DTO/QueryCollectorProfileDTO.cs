using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;

namespace Application.DTO
{
    public class QueryCollectorProfileDTO
    {
        public int PageIndex { get; set; } = 1;
        public int PageLength { get; set; } = 10;
        public string Search { get; set; } = string.Empty;
        public bool isActive { get; set; } = true;
    }
}
