using NAudio.Wave;
using System;
using System.Runtime.InteropServices;

namespace VocaluxeProblemFixer.Mic
{
    class PolicyConfig
    {

        [ComImport, Guid("870AF99C-171D-4F9E-AF0D-E63DF40C2BC9")]
        private class PolicyConfigClient
        {
        }

        // ReSharper disable once SuspiciousTypeConversion.Global
        private readonly Native.IPolicyConfig _config = new PolicyConfigClient() as Native.IPolicyConfig;

        public WaveFormatExtensible GetDeviceFormat(string deviceId)
        {
            WaveFormatExtensible result = null;

            IntPtr ppFormat = IntPtr.Zero;
            try
            {    
                Marshal.ThrowExceptionForHR(_config.GetDeviceFormat(deviceId, 0, out  ppFormat));
                result = (WaveFormatExtensible)Marshal.PtrToStructure(ppFormat, typeof(WaveFormatExtensible));
            }
            finally
            {
                Marshal.FreeCoTaskMem(ppFormat);
            }
            return result;
        }

        public WaveFormatExtensible GetMixFormat(string deviceId)
        {
            WaveFormatExtensible result = null;

            IntPtr ppFormat = IntPtr.Zero;
            try
            {
                Marshal.ThrowExceptionForHR(_config.GetMixFormat(deviceId, out  ppFormat));
                result = (WaveFormatExtensible)Marshal.PtrToStructure(ppFormat, typeof(WaveFormatExtensible));
            }
            finally
            {
                Marshal.FreeCoTaskMem(ppFormat);
            }
            return result;
        }

        public void SetDeviceFormat(string deviceId, WaveFormatExtensible endpointFormat, WaveFormatExtensible mixFormat)
        {
            IntPtr endpointFormatPointer = IntPtr.Zero;
            IntPtr mixFormatPointer = IntPtr.Zero;

            try
            {
                endpointFormatPointer = Marshal.AllocHGlobal(Marshal.SizeOf(endpointFormat));
                Marshal.StructureToPtr(endpointFormat, endpointFormatPointer, false);
                mixFormatPointer = Marshal.AllocHGlobal(Marshal.SizeOf(mixFormat));
                Marshal.StructureToPtr(mixFormat, mixFormatPointer, false);
                Marshal.ThrowExceptionForHR(_config.SetDeviceFormat(deviceId, endpointFormatPointer, mixFormatPointer));
            }
            finally
            {
                Marshal.FreeHGlobal(endpointFormatPointer);
                Marshal.FreeHGlobal(mixFormatPointer);
            }
        }

    }
}
