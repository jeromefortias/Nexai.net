namespace Musuko.Kaonashi
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class SystrayApplicationContext : ApplicationContext
    {
        private NotifyIcon notifyIcon;
        private MainForm? mainForm;
        private AppConfig config;

        public SystrayApplicationContext()
        {
            // Load configuration
            config = AppConfig.Load();

            // Initialize systray icon
            InitializeSystrayIcon();

            // Don't show the main form immediately - let user double-click to open it
            // You can uncomment the next line if you want the form to show on startup
            // ShowMainForm();
        }

        private void InitializeSystrayIcon()
        {
            // Load custom icon
            Icon? customIcon = IconHelper.GetApplicationIcon();
            
            notifyIcon = new NotifyIcon
            {
                Icon = customIcon ?? SystemIcons.Application,
                Visible = true,
                Text = "Kaonashi AI Assistant"
            };

            // Create context menu
            var contextMenu = new ContextMenuStrip();
            
            var openMenuItem = new ToolStripMenuItem("Open Kaonashi", null, OnOpen);
            openMenuItem.Font = new Font(openMenuItem.Font, FontStyle.Bold);
            contextMenu.Items.Add(openMenuItem);
            
            contextMenu.Items.Add(new ToolStripSeparator());
            
            contextMenu.Items.Add(new ToolStripMenuItem("Entity Manager", null, OnEntityManager));
            contextMenu.Items.Add(new ToolStripMenuItem("JSON Client", null, OnJsonClient));
            contextMenu.Items.Add(new ToolStripMenuItem("News Viewer", null, OnNewsViewer));
            
            contextMenu.Items.Add(new ToolStripSeparator());
            
            contextMenu.Items.Add(new ToolStripMenuItem("Settings", null, OnSettings));
            
            contextMenu.Items.Add(new ToolStripSeparator());
            
            contextMenu.Items.Add(new ToolStripMenuItem("Exit", null, OnExit));

            notifyIcon.ContextMenuStrip = contextMenu;

            // Handle double-click to open main form
            notifyIcon.DoubleClick += OnDoubleClick;

            // Show balloon tip on startup
            notifyIcon.ShowBalloonTip(3000, "Kaonashi AI", "Application is running in system tray", ToolTipIcon.Info);
        }

        private void OnDoubleClick(object? sender, EventArgs e)
        {
            ShowMainForm();
        }

        private void OnOpen(object? sender, EventArgs e)
        {
            ShowMainForm();
        }

        private void ShowMainForm()
        {
            if (mainForm == null || mainForm.IsDisposed)
            {
                mainForm = new MainForm();
                mainForm.FormClosing += MainForm_FormClosing;
                mainForm.Show();
            }
            else
            {
                if (mainForm.WindowState == FormWindowState.Minimized)
                {
                    mainForm.WindowState = FormWindowState.Normal;
                }
                mainForm.Show();
                mainForm.Activate();
                mainForm.BringToFront();
            }
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Minimize to systray instead of closing
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                if (mainForm != null)
                {
                    mainForm.Hide();
                    notifyIcon.ShowBalloonTip(2000, "Kaonashi AI", "Application minimized to system tray", ToolTipIcon.Info);
                }
            }
        }

        private void OnEntityManager(object? sender, EventArgs e)
        {
            // Open Entity Manager
            var entityManagerForm = new EntityManagerForm();
            entityManagerForm.Show();
        }

        private void OnJsonClient(object? sender, EventArgs e)
        {
            // Open JSON Client
            var jsonClientForm = new JsonClientForm();
            jsonClientForm.Show();
        }

        private void OnNewsViewer(object? sender, EventArgs e)
        {
            // Open News Viewer
            var newsViewerForm = new NewsViewerForm(config.CompletionHost, config.CompletionPort);
            newsViewerForm.Show();
        }

        private void OnSettings(object? sender, EventArgs e)
        {
            // Show settings dialog
            using (var settingsForm = new SettingsForm())
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // Reload configuration
                    config = AppConfig.Load();
                    
                    // If main form is open, notify it to reload config
                    if (mainForm != null && !mainForm.IsDisposed)
                    {
                        mainForm.ReloadConfiguration();
                    }
                }
            }
        }

        private void OnExit(object? sender, EventArgs e)
        {
            // Clean up
            notifyIcon.Visible = false;
            notifyIcon.Dispose();

            // Close main form if it's open
            if (mainForm != null && !mainForm.IsDisposed)
            {
                mainForm.FormClosing -= MainForm_FormClosing; // Remove event handler to allow actual closing
                mainForm.Close();
            }

            // Exit application
            Application.Exit();
        }
    }
}


