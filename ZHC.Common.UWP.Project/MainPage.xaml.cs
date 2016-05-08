using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace ZHC.Common.UWP.Project
{


    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string ReChargeTypeName
        {
            get { return "小石头"; }
        }
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }        /// <summary>
                 /// 消息提醒
                 /// </summary>
                 /// <param name="msg"></param>
        public static async void ShowToast(string msg = "")
        {
            try
            {
                Modal.Show("测试文字");
                //var size = ApplicationView.PreferredLaunchViewSize;
                //var rect = PointerDevice.GetPointerDevices().Last().ScreenRect;
                //var scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                //Popup popup = new Popup()
                //{
                //    //Height = (int)(rect.Height * scale),
                //    //Width = (int)(rect.Width * scale),
                //    //VerticalAlignment = VerticalAlignment.Bottom,
                //    //HorizontalAlignment = HorizontalAlignment.Center,

                //};
                //TipsControl tips = new TipsControl()
                //{
                //    //Height = Window.Current.Bounds.Height / 4,
                //    //Width = Window.Current.Bounds.Width / 4,
                //    //HorizontalAlignment = HorizontalAlignment.Center,
                //    //VerticalAlignment = VerticalAlignment.Bottom,

                //};
                //var grid = new Grid()
                //{
                //    Height = Window.Current.Bounds.Height,// (int)(rect.Height * scale),
                //    Width = Window.Current.Bounds.Width,//(int)(rect.Width * scale),
                //    HorizontalAlignment = HorizontalAlignment.Center,
                //    //VerticalAlignment = VerticalAlignment.Bottom,
                //    Margin = new Thickness(0, 0, 0, 250),
                //    //Background = new SolidColorBrush(Colors.Black),
                //};

                //grid.Children.Add(tips);
                //popup.Child = grid;
                //popup.IsOpen = true;
                //tips.Show();
                //await Task.Delay(TimeSpan.FromSeconds(3));
                //popup.IsOpen = false;
                //popup = null;

            }
            catch
            {
            }
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {

            //MessageDialog md = new MessageDialog("cessss");
            //await md.ShowAsync();
            ShowToast();
        }
    }
}
