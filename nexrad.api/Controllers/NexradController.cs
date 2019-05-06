using Autofac.Integration.WebApi;
using nexrad.api.Models;
using nexrad.models;
using nexrad.reader.Level2;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace nexrad.api.Controllers
{
    [AutofacControllerConfiguration]
    [RoutePrefix("api/v1/nexrad")]
    public class NexradController : ApiController
    {
        private readonly ILevel2RadarReader _level2RadarReader;

        public NexradController(ILevel2RadarReader level2RadarReader)
        {
            _level2RadarReader = level2RadarReader;
        }

        [HttpGet]
        [Route("high-resolution-reflectivity")]
        public IHttpActionResult GetHighResReflectivity(RadarQuery query)
        {
            var data = _level2RadarReader.RunLevel2Radar("F://" + query.RadarFile);

            if (query.Scan.HasValue == true)
            {
                return Ok(data[query.ElevationNumber - 1].RecordMessages[query.Scan.Value].Record.ReflectivityData);
            }
            else
            {
                var scans = new List<MomentData>();

                for (var i = 0; i < data[query.ElevationNumber - 1].RecordMessages.Count; i++)
                {
                    scans.Add(data[query.ElevationNumber - 1].RecordMessages[i].Record.ReflectivityData);
                }

                return Ok(scans);
            }
        }

        [HttpGet]
        [Route("scans")]
        public IHttpActionResult GetScans(RadarQuery query)
        {
            var data = _level2RadarReader.RunLevel2Radar("F://" + query.RadarFile);

            return Ok(data[query.ElevationNumber - 1].RecordMessages.Count());
        }
    }
}
