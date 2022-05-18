using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VideoCommon
{
    public class VideoFrame : IDisposable
    {
        private readonly AutoResetEvent _frameARE = new AutoResetEvent(false);
        private bool disposedValue;
        private Camera _camera;
        private readonly FrameInfo _frameInfo;
        private readonly int _fps = 30;
        private DateTime _sendLasttime;

        public VideoFrame(Camera camera)
        {
            _camera = camera;
            _frameInfo = new FrameInfo
            {
                width = 960,
                height = 540,
            };
            _frameInfo.frameSize = _frameInfo.width * _frameInfo.height * 4;
            if (_frameInfo.width == 1920)
            {
                ReadTestData1920_1080();
            }
            else if(_frameInfo.width == 960)
            {
                ReadTestData960_540();
            }
            Task.Factory.StartNew(Display, TaskCreationOptions.LongRunning);
            _camera.DebugInfo.DisplayFps = 1000 / _fps;
        }

        private void ReadTestData960_540()
        {
            byte[] image = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "test.bgr32");
            _frameInfo.data = image.BytesToIntptr(0);
        }

        private void ReadTestData1920_1080()
        {
            byte[] image = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "test.bgr32");
            List<byte> temps = new List<byte>();
            temps.AddRange(image);
            temps.AddRange(image);
            temps.AddRange(image);
            temps.AddRange(image);
            _frameInfo.data = temps.ToArray().BytesToIntptr(0);
        }

        private void Display()
        {
            int excuteTime = 0;
            while (!disposedValue)
            {
                try
                {
                    DateTime start = DateTime.Now;
                    _camera.DebugInfo.DisplayRealInterval = (int)(start - _sendLasttime).TotalMilliseconds;
                    _sendLasttime = start;

                    _camera?.BitmapSource?.DisplayFrame(_frameInfo);
                    _camera?.BitmapSource?.DisplayVUMeter();

                    excuteTime = (int)(DateTime.Now - start).TotalMilliseconds;
                }
                catch
                {
                    //
                }
                if (excuteTime < _camera.DebugInfo.DisplayFps) _frameARE.WaitOne(_camera.DebugInfo.DisplayFps - excuteTime);
            }
            _camera = null;
        }

        public void Dispose()
        {
            disposedValue = true;
            _frameARE.Set();
        }
    }
}
