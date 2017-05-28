using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public static class DistanceCalculator
    {
        //haversine formula
        public static double Distance(Position pos1, Position pos2)
        {
            double R = 6371;

            double dLat = ToRad(pos2.Latitude - pos1.Latitude);
            double dLon = ToRad(pos2.Longitude - pos1.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(pos1.Latitude)) * Math.Cos(ToRad(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d * 1000;
        }

        static double ToRad(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
