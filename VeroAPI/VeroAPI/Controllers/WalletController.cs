using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {

        // GET api/wallet
        [HttpPost]
        public ActionResult<IEnumerable<string>> CreateWallet()
        {
            var user = Guid.NewGuid();

            UserManager manager = new UserManager();

            manager.Register(user.ToString("N"));

            var file=manager.GetUserCredentials(user.ToString("N"));
            Response.Headers["w-user"] = user.ToString("N");

            return File(file, "application/zip");
        }


    }
}