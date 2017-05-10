using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms.GoogleMaps;

namespace App2
{
    class Location : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public Position Coords { get; set; }
        public int Radius { get; set; }
        public string Description { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
