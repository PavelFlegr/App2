using System;

using Android.Content;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace App2.Android
{
    [Activity(Label = "App2", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            var prefs = GetPreferences(FileCreationMode.Private);
            if(!prefs.GetBoolean("firstRun", true)) {
                var am = (AlarmManager)GetSystemService(AlarmService);
                am.SetRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime(), 1000 * 60, PendingIntent.GetBroadcast(ApplicationContext, 0, new Intent(ApplicationContext, new AlarmReciever().Class), 0));

                var pEdit = prefs.Edit();
                pEdit.PutBoolean("firstRun", false);
                pEdit.Commit();
            }
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsGoogleMaps.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

