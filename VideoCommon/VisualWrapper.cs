using System;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
//using Lib.Log;

namespace Lib.WPF.Graphics
{
    /// <summary>
    ///     The VisualWrapper simply integrates a raw Visual child into a tree
    ///     of FrameworkElements.
    /// </summary>
    [ContentProperty("Child")]
    public class VisualWrapper<T> : FrameworkElement where T : Visual
    {
        public T Child
        {
            get
            {
                return _child;
            }

            set
            {
                if (_child != null)
                {
                    RemoveVisualChild(_child);
                }
                _child = value;

                if (_child != null)
                {
                    AddVisualChild(_child);
                }
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (_child != null && index == 0)
            {
                return _child;
            }
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        protected override int VisualChildrenCount => _child != null ? 1 : 0;

        private T _child;
    }

    /// <summary>
    ///     The VisualWrapper simply integrates a raw Visual child into a tree
    ///     of FrameworkElements.
    /// </summary>
    public sealed class VisualWrapper : VisualWrapper<Visual>, IDisposable
    {
        private Func<FrameworkElement> CreateYourElement { get; set; }

        private VisualTargetPresentationSource VisualTargetPresentationSource { get; set; }

        public void Init(Func<FrameworkElement> createYourElement)
        {
            CreateYourElement = createYourElement;

            Child = CreateGuiOnWorkerThread();
        }

        private HostVisual CreateGuiOnWorkerThread()
        {
            AutoResetEvent notify = new AutoResetEvent(false);
            HostVisual hostVisual = new HostVisual();
            Thread thread = new Thread(obj => MediaWorkerThread(hostVisual, notify));
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();

            // Wait for the worker thread to spin up and create the VisualTarget.
            notify.WaitOne();

            return hostVisual;
        }

        Visual InitEmbed()
        {
            FrameworkElement embeddedVisual = CreateYourElement();
            embeddedVisual.Width = ActualWidth;
            embeddedVisual.Height = ActualHeight;
            return embeddedVisual;
        }

        private void MediaWorkerThread(HostVisual hostVisual, AutoResetEvent notify)
        {
            try
            {
                VisualTargetPresentationSource = new VisualTargetPresentationSource(hostVisual) {RootVisual = InitEmbed() };
                VisualTargetPresentationSource.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
                notify.Set();
                Dispatcher.Run();
            }
            catch (Exception e)
            {
                //Logger.Error("VisualWrapper.log", "MediaWorkerThread error", e);
            }
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //Logger.Error("VisualWrapper.log", "VisualTargetPresentationSource dispatcherUnhandledException error", e.Exception);
            e.Handled = true;
        }

        public void Dispose()
        {
            try
            {
                if (VisualTargetPresentationSource != null)
                {
                    VisualTargetPresentationSource.Dispatcher.UnhandledException -= Dispatcher_UnhandledException;
                    VisualTargetPresentationSource?.Dispose();
                    VisualTargetPresentationSource = null;
                }
                Child = null;
                CreateYourElement = null;
            }
            catch (Exception e)
            {
                //Logger.Error("VisualWrapper.log", "Dispose error", e);
            }
        }
    }


    public sealed class VisualTargetPresentationSource : PresentationSource, IDisposable
    {
        private VisualTarget _visualTarget;

        public VisualTargetPresentationSource(HostVisual hostVisual)
        {
            _visualTarget = new VisualTarget(hostVisual);
        }

        public override Visual RootVisual
        {
            get
            {
                return _visualTarget.RootVisual;
            }

            set
            {
                Visual oldRoot = _visualTarget.RootVisual;


                // Set the root visual of the VisualTarget.  This visual will
                // now be used to visually compose the scene.
                _visualTarget.RootVisual = value;

                // Hook the SizeChanged event on framework elements for all
                // future changed to the layout size of our root, and manually
                // trigger a size change.
                //FrameworkElement rootFE = value as FrameworkElement;
                //if (rootFE != null)
                //{
                //    rootFE.DataContext = _dataContext;
                //}

                // Tell the PresentationSource that the root visual has
                // changed.  This kicks off a bunch of stuff like the
                // Loaded event.
                RootChanged(oldRoot, value);

                // Kickoff layout...
                if (value is UIElement rootElement)
                {
                    rootElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    rootElement.Arrange(new Rect(rootElement.DesiredSize));
                }
            }
        }

        public override bool IsDisposed
        {
            get
            {
                // We don't support disposing this object.
                return true;
            }
        }

        protected override CompositionTarget GetCompositionTargetCore()
        {
            return _visualTarget;
        }

        public void Dispose()
        {
            try
            {
                _visualTarget?.Dispatcher.Invoke(() =>
                {
                    _visualTarget?.Dispose();
                });
                _visualTarget?.Dispatcher.InvokeShutdown();
                _visualTarget = null;
            }
            catch (Exception e)
            {
                //Logger.Error("VisualWrapper.log", "Dispose error", e);
            }
        }
    }
}
