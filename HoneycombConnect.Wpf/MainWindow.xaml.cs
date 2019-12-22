using HoneycombConnect.SimConnectFSX;
using Microsoft.Extensions.Logging;
using SharpDX.DirectInput;
using System.Diagnostics;
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

        public MainWindow(ILogger<MainWindow> logger, FlightConnect flightConnect)
        {
            InitializeComponent();
            this.logger = logger;
            this.flightConnect = flightConnect;
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
                bool[] lastButtons = null;
                while (true)
                {
                    input.Poll();
                    var state = input.GetCurrentState();
                    var data = input.GetBufferedData();
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
                                        case 20:
                                            // Beacon
                                            logger.LogInformation("Beacon ON");
                                            flightConnect.BeaconOn();
                                            break;
                                        case 21:
                                            // Beacon
                                            logger.LogInformation("Beacon OFF");
                                            flightConnect.BeaconOff();
                                            break;

                                        case 22:
                                            // Beacon
                                            logger.LogInformation("Landing Lights ON");
                                            flightConnect.LandingOn();
                                            break;
                                        case 23:
                                            // Beacon
                                            logger.LogInformation("Landing Lights OFF");
                                            flightConnect.LandingOff();
                                            break;

                                        case 24:
                                            // Taxi
                                            logger.LogInformation("Taxi ON");
                                            flightConnect.TaxiOn();
                                            break;
                                        case 25:
                                            // Taxi
                                            logger.LogInformation("Taxi OFF");
                                            flightConnect.TaxiOff();
                                            break;


                                        case 26:
                                            // Nav
                                            logger.LogInformation("Nav ON");
                                            flightConnect.NavOn();
                                            break;
                                        case 27:
                                            // Nav
                                            logger.LogInformation("Nav OFF");
                                            flightConnect.NavOff();
                                            break;


                                        case 28:
                                            // Strobe
                                            logger.LogInformation("Strobe ON");
                                            flightConnect.StrobeOn();
                                            break;
                                        case 29:
                                            // Strobe
                                            logger.LogInformation("Strobe OFF");
                                            flightConnect.StrobeOff();
                                            break;
                                    }
                                }
                            }
                        }
                        lastButtons = state.Buttons;
                        await Task.Delay(100);
                    }
                }
            });
        }
    }
}
