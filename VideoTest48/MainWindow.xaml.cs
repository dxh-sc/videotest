using System.Windows;
using VideoCommon;

namespace VideoTest48
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainView;

        public MainWindow()
        {
            _mainView = new MainViewModel();
            InitializeComponent();
            DataContext = _mainView;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mainView.Init();
        }
    }
}
