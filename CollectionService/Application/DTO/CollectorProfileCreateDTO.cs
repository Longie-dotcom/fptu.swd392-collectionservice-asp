using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CollectorProfileCreateDTO
    {
        public Guid UserID { get; private set; }
        public string ContactInfo { get; private set; } = string.Empty;
        public bool IsActive { get; private set; } = true;
    }
}
