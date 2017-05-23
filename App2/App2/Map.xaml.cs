﻿using System;
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
        Dictionary<Pin, Location> pins = new Dictionary<Pin, Location>();
        LocationVM currentLoc
        {
            get
            {
                return (LocationVM)BindingContext;
            }
            set
            {
                BindingContext = value;
            }
        }
        Plugin.Geolocator.Abstractions.IGeolocator locator;

        public Map()
        {
            InitializeComponent();
            locator = CrossGeolocator.Current;
            //conn.DeleteAll<Location>();
            LoadLocations();
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

            try
            {
                var pos = await locator.GetPositionAsync();
                await map.MoveCamera(CameraUpdateFactory.NewPosition(new Xamarin.Forms.GoogleMaps.Position(pos.Latitude, pos.Longitude)));
            }
            catch (Exception e)
            {
                throw (e);
            }
            
        }

        void LoadLocations()
        {
            var locations = LocationDB.GetLocationList();
            foreach (var location in locations)
            {
                var locVM = new LocationVM(location);
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

            currentLoc = new LocationVM(location);

        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (currentLoc != null) {
                currentLoc.MapCircle.Radius = new Distance(currentLoc.Loc.Radius);
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
            LocationDB.SaveItem(currentLoc.Loc);
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

        private void MyLocations_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new MyLocations(), this);
            Navigation.PopAsync();
        }

        private void title_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentLoc.MapPin.Label = e.NewTextValue;
        }
    }
}
