using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace IntelligentDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        public static SecretValues Secrets { get; } = SecretValues.Load();

        public class SecretValues
        {
            public static SecretValues Load()
            {
                return JsonConvert.DeserializeObject<SecretValues>(File.ReadAllText("secrets.json"));
            }

            public string EmotionKey { get; set; }
            public string SentimentKey { get; set; }
        }
    }
}
