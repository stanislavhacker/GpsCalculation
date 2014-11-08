using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GpsCalculation
{
    public class Geo
    {
        public const double RADIUS = 6371;
        public const double GLOBE_WIDTH = 256;

        public GeoPosition position { get; set; }

        /// <summary>
        /// New instance of geo calculation calss
        /// </summary>
        /// <param name="poition"></param>
        public Geo(GeoPosition poition)
        {
            this.position = poition;
        }

        /// <summary>
        /// Calculate middle point to position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public GeoPosition middlePointTo(GeoPosition position)
        {
            var deltaLatitude = GeoMath.ToRad(this.position.Latitude - position.Latitude);
            var deltaLongitude = GeoMath.ToRad(this.position.Longitude - position.Longitude);
            var p2Lat = GeoMath.ToRad(this.position.Latitude);
            var p1Lat = GeoMath.ToRad(position.Latitude);
            var p1Lon = GeoMath.ToRad(position.Longitude);

            var Bx = Math.Cos(p2Lat) * Math.Cos(deltaLongitude);
            var By = Math.Cos(p2Lat) * Math.Sin(deltaLongitude);
            var lat3 = Math.Atan2(Math.Sin(p1Lat) + Math.Sin(p2Lat), Math.Sqrt((Math.Cos(p1Lat) + Bx) * (Math.Cos(p1Lat) + Bx) + By * By));
            var lon3 = p1Lon + Math.Atan2(By, Math.Cos(p1Lat) + Bx);

            return new GeoPosition(GeoMath.ToDegrees(lat3), GeoMath.ToDegrees(lon3), position.Accuracy);
        }

        /// <summary>
        /// Calculate final bearing to position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public double finalBearingTo(GeoPosition position)
        {
            var lat1 = GeoMath.ToRad(position.Latitude);
            var lat2 = GeoMath.ToRad(this.position.Latitude);
            var dLon = GeoMath.ToRad(this.position.Longitude - position.Longitude);

            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            var brng = Math.Atan2(y, x);

            return (GeoMath.ToDegrees(brng) + 180) % 360;
        }

        /// <summary>
        /// Return ditance to position
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Distance in KM</returns>
        public double distanceTo(GeoPosition position)
        {
            var R = RADIUS;
            var lat1 = GeoMath.ToRad(this.position.Latitude);
            var lon1 = GeoMath.ToRad(this.position.Longitude);
            var lat2 = GeoMath.ToRad(position.Latitude);
            var lon2 = GeoMath.ToRad(position.Longitude);
            var dLat = lat2 - lat1;
            var dLon = lon2 - lon1;

            var a = Math.Sin(dLat/2) * Math.Sin(dLat/2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon/2) * Math.Sin(dLon/2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));

            return R * c;
        }

        /// <summary>
        /// Zoom to
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public double zoomTo(GeoPosition position, double width)
        {
            var GLOBE_WIDTH = 512;
            var west = this.position.Longitude;
            var east = position.Longitude;
            var angle = east - west;

            if (angle < 0) {
              angle += 360;
            }
            return Math.Round(Math.Log(width * 360 / angle / GLOBE_WIDTH) / Math.Log(2, Math.E));
        }

        /// <summary>
        /// Is position accured
        /// </summary>
        /// <param name="geoCoordinate"></param>
        /// <param name="accuracy"></param>
        /// <returns></returns>
        private bool isPositionAccured(GeoPosition geoCoordinate, double accuracy)
        {
            return geoCoordinate.Accuracy <= accuracy;
        }



        /// <summary>
        /// Speed btween two points
        /// </summary>
        /// <param name="positions"></param>
        /// <returns>Speed</returns>
        public static double SpeedTo(List<GeoPosition> positions, double accuracy = 20)
        {
            //distance init
            var km = 0.0;
            //time init
            var hours = 0.0;
            //geo
            Geo geo;

            for (var i = 0; i < positions.Count - 2; i++)
            {
                //save
                var first = positions[i];
                var second = positions[i + 1];
                //geo
                geo = new Geo(first);
                //calculate data
                TimeSpan sub = second.Date.Subtract(first.Date);
                hours += sub.TotalHours;
                km += geo.distanceTo(second);
            }

            if (hours == 0)
            {
                return 0;
            }
            //calculate
            Double multiplier = 1 / hours;
            Double kmh = km * multiplier;
            //return
            return kmh;
        }
    }
}
