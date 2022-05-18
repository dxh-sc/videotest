using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoCommon
{
    public sealed class FrameInfo
    {
        public IntPtr data { get; set; }
        public int frameSize { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
