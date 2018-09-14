using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using ConfigurationManager;
using Ionic.Zip;
using SteamIDResolverGUI.Forms;
using SteamIDResolverGUI.Misc;

namespace SteamIDResolverGUI.Async
{
    internal class AsyncActions
    {
        public static Action<Form, object[]> LoadSettings => (loadingForm, args) =>
        {
            void ChangeState(Control с, string state)
            {
                с.Invoke((Action) (() => с.Text = state));
            }

            var settings = 
                ExtSingleton.CreateInstance<ApplicationSettings>(
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\steamidresolver.json");

            settings.LoadConfigCreateIfNeeded(new Dictionary<string, object>
            {
                { "steamcmd_path", null },
                { "resolve_cache", new Dictionary<string, int>() }
            });

            var steamCmdPath = settings.GetConfigEntry<string>("steamcmd_path");

            if (!string.IsNullOrEmpty(steamCmdPath))
            {
                if (File.Exists(steamCmdPath))
                {
                    return;
                }
            }

            //Resetting config path to null so if installation will fail program will not enter dead state.
            settings.SetConfigEntry("steamcmd_path", (string) null);
            settings.SaveConfig();

            var response = DialogResult.None;

            loadingForm.Invoke((Action) (() => 
                response = MessageBox.Show(loadingForm,
                "Hello.\n" +
                "It's seems like you're using this application for the first time or you removed SteamCMD from your computer.\n" +
                "We need to know your SteamCMD path. You can select it or let us create our own.\n\n" +
                "Press [Yes] to automaticly setup SteamCMD\n" +
                "Press [No] to manually select SteamCMD location",
                "First setup",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)));

            if (response == DialogResult.Yes)
            {
                ChangeState(loadingForm, "Setting up SteamCMD..");

                var baseDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SteamCMD";

                Directory.CreateDirectory(baseDirectory);

                const string steamCmdInstallerUrl = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
                var installerPath = $"{baseDirectory}\\steamcmd_installer.zip";

                var webClient = new WebClient();

                ChangeState(loadingForm, "Downloading..");

                try
                {
                    webClient.DownloadFile(new Uri(steamCmdInstallerUrl), installerPath);
                }
                catch (Exception ex)
                {
                    loadingForm.Invoke((Action) (() => 
                        MessageBox.Show(loadingForm,
                            "Unexpected error occured while downloading SteamCMD installer.\n\n" +
                            $"Exception: {ex.Message}\n\n" +
                            "Maybe URL is outdated?",
                            "Error!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)));

                    Environment.Exit(0);
                }

                if (!File.Exists(installerPath)) //Sanity checks are always good!
                {
                    loadingForm.Invoke((Action) (() => 
                        MessageBox.Show(loadingForm,
                            "Can't find downloaded file. Check your antivirus software.",
                            "Error!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)));

                    Environment.Exit(0);
                }

                ChangeState(loadingForm, "Unpacking..");

                var zip = new ZipFile(installerPath);
                var zipEntry = zip.Entries.First();

                if (!zipEntry.FileName.Equals("steamcmd.exe"))
                {
                    loadingForm.Invoke((Action) (() => 
                        MessageBox.Show(loadingForm,
                            "Downloaded archive is invalid.",
                            "Error!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)));

                    Environment.Exit(0);
                }

                zipEntry.Extract(baseDirectory, ExtractExistingFileAction.OverwriteSilently);
                zip.Dispose();

                File.Delete(installerPath);

                ChangeState(loadingForm, "Installing..");

                var steamCmdExecutablePath = $"{baseDirectory}\\steamcmd.exe";

                var process = Process.Start(steamCmdExecutablePath, "+quit");

                if (process == null)
                {
                    loadingForm.Invoke((Action) (() => 
                        MessageBox.Show(loadingForm,
                            "Can't start the installer! Check your antivirus software.",
                            "Error!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)));

                    Environment.Exit(0);
                }

                Thread.Sleep(300);
                
                loadingForm.Invoke((Action) (() => loadingForm.TopMost = true));

                process.WaitForExit();

                if (process.ExitCode == -1)
                {
                    loadingForm.Invoke((Action) (() => 
                        MessageBox.Show(loadingForm,
                            "SteamCMD exited with code [-1] when expected 7.\n" +
                            "This often happens when you're trying to install SteamCMD over an older installation.\n",
                            "Error!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)));

                    Environment.Exit(0);
                }

                if (process.ExitCode != 7)
                {
                    loadingForm.Invoke((Action) (() => 
                        MessageBox.Show(loadingForm,
                            $"SteamCMD exited with code [{process.ExitCode}] when expected 7.\n" +
                            "Installation has failed.",
                            "Error!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)));

                    Environment.Exit(0);
                }

                loadingForm.Invoke((Action) (() => loadingForm.TopMost = false));

                ChangeState(loadingForm, "Saving settings..");

                settings.SetConfigEntry("steamcmd_path", steamCmdExecutablePath);
                settings.SaveConfig();

                ChangeState(loadingForm, "Cleaning up..");

                var garbageFileList = 
                    Directory.GetFiles(baseDirectory, "*.delete", SearchOption.AllDirectories).ToList();

                garbageFileList.AddRange(Directory.GetFiles(baseDirectory, "*.old", SearchOption.AllDirectories)
                    .ToList());

                foreach (var file in garbageFileList)
                {
                    try
                    {
                        File.Delete(file);
                    } catch { /* ignored */ }
                }

                ChangeState(loadingForm, "Finished.");

                var shouldOpenFolder = false;

                loadingForm.Invoke((Action) (() => 
                    shouldOpenFolder = MessageBox.Show(loadingForm,
                        "Installation is finished.\n" +
                        $"SteamCMD is installed in [{baseDirectory}].\n\n" +
                        "Press [Yes] to open the installation folder.",
                        "Success!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Asterisk) == DialogResult.Yes));

                if (shouldOpenFolder)
                {
                    Process.Start("explorer", baseDirectory);
                }

                return;
            }

            if (response != DialogResult.No) return;

            var selectFileDialog = new OpenFileDialog
            {
                Title = "Select [steamcmd.exe] file",
                CheckFileExists = true,
                Filter = "SteamCMD|steamcmd.exe"
            };

            var filename = string.Empty;

            loadingForm.Invoke((Action) delegate
            {
                if (selectFileDialog.ShowDialog(loadingForm) != DialogResult.OK)
                {
                    MessageBox.Show(loadingForm, "To proceed you need to select [steamcmd.exe]!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    Environment.Exit(0);
                }

                filename = selectFileDialog.FileName;
            });

            settings.SetConfigEntry("steamcmd_path", filename);
            settings.SaveConfig();
        };

        public static Action<Form, object[]> ResolveSteamUsername => (owner, args) =>
        {
            void ChangeState(Control f, string state)
            {
                f.Invoke((Action) (() => f.Text = state));
            }

            void SetCallbackResult(MainForm f, int res)
            {
                f.LastResolveResult = res;
            }

            if (args.Length != 2) return;
            if (!(args[1] is MainForm)) return;

            var login = (string) args[0];
            var mainForm = (MainForm) args[1];

            ChangeState(owner, "Preparing..");

            var steamCmdProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ExtSingleton.GetInstance<ApplicationSettings>().GetConfigEntry<string>("steamcmd_path"),
                    Arguments = $"+login {login} test",
                    UseShellExecute = false,
                    CreateNoWindow = true,
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

                File.AppendAllText("./dump.bin", $"{eventArgs.Data}\n");

                if (eventArgs.Data.Normalize().StartsWith("Loading Steam API...OK.", true, CultureInfo.InvariantCulture))
                {
                    steamCmdStarted = true;
                }
                if (eventArgs.Data.Normalize().StartsWith("FAILED with result code 5", StringComparison.InvariantCulture))
                {
                    loginFailed = true;
                }
            };

            var steamCmdConfigPath = $"{Path.GetDirectoryName(steamCmdProcess.StartInfo.FileName)}\\steam.cfg";

            if (!File.Exists(steamCmdConfigPath))
            {
                File.WriteAllText(steamCmdConfigPath, "BootStrapperInhibitAll=Enable");
            }

            steamCmdProcess.Start();
            steamCmdProcess.BeginOutputReadLine();

            ChangeState(owner, "Starting..");

            while (!steamCmdStarted) {}

            ChangeState(owner, "Resolving..");

            while (!loginFailed) {}

            ChangeState(owner, "Parsing..");

            var dataBuffer = new byte[4];
            var pattern = new byte[] { 0x01, 0x00, 0x10, 0x01, 0xC0, 0x01, 0x00, 0xC8, 0x01, 0x00 };

            var newscan = new AobScan
            {
                ProcessID = (uint) steamCmdProcess.Id
            };

            var address = newscan.Scan(pattern);

            if (address == IntPtr.Zero)
            {
                SetCallbackResult(mainForm, 0);
                return;
            }

            address -= 4;

            WinAPI.ReadProcessMemory(steamCmdProcess.Handle, address, dataBuffer, (uint)dataBuffer.Length, 0);

            using (steamCmdProcess)
            {
                steamCmdProcess.Kill();
            }

            SetCallbackResult(mainForm, BitConverter.ToInt32(dataBuffer, 0));
        };
    }
}