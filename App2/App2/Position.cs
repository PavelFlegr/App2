using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    struct Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Position(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }
    }
}
