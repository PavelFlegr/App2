using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        Dictionary<Pin, Location> pins = new Dictionary<Pin, Location>();
        LocationVM currentLoc;
        LocationVM lastLoc;
        public Map()
        {
            InitializeComponent();
            map.PinClicked += Map_PinClicked;
            map.IsShowingUser = true;
            // Map Clicked
            map.MapClicked += (sender, e) =>
            {
                var lat = e.Point.Latitude.ToString("0.000");
                var lng = e.Point.Longitude.ToString("0.000");
                CreateLocation(double.Parse(lat), double.Parse(lng));
                map.Circles.Add(currentLoc.MapCircle);
                map.Pins.Add(currentLoc.MapPin);
                ToggleSettings();
            };
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
            if(currentLoc == null)
            {
                settings.IsVisible = false;
            }
            else
            {
                settings.IsVisible = true;
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            if (!pins.ContainsKey(currentLoc.MapPin))
            {
                
                map.Pins.Remove(currentLoc.MapPin);
            }
            map.Circles.Remove(currentLoc.MapCircle);
            currentLoc = null;
            ToggleSettings();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            map.Circles.Remove(currentLoc.MapCircle);
            pins[currentLoc.MapPin] = currentLoc.Loc;
            currentLoc.Loc.Radius = (int)currentLoc.MapCircle.Radius.Meters;
            currentLoc = null;
            ToggleSettings();
        }

        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            currentLoc = new LocationVM(pins[e.Pin].ShallowCopy());
            map.Circles.Add(currentLoc.MapCircle);
            ToggleSettings();
        }
    }
}
