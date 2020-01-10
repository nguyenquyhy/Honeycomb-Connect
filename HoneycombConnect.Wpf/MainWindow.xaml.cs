using HoneycombConnect.SimConnectFSX;
using Microsoft.Extensions.Logging;
using SharpDX.DirectInput;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HoneycombConnect.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> logger;
        private readonly FlightConnect flightConnect;
        private readonly MainViewModel viewModel;

        public MainWindow(ILogger<MainWindow> logger, FlightConnect flightConnect, MainViewModel viewModel)
        {
            InitializeComponent();
            this.logger = logger;
            this.flightConnect = flightConnect;
            this.viewModel = viewModel;
            DataContext = viewModel;

            flightConnect.PlaneStatusUpdated += FlightConnect_PlaneStatusUpdated;
        }

        private void FlightConnect_PlaneStatusUpdated(object sender, PlaneStatusUpdatedEventArgs e)
        {
            viewModel.PlaneStatus = e.PlaneStatus;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var directInput = new DirectInput();
            var devices = directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);

            var alpha = devices.FirstOrDefault(o => o.ProductName == "Alpha Flight Controls");

            var input = new Joystick(directInput, alpha.InstanceGuid);
            input.Properties.BufferSize = 256;
            input.Acquire();

            Task.Run(async () =>
            {
                await MonitorButtonsAsync(input);
            });

            Task.Run(async () =>
            {
                await SyncButtonsAsync(input);
            });
        }

        private async Task MonitorButtonsAsync(Joystick input)
        {
            bool[] lastButtons = null;
            while (true)
            {
                input.Poll();
                var state = input.GetCurrentState();
                //var data = input.GetBufferedData();
                if (lastButtons == null)
                    lastButtons = state.Buttons;
                else
                {
                    for (var i = 0; i < lastButtons.Length; i++)
                    {
                        if (lastButtons[i] != state.Buttons[i])
                        {
                            logger.LogDebug($"Button {i} changed to {state.Buttons[i]}");

                            if (state.Buttons[i])
                            {
                                switch (i)
                                {
                                    case 12:
                                        // Master ALT
                                        logger.LogInformation("Master ALT ON");
                                        flightConnect.MasterAltSet(true);
                                        break;
                                    case 13:
                                        // Master ALT
                                        logger.LogInformation("Master Alt OFF");
                                        flightConnect.MasterAltSet(false);
                                        break;

                                    case 14:
                                        // Master Battery
                                        logger.LogInformation("Battery ON");
                                        flightConnect.BatterySet(true);
                                        break;
                                    case 15:
                                        // Master Battery
                                        logger.LogInformation("Master Alt OFF");
                                        flightConnect.BatterySet(false);
                                        break;

                                    case 16:
                                        // Master Avionics
                                        logger.LogInformation("Avionics ON");
                                        flightConnect.AvionicsSet(true);
                                        break;
                                    case 17:
                                        // Master Avionics
                                        logger.LogInformation("Avionics OFF");
                                        flightConnect.AvionicsSet(false);
                                        break;

                                    case 20:
                                        // Beacon
                                        if (viewModel.BeaconEnabled)
                                        {
                                            logger.LogInformation("Beacon ON");
                                            flightConnect.BeaconSet(true);
                                        }
                                        break;
                                    case 21:
                                        // Beacon
                                        if (viewModel.BeaconEnabled)
                                        {
                                            logger.LogInformation("Beacon OFF");
                                            flightConnect.BeaconSet(false);
                                        }
                                        break;

                                    case 22:
                                        // Landing
                                        if (viewModel.LandingEnabled)
                                        {
                                            logger.LogInformation("Landing Lights ON");
                                            flightConnect.LandingSet(true);
                                        }
                                        break;
                                    case 23:
                                        // Landing
                                        if (viewModel.LandingEnabled)
                                        {
                                            logger.LogInformation("Landing Lights OFF");
                                            flightConnect.LandingSet(false);
                                        }
                                        break;

                                    case 24:
                                        // Taxi
                                        if (viewModel.TaxiEnabled)
                                        {
                                            logger.LogInformation("Taxi ON");
                                            flightConnect.TaxiSet(true);
                                        }
                                        break;
                                    case 25:
                                        // Taxi
                                        if (viewModel.TaxiEnabled)
                                        {
                                            logger.LogInformation("Taxi OFF");
                                            flightConnect.TaxiSet(false);
                                        }
                                        break;


                                    case 26:
                                        // Nav
                                        if (viewModel.NavEnabled)
                                        {
                                            logger.LogInformation("Nav ON");
                                            flightConnect.NavSet(true);
                                        }
                                        break;
                                    case 27:
                                        // Nav
                                        if (viewModel.NavEnabled)
                                        {
                                            logger.LogInformation("Nav OFF");
                                            flightConnect.NavSet(false);
                                        }
                                        break;


                                    case 28:
                                        // Strobe
                                        if (viewModel.StrobeEnabled)
                                        {
                                            logger.LogInformation("Strobe ON");
                                            flightConnect.StrobeSet(true);
                                        }
                                        break;
                                    case 29:
                                        // Strobe
                                        if (viewModel.StrobeEnabled)
                                        {
                                            logger.LogInformation("Strobe OFF");
                                            flightConnect.StrobeSet(false);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    lastButtons = state.Buttons;
                    await Task.Delay(100);
                }
            }
        }

        private async Task SyncButtonsAsync(Joystick input)
        {
            while (true)
            {
                input.Poll();
                var state = input.GetCurrentState();
                if (viewModel.BeaconSync)
                {
                    logger.LogDebug("Sync Beacon " + (state.Buttons[20] ? "ON" : "OFF"));
                    flightConnect.BeaconSet(state.Buttons[20]);
                }
                if (viewModel.LandingSync)
                {
                    logger.LogDebug("Sync Landing " + (state.Buttons[22] ? "ON" : "OFF"));
                    flightConnect.LandingSet(state.Buttons[22]);
                }
                if (viewModel.TaxiSync)
                {
                    logger.LogDebug("Sync Taxi " + (state.Buttons[24] ? "ON" : "OFF"));
                    flightConnect.TaxiSet(state.Buttons[24]);
                }
                if (viewModel.NavSync)
                {
                    logger.LogDebug("Sync Nav " + (state.Buttons[26] ? "ON" : "OFF"));
                    flightConnect.NavSet(state.Buttons[26]);
                }
                if (viewModel.StrobeSync)
                {
                    logger.LogDebug("Sync Strobe " + (state.Buttons[28] ? "ON" : "OFF"));
                    flightConnect.StrobeSet(state.Buttons[28]);
                }
                await Task.Delay(5000);
            }
        }
    }
}
