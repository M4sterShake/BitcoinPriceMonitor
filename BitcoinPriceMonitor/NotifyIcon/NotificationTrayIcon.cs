using System.Globalization;
using BitcoinPriceMonitor.Observer;
using BitcoinPriceMonitor.Utils;

namespace BitcoinPriceMonitor.NotifyIcon
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using ContextMenu;

    public class NotificationTrayIcon : INotificationTrayIcon
    {
        private readonly ITradePriceMonitorContextMenu _contextMenu;
        private readonly System.Windows.Forms.NotifyIcon _notifyIcon;

        public NotificationTrayIcon(ITradePriceMonitorContextMenu contextMenu)
        {
            _contextMenu = contextMenu;
            _notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                ContextMenu = _contextMenu.Menu,
                Icon = CreateIconImage("-")
            };
        }

        public Guid ObserverId { get; } = Guid.NewGuid();

        public void Close()
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            TaskBarUtils.RefreshNotificationArea();
        }

        public void Update(TradePrice price)
        {
            _notifyIcon.Icon = CreateIconImage(Math.Round(price.Price).ToString(CultureInfo.InvariantCulture));
            _notifyIcon.Text = $"{price.Price} {price.Currency}";
        }

        private Icon CreateIconImage(string sImageText)
        {
            Bitmap objBmpImage = new Bitmap(1, 1);
            Font objFont = new Font("Arial", 28, FontStyle.Regular, GraphicsUnit.Pixel);
            Graphics objGraphics = Graphics.FromImage(objBmpImage);
            var intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            var intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;
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
