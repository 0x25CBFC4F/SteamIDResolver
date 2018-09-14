using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace SteamIDResolver
{
    internal class Program
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadProcessMemory([In] IntPtr processHandle, [In] IntPtr processAddress, [Out] byte[] buffer, [In] int bytesToRead, [Out] out IntPtr bytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        private static void Main()
        {
            Console.Title = "Steam ID Resolver";
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("============== STEAM ID RESOLVER ==============\n\n");

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("Enter username to resolve >> ");
            var login = Console.ReadLine();

            Console.Write('\n');

            if (!File.Exists("steamcmd.exe"))
            {
                FatalError("Can't find [steamcmd.exe]. Please put it somewhere near me!~");
            }

            Log("Starting SteamCMD..");

            var steamCmdProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "steamcmd.exe",
                    Arguments = $"+login {login} test",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            var steamCmdStarted = false;
            var loginFailed = false;

            steamCmdProcess.OutputDataReceived += (sender, eventArgs) =>
            {
                if (string.IsNullOrEmpty(eventArgs.Data)) return;
                
                if (eventArgs.Data.EndsWith("...ok.", StringComparison.InvariantCultureIgnoreCase))
                {
                    steamCmdStarted = true;
                }
                if (eventArgs.Data.EndsWith("failed with result code 5", StringComparison.InvariantCultureIgnoreCase))
                {
                    loginFailed = true;
                }
            };

            steamCmdProcess.Start();
            steamCmdProcess.BeginOutputReadLine();

            while (!steamCmdStarted) {}

            Log($"SteamCMD started. Resolving username [{login}]..", ConsoleColor.Green);

            while (!loginFailed) {}

            Log("Resolving complete. Parsing..");

            var dataBuffer = new byte[4];
            var pattern = new byte[] { 0x01, 0x00, 0x10, 0x01, 0xC0, 0x01, 0x00, 0xC8, 0x01, 0x00 };

            var newscan = new AobScan
            {
                ProcessID = (uint) steamCmdProcess.Id
            };

            var address = newscan.Scan(pattern);

            if (address == IntPtr.Zero)
            {
                FatalError("Can't find SteamID, maybe login is incorrect?");
            }

            address -= 4;

            ReadProcessMemory(steamCmdProcess.Handle, address, dataBuffer, dataBuffer.Length, out var _);

            using (steamCmdProcess)
            {
                steamCmdProcess.Kill();
            }

            var steamID32 = BitConverter.ToInt32(dataBuffer, 0);

            if (steamID32 == 0)
            {
                FatalError("Can't find SteamID, maybe login is incorrect?");
            }

            var steamID64 = "765" + (steamID32 + 61197960265728);
            
            Log($"SteamID: {steamID64} [{steamID32}]\n", ConsoleColor.Green);
            Log("Press any key to exit..", ConsoleColor.Green);
            
            Console.ReadKey(true);
        }

        private static void FatalError(string s)
        {
            Log(s, ConsoleColor.Red);
            Log("Press any key to quit..", ConsoleColor.Red);
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        private static void Log(string s, ConsoleColor color = ConsoleColor.White)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[x] {s}");
            Console.ForegroundColor = prev;
        }
    }
}