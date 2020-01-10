using HoneycombConnect.SimConnectFSX;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HoneycombConnect.Wpf
{
    public enum ConnectionState
    {
        Idle,
        Connecting,
        Connected,
        Failed
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            //if ((storage == null && value != null) || !storage.Equals(value))
            {
                storage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            //return false;
        }

        private ConnectionState simConnectionState = ConnectionState.Idle;
        public ConnectionState SimConnectionState { get => simConnectionState; set => SetProperty(ref simConnectionState, value); }

        private PlaneStatus planeStatus = null;
        public PlaneStatus PlaneStatus { get => planeStatus; set => SetProperty(ref planeStatus, value); }


        private bool beaconEnabled = true;
        public bool BeaconEnabled { get { return beaconEnabled; } set { SetProperty(ref beaconEnabled, value); } }

        private bool beaconSync = true;
        public bool BeaconSync { get { return beaconSync; } set { SetProperty(ref beaconSync, value); } }


        private bool landingEnabled = true;
        public bool LandingEnabled { get { return landingEnabled; } set { SetProperty(ref landingEnabled, value); } }

        private bool landingSync = true;
        public bool LandingSync { get { return landingSync; } set { SetProperty(ref landingSync, value); } }


        private bool taxiEnabled = true;
        public bool TaxiEnabled { get { return taxiEnabled; } set { SetProperty(ref taxiEnabled, value); } }

        private bool taxiSync = true;
        public bool TaxiSync { get { return taxiSync; } set { SetProperty(ref taxiSync, value); } }

        private bool navEnabled = true;
        public bool NavEnabled { get { return navEnabled; } set { SetProperty(ref navEnabled, value); } }


        private bool navSync = true;
        public bool NavSync { get { return navSync; } set { SetProperty(ref navSync, value); } }

        private bool strobeEnabled = true;
        public bool StrobeEnabled { get { return strobeEnabled; } set { SetProperty(ref strobeEnabled, value); } }

        private bool strobeSync = true;
        public bool StrobeSync { get { return strobeSync; } set { SetProperty(ref strobeSync, value); } }
    }
}
