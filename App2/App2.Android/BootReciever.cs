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
using Plugin.LocalNotifications;

namespace App2.Android
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            AddAllLocations();
        }

        void AddAllLocations()
        {
            var setup = new GeofenceSetup();
            foreach(Location location in LocationDB.GetLocations().Where(l => l.Active))
            {
                setup.Monitor(location);
            }
        }
    }
}