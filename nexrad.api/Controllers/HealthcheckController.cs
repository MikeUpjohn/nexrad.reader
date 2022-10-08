using System;
using System.Web.Http;

namespace nexrad.api.Controllers
{
    [RoutePrefix("api/v1")]
    public class HealthcheckController : ApiController
    {
        public HealthcheckController()
        {
        }

        [HttpGet]
        [Route("am-i-alive")]
        public IHttpActionResult AmIAlive()
        {
            return Ok(new { AmIAlive = true, Date = DateTime.Now.ToString("ddd dd MMM yyyy HH:mm:ss.FFFFFF") });
        }
    }
}