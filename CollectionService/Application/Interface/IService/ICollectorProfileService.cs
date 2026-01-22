using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Helper;

namespace Application.Interface.IService
{
    public interface ICollectorProfileService
    {
        Task<GenericResult<IEnumerable<CollectorProfileDTO>>> GetAllCollectorProfile(string sortBy, QueryCollectorProfileDTO dto);
        Task<GenericResult<CollectorProfileDTO>> AddCollectorProfileAsync(string performedBy, CollectorProfileCreateDTO dto);
        Task CreateCollectorProfileAsync(SWD392.MessageBroker.CollectorProfileDTO request);
    }
}
