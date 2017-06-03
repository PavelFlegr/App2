using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    public class GeofenceSetup
    {
        static IGeofenceSetup impl;
        static GeofenceSetup()
        {
            impl = DependencyService.Get<IGeofenceSetup>();
        }
        public static void Setup()
        {
            var locations = LocationDB.GetLocations();
            foreach (Location location in locations)
            {
                Monitor(location);
            }
        }

        public static void Monitor(Location location)
        {
            impl.Monitor(location); 
        }

        public static void RemoveMonitor(Location location)
        {
            impl.RemoveMonitor(location);
        }
    }
}
