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
using PropertyChanged;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage, System.ComponentModel.INotifyPropertyChanged
    {
        Dictionary<Pin, Location> pins = new Dictionary<Pin, Location>();
        public LocationVM Current
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
                if (value != null)
                {
                    settings.IsVisible = true;
                }
                else settings.IsVisible = false;
                OnPropertyChanged(nameof(Current));
            }
        }
        LocationVM current;
        Plugin.Geolocator.Abstractions.IGeolocator locator;

        public Map()
        {
            InitializeComponent();
            //conn.DeleteAll<Location>();
            LoadLocations();
            InitAsync();

            map.PinClicked += Map_PinClicked;
            map.IsShowingUser = true;
            // Map Clicked
            map.MapClicked += (sender, e) =>
            {
                CancelButton_Clicked(null, EventArgs.Empty);
                var lat = e.Point.Latitude.ToString("0.000");
                var lng = e.Point.Longitude.ToString("0.000");
                CreateLocation(double.Parse(lat), double.Parse(lng));
                map.Circles.Add(Current.MapCircle);
                map.Pins.Add(Current.MapPin);
                ToggleSettings();
            };
        }

        public Map(Location loc) : this()
        {
            MyPinClicked(pins.Where((pair) => pair.Value.Id == loc.Id).First().Key);
            ToggleSettings();
        }

        //this needs to be asynchronous because awaititng in the main thread causes a deadlock
        //sets the camera on user position if available
        async Task InitAsync()
        {
            locator = CrossGeolocator.Current;

            try
            {
                var pos = await locator.GetPositionAsync();
                await map.MoveCamera(CameraUpdateFactory.NewPosition(new Xamarin.Forms.GoogleMaps.Position(pos.Latitude, pos.Longitude)));
            }
            catch
            {

            }

        }

        LocationVM CreateLocVM(Location loc)
        {
            return new LocationVM(loc, OnSave, OnCancel);
        }

        void OnSave()
        {
            pins[Current.MapPin] = Current.Loc;
            Current = null;
        }

        void OnCancel()
        {
            if (!pins.ContainsKey(Current.MapPin))
            {

                map.Pins.Remove(Current.MapPin);
            }
            map.Circles.Remove(Current.MapCircle);
            Current = null;
        }

        //populates map with saved locations and their pins
        void LoadLocations()
        {
            var locations = LocationDB.GetLocationList();
            foreach (var location in locations)
            {
                var locVM = CreateLocVM(location);
                pins.Add(locVM.MapPin, location);
                map.Pins.Add(locVM.MapPin);
            }
        }

        void CreateLocation(double lat, double lng)
        {
            var location = new Location
            {
                Coords = new Position(lat, lng)
            };

            Current = CreateLocVM(location);

        }

        //hides or shows settings depending on whether they have something to display 
        void ToggleSettings()
        {
            map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(map.CameraPosition);
            if (Current == null)
            {
                settings.IsVisible = false;
            }
            else
            {
                settings.IsVisible = true;
                slider.Value = Current.MapCircle.Radius.Meters;
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            /*if (Current != null)
            {
                if (!pins.ContainsKey(Current.MapPin))
                {

                    map.Pins.Remove(Current.MapPin);
                }
                map.Circles.Remove(Current.MapCircle);
                Current = null;
                ToggleSettings();
            }*/
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            /*map.Circles.Remove(Current.MapCircle);
            Current.Loc.Title = title.Text;
            Current.MapPin.Label = title.Text;
            Current.Loc.Description = description.Text;
            pins[Current.MapPin] = Current.Loc;
            LocationDB.SaveItem(Current.Loc);
            Current.Loc.Radius = (int)Current.MapCircle.Radius.Meters;
            Current = null;
            ToggleSettings();*/
        }

        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            MyPinClicked(e.Pin);
        }

        void MyPinClicked(Pin pin)
        {
            Current = CreateLocVM(pins[pin].ShallowCopy());
            /*CancelButton_Clicked(null, EventArgs.Empty);
            Current = new LocationVM(pins[pin].ShallowCopy());
            title.Text = Current.Loc.Title;
            description.Text = Current.Loc.Description;
            map.Circles.Add(Current.MapCircle);
            ToggleSettings();*/
        }

        private void MyLocations_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new MyLocations(), this);
            Navigation.PopAsync();
        }
    }
}
