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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set up IoC container.
            var container = new Container(new IocRegistry());
            ITradePriceMonitor priceMonitor = container.GetInstance<ITradePriceMonitor>();
            INotificationTrayIcon trayIcon = container.GetInstance<INotificationTrayIcon>();
            priceMonitor.StartMonitoring((result) =>
            {
                trayIcon.Update(Math.Round(result).ToString());
            });

            Application.Run();
        }
    }
}
