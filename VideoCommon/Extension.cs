using System;
using System.Runtime.InteropServices;

namespace VideoCommon
{
    public static class Extension
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlCopyMemory")]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public static IntPtr BytesToIntptr(this byte[] bytes, int offset)
        {
            var destination = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, offset, destination, bytes.Length);
            return destination;
        }
    }
}
