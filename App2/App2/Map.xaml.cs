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
        Circle EditingCircle { get; set; }
        public Map()
        {
            InitializeComponent();
            map.IsShowingUser = true;
            // Map Clicked
            map.MapClicked += (sender, e) =>
            {
                var lat = e.Point.Latitude.ToString("0.000");
                var lng = e.Point.Longitude.ToString("0.000");
                map.Pins.Add(MakePin(lat, lng));
            };
        }

        Pin MakePin(string lat, string lng)
        {
            var pin = new Pin();
            var pos = new Position(double.Parse(lat), double.Parse(lng));
            pin.Position = pos;
            pin.Label = "new location";
            map.Circles.Add(new Circle { Center = pos, Radius = new Distance(1000), FillColor = Color.Transparent, StrokeColor = Color.LightBlue });
            return pin;
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            EditingCircle.Radius = new Distance(e.NewValue);
        }
    }
}
