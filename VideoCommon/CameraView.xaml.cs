using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RefBox.Models.Media;
using VideoCommon;

namespace RefBox.Views.Cams
{
    /// <summary>
    /// Interaction logic for Camera.xaml
    /// </summary>
    public partial class CameraView : UserControl
    {
        public CameraView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnLoaded;
            try
            {
                if (!(DataContext is Camera camera))
                {
                    return;
                }
                WorkThreadHost.Init(() =>
                {
                    camera.BitmapSource?.Dispose();
                    camera.BitmapSource = null;
                    CameraThread control = new CameraThread { DataContext = camera };
                    camera.BitmapSource = new MediaSource(control, camera);
                    camera.BitmapSource.Init(control.ImageBrush);
                    return control;
                });
            }
            catch (Exception e)
            {
                //Logger.Error("BitmapSource.log", "[CameraView]OnLoaded error", e);
            }
        }

        private void UIElement_OnManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.Mode = ManipulationModes.Scale | ManipulationModes.Translate;
        }
    }
}
