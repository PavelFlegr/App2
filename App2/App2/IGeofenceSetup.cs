using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public interface IGeofenceSetup
    {
        void Monitor(Location location);
        void RemoveMonitor(Location location);
    }
}
