using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using DailyPriceNotifier_ExtensionObejcts;

namespace DailyPriceNotifier_ExtensionObejcts.Notifiers
{
    public class SimpleNotifier : Notifier
    {
        private NotifyIcon TrayIcon;
        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem CloseMenuItem;

        public SimpleNotifier(string message, StringBuilder log) : base(message,log)
        {
        }
        public override void Notify()
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

            if (base._extensions != null)
            {
                foreach (var extension in _extensions)
                {
                    extension.Value.Notify();
                }
            }
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
