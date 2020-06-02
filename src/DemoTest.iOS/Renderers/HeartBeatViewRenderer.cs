using System;
using System.Threading.Tasks;
using CoreAnimation;
using DemoTest;
using DemoTest.iOS.Renderers;
using Foundation;
using MyAwesomeExtensions;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HeartBeatView), typeof(HeartBeatViewRenderer))]
namespace DemoTest.iOS.Renderers
{
    public class HeartBeatViewRenderer : ViewRenderer<HeartBeatView, UIImageView>
    {
        public HeartBeatViewRenderer()
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

        protected override UIImageView CreateNativeControl()
        {
            var nativeView = new UIImageView(this.Frame);
            nativeView.ContentMode = UIViewContentMode.ScaleAspectFit;
            return nativeView;
        }

        private async Task BuildHeartBeat()
        {
            Control.Image = await Element.Source.GetNativeImageAsync();
        }

        void UpdateBeatState()
        {
            var hasAnimationRegistered = Control.Layer.AnimationForKey("HeartAnimation") != null;
            if (Element.IsBeating)
            {
                if (!hasAnimationRegistered)
                {
                    CABasicAnimation animation = CABasicAnimation.FromKeyPath("transform.scale");
                    animation.AutoReverses = true;
                    animation.From = new NSNumber(1);
                    animation.To = new NSNumber(1.2f);
                    animation.Duration = 0.5;
                    animation.RepeatCount = int.MaxValue;
                    animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseIn);

                    Control.Layer.AddAnimation(animation, "HeartAnimation");
                }
            }
            else
            {
                if (hasAnimationRegistered)
                {
                    Control.Layer.RemoveAnimation("HeartAnimation");
                }
            }
        }
    }
}
