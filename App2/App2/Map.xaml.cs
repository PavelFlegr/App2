using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Plugin.Geolocator;
using SQLite;
using PCLStorage;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        SQLiteAsyncConnection conn;
        Dictionary<Pin, Location> pins = new Dictionary<Pin, Location>();
        LocationVM currentLoc;
        Plugin.Geolocator.Abstractions.IGeolocator locator;

        public Map()
        {
            conn = new SQLiteAsyncConnection(FileSystem.Current.LocalStorage.Path + "/db");
            InitializeComponent();
            locator = CrossGeolocator.Current;
            Init();

            map.PinClicked += Map_PinClicked;
            map.IsShowingUser = true;
            // Map Clicked
            map.MapClicked += (sender, e) =>
            {
                CancelButton_Clicked(null, EventArgs.Empty);
                var lat = e.Point.Latitude.ToString("0.000");
                var lng = e.Point.Longitude.ToString("0.000");
                CreateLocation(double.Parse(lat), double.Parse(lng));
                map.Circles.Add(currentLoc.MapCircle);
                map.Pins.Add(currentLoc.MapPin);
                ToggleSettings();
            };
        }

        async Task Init()
        {
            var t2 = LoadLocations();
            var t = conn.CreateTableAsync<Location>();
            try
            {
                var pos = await locator.GetPositionAsync();
                await map.MoveCamera(CameraUpdateFactory.NewPosition(new Xamarin.Forms.GoogleMaps.Position(pos.Latitude, pos.Longitude)));
            }
            catch (Exception e)
            {
                throw (e);
            }
            await t2;
        }

        async Task LoadLocations()
        {
            var locations = await conn.Table<Location>().ToListAsync();
            foreach(var location in locations)
            {
                var locVM = new LocationVM(location);
                map.Pins.Add(locVM.MapPin);
            }
        }

        void CreateLocation(double lat, double lng)
        {
            var location = new Location
            {
                Coords = new Position(lat, lng)
            };

            currentLoc = new LocationVM(location);

        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (currentLoc != null) {
                currentLoc.MapCircle.Radius = new Distance(e.NewValue);
            }
        }

        void ToggleSettings()
        {
            map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(map.CameraPosition);
            if (currentLoc == null)
            {
                settings.IsVisible = false;
            }
            else
            {
                settings.IsVisible = true;
                slider.Value = currentLoc.MapCircle.Radius.Meters;
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            if (currentLoc != null)
            {
                if (!pins.ContainsKey(currentLoc.MapPin))
                {

                    map.Pins.Remove(currentLoc.MapPin);
                }
                map.Circles.Remove(currentLoc.MapCircle);
                currentLoc = null;
                ToggleSettings();
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            map.Circles.Remove(currentLoc.MapCircle);
            pins[currentLoc.MapPin] = currentLoc.Loc;
            SaveLocationToDB(currentLoc.Loc);
            currentLoc.Loc.Radius = (int)currentLoc.MapCircle.Radius.Meters;
            currentLoc = null;
            ToggleSettings();
        }

        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            CancelButton_Clicked(null, EventArgs.Empty);
            currentLoc = new LocationVM(pins[e.Pin].ShallowCopy());
            map.Circles.Add(currentLoc.MapCircle);
            ToggleSettings();
        }

        async Task SaveLocationToDB(Location location)
        {
            if(location.Id == 0)
            {
                await conn.InsertAsync(location);
            }
            else
            {
                await conn.UpdateAsync(location);
            }
        }
    }
}
