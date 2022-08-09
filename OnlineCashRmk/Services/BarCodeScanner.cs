using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Microsoft.Extensions.Configuration;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace OnlineCashRmk.Services
{
    public class BarCodeScanner
    {
        public SerialPort port;
        public BarCodeScanner(IConfiguration configuration)
        {
            string portNumber = configuration.GetSection("BarCodeScanner").Value;
            if (portNumber != "")
                try
                {
                    port = new SerialPort(portNumber);
                    // настройки порта
                    port.BaudRate = 9600;
                    port.Parity = Parity.None;
                    port.StopBits = StopBits.One;
                    port.DataBits = 8;
                    port.Handshake = Handshake.None;
                    port.RtsEnable = true;
                    port.DataReceived += new SerialDataReceivedEventHandler((s, e) =>
                      {
                          string[] forms = new string[] { nameof(Form1), nameof(FormArrival), nameof(FormStocktaking), nameof(FormWriteOf) };
                          if (forms.Where(f => f == Form.ActiveForm?.Name).FirstOrDefault() == null)
                          {
                              (s as SerialPort).ReadExisting();
                          };
                      });
                    port.Open();
                }
                catch (Exception e)
                {
                    return;
                }
        }
    }
}
