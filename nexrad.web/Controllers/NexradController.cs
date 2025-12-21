using System.Collections.Generic;
using System.Web.Mvc;
using nexrad.api.Models;
using nexrad.models;
using nexrad.reader.Level2;

namespace nexrad.web.Controllers {
    public class NexradController : Controller {
        private readonly ILevel2RadarReader _level2RadarReader;

        public NexradController(ILevel2RadarReader level2RadarReader)
        {
            _level2RadarReader = level2RadarReader;
        }

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public JsonResult LoadRadarFile(RadarQuery query) {
            var data = _level2RadarReader.RunLevel2Radar("https://nexrad-reader-files.s3.eu-west-1.amazonaws.com/" + query.RadarFile);

            if (query.Scan.HasValue == true)
            {
                return Json(data[query.ElevationNumber - 1].RecordMessages[query.Scan.Value].Record.ReflectivityData);
            }
            else
            {
                var scans = new List<MomentData>();

                for (var i = 0; i < data[query.ElevationNumber - 1].RecordMessages.Count; i++)
                {
                    scans.Add(data[query.ElevationNumber - 1].RecordMessages[i].Record.ReflectivityData);
                }

                var json = Json(scans, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;

                return json;
            }
        }
    }
}