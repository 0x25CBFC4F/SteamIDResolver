using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SteamIDResolverGUI.Misc
{
    internal class AobScan 
    { 
        public uint ProcessID { get; set; }

        public AobScan() {}

        public AobScan(uint processID) 
        { 
            ProcessID = processID; 
        }

        protected List<WinAPI.MemoryBasicInformation> MemoryRegion { get; set; } 

        protected void MemInfo(IntPtr pHandle) 
        { 
            var addy = new IntPtr(); 
            while (true) 
            { 
                var memInfo = new WinAPI.MemoryBasicInformation(); 
                var memDump = WinAPI.VirtualQueryEx(pHandle, addy, out  memInfo, Marshal.SizeOf(memInfo)); 
                if (memDump == 0) break; 
                if ((memInfo.State & 0x1000) != 0 && (memInfo.Protect & 0x100) == 0) 
                    MemoryRegion.Add(memInfo); 
                addy = new IntPtr(memInfo.BaseAddress.ToInt32() + (int)memInfo.RegionSize); 
            } 
        }

        protected IntPtr Scan(byte[] sIn, byte[] sFor)
        {
            var sBytes = new int[256];
            var pool = 0;
            var end = sFor.Length - 1;

            for (var i = 0; i < 256; i++)
            {
                sBytes[i] = sFor.Length;
            }

            for (var i = 0; i < end; i++) {

                sBytes[sFor[i]] = end - i;
            }
            
            while (pool <= sIn.Length - sFor.Length) 
            { 
                for (var i = end; sIn[pool + i] == sFor[i]; i--)
                {
                    if (i == 0) return new IntPtr(pool);
                }

                pool += sBytes[sIn[pool + end]]; 
            }

            return IntPtr.Zero; 
        } 
        public IntPtr Scan(byte[] pattern) 
        { 
            var game = Process.GetProcessById((int)ProcessID); 
            if (game.Id == 0) return IntPtr.Zero; 
            MemoryRegion = new List<WinAPI.MemoryBasicInformation>(); 
            MemInfo(game.Handle); 
            for (var i = 0; i < MemoryRegion.Count; i++) 
            { 
                var buff = new byte[MemoryRegion[i].RegionSize]; 
                WinAPI.ReadProcessMemory(game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0); 

                var result = Scan(buff, pattern); 
                if (result != IntPtr.Zero) 
                    return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + result.ToInt32()); 
            } 
            return IntPtr.Zero; 
        } 
    }
}