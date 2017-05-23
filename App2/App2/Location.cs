using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms.GoogleMaps;
using SQLite;

namespace App2
{
    class Location : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        [Ignore]
        public Position Coords
        {
            get
            {
                return new Position(lat, lon);
            }
            set
            {
                lat = value.Latitude;
                lon = value.Longitude;
            }
        }
        public double lat { get; set; }
        public double lon { get; set; }
        public double Radius { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; } = true;
        public event PropertyChangedEventHandler PropertyChanged;

        public Location()
        {
            Radius = 100;
        }

        public Location ShallowCopy()
        {
            return new Location { Id = Id, Coords = Coords, Description = Description, Radius = Radius, Title = Title };
        }
    }
}
