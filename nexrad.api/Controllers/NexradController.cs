using Autofac.Integration.WebApi;
using nexrad.api.Models;
using nexrad.models;
using nexrad.reader.Level2;
using System.Collections.Generic;
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
                return Ok(data[query.ElevationNumber].RecordMessages[query.Scan.Value].Record.ReflectivityData);
            }
            else
            {
                var scans = new List<MomentData>();

                for (var i = 0; i < data[query.ElevationNumber].RecordMessages.Count; i++)
                {
                    scans.Add(data[query.ElevationNumber].RecordMessages[i].Record.ReflectivityData);
                }

                return Ok(scans);
            }
        }
    }
}
