using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GpsCalculation
{
    public class GeoPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public DateTime Date { get; private set; }

        /// <summary>
        /// Create new geopoisition
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="accuracy"></param>
        public GeoPosition(double latitude, double longitude, double accuracy)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Accuracy = accuracy;
            this.Date = DateTime.Now;
        }
    }
}
