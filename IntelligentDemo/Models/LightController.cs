using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IntelligentDemo.Models
{
    public class LightController
    {
        //private SerialDevice _arduinoPort;
        //private DataWriter _arduinoWriter;


        //var aqs = SerialDevice.GetDeviceSelector();
        //var dis = DeviceInformation.FindAllAsync(aqs).AsTask().Result;
        //_arduinoPort = SerialDevice.FromIdAsync(dis[0].Id).AsTask().Result;
        //_arduinoPort.BaudRate = 9600;
        //_arduinoWriter = new DataWriter(_arduinoPort.OutputStream);

        public void SetColor(Color color)
        {
            var msg = $"{color.R},{color.G},{color.B},";
            //_arduinoWriter.WriteString(msg);
            // await _arduinoWriter.StoreAsync();
        }

        public void Dispose()
        {
            //SetColor(Color.FromRgb(0, 0, 0));
            //_midi.Dispose();
            //_arduinoPort.Dispose();
        }
    }
}
