using API.Helper;
using Application.Interface.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CollectorsController : ControllerBase
    {
        #region Attributes
        private readonly ICollectorService collectorService;
        #endregion

        #region Properties
        #endregion

        public CollectorsController(ICollectorService collectorService)
        {
            this.collectorService = collectorService;
        }

        #region Methods
        #endregion
    }
}
