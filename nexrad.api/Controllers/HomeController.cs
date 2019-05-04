using Autofac.Integration.WebApi;
using nexrad.reader.Level2;
using System.Web.Http;

namespace nexrad.api.Controllers
{
    [AutofacControllerConfiguration]
    [RoutePrefix("api/v1/nexrad")]
    public class HomeController : ApiController
    {
        private readonly ILevel2RadarReader _level2RadarReader;

        public HomeController(ILevel2RadarReader level2RadarReader)
        {
            _level2RadarReader = level2RadarReader;
        }

        [Route("high-res")]
        public IHttpActionResult GetHighResReflectivity(string fileName)
        {
            var data = _level2RadarReader.RunLevel2Radar("F:// " + fileName);
            return Ok(true);
        }
    }
}
