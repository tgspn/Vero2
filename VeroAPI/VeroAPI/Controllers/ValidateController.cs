using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                {
                    var value = keys[id];
                    if (!string.IsNullOrEmpty(value))
                    {
                        var options = new Microsoft.AspNetCore.Http.CookieOptions()
                        {
                            Domain = "hyper.in",
                            Path = "/",
                            SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                            Secure = false,
                            Expires=DateTime.Now.AddYears(100),
                            IsEssential=true                            
                        };
                        Response.Cookies.Delete("pkey");
                        Response.Cookies.Append("pkey", value, options);
                    }
                    return value;
                }
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
        [JsonProperty("computerName")]
        public string ComputerName { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("ip")]
        public string IP { get; set; }
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
    public class ValidadoPcModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }
    }
}
