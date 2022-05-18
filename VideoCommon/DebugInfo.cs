using System.ComponentModel;

namespace VideoCommon
{
    public sealed class DebugInfo : INotifyPropertyChanged
    {
        private double _ReceiveRealInterval;
        private int _reveveFps;
        private double _displayRealInterval;
        private int _displayFps;
        private double _ReceiveFrame;
        private double _dispalyFrame;
        private int _frameCatchCount;
        private int _frameUIMiss;
        private double _ReceiveRealIntervalMax;
        private double _displayRealIntervalMax;
        private double _displayCostMax;
        private double _displayCost;

        public double DispalyFrame
        {
            get => this._dispalyFrame;
            set
            {
                if (this._dispalyFrame == value)
                    return;
                this._dispalyFrame = value;
                this.OnPropertyChanged(nameof(DispalyFrame));
            }
        }

        public double ReceiveFrame
        {
            get => this._ReceiveFrame;
            set
            {
                if (this._ReceiveFrame == value)
                    return;
                this._ReceiveFrame = value;
                this.OnPropertyChanged(nameof(ReceiveFrame));
            }
        }

        public int DisplayFps
        {
            get => this._displayFps;
            set
            {
                if (this._displayFps == value)
                    return;
                this._displayFps = value;
                this.OnPropertyChanged(nameof(DisplayFps));
            }
        }

        public double DisplayRealInterval
        {
            get => this._displayRealInterval;
            set
            {
                if (this._displayRealInterval == value)
                    return;
                this._displayRealInterval = value;
                this.OnPropertyChanged(nameof(DisplayRealInterval));
            }
        }

        public double DisplayCost
        {
            get => this._displayCost;
            set
            {
                if (this._displayCost == value)
                    return;
                this._displayCost = value;
                this.OnPropertyChanged(nameof(DisplayCost));
            }
        }

        public double DisplayCostMax
        {
            get => this._displayCostMax;
            set
            {
                if (this._displayCostMax == value)
                    return;
                this._displayCostMax = value;
                this.OnPropertyChanged(nameof(DisplayCostMax));
            }
        }

        public double DisplayRealIntervalMax
        {
            get => this._displayRealIntervalMax;
            set
            {
                if (this._displayRealIntervalMax == value)
                    return;
                this._displayRealIntervalMax = value;
                this.OnPropertyChanged(nameof(DisplayRealIntervalMax));
            }
        }

        public int FrameCatchCount
        {
            get => this._frameCatchCount;
            set
            {
                if (this._frameCatchCount == value)
                    return;
                this._frameCatchCount = value;
                this.OnPropertyChanged(nameof(FrameCatchCount));
            }
        }

        public int FrameUIMiss
        {
            get => this._frameUIMiss;
            set
            {
                if (this._frameUIMiss == value)
                    return;
                this._frameUIMiss = value;
                this.OnPropertyChanged(nameof(FrameUIMiss));
            }
        }

        public int ReceiveFps
        {
            get => this._reveveFps;
            set
            {
                if (this._reveveFps == value)
                    return;
                this._reveveFps = value;
                this.OnPropertyChanged(nameof(ReceiveFps));
            }
        }

        public double ReceiveRealInterval
        {
            get => this._ReceiveRealInterval;
            set
            {
                if (this._ReceiveRealInterval == value)
                    return;
                this._ReceiveRealInterval = value;
                this.OnPropertyChanged(nameof(ReceiveRealInterval));
            }
        }

        public double ReceiveRealIntervalMax
        {
            get => this._ReceiveRealIntervalMax;
            set
            {
                if (this._ReceiveRealIntervalMax == value)
                    return;
                this._ReceiveRealIntervalMax = value;
                this.OnPropertyChanged(nameof(ReceiveRealIntervalMax));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged((object)this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
