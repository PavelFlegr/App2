using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms;
using System.ComponentModel;

namespace App2
{
    public class LocationVM
    {
        public Location Loc { get; set; }
        public string Title
        {
            get
            {
                return Loc.Title;
            }
            set
            {
                Loc.Title = value;
                MapPin.Label = value;
            }
        }

        public string Description
        {
            get
            {
                return Loc.Description;
            }
            set
            {
                Loc.Description = value;
            }
        }
        
        public double Radius
        {
            get
            {
                return Loc.Radius;
            }
            set
            {
                Loc.Radius = value;
                MapCircle.Radius = new Distance(value);
            }
        }
        public Pin MapPin { get; set; }
        public Circle MapCircle { get; set; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        Action saveCB;
        Action cancelCB;

        public LocationVM(Location loc, Action saveCB, Action cancelCB)
        {
            this.saveCB = saveCB;
            this.cancelCB = cancelCB;
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
            Loc = loc;
            MapPin = new Pin { Label = "new location", Position = new Xamarin.Forms.GoogleMaps.Position(loc.Coords.Latitude, loc.Coords.Longitude) };
            MapCircle = new Circle { Center = new Xamarin.Forms.GoogleMaps.Position(loc.Coords.Latitude, loc.Coords.Longitude), FillColor = Color.Transparent, StrokeColor = Color.LightBlue, StrokeWidth = 3, Radius = new Distance(Loc.Radius) };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void Save()
        {
            LocationDB.SaveItem(Loc);
            saveCB?.Invoke();
        }

        void Cancel()
        {
            saveCB?.Invoke();
        }

        void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
