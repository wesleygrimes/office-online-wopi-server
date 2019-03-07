using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using office_online_wopi_server.Models;

namespace office_online_wopi_server.Controllers
{
    // https://wopi.readthedocs.io/projects/wopirest/en/latest
    [Route("wopi/files")]
    public class FilesController : ControllerBase
    {
        private IConfiguration _configuration;
        public string _validAccessToken => _configuration.GetValue<string>("ValidAccessToken");

        public FilesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCheckFileInfo([FromQuery] string access_token, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(access_token) || access_token != _validAccessToken)
                {
                    return Unauthorized();
                }

                string sessionContext = Request.Headers["X-WOPI-SessionContext"];

                var (file, fileName) = await getFileAsync(id);

                if (file == null)
                {
                    return NotFound();
                }

                return Ok(new CheckFileInfo()
                {
                    BaseFileName = fileName,
                    OwnerId = "",
                    Size = file.Length,
                    Version = ""
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.GetBaseException());
            }
        }

        [HttpGet("{id}/ancestry")]
        [ProducesResponseType(401)]
        [ProducesResponseType(501)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEnumerateAncestors([FromQuery] string access_token, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(access_token) || access_token != _validAccessToken)
                {
                    return Unauthorized();
                }

                return StatusCode(501);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.GetBaseException());
            }
        }

        [HttpGet("{id}/contents")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(412)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFile([FromQuery] string access_token, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(access_token) || access_token != _validAccessToken)
                {
                    return Unauthorized();
                }

                int maxExpectedSize = 0;

                if (string.IsNullOrEmpty(Request.Headers["X-WOPI-MaxExpectedSize"]))
                {
                    int.TryParse(Request.Headers["X-WOPI-MaxExpectedSize"], out maxExpectedSize);
                }

                var (file, fileName) = await getFileAsync(id);

                if (file == null)
                {
                    return NotFound();
                }

                if (maxExpectedSize > 0 && file.Length > maxExpectedSize)
                {
                    //Pre-Condition Failed
                    return StatusCode(412);
                }

                return File(file, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.GetBaseException());
            }
        }

        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [ProducesResponseType(501)]
        public async Task<IActionResult> PerformAction([FromQuery] string access_token, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(access_token) || access_token != _validAccessToken)
                {
                    return Unauthorized();
                }

                string wopiAction = Request.Headers["X-WOPI-Override"];

                var (file, fileName) = await getFileAsync(id);

                if (file == null)
                {
                    return NotFound();
                }

                switch (wopiAction)
                {
                    case "LOCK":
                    case "REFRESH_LOCK":
                        return await getLockResultAsync(id, Request);
                    case "GET_LOCK":
                        return await getGetLockResultAsync(id, Request);
                    case "UNLOCK":
                        return await getUnlockResultAsync(id, Request);
                    case "PUT_RELATIVE":
                    case "RENAME_FILE":
                    case "DELETE_FILE":
                    case "GET_SHARE_URL":
                    case "PUT_USER_INFO":
                    default:
                        return StatusCode(501);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.GetBaseException());
            }
        }

        [HttpPost("{id}/contents")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(413)]
        [ProducesResponseType(500)]
        [ProducesResponseType(501)]
        public async Task<IActionResult> PutFile([FromBody] byte[] contents, [FromQuery] string access_token, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(access_token) || access_token != _validAccessToken)
                {
                    return Unauthorized();
                }

                string wopiAction = Request.Headers["X-WOPI-Override"];

                if (wopiAction != "PUT")
                {
                    return StatusCode(501);
                }

                var fileLock = await getFileLockAsync(id);

                string wopiLock = Request.Headers["X-WOPI-Lock"];

                if (!string.IsNullOrEmpty(fileLock) && fileLock != wopiLock)
                {
                    Response.Headers.Add("X-WOPI-Lock", wopiLock);
                    return Conflict();
                }

                await updateFileAsync(id, contents);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.GetBaseException());
            }
        }

        private async Task updateFileAsync(string id, byte[] contents)
        {
            var files = Directory.GetFiles("sample_files", $"*{id}*.*");

            if (files.Any())
            {
                await System.IO.File.WriteAllBytesAsync(files[0], contents);
            }
        }

        private async Task<(byte[], string)> getFileAsync(string id)
        {
            byte[] file = null;
            string fileName = null;

            var files = Directory.GetFiles("sample_files", $"*{id}*.*");

            if (files.Any())
            {
                file = await System.IO.File.ReadAllBytesAsync(files[0]);
                fileName = Path.GetFileName(files[0]);
            }

            return (file, fileName);
        }

        private async Task<IActionResult> getLockResultAsync(string id, HttpRequest request)
        {
            string wopiLock = Request.Headers["X-WOPI-Lock"];
            string oldWopiLock = Request.Headers["X-WOPI-OldLock"];

            if (string.IsNullOrEmpty(wopiLock))
            {
                return BadRequest();
            }

            var fileLock = await getFileLockAsync(id);

            if (fileLock == null)
            {
                await lockFileAsync(id);
            }

            if (fileLock == wopiLock || fileLock == oldWopiLock)
            {
                await refreshLockAsync(fileLock);
            }

            if (fileLock != wopiLock && fileLock != oldWopiLock)
            {
                Response.Headers.Add("X-WOPI-Lock", wopiLock);
                return Conflict();
            }

            return Ok();
        }

        private async Task<IActionResult> getGetLockResultAsync(string id, HttpRequest request)
        {
            var fileLock = await getFileLockAsync(id);

            if (fileLock == null)
            {
                Response.Headers.Add("X-WOPI-Lock", "");
            }

            if (string.IsNullOrEmpty(fileLock))
            {
                Response.Headers.Add("X-WOPI-Lock", fileLock);
            }

            return Ok();
        }

        private async Task<IActionResult> getUnlockResultAsync(string id, HttpRequest request)
        {
            string wopiLock = Request.Headers["X-WOPI-Lock"];

            if (string.IsNullOrEmpty(wopiLock))
            {
                return BadRequest();
            }

            var fileLock = await getFileLockAsync(id);

            if (fileLock == wopiLock)
            {
                await unlockFileAsync(fileLock);
            }

            if (fileLock != wopiLock)
            {
                Response.Headers.Add("X-WOPI-Lock", wopiLock);
                return Conflict();
            }

            return Ok();
        }

        private async Task refreshLockAsync(object fileLock)
        {
        }

        private async Task<string> getFileLockAsync(string id)
        {
            return "mock-lock-string";
        }

        private async Task lockFileAsync(string id)
        {
        }

        private async Task unlockFileAsync(string id)
        {
        }
    }
}