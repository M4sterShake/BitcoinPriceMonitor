namespace BitcoinPriceMonitor.NotifyIcon
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using ContextMenu;
    using System.Globalization;
    using System.Timers;
    using Observer;
    using Utils;

    public class NotificationTrayIcon : INotificationTrayIcon
    {
        private readonly ITradePriceMonitorContextMenu _contextMenu;
        private readonly System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _displayingCurrency = false;
        private Timer _iconSwitcherTimer;
        private TradePrice _currentPrice;

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
            NativeMethods.RefreshNotificationArea();
        }

        public void Update(TradePrice price)
        {
            _currentPrice = price;
            if (_iconSwitcherTimer == null)
            {
                _iconSwitcherTimer = new Timer();
                _iconSwitcherTimer.Elapsed += delegate { SwitchIcon(); };
                _iconSwitcherTimer.Interval = 5000;
                _iconSwitcherTimer.Enabled = true;
            }
            _notifyIcon.Icon = CreateIconImage(_displayingCurrency
                    ? _currentPrice.Currency.ToString()
                    : Math.Round(_currentPrice.Price).ToString(CultureInfo.InvariantCulture));
            _notifyIcon.Text = $"{price.Price} {price.Currency}";
        }

        private void SwitchIcon()
        {
            if (_displayingCurrency)
            {
                _notifyIcon.Icon = CreateIconImage(Math.Round(_currentPrice.Price).ToString(CultureInfo.InvariantCulture));
                _displayingCurrency = false;
            }
            else
            {
                _notifyIcon.Icon = CreateIconImage(_currentPrice.Currency.ToString());
                _displayingCurrency = true;
            }
        }

        private Icon CreateIconImage(string sImageText)
        {
            Bitmap iconBmpImage = new Bitmap(64, 64);
            Font preferedFont = new Font("Courier New", 40, FontStyle.Bold, GraphicsUnit.Pixel);
            Graphics iconGraphics = Graphics.FromImage(iconBmpImage);
            var scaledFont = GetAdjustedFont(iconGraphics, sImageText, preferedFont, iconBmpImage.Width, iconBmpImage.Height, 100, 3, true);
            var iconTrimmedSize= iconGraphics.MeasureString(sImageText, scaledFont);
            iconBmpImage = new Bitmap(iconBmpImage, new Size((int)iconTrimmedSize.Width, (int)iconTrimmedSize.Height -1));
            iconGraphics = Graphics.FromImage(iconBmpImage);
            iconGraphics.Clear(Color.Transparent);
            iconGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            iconGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;//.ClearTypeGridFit;
            iconGraphics.DrawString(sImageText, scaledFont, new SolidBrush(Color.FromArgb(255, 255, 255)), 1, 0);
            iconGraphics.Flush();

            return Icon.FromHandle(iconBmpImage.GetHicon());
        }

        public Font GetAdjustedFont(Graphics GraphicRef, string GraphicString, Font OriginalFont, int ContainerWidth, int ContainerHeight, int MaxFontSize, int MinFontSize, bool SmallestOnFail)
        {         
            for (int AdjustedSize = MaxFontSize; AdjustedSize >= MinFontSize; AdjustedSize--)
            {
                Font TestFont = new Font(OriginalFont.Name, AdjustedSize, OriginalFont.Style);
                SizeF AdjustedSizeNew = GraphicRef.MeasureString(GraphicString, TestFont);

                if (ContainerWidth >= Convert.ToInt32(AdjustedSizeNew.Width))
                {
                    return TestFont;
                }
            }

            if (SmallestOnFail)
            {
                return new Font(OriginalFont.Name, MinFontSize, OriginalFont.Style);
            }
            else
            {
                return OriginalFont;
            }
        }
    }
}
