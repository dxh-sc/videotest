using System.Windows;
using System.Windows.Controls;

namespace VideoCommon
{
    /// <summary>
    /// Interaction logic for MainArea.xaml
    /// </summary>
    public partial class MainArea : UserControl
    {
        public MainArea()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel mainView)
            {
                foreach (Camera camera in mainView.Cameras)
                {
                    camera.DebugInfo.FrameUIMiss = 0;
                }
            }
        }
    }
}
