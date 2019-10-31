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
        public ActionResult/*Task<Dictionary<string, string>>*/ RequestUser(RequestUserModel model)
        {
            if (!Request.Cookies.ContainsKey("pkey"))
                return StatusCode(StatusCodes.Status401Unauthorized); //throw new Exception("o cookie não foi encontrado");// 

            var id = Request.Cookies["pkey"].ToString();
            if (string.IsNullOrEmpty(id))
                return StatusCode(StatusCodes.Status401Unauthorized);//throw new Exception("O id não enviado");

            model.Id = id;//"5Kb8kLf9zgWQnogidDA76Mz_SAMPLE_PRIVATE_KEY_DO_NOT_IMPORT_PL6TsZZY36hWXMssSzNydYXYB9KF";
            aguardando[model.Id] = model;
            return Ok();
            //return await Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        if (finalizado.ContainsKey(model.Id))
            //        {
            //            var m = finalizado[model.Id];
            //            finalizado.Remove(model.Id);
            //            return m.Response;
            //        }

            //        Task.Delay(1000);
            //    }
            //});


        }
        [HttpGet()]
        public ActionResult<Dictionary<string, string>> Result()
        {
            var id = Request.Cookies["pkey"].ToString();
            if (finalizado.ContainsKey(id))
            {
                var m = finalizado[id];
                finalizado.Remove(id);
                if (m.Response.Count ==0)
                    return StatusCode(StatusCodes.Status406NotAcceptable);

                return Ok(m.Response);
            }
            return NoContent();
        }
        [HttpOptions]
        public ActionResult Options()
        {
            return Ok();
        }
        [HttpGet("{id}",Name ="getUser")]
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
            var reqId = Guid.NewGuid();
            try
            {
                HyperledgerTest.VeroChain chain = new HyperledgerTest.VeroChain(id);

                chain.SalvarInfo(reqId.ToString(), model.Response);
                finalizado[model.Id] = model;
            }
            catch (Exception ex)
            {
                model.Response?.Clear();
            }
            finally
            {
                finalizado[model.Id] = model;
            }
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