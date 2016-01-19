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

            var container = new Container(new IocRegistry());
            var priceMonitor = container.GetInstance<ITradePriceMonitor>();
            var trayIcon = container.GetInstance<INotificationTrayIcon>();
            priceMonitor.StartMonitoring((result) =>
            {
                trayIcon.Update(Math.Round(result).ToString());
            });

            AppDomain.CurrentDomain.ProcessExit += (object sender, EventArgs e) => trayIcon.Close();
            Application.Run();
        }
    }
}
