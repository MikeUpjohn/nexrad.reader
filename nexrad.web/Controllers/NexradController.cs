using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using nexrad.api.Models;
using nexrad.models;
using nexrad.reader.Level2;

namespace nexrad.web.Controllers {
    public class NexradController : Controller {
        private readonly ILevel2RadarReader _level2RadarReader;

        public NexradController(ILevel2RadarReader level2RadarReader) {
            _level2RadarReader = level2RadarReader;
        }

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public JsonResult GetReflectivityData(RadarQuery query) {
            var data = _level2RadarReader.RunLevel2Radar("https://nexrad-reader-files.s3.eu-west-1.amazonaws.com/" + query.RadarFile);

            if (query.Scan.HasValue == true) {
                return Json(data[query.ElevationNumber - 1].RecordMessages[query.Scan.Value].Record.ReflectivityData);
            } else {
                var scans = new List<MomentData>();

                for (var i = 0; i < data[query.ElevationNumber - 1].RecordMessages.Count; i++) {
                    scans.Add(data[query.ElevationNumber - 1].RecordMessages[i].Record.ReflectivityData);
                }

                var json = Json(scans, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;

                return json;
            }
        }

        [HttpPost]
        public JsonResult GetAzimuthData(RadarQuery query) {
            var data = _level2RadarReader.RunLevel2Radar("https://nexrad-reader-files.s3.eu-west-1.amazonaws.com/" + query.RadarFile);

            var azimuthResult = new AzimuthResult();

            if (query.Scan != null) {
                return Json(data[query.ElevationNumber].RecordMessages[query.Scan.GetValueOrDefault()].Record.Azimuth);
            } else {
                var azimuths = new Dictionary<string, List<float>>();

                //for (var i = 0; i < data[query.ElevationNumber - 1].RecordMessages.Count; i++) {
                //    var record = data[query.ElevationNumber - 1].RecordMessages[i].Record;

                //    azimuths.Add(record.Azimuth);
                //}

                foreach (var groupedMomentData in data) {
                    azimuthResult.AvailableElevationScans.Add(groupedMomentData.ElevationNumber);

                    foreach (var item in groupedMomentData.RecordMessages) {
                        var azimuthValue = item.Record.Azimuth;

                        if (azimuths.ContainsKey(groupedMomentData.ElevationNumber.ToString())) {
                            azimuths[groupedMomentData.ElevationNumber.ToString()].Add(azimuthValue);
                        } else {
                            azimuths.Add(groupedMomentData.ElevationNumber.ToString(), new List<float> { azimuthValue });
                        }
                    }
                }

                azimuthResult.AzimuthData = azimuths;

                return Json(azimuthResult);
            }
        }
    }
}