using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using RefBox.Views.Cams;

namespace VideoCommon
{
    public class MainViewModel : ObservableObject
    {
        private ObservableCollection<Camera> _cameras = new ObservableCollection<Camera>();

        public ObservableCollection<Camera> Cameras
        {
            get { return _cameras; }
            set { SetProperty(ref _cameras, value); }
        }

        public void Init()
        {
            for (int i = 0; i < 25; i++)
            {
                Camera camera = new Camera();
                camera.InitVideoPlayer(() => new CameraView());
                Cameras.Add(camera);
            }
        }
    }
}
