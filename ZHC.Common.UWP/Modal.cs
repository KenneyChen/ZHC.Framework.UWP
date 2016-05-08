using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using ZHC.Common.UWP.Controls;

namespace ZHC.Common.UWP
{
    /// <summary>
    /// 弹框
    /// </summary>
    public class Modal
    {
        public static void Show(string msg)
        {
            //var size = ApplicationView.PreferredLaunchViewSize;
            //var rect = PointerDevice.GetPointerDevices().Last().ScreenRect;
            //var scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            Popup popup = new Popup()
            {
            };
            TipsControl tips = new TipsControl()
            {
                Text = msg,
            };
            var grid = new Grid()
            {
                Height = Window.Current.Bounds.Height,// (int)(rect.Height * scale),
                Width = Window.Current.Bounds.Width,//(int)(rect.Width * scale),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 150),
            };

            grid.Children.Add(tips);
            popup.Child = grid;
            popup.IsOpen = true;
            tips.Show();
            //popup.IsOpen = false;
            //popup = null;
        }
    }
}
