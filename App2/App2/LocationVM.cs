using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms;

namespace App2
{
    class LocationVM
    {
        public Location Loc { get; set; }
        public Pin MapPin { get; set; }
        public Circle MapCircle { get; set; }

        public LocationVM(Location loc)
        {
            Loc = loc;
            MapPin = new Pin { Label = "new location", Position = new Xamarin.Forms.GoogleMaps.Position(loc.Coords.Latitude, loc.Coords.Longitude) };
            MapCircle = new Circle { Center = new Xamarin.Forms.GoogleMaps.Position(loc.Coords.Latitude, loc.Coords.Longitude), FillColor = Color.Transparent, StrokeColor = Color.LightBlue, StrokeWidth = 3, Radius = new Distance(Loc.Radius) };
        }
    }
}
