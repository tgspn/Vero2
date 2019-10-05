using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {
        private static Dictionary<Guid, string> keys = new Dictionary<Guid, string>();
        // GET: api/Validate
        [HttpGet]
        public ValidaPcModel Get()
        {
            var ua = Request.Headers["User-Agent"].ToString();

            var match = Regex.Match(ua, @"(.*?) \((?'plataforma'.*?)\) ");

            var ip = ControllerContext.HttpContext.Connection.RemoteIpAddress.ToString();

            var model = new ValidaPcModel()
            {
                ComputerName = match.Groups["plataforma"].Value.Split(';')[0],
                Date = DateTime.Now,
                IP = ip,
                Id = Guid.NewGuid(),
            };
            keys[model.Id] = "";

            return model;
        }
        [HttpGet("{id}")]
        public string GetPubKey(Guid id)
        {
            try
            {
                if (keys.ContainsKey(id))
                    return keys[id];
                else

                    return "";
            }
            catch
            {
                return "";
            }
        }

        [HttpPost]
        public string SetPkey(ValidadoPcModel dados)
        {

            if (keys.ContainsKey(dados.Id))
                keys[dados.Id] = dados.PublicKey;

            return "";
        }

        [HttpOptions]
        public ActionResult Options()
        {
            return StatusCode(204);
        }
    }

    public class ValidaPcModel
    {
        public string ComputerName { get; set; }
        public DateTime Date { get; set; }
        public string IP { get; set; }
        public Guid Id { get; set; }
    }
    public class ValidadoPcModel
    {
        public Guid Id { get; set; }
        public string PublicKey { get; set; }
    }
}
