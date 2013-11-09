using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GpsCalculation
{
    public class GeoMath
    {
        /// <summary>
        /// Convert deggre ro radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        /// <summary>
        /// Covert radians to deggre
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }
    }
}
