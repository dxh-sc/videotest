using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VideoCommon;

namespace RefBox.Models.Media
{
    public class MediaSource : IDisposable
    {
        protected WriteableBitmap _frameBitmap;
        protected UserControl _element;
        protected ImageBrush _image;
        protected int _frameSize;
        protected IntPtr _backBufferPointer;
        protected Int32Rect _int32Rect;
        protected string _ip;
        protected Camera _camera;
        protected DateTime _lastDateTime;
        private readonly DebugInfo _debugInfo = new DebugInfo();

        public MediaSource(UserControl userControl, Camera camera)
        {
            _element = userControl;
            _camera = camera;
        }

        public void Init(ImageBrush image)
        {
            _image = image;
        }

        protected void InitPlayer(int width, int height)
        {
            _frameSize = width * height * 4;
            _int32Rect = new Int32Rect(0, 0, width, height);
            _element?.Dispatcher?.Invoke(() =>
            {
                if (_frameBitmap != null) _frameBitmap = null;
                _frameBitmap = new WriteableBitmap(width, height, 96.0, 96.0, PixelFormats.Bgr32, null);
                _backBufferPointer = _frameBitmap.BackBuffer;
                _image.ImageSource = _frameBitmap;
            }, DispatcherPriority.Send);
        }

        public void DisplayFrame(FrameInfo frameInfo)
        {
            _lastDateTime = DateTime.Now;
            if (frameInfo.frameSize != _frameSize)
            {
                InitPlayer(frameInfo.width, frameInfo.height);
                //Logger.Warning("BitmapSource.log",$"[MediaSourceBase][{_child}]source and target frame size are not same, {_frameSize} - {frameInfo.frameSize}, {_channel}_{_camera.Channel}_{_ip}");
            }
            else
            {
                _element?.Dispatcher?.Invoke(() =>
                {
                    _frameBitmap.Lock();
                    //_frameBitmap.WritePixels(_int32Rect, frameInfo.data, frameInfo.frameSize, _frameBitmap.BackBufferStride);
                    unsafe
                    {
                        Buffer.MemoryCopy((void*)frameInfo.data, (void*)_backBufferPointer, _frameSize, frameInfo.frameSize);
                    }
                    _frameBitmap.AddDirtyRect(_int32Rect); // Specify the area of the bitmap that changed
                    _frameBitmap.Unlock(); // Release the back buffer and make it available for display.
                }, DispatcherPriority.Send);
            }
            _debugInfo.DisplayCost = (int)(DateTime.Now - _lastDateTime).TotalMilliseconds;
            if (_debugInfo.DisplayCost > _debugInfo.DisplayCostMax)
            {
                _debugInfo.DisplayCostMax = _debugInfo.DisplayCost;
            }
        }

        public void DisplayVUMeter()
        {
            try
            {
                if (_element != null)
                {
                    _camera.DebugInfo.DisplayCost = _debugInfo.DisplayCost;
                    _camera.DebugInfo.DisplayCostMax = _debugInfo.DisplayCostMax;
                    if (_camera.DebugInfo.DisplayCost > _camera.DebugInfo.DisplayFps) _camera.DebugInfo.FrameUIMiss++;
                }
            }
            catch
            {
                //
            }
        }

        public void Dispose()
        {
            try
            {
                _element?.Dispatcher?.InvokeShutdown();
                _element = null;
            }
            catch (Exception e)
            {
                //Logger.Error("BitmapSource.log",$"[MediaSourceBase][{_child}]Dispose error. {_channel}_{_ip}", e);
            }
        }
    }
}
