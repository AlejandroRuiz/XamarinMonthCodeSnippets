using System;
using Xamarin.Forms;

namespace DemoTest
{
    public class HeartBeatView : View
    {
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(HeartBeatView), default(ImageSource));

        public static readonly BindableProperty IsBeatingProperty =
            BindableProperty.Create(nameof(IsBeating), typeof(bool), typeof(HeartBeatView), default(bool));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public bool IsBeating
        {
            get => (bool)GetValue(IsBeatingProperty);
            set => SetValue(IsBeatingProperty, value);
        }
    }
}
