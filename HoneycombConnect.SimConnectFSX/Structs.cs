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
        TAXI_TOGGLE,
        NAV_TOGGLE,
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
        public int TaxiLight;
        public int NavLight;
    }
}