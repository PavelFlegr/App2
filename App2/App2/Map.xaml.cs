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

        public double VisibleRadius
        {
            get
            {
                if (radiusChanged == true)
                {
                    return map.VisibleRegion != null ? map.VisibleRegion.Radius.Meters : 1;
                }
                else
                {
                    return lastVisibleRadius > 0 ? lastVisibleRadius : 1; 
                }
            }
        }

        double lastVisibleRadius;

        bool radiusChanged;

        public LocationVM Current
        {
            get
            {
                return current;
            }
            set
            {
                //camera jumps to initial position when map is resized
                map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(map.CameraPosition);
                if (value != null)
                {
                    map.Circles.Add(value.MapCircle);
                    settings.IsVisible = true;
                }
                else if(current != null)
                {
                    settings.IsVisible = false;
                }
                radiusChanged = false;
                current = value;
                OnPropertyChanged(nameof(Current));
            }
        }
        LocationVM current;
        Plugin.Geolocator.Abstractions.IGeolocator locator;

        public Map()
        {
            BindingContext = this;
            InitializeComponent();
            InitAsync();
            LoadLocations();

            map.PinClicked += Map_PinClicked;
            map.IsShowingUser = true;
            map.MapClicked += Map_MapClicked;
            map.CameraChanged += Map_CameraChanged;
        }

        private void Map_CameraChanged(object sender, CameraChangedEventArgs e)
        {
            if(Current != null && map.VisibleRegion.Radius.Meters < Current.Radius)
            {
                lastVisibleRadius = Current.Radius;
                radiusChanged = false;
            }
            else
            {
                radiusChanged = true;
            }
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            Current?.CancelCommand.Execute(null);
            CreateLocation(e.Point.Latitude, e.Point.Longitude);
        }

        public Map(Location loc) : this()
        {
            Current = CreateLocVM((pins.Where((pair) => pair.Value.Id == loc.Id).First().Value.ShallowCopy()));
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
            catch { }

        }

        LocationVM CreateLocVM(Location loc)
        {
            return new LocationVM(loc, OnSave, OnCancel);
        }

        void OnSave()
        {
            pins[Current.MapPin] = Current.Loc;
            map.Circles.Remove(Current.MapCircle);
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
            var locations = LocationDB.GetLocations();
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
            map.Pins.Add(Current.MapPin);
        }

        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            Current?.CancelCommand.Execute(null);
            Current = CreateLocVM(pins[e.Pin].ShallowCopy());
        }

        private void MyLocations_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new MyLocations(), this);
            Navigation.PopAsync();
        }

        private void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            radiusChanged = true;
            OnPropertyChanged(nameof(VisibleRadius));
        }
    }
}
