using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using office_online_wopi_server.Models;

namespace office_online_wopi_server.Controllers
{
    [Route("wopi/files")]
    public class FilesController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> CheckFileInfo(string id)
        {
            return Ok(new WopiFileInfo());
        }

        [HttpGet("{id}/contents")]
        public async Task<IActionResult> GetFile(string id)
        {
            var file = new byte[] { };
            var files = Directory.GetFiles("sample_files", $"*{id}*.*");

            if (files.Any())
            {
                file = await System.IO.File.ReadAllBytesAsync(files[0]);
            }

            var fileName = Path.GetFileName(files[0]);
            return File(file, "application/octet-stream", fileName);
        }

        [HttpPut("{id}/contents")]
        [HttpPost("{id}/contents")]
        public async Task<IActionResult> PutFile(string id)
        {
            return Ok(new WopiLockResult());
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PerformAction(string id)
        {
            return Ok(new WopiLockResult());
        }
    }
}