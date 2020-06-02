using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Views.Animations;
using Android.Widget;
using DemoTest;
using DemoTest.Droid.Renderers;
using MyAwesomeExtensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HeartBeatView), typeof(HeartBeatViewRenderer))]
namespace DemoTest.Droid.Renderers
{
    public class HeartBeatViewRenderer : ViewRenderer<HeartBeatView, ImageView>
    {
        public HeartBeatViewRenderer(Context context) : base(context)
        {
        }

        protected async override void OnElementChanged(ElementChangedEventArgs<HeartBeatView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(CreateNativeControl());
            }

            if (e.NewElement != null)
            {
                await BuildHeartBeat();
                UpdateBeatState();
            }
        }

        protected async override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == HeartBeatView.SourceProperty.PropertyName)
            {
                await BuildHeartBeat();
            }
            else if (e.PropertyName == HeartBeatView.IsBeatingProperty.PropertyName)
            {
                UpdateBeatState();
            }
        }

        protected override ImageView CreateNativeControl()
        {
            return new ImageView(Context);
        }

        private async Task BuildHeartBeat()
        {
            Control.SetImageBitmap(await Element.Source.GetNativeImageAsync());
        }

        private void UpdateBeatState()
        {
            if (Element.IsBeating)
            {
                Action animation = new Action(() =>
                {
                    ScaleAnimation Ani = new ScaleAnimation(1, 1.2f, 1, 1.2f, Control.Width / 2, Control.Height / 2);
                    Ani.RepeatCount = ScaleAnimation.Infinite;
                    Ani.Duration = 750;
                    Ani.Interpolator = new HeartBeatInterpolator();
                    Control.StartAnimation(Ani);
                });
                Control.Post(animation);
            }
            else
            {
                Control.ClearAnimation();
            }
        }

        private class HeartBeatInterpolator : Java.Lang.Object, IInterpolator
        {
            public float GetInterpolation(float input)
            {
                float x = input < 1 / 3f ? 2 * input : (1 + input) / 2;
                return (float)Math.Sin(x * Math.PI);
            }
        }
    }
}
