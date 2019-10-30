using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace VeroServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private static Dictionary<string, RequestUserModel> aguardando = new Dictionary<string, RequestUserModel>();
        private static Dictionary<string, RequestUserModel> finalizado = new Dictionary<string, RequestUserModel>();
        [HttpPost]
        public async Task<Dictionary<string, string>> RequestUser(RequestUserModel model)
        {
            if (!Request.Cookies.ContainsKey("pkey"))
                throw new Exception("o cookie não foi encontrado");// StatusCode(StatusCodes.Status401Unauthorized);

            var id = Request.Cookies["pkey"].ToString();
            if (string.IsNullOrEmpty(id))
                throw new Exception("O id não enviado");

            model.Id = id;//"5Kb8kLf9zgWQnogidDA76Mz_SAMPLE_PRIVATE_KEY_DO_NOT_IMPORT_PL6TsZZY36hWXMssSzNydYXYB9KF";
            aguardando[model.Id] = model;
            return await Task.Run(() =>
            {
                while (true)
                {
                    if (finalizado.ContainsKey(model.Id))
                    {
                        var m = finalizado[model.Id];
                        finalizado.Remove(model.Id);
                        return m.Response;
                    }

                    Task.Delay(1000);
                }
            });


        }
        [HttpOptions]
        public ActionResult Options()
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public RequestUserModel CheckUser(string id)
        {

            if (aguardando.ContainsKey(id))
            {
                var a = aguardando[id];
                aguardando.Remove(id);
                return a;
            }
            else
                return null;
        }

        [HttpPost("{id}")]
        public string ConfirmarTransacao(string id, RequestUserModel model)
        {
            HyperledgerTest.VeroChain chain = new HyperledgerTest.VeroChain(id);
            var reqId = Guid.NewGuid();
            chain.SalvarInfo(reqId.ToString(), model.Response);
            finalizado[id] = model;

            return reqId.ToString();
        }

    }

    public class RequestUserModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("storeName")]
        public string StoreName { get; set; }
        [JsonProperty("fields")]
        public string[] Fields { get; set; }
        [JsonProperty("value")]
        public double Value { get; set; }
        [JsonProperty("response")]
        public Dictionary<string, string> Response { get; set; }

    }
}