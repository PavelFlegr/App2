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
using Plugin.LocalNotifications;

namespace App2.Android
{
    [BroadcastReceiver]
    public class AlarmReciever : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var status = GoAsync();
            Task.WhenAny(AlarmHandler.OnAlarm(), Task.Delay(8000)).ContinueWith((t) => status.Finish());
        }
    }
}