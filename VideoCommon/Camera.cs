using System;
using System.Windows.Controls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using RefBox.Models.Media;

namespace VideoCommon
{
    public sealed class Camera : ObservableObject, IDisposable
    {
        private UserControl _player;
        private DebugInfo _debugInfo = new DebugInfo();

        public UserControl Player
        {
            get { return _player; }
            set { SetProperty(ref _player, value); }
        }

        public MediaSource BitmapSource { get; set; }

        public DebugInfo DebugInfo
        {
            get => _debugInfo;
            set => SetProperty(ref _debugInfo, value);
        }

        public VideoFrame VideoFrame { get; set; }

        public void InitVideoPlayer(Func<UserControl> createFunc)
        {
            if (createFunc != null)
            {
                if (Player == null)
                {
                    Player = createFunc();
                    Player.DataContext = this;
                    VideoFrame = new VideoFrame(this);
                }
            }
        }


        public void Dispose()
        {
            
        }
    }
}
