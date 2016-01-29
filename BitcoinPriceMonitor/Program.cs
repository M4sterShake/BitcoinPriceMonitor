using BitcoinPriceMonitor.ApplicationEntryPoint;
using BitcoinPriceMonitor.Config;

namespace BitcoinPriceMonitor
{
    using System;
    using System.Windows.Forms;
    using StructureMap;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigureIocContainer();
            
            Application.Run();
        }

        private static void ConfigureIocContainer()
        {
            var container = new Container(new IocRegistry());
            IBitcoinPriceMonitorApp app = container.GetInstance<IBitcoinPriceMonitorApp>();
            app.Start();
        }
    }
}
