using System.Web.Http;

namespace nexrad.api.Controllers
{
    [RoutePrefix("api/v1/nexrad")]
    public class HomeController : ApiController
    {
        [Route("high-res")]
        public IHttpActionResult GetHighResReflectivity()
        {
            return Ok(true);
        }
    }
}
