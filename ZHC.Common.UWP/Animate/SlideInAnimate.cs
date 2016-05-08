using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ZHC.Common.UWP.Animate
{
    public enum SlideType
    {
        LeftIn = 0,
        UpIn = 1
    }

    public class SlideInAnimate
    {
        public Action Completed { get; set; }

        public void Animate(List<UIElement> elements, SlideType type = 0, double time1 = 0.2, double time2 = 0.6, double interval = 0.1)
        {
            Storyboard storyboard = new Storyboard();

            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].RenderTransform=new CompositeTransform();
                time1 += interval;
                time2 += interval;
                DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTargetProperty(frames, "(UIElement.Opacity)");
                Storyboard.SetTarget(frames, elements[i]);
                DiscreteDoubleKeyFrame frame7 = new DiscreteDoubleKeyFrame();
                frame7.KeyTime=KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
                frame7.Value=0.0;
                DiscreteDoubleKeyFrame frame = frame7;
                DiscreteDoubleKeyFrame frame8 = new DiscreteDoubleKeyFrame();
                frame8.KeyTime=KeyTime.FromTimeSpan(TimeSpan.FromSeconds(time1));
                frame8.Value=0.0;
                DiscreteDoubleKeyFrame frame2 = frame8;
                EasingDoubleKeyFrame frame9 = new EasingDoubleKeyFrame();
                frame9.KeyTime=KeyTime.FromTimeSpan(TimeSpan.FromSeconds(time2));
                frame9.Value=1.0;
                EasingDoubleKeyFrame frame3 = frame9;
                CubicEase ease = new CubicEase();
                ease.EasingMode = EasingMode.EaseIn;
                frame3.EasingFunction=ease;
                frames.KeyFrames.Add(frame);
                frames.KeyFrames.Add(frame2);
                frames.KeyFrames.Add(frame3);
                DoubleAnimationUsingKeyFrames frames2 = new DoubleAnimationUsingKeyFrames();
                string str = "";
                switch (type)
                {
                    case SlideType.LeftIn:
                        str = "(UIElement.RenderTransform).(CompositeTransform.TranslateX)";
                        break;

                    case SlideType.UpIn:
                        str = "(UIElement.RenderTransform).(CompositeTransform.TranslateY)";
                        break;

                    default:
                        str = "(UIElement.RenderTransform).(CompositeTransform.TranslateX)";
                        break;
                }
                Storyboard.SetTargetProperty(frames2, str);
                Storyboard.SetTarget(frames2, elements[i]);
                DiscreteDoubleKeyFrame frame10 = new DiscreteDoubleKeyFrame();
                frame10.KeyTime=KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
                frame10.Value=60.0;
                DiscreteDoubleKeyFrame frame4 = frame10;
                DiscreteDoubleKeyFrame frame11 = new DiscreteDoubleKeyFrame();
                frame11.KeyTime=KeyTime.FromTimeSpan(TimeSpan.FromSeconds(time1));
                frame11.Value=60.0;
                DiscreteDoubleKeyFrame frame5 = frame11;
                EasingDoubleKeyFrame frame12 = new EasingDoubleKeyFrame();
                frame12.KeyTime=KeyTime.FromTimeSpan(TimeSpan.FromSeconds(time2));
                frame12.Value=0.0;
                EasingDoubleKeyFrame frame6 = frame12;
                CubicEase ease2 = new CubicEase();
                ease2.EasingMode= EasingMode.EaseIn ;
                frame6.EasingFunction=ease2;
                frames2.KeyFrames.Add(frame4);
                frames2.KeyFrames.Add(frame5);
                frames2.KeyFrames.Add(frame6);
                storyboard.Children.Add(frames);
                storyboard.Children.Add(frames2);
            }
            storyboard.Begin();
            if (Completed != null)
            {
                storyboard.Completed += sb_Completed;
            }
        }

       
        private void sb_Completed(object sender, object e)
        {
            (sender as Storyboard).Completed-= sb_Completed;
            if (this.Completed != null)
            {
                this.Completed();
            }
        }
    }
}
