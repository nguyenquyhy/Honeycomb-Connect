using System;

namespace HoneycombConnect.SimConnectFSX
{
    public class PlaneStatus
    {
        internal PlaneStatus(PlaneStatusStruct data)
        {
            BeaconLight = data.BeaconLight == 1;
            LandingLight = data.LandingLight == 1;
            TaxiLight = data.TaxiLight == 1;
            NavLight = data.NavLight == 1;
            StrobeLight = data.StrobeLight == 1;

            Engine1Generator = data.Engine1Generator == 1;
            MasterBattery = data.MasterBattery == 1;
        }

        public bool BeaconLight { get; set; }
        public bool LandingLight { get; set; }
        public bool TaxiLight { get; set; }
        public bool NavLight { get; set; }
        public bool StrobeLight { get; set; }

        public bool Engine1Generator { get; set; }
        public bool MasterBattery { get; set; }
    }

    public class PlaneStatusUpdatedEventArgs : EventArgs
    {
        public PlaneStatusUpdatedEventArgs(PlaneStatus planeStatus)
        {
            PlaneStatus = planeStatus;
        }

        public PlaneStatus PlaneStatus { get; }
    }
}
