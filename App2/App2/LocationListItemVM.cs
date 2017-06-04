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
        public Command EditCommand { get; set; }
        public bool Active
        {
            get
            {
                return location.Active;
            }
            set
            {
                location.Active = value;
                LocationDB.SaveItem(location);
            }
        }
        public LocationListItemVM(Location location)
        {
            this.location = location;
            DeleteCommand = new Command(() => {
                GeofenceSetup.RemoveMonitor(location);
                LocationDB.DeleteItem(location);
                RaiseRemoved();
            }
            );
            EditCommand = new Command(() =>
            {
                RaiseEdit();
            });
        }

        public event EventHandler Removed;

        void RaiseRemoved()
        {
            Removed?.Invoke(this, EventArgs.Empty);
        }

        public EventHandler Edit;

        void RaiseEdit()
        {
            Edit?.Invoke(this, EventArgs.Empty);
        }
    }
}
