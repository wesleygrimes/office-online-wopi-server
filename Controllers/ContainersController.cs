using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using office_online_wopi_server.Models;

namespace office_online_wopi_server.Controllers
{
    [Route("wopi/containers")]
    public class ContainersController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCheckContainerInfo(string id)
        {
            return Ok(new WopiContainerInfo { Name = "test" });
        }

        [HttpGet("{id}/children")]
        public async Task<IActionResult> GetChildren(string id)
        {
            return Ok(new WopiContainer());
        }
    }
}