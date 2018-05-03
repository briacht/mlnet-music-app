using System.IO.Ports;
using System.Windows.Media;

namespace IntelligentDemo.Models
{
    public class LightController
    {
        private SerialPort _port;

        public LightController()
        {
            _port = new SerialPort("COM3", 9600);
            _port.Open();
            SetColor(Color.FromRgb(0, 0, 0));
        }

        public void SetColor(Color color)
        {
            var msg = $"{color.R},{color.G},{color.B},";
            _port.Write(msg);
        }

        public void Dispose()
        {
            SetColor(Color.FromRgb(0, 0, 0));
            _port.Close();
            _port.Dispose();
        }
    }
}
