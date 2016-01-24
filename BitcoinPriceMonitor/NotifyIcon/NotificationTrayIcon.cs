namespace BitcoinPriceMonitor
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    public class NotificationTrayIcon : INotificationTrayIcon
    {
        private ITradePriceMonitorContextMenu _contextMenu;
        private NotifyIcon _notifyIcon;

        public NotificationTrayIcon(ITradePriceMonitorContextMenu contextMenu)
        {
            _contextMenu = contextMenu;
            _notifyIcon = new NotifyIcon
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

        public void Update(double value)
        {
            _notifyIcon.Icon = CreateIconImage(Math.Round(value).ToString());
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
