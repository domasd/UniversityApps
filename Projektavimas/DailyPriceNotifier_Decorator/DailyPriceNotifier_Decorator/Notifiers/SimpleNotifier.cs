using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DailyPriceNotifier_Decorator.Notifiers
{
    public class SimpleNotifier : INotifier
    {
        private NotifyIcon TrayIcon;
        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem CloseMenuItem;

        public string Message { get; set; }

        private StringBuilder logStringBuilder;

        public SimpleNotifier(string message, StringBuilder log)
        {
            this.Message = message;
            logStringBuilder = log;
        }
        public void Notify()
        {
            TrayIcon = new NotifyIcon();
            TrayIcon.BalloonTipIcon = ToolTipIcon.Info;
            TrayIcon.BalloonTipText = this.Message;
            TrayIcon.BalloonTipTitle = Resource.CheckPrices;

            TrayIcon.Text = Resource.CheckPrices;
            TrayIcon.Icon = SystemIcons.Information;
            TrayIcon.Visible = true;
            TrayIcon.ShowBalloonTip(10000);

            TrayIcon.DoubleClick += Trayicon_DoubleClick;
            TrayIcon.BalloonTipClicked += Trayicon_DoubleClick;

            TrayIconContextMenu = new ContextMenuStrip();
            CloseMenuItem = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();
            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
            this.CloseMenuItem});
            this.TrayIconContextMenu.Name = "TrayIconContextMenu";
            this.TrayIconContextMenu.Size = new Size(153, 70);
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new Size(152, 22);
            this.CloseMenuItem.Text = Resource.Exit;
            this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);

            TrayIconContextMenu.ResumeLayout(false);
            TrayIcon.ContextMenuStrip = TrayIconContextMenu;

            OnSuccessAppendLog("SimpleNotifier");
        }

        public void OnSuccessAppendLog(string componentName)
        {
            logStringBuilder.AppendLine($"Component {componentName} notified it's target successfuly at {DateTime.Now}");
        }

        public int LoggedCount()
        {
            return 1;
        }

        private void Trayicon_DoubleClick(object sender,EventArgs args)
        {
            var result = MessageBox.Show(this.Message + Environment.NewLine + Resource.CopyToClipboard, Resource.CheckPrices,MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                Clipboard.SetText(this.Message);
            }
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            TrayIcon.Visible = false;
            Application.Exit();
        }

    }
}
