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
        public override async void OnReceive(Context context, Intent intent)
        {
            //this is neccessary because it's the only way to avoid deadlock here
            var status = GoAsync();

            try
            {
                await AlarmHandler.OnAlarm();
            }
            finally
            {
                //notify android we are done; otherwise the app will be flagged as not responding
                status.Finish();
            }
        }
    }
}