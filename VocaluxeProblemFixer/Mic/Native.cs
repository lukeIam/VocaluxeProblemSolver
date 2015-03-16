using System;
using System.Runtime.InteropServices;


namespace VocaluxeProblemFixer.Mic
{
    internal class Native
    {
        [Guid("f8679f50-850a-41cf-9c72-430f290290c8"),
         InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IPolicyConfig
        {
            [PreserveSig]
            int GetMixFormat(string pszDeviceName, out IntPtr ppFormat);

            [PreserveSig]
            int GetDeviceFormat(string pszDeviceName, int bDefault, out IntPtr ppFormat);

            [PreserveSig]
            int SetDeviceFormat(string pszDeviceName, IntPtr pEndpointFormat, IntPtr mixFormat);

        }
    }


}