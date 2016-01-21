using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using StructureMap;

namespace BitcoinPriceMonitor
{
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
