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
    public class BootReciever : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var am = (AlarmManager)context.GetSystemService(Context.AlarmService);
            am.SetRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime(), 1000 * 10, PendingIntent.GetBroadcast(context, 0, new Intent(context, new AlarmReciever().Class), 0));
            CrossLocalNotifications.Current.Show("test", "test");
        }
    }
}