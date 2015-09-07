using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ZHC.Common.UWP
{
    public class Toast
    {
        public static void NotNetwork()
        {
            SendToast("提示", "网络异常");
        }

        public static void SendToast(string title, string body,bool suppressPopup=false, Action callBack = null)
        {
            //ToastNotificationManager.CreateToastNotifier();
            string toastHeading = title;
            string toastBody = body;

            // Using the ToastText02 toast template.
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;

            // Retrieve the content part of the toast so we can change the text.
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            //Find the text component of the content
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");

            // Set the text on the toast. 
            // The first line of text in the ToastText02 template is treated as header text, and will be bold.
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastHeading));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(toastBody));

            // Set the duration on the toast
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");

            //指定Launch的參數，param1與param2是自定的
            ((XmlElement)toastNode).SetAttribute("launch",
                       "{\"type\":\"pin\"}");

            ((XmlElement)toastNode).SetAttribute("duration", "long");

            // Create the actual toast object using this toast specification.
            ToastNotification toast = new ToastNotification(toastXml);

            toast.Activated += (s,e)=> 
            {
                if (callBack!=null)
                {
                    callBack();
                }
            };
            toast.Dismissed += toast_Dismissed;
            toast.Failed += toast_Failed;
            //toast.ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(3600);
            // Set SuppressPopup = true on the toast in order to send it directly to action center without 
            // producing a popup on the user's phone.
            toast.SuppressPopup = suppressPopup;

            // Send the toast.
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        static void toast_Failed(ToastNotification sender, ToastFailedEventArgs args)
        {
            //throw new NotImplementedException();
        }

        static void toast_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            //throw new NotImplementedException();
        }
    }
}
