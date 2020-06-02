using System.Threading.Tasks;
using Xamarin.Forms;
#if __IOS__
using Xamarin.Forms.Platform.iOS;
using UIKit;
#elif __ANDROID__
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;
#endif

namespace MyAwesomeExtensions
{
    public static class ImageSourceExtensions
    {
        public static IImageSourceHandler GetDefaultHandler(this ImageSource source)
        {
            IImageSourceHandler handler = null;
            if (source is UriImageSource)
            {
                handler = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                handler = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                handler = new StreamImagesourceHandler();
            }
            else if (source is FontImageSource)
            {
                handler = new FontImageSourceHandler();
            }
            return handler;
        }

        public static
#if __IOS__
            Task<UIImage>
#elif __ANDROID__
            Task<Bitmap>
#endif
            GetNativeImageAsync(this ImageSource source)
        {
            if (source == null)
                return null;

            var nativeImageHandler = source.GetDefaultHandler();
#if __IOS__
            float scale = (float)UIScreen.MainScreen.Scale;

            return nativeImageHandler.LoadImageAsync(source, scale: scale);
#elif __ANDROID__
            return nativeImageHandler.LoadImageAsync(source, Application.Context);
#endif
        }
    }
}
