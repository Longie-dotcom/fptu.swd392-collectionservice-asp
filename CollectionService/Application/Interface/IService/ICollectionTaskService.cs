using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Application.Helper;

namespace Application.Interface.IService
{
    public interface ICollectionTaskService
    {
        Task<GenericResult<IEnumerable<CollectionTaskDTO>>> GetAllCollectionTask(string sortBy, QueryCollectionTaskDTO dto);
    }
}
