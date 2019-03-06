using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using office_online_wopi_server.Models;

namespace office_online_wopi_server.Controllers
{
    [Route("wopi/ecosystem")]
    public class EcosystemController : ControllerBase
    {
        [HttpGet("root_container_pointer")]
        public async Task<IActionResult> GetRootContainer(string id)
        {
            return Ok(new WopiRootContainerInfo());
        }
    }
}