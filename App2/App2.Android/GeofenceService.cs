using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Location;
using Plugin.LocalNotifications;
using Plugin.Vibrate;

namespace App2.Android
{
    [Service]
    class GeofenceService : IntentService
    {
        protected override void OnHandleIntent(Intent intent)
        {
            var gEvent = GeofencingEvent.FromIntent(intent);
            if (gEvent.GeofenceTransition == Geofence.GeofenceTransitionEnter) {
                foreach (var geofence in gEvent.TriggeringGeofences)
                {
                    var location = LocationDB.GetLocation(int.Parse(geofence.RequestId));
                    CrossLocalNotifications.Current.Show(location.Title, location.Description);
                    CrossVibrate.Current.Vibration();
                }
            }
        }
    }
}