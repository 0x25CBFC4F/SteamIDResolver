using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamIDResolverGUI.Forms
{
    public partial class LoadingForm : Form
    {
        public Action<Form, object[]> NeededAction { get; set; }
        public object[] Arguments { get; set; }

        public BackgroundWorker AsyncWorker = new BackgroundWorker
        {
            WorkerSupportsCancellation = true,
            WorkerReportsProgress = false
        };

        public LoadingForm()
        {
            InitializeComponent();
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            AsyncWorker.DoWork += (o, args) =>
            {
                try
                {
                    NeededAction(this, Arguments);
                }
                catch
                {
                    DialogResult = DialogResult.Abort;
                }

                DialogResult = DialogResult.OK;
            };

            AsyncWorker.RunWorkerAsync();
        }
        
        public static void DoAsyncAction(Action<Form, object[]> a, IWin32Window owner, string description, params object[] args)
        {
            var loadingForm = new LoadingForm
            {
                NeededAction = a,
                Arguments = args,
                Text = description
            };

            loadingForm.ShowDialog(owner);
        }
    }
}
