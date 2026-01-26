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
        Task<GenericResult<CollectorProfileDTO>> CreateCollectorProfileAsync(string createBy, CollectorProfileCreateDTO dto);

        Task<GenericResult<CollectorProfileDTO>> UpdateCollectorProfileAsync(Guid collectorProfileId, CollectorProfileUpdateDTO dto, string updatedBy);

        Task<(bool Success, string? Error)> DeleteCollectorProfileAsync(Guid collectorProfileId, string deletedBy);

        Task<GenericResult<CollectorProfileDTO>> GetCollectorProfileByIdAsync(Guid collectorProfileId);

        Task<GenericResult<IEnumerable<CollectorProfileDTO>>> GetActiveCollectorsAsync(string? wardCode);

        Task<GenericResult<CollectorProfileDTO>> GetCollectorProfileByUserIdAsync(string userId);

        Task CreateCollectorProfileAsync(SWD392.MessageBroker.CollectorProfileDTO request);
    }
}
