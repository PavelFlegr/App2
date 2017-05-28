using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Geolocator;
using System.Threading.Tasks;

namespace App2.Android
{
    [BroadcastReceiver]
    public class AlarmReciever : BroadcastReceiver
    {
        Context context;

        public override async void OnReceive(Context context, Intent intent)
        {
            //this is neccessary because it's the only way to avoid deadlock here
            var status = GoAsync();

            this.context = context;
            await CheckNearbyLocations();
            
            //notify android we are done; otherwise the app will be flagged as not responding
            status.Finish();

        }

        async Task<Plugin.Geolocator.Abstractions.Position> GetPosition()
        {
            var locator = CrossGeolocator.Current;
            return await locator.GetPositionAsync();
        }

        async Task CheckNearbyLocations()
        {
            var pos = await GetPosition();
            var position = new Position(pos.Latitude, pos.Longitude);
            var locations = LocationDB.GetLocationList();
            foreach (Location location in locations)
            {
                double distance = DistanceCalculator.Distance(location.Coords, position);
                if (location.Timeout && distance > location.Radius + 10)
                {
                    location.Timeout = false;
                }
                if(location.Active && !location.Timeout && distance < location.Radius)
                {
                    NotifyNearby(location);
                    location.Timeout = true;
                }

                LocationDB.SaveItem(location);
            }
            
        }

        double GetDistance(Position pos1, Position pos2)
        {
            return Math.Sqrt(Math.Pow(pos2.Latitude - pos1.Latitude, 2) + Math.Pow(pos2.Longitude - pos1.Longitude, 2));
        }

        void NotifyNearby(Location location)
        {
            Notification.Builder b = new Notification.Builder(context)
                .SetSmallIcon(Resource.Drawable.notification_template_icon_bg)
                .SetContentTitle(location.Title)
                .SetContentText(location.Description)
                .SetVibrate(new long[] { 0, 500 });
                
            NotificationManager mNotificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            mNotificationManager.Notify(0, b.Build());
        }
    }
}