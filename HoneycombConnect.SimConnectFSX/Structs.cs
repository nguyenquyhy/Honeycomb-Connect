﻿using System.Runtime.InteropServices;

namespace HoneycombConnect.SimConnectFSX
{
    enum DEFINITIONS
    {
        PlaneStatus
    }

    enum EVENTS
    {
        PAUSE,
        BEACON_TOGGLE,
        LANDING_LIGHTS_TOGGLE,
        TAXI_TOGGLE,
        NAV_TOGGLE,
        STROBE_ON,
        STROBE_OFF,
        MASTER_ALT_TOGGLE,
        MASTER_BATTERY_TOGGLE,
        //MASTER_AVIONICS_TOGGLE,
        MASTER_AVIONICS_SET,
    }

    enum GROUPID
    {
        FLAG = 2000000000,
        MAX = 1,
    }

    internal enum DATA_REQUESTS
    {
        NONE,
        PLANE_STATUS
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct PlaneStatusStruct
    {
        public int BeaconLight;
        public int LandingLight;
        public int TaxiLight;
        public int NavLight;
        public int StrobeLight;

        public int Engine1Generator;
        public int MasterBattery;
        public int MasterAvionics;
    }
}
