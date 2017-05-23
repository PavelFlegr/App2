using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    class LocationListItemVM
    {
        public Location location { get; set; }
        public Command DeleteCommand { get; set; }
        public LocationListItemVM(Location location)
        {
            this.location = location;
            DeleteCommand = new Command(() => {
                LocationDB.DeleteItem(location);
                RaiseRemoved();
                }
            );
        }

        public event EventHandler Removed;

        void RaiseRemoved()
        {
            Removed?.Invoke(this, EventArgs.Empty);
        }
    }
}
