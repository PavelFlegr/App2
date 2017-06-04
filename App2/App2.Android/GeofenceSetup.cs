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
using Android.Gms.Location;
using Android.Gms.Common.Apis;
using Android.Gms.Common;

[assembly: Xamarin.Forms.Dependency(typeof(App2.Android.GeofenceSetup))]
namespace App2.Android
{
    class GeofenceSetup : Java.Lang.Object, IGeofenceSetup, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        static GoogleApiClient client;
        static GeofenceSetup()
        {
            client = BuildApiClient();
        }

        public void Monitor(Location location)
        {
            var geofence = new GeofenceBuilder()
            .SetRequestId(location.Id.ToString())
            .SetCircularRegion(location.lat, location.lon, (float)location.Radius)
            .SetTransitionTypes(Geofence.GeofenceTransitionEnter)
            .SetExpirationDuration(Geofence.NeverExpire)
            .Build();

            var request = new GeofencingRequest.Builder()
            .AddGeofence(geofence)
            .Build();
            LocationServices.GeofencingApi.AddGeofences(client, request, PendingIntent.GetService(Application.Context, 0, new Intent(Application.Context, new GeofenceService().Class), PendingIntentFlags.UpdateCurrent));
        }

        static GoogleApiClient BuildApiClient()
        {
            var c = new GoogleApiClient.Builder(Application.Context).
            AddApi(LocationServices.API)
            .Build();
            c.Connect();
            return c;
        }

        public void RemoveMonitor(Location location)
        {
            LocationServices.GeofencingApi.RemoveGeofences(client, new List<string> { location.Id.ToString()});
        }

        public void OnConnected(Bundle connectionHint)
        {
            throw new NotImplementedException();
        }

        public void OnConnectionSuspended(int cause)
        {
            throw new NotImplementedException();
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            throw new NotImplementedException();
        }
    }
}