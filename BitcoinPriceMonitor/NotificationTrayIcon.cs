using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitcoinPriceMonitor
{
    public class NotificationTrayIcon : INotificationTrayIcon
    {
        private ITradePriceMonitorContextMenu _contextMenu;
        private NotifyIcon notifyIcon;

        public NotificationTrayIcon(ITradePriceMonitorContextMenu contextMenu)
        {
            _contextMenu = contextMenu;
            notifyIcon = new NotifyIcon
            {
                Visible = true,
                ContextMenu = _contextMenu.GetMenu()
            };
        }

        public void Update(string iconText)
        {
            notifyIcon.Icon = CreateIconImage(iconText);
        }

        private Icon CreateIconImage(string sImageText)
        {
            Bitmap objBmpImage = new Bitmap(1, 1);
            int intWidth = 0;
            int intHeight = 0;
            Font objFont = new Font("Arial", 28, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            Graphics objGraphics = Graphics.FromImage(objBmpImage);
            intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;
            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));
            objGraphics = Graphics.FromImage(objBmpImage);
            objGraphics.Clear(Color.Transparent);
            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(255, 255, 255)), 0, 0);
            objGraphics.Flush();

            return Icon.FromHandle(objBmpImage.GetHicon());
        }


    }
}
