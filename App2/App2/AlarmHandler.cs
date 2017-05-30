using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotifications;
using Plugin.Vibrate;
using Plugin.Geolocator;

namespace App2
{
    public static class AlarmHandler
    {
        public static async Task OnAlarm()
        {
            await CheckNearbyLocation();
        }

        static void NotifyNearby(Location location)
        {
            CrossLocalNotifications.Current.Show(location.Title, location.Description);
            CrossVibrate.Current.Vibration();
        }

        static async Task CheckNearbyLocation()
        {
            var position = GetPosition();
            var locations = LocationDB.GetLocationList();
            foreach (Location location in locations)
            {
                double distance = DistanceCalculator.Distance(location.Coords, await position);
                if (location.Timeout && distance > location.Radius + 10)
                {
                    location.Timeout = false;
                }
                if (location.Active && !location.Timeout && distance < location.Radius)
                {
                    NotifyNearby(location);
                    location.Timeout = true;
                }

                LocationDB.SaveItem(location);
            }
            
        }

        static async Task<Position> GetPosition()
        {
            var locator = CrossGeolocator.Current;
            var pos = await locator.GetPositionAsync();
            return new Position(pos.Latitude, pos.Longitude);
        }

        static double GetDistance(Position pos1, Position pos2)
        {
            return Math.Sqrt(Math.Pow(pos2.Latitude - pos1.Latitude, 2) + Math.Pow(pos2.Longitude - pos1.Longitude, 2));
        }
    }
}
