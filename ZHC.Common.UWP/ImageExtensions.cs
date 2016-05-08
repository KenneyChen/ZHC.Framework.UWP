using Q42.WinRT.Data;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace ZHC.Common.UWP.Controls
{
    /// <summary>
    /// Attached properties for Images
    /// </summary>
    public static class ImageExtensions
    {

        /// <summary>
        /// Using a DependencyProperty as the backing store for WebUri.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CacheUriProperty =
            DependencyProperty.RegisterAttached(
                "CacheUri",
                typeof(Uri),
                typeof(ImageExtensions),
                new PropertyMetadata(null, OnCacheUriChanged));

        /// <summary>
        /// Gets the CacheUri property. This dependency property 
        /// WebUri that has to be cached
        /// </summary>
        public static Uri GetCacheUri(DependencyObject d)
        {
            return (Uri)d.GetValue(CacheUriProperty);
        }

        /// <summary>
        /// Sets the CacheUri property. This dependency property 
        /// WebUri that has to be cached
        /// </summary>
        public static void SetCacheUri(DependencyObject d, Uri value)
        {
            d.SetValue(CacheUriProperty, value);
        }

        private static async void OnCacheUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            //Uri oldCacheUri = (Uri)e.OldValue;
            Uri newCacheUri = (Uri)d.GetValue(CacheUriProperty);

            if (newCacheUri != null)
            {

                try
                {

                    //Get image from cache (download and set in cache if needed)
                    var cacheUri = await WebDataCache.GetLocalUriAsync(newCacheUri);

                    // Check if the wanted image uri has not changed while we were loading
                    if (newCacheUri != (Uri)d.GetValue(CacheUriProperty)) return;

                    //Set cache uri as source for the image
                    SetSourceOnObject(d, new BitmapImage(cacheUri));



                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);

                    //Revert to using passed URI
                    SetSourceOnObject(d, new BitmapImage(newCacheUri), false);
                }

            }
            else
            {
                SetSourceOnObject(d, null, false);
            }

        }

        private static void SetSourceOnObject(object imgControl, ImageSource imageSource, bool throwEx = true)
        {

            try
            {
                if (imgControl is Image)
                {
                    ((Image)imgControl).Source = imageSource;
                }
                else
                {
                    if (imgControl is ImageBrush)
                    {
                        ((ImageBrush)imgControl).ImageSource = imageSource;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                if (throwEx)
                {
                    throw ex;
                }
            }

        }
        

    }

}