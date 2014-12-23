using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GpsCalculation
{
    public class GeoCircle
    {
        /// <summary>
        /// Positions
        /// </summary>
        public List<GeoPosition> Positions { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        public Double Length { get; set; }

        /// <summary>
        /// Center
        /// </summary>
        public GeoPosition Center { get; set; }

        /// <summary>
        /// Get similarity
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="treshold"></param>
        /// <returns></returns>
        public Double GetSimilarity(GeoCircle circle, double treshold = 0.02)
        {
            //variables
            Double match = 0;
            Double max = Math.Min(circle.Positions.Count, this.Positions.Count);
            var distance = 0.0;
            var i = 0;
            var p = 0;

            //CHECK: LENGTH
            var length = Math.Abs(circle.Length - this.Length);
            //another distance
            if (length > 0.05)
            {
                return 0;
            }

            //CHECK: START
            var startGeo = new Geo(circle.Positions[0]);
            //all positions
            for (i = 0; i < this.Positions.Count; i++)
            {
                //distance
                distance = startGeo.distanceTo(this.Positions[i]);
                //treshold
                if (distance <= treshold)
                {
                    break;
                }
            }
            //start not found
            if (i == this.Positions.Count)
            {
                return 0;
            }

            //CHECK: PROCESS ALL PATH
            while (p < max)
            {
                //nearest
                int a;
                var nearest = Geo.GetNearest(circle.Positions[p], this.Positions, out a, i);
                i = a;
                //match
                match += nearest < treshold ? 1 : 0;
                //processed
                p++;
            }
            //return
            return (match / max) * 100;
        }

    }
}
