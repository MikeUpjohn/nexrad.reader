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

        [HttpPost]
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

        [HttpPost]
        [Route("azimuth")]
        public IHttpActionResult GetAzimuth(RadarQuery query)
        {
            var data = _level2RadarReader.RunLevel2Radar("F://" + query.RadarFile);

            if (query.Scan != null)
            {
                return Ok(data[query.ElevationNumber].RecordMessages[query.Scan.GetValueOrDefault()].Record.Azimuth);
            }
            else
            {
                var azimuths = new List<float>();

                for(var i = 0; i < data[query.ElevationNumber - 1].RecordMessages.Count; i++)
                {
                    azimuths.Add(data[query.ElevationNumber - 1].RecordMessages[i].Record.Azimuth);
                }

                return Ok(azimuths);
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("scans")]
        public IHttpActionResult GetScans(RadarQuery query)
        {
            var data = _level2RadarReader.RunLevel2Radar("F://" + query.RadarFile);

            return Ok(data[query.ElevationNumber - 1].RecordMessages.Count());
        }

        [HttpGet]
        [Route("high-resolution-velocity")]
        public IHttpActionResult GetHighResolutionVelocity(RadarQuery query)
        {
            var data = _level2RadarReader.RunLevel2Radar("F://" + query.RadarFile);

            return Ok(data[query.ElevationNumber - 1].RecordMessages[query.Scan.GetValueOrDefault()].Record.VelocityData);
        }

        [HttpGet]
        [Route("high-resolution-spectrum")]
        public IHttpActionResult GetHighResolutionSpectrum(RadarQuery query)
        {
            var data = _level2RadarReader.RunLevel2Radar("F://" + query.RadarFile);

            return Ok(data[query.ElevationNumber - 1].RecordMessages[query.Scan.GetValueOrDefault()].Record.SpectrumData);
        }

        [HttpGet]
        [Route("high-resolution-differential-reflectivity")]
        public IHttpActionResult GetHighResolutionDifferentialReflectivity(RadarQuery query)
        {
            var data = _level2RadarReader.RunLevel2Radar("F://" + query.RadarFile);

            return Ok(data[query.ElevationNumber - 1].RecordMessages[query.Scan.GetValueOrDefault()].Record.ZDRData);
        }

        [HttpGet]
        [Route("high-resolution-differential-phase")]
        public IHttpActionResult GetHighResolutionDifferentialPhase(RadarQuery query)
        {
            var data = _level2RadarReader.RunLevel2Radar("F://" + query.RadarFile);

            return Ok(data[query.ElevationNumber - 1].RecordMessages[query.Scan.GetValueOrDefault()].Record.PHIData);
        }

        [HttpGet]
        [Route("high-resolution-correlation-coefficient")]
        public IHttpActionResult GetHighResolutionCorrelationCoefficient(RadarQuery query)
        {
            var data = _level2RadarReader.RunLevel2Radar("F://" + query.RadarFile);

            return Ok(data[query.ElevationNumber - 1].RecordMessages[query.Scan.GetValueOrDefault()].Record.RhoData);
        }
    }
}
