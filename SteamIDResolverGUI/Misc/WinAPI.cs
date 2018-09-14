using System;
using System.Runtime.InteropServices;

namespace SteamIDResolverGUI.Misc
{
    internal class WinAPI
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadProcessMemory([In] IntPtr processHandle, [In] IntPtr processAddress, [Out] byte[] buffer, [In] uint bytesToRead, int bytesRead);

        [DllImport("kernel32.dll")] 
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemoryBasicInformation lpBuffer, int dwLength); 

        [StructLayout(LayoutKind.Sequential)] 
        public struct MemoryBasicInformation 
        { 
            public IntPtr BaseAddress; 
            public IntPtr AllocationBase; 
            public uint AllocationProtect; 
            public uint RegionSize; 
            public uint State; 
            public uint Protect; 
            public uint Type; 
        } 
    }
}
