namespace BitcoinPriceMonitorTests
{ 
    using System;
    using System.Windows.Forms;
    using BitcoinPriceMonitor;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Drawing;
    using System.Reflection;
    using System.Security.Cryptography;

    /// <summary>
    /// Summary description for NotificationTrayIconTests
    /// </summary>
    [TestClass]
    public class NotificationTrayIconTests
    {
        [TestMethod]
        public void NotificationTrayIcon_ObserverIdTest()
        {
            // Arrange
            var mockContextMenu = new Mock<ITradePriceMonitorContextMenu>();
            var target = new NotificationTrayIcon(mockContextMenu.Object);

            //Act
            var observerId = target.ObserverId;

            //Assert
            Assert.IsNotNull(observerId);
            Assert.IsTrue(observerId.ToString().Length >= 32);
        }

        [TestMethod]
        public void NotificationTrayIcon_UpdateTest()
        {
            // Arrange
            var mockContextMenu = new Mock<ITradePriceMonitorContextMenu>();
            var target = new NotificationTrayIcon(mockContextMenu.Object);
            var originalIcon = ((NotifyIcon)GetInstanceField(typeof (NotificationTrayIcon), target, "_notifyIcon")).Icon;//((NotifyIcon) targetPrivateObj.GetFieldOrProperty("_notifyIcon", BindingFlags.GetField)).Icon;

            // Act
            target.Update(2.5);
            var iconAfterUpdate = ((NotifyIcon)GetInstanceField(typeof(NotificationTrayIcon), target, "_notifyIcon")).Icon;//((NotifyIcon) targetPrivateObj.GetFieldOrProperty("_notifyIcon",BindingFlags.GetField)).Icon;

            // Assert
            Assert.IsFalse(CompareIcons(originalIcon, iconAfterUpdate));
        }

        [TestMethod]
        public void NotificationTrayIcon_CloseTest()
        {
            // Arrange
            var mockContextMenu = new Mock<ITradePriceMonitorContextMenu>();
            var target = new NotificationTrayIcon(mockContextMenu.Object);
            var originalIconVisibility = ((NotifyIcon) GetInstanceField(typeof (NotificationTrayIcon), target, "_notifyIcon")).Visible;

            // Act
            target.Close();
            var iconVisibilityAfterUpdate = ((NotifyIcon)GetInstanceField(typeof(NotificationTrayIcon), target, "_notifyIcon")).Visible;//((NotifyIcon) targetPrivateObj.GetFieldOrProperty("_notifyIcon",BindingFlags.GetField)).Icon;
            
            // Assert
            Assert.IsTrue(originalIconVisibility);
            Assert.IsFalse(iconVisibilityAfterUpdate);
        }

        private bool CompareIcons(Icon firstIcon, Icon secondIcon)
        {
            return ComputeIconHash(firstIcon) == ComputeIconHash(secondIcon);
        }

        private byte[] ComputeIconHash(Icon icon)
        {
            ImageConverter converter = new ImageConverter();
            byte[] rawIcon = converter.ConvertTo(icon.ToBitmap(), typeof(byte[])) as byte[];
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(rawIcon);
        }

        /// <summary>
        /// Uses reflection to get the field value from an object.
        /// </summary>
        ///
        /// <param name="type">The instance type.</param>
        /// <param name="instance">The instance object.</param>
        /// <param name="fieldName">The field's name which is to be fetched.</param>
        ///
        /// <returns>The field value from the object.</returns>
        internal static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}
