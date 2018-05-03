using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace IntelligentDemo.Convertors
{
    public class ScoreToColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var score = value as double?;
            if (score == null || targetType != typeof(Brush))
            {
                return null;
            }
            else
            {
                return new SolidColorBrush(Convert(score));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static Color Convert(double? score)
        {
            // Green - orange - red
            var red = System.Convert.ToByte(score <= 0.5 ? 255 : (1 - score) * 255);
            var green = System.Convert.ToByte(score >= 0.5 ? 255 : score * 255);
            return Color.FromArgb(255, red, green, 0);

            //if (score > 0.6)
            //{
            //    return Color.FromArgb(255, System.Convert.ToByte(255 * (1 - score.Value)), 255, 0);
            //}
            //else if (score < 0.4)
            //{
            //    return Color.FromArgb(255, 255, System.Convert.ToByte(score * 2 * 255), 0);
            //}
            //else
            //{
            //    return Color.FromArgb(255, System.Convert.ToByte(255 * (1 - score.Value) / 2), System.Convert.ToByte(score * 255), 255);
            //}
        }
    }
}
