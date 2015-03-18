using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace VocaluxeProblemFixer.Mic
{
    static class Microphone
    {
        private static List<MMDevice> GetAllMicrophones()
        {
            List<MMDevice> resultList = new List<MMDevice>();
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All))
            {
                resultList.Add(device);
            }

            return resultList;
        }

        private static Dictionary<MMDevice, WaveFormatExtensible> GetAudioDeviceFormat(IEnumerable<MMDevice> devices)
        {
            Dictionary<MMDevice, WaveFormatExtensible> result = new Dictionary<MMDevice, WaveFormatExtensible>();

            PolicyConfig config = new PolicyConfig();

            foreach (var device in devices)
            {
                try
                {
                    result.Add(device, config.GetDeviceFormat(device.ID));
                }
                catch (System.IO.DirectoryNotFoundException)
                {
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                }
            }
            return result;
        }

        private static bool SetAudioDeviceFormat(MMDevice device, WaveFormatExtensible newFormat)
        {

            PolicyConfig config = new PolicyConfig();

            try
            {
                config.SetDeviceFormat(device.ID, newFormat, newFormat);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                return false;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return false;
            }

            return true;
        }

        public static Dictionary<string, bool> FixSingstarMicrophones()
        {
            IEnumerable<MMDevice> singstarMics;

            try
            {
                singstarMics = (from m in GetAllMicrophones()
                    where m.DeviceFriendlyName.StartsWith("USBMIC Serial")
                          && m.State == DeviceState.Active
                    select m).ToList();
            }
            catch (COMException e)
            {
                Log.WriteErrorLine("Error while accessing microphones.");
                Log.WriteErrorLine(e.Message);
                Log.WriteErrorLine(e.StackTrace);
                singstarMics = new List<MMDevice>();
            }
            

            return CheckAndFixMics(singstarMics, 2, 16, 48000);
        }

        private static Dictionary<string, bool> CheckAndFixMics(IEnumerable<MMDevice> mics, int requiredChannels, int requiredBitsPerSample,
            int requiredSampleRate)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            var micsFormatData = GetAudioDeviceFormat(mics);

            foreach (var mic in micsFormatData)
            {
                if (mic.Value.Channels < requiredChannels
                    || mic.Value.BitsPerSample < requiredBitsPerSample
                    || mic.Value.SampleRate != requiredSampleRate)
                {
                    var newFormat = new WaveFormatExtensible(requiredSampleRate, requiredBitsPerSample, requiredChannels);
                    if (SetAudioDeviceFormat(mic.Key, newFormat))
                    {
                        result.Add(mic.Key.FriendlyName, true);
                    }
                }
                else
                {
                    result.Add(mic.Key.FriendlyName, false);
                }
            }
            return result;
        }
    }
}
