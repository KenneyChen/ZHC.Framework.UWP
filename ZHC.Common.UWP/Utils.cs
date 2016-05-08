using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Input;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ZHC.Common.UWP
{
    public class Utils
    {
        /// <summary>
        /// Copy a string to Windows Clipboard
        /// </summary>
        /// <param name="str"></param>
        public static void CopyToClipBoard(string str)
        {
            var dp = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy,
            };
            dp.SetText(str);
            Clipboard.SetContent(dp);
        }

        public static int GetScreenHeight()
        {
            var rect = PointerDevice.GetPointerDevices().Last().ScreenRect;
            var scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            return (int)(rect.Height * scale);
        }

        public static int GetScreenWidth()
        {
            var rect = PointerDevice.GetPointerDevices().Last().ScreenRect;
            var scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            return (int)(rect.Width * scale);
        }

        public static async Task HideStatusBar()
        {
            if (IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }
        }

        /// <summary>
        /// 判断能否包括某个程序集 如某个dll pc端不能调用
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsTypePresent(string typeName)
        {
            return Windows.Foundation.Metadata.ApiInformation.IsTypePresent(typeName);
        }

        /// <summary>
        /// 非mobile显示后退按钮
        /// </summary>
        public static void ShowCanBack()
        {
            if (Utils.IsTypePresent("Windows.UI.Core.SystemNavigationManager"))
            {
                var m = SystemNavigationManager.GetForCurrentView();
                m.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                m.BackRequested += (s, e) =>
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    if (rootFrame == null)
                    {
                        return;
                    }
                    if (rootFrame.CanGoBack)
                    {
                        e.Handled = true;
                        rootFrame.GoBack();
                    }
                };
            }
        }

        /// <summary>
        /// open url webbroswer
        /// </summary>
        /// <param name="uriToLaunch"></param>
        /// <returns></returns>
        public static async Task LaunchUriAsync(string uriToLaunch)
        {
            var uri = new Uri(uriToLaunch);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        /*协议参考https://msdn.microsoft.com/en-us/library/windows/apps/mt228343.aspx*/
        /// <summary>
        /// 跳转到评论页
        /// </summary>
        /// <returns></returns>
        public static async Task ReviewApp(string productId)
        {
            //http://www.cnblogs.com/zhxilin/p/4819372.html

            //await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=[app ID]"));
            // or simply for the current app:
            //await LaunchUriAsync("ms-windows-store:reviewapp");
            // await LaunchUriAsync("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName);
            await LaunchUriAsync("ms-windows-store:REVIEW?PFN=" + productId);
        }

        ///// <summary>
        ///// 跳转到评论页
        ///// </summary>
        ///// <returns></returns>
        //public static async Task Download()
        //{
        //    //await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=[app ID]"));
        //    // or simply for the current app:
        //    //await LaunchUriAsync("ms-windows-store:reviewapp");
        //    await LaunchUriAsync("ms-windows-store:REVIEW?PFN=" + Package.Current.Id.FamilyName);
        //}


        //public string Test { get; set; } = "hello world";

        public static async Task SendEmail(string address, string title, string body)
        {
            //Uri mailto = new Uri("mailto:?to=recipient@abc.com&subject=email subject&body=Hello from  Windows 8.1.");
            await LaunchUriAsync($"mailto:?to={address}&subject={title}&body={body}");
        }
    }
}
