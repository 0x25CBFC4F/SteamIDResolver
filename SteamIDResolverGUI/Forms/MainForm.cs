using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using ConfigurationManager;
using Newtonsoft.Json.Linq;
using SteamIDResolverGUI.Async;
using SteamIDResolverGUI.Misc;

namespace SteamIDResolverGUI.Forms
{
    public partial class MainForm : Form
    {
        public int LastResolveResult { get; set; } //shitty async callback result

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            LoadingForm.DoAsyncAction(AsyncActions.LoadSettings, this, "Loading settings..");
        }

        private void resolveLoginButton_Click(object sender, System.EventArgs e)
        {
            var login = loginTextBox.Text.Trim();

            if (string.IsNullOrEmpty(login)) return;

            var settings = ExtSingleton.GetInstance<ApplicationSettings>();
            var cache = settings.GetConfigEntry<JObject>("resolve_cache").ToObject<Dictionary<string, int>>();

            if (cache.ContainsKey(login))
            {
                if (Size.Height != 266)
                {
                    SetBounds(Location.X, 
                        Location.Y - 67,
                        Size.Width,
                        266);
                }
                
                var cachedSteamId32 = cache[login];
                var cachedSteamID64 = "765" + (cachedSteamId32 + 61197960265728);

                steamID32Link.Text = $"[U:1:{cachedSteamId32}]";
                steamID64Link.Text = cachedSteamID64;

                return;
            }

            resolveLoginButton.Enabled = false;
            loginTextBox.Enabled = false;

            LastResolveResult = 0;

            LoadingForm.DoAsyncAction(AsyncActions.ResolveSteamUsername,
                this,
                $"Resolving username [{login}]..",
                login, this);
            
            resolveLoginButton.Enabled = true;
            loginTextBox.Enabled = true;

            if (LastResolveResult == 0)
            {
                MessageBox.Show(this,
                    "Failed to resolve SteamID. Maybe login is incorrect?",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var steamID32 = LastResolveResult;
            var steamID64 = "765" + (steamID32 + 61197960265728);

            if (Size.Height != 266)
            {
                SetBounds(Location.X, 
                        Location.Y - 67,
                        Size.Width,
                        266);
            }

            cache.Add(login, steamID32);

            settings.SetConfigEntry("resolve_cache", JObject.FromObject(cache));
            settings.SaveConfig();

            steamID32Link.Text = $"[U:1:{steamID32}]";
            steamID64Link.Text = steamID64;
        }

        private void steamID32Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(steamID32Link.Text);
        }

        private void steamID64Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(steamID64Link.Text);
        }

        private void steamIDLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start($"https://steamid.io/lookup/{steamID64Link.Text}");
        }

        private void steamRepLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start($"https://steamrep.com/search?q={steamID64Link.Text}");
        }
    }
}
