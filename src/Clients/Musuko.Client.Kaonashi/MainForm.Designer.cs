using System;
using System.Drawing;
using System.Windows.Forms;

namespace Musuko.Kaonashi
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                newsScrollTimer?.Stop();
                newsScrollTimer?.Dispose();
                newsRefreshTimer?.Stop();
                newsRefreshTimer?.Dispose();
                newsService?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "Kaonashi";
            this.Size = new Size(1300, 700);
            this.MinimumSize = new Size(1300, 400);
            //this.MaximumSize = new Size(1300, Screen.PrimaryScreen?.WorkingArea.Height ?? 2000);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);

            // News Ticker Panel (at the very top)
            newsTickerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1300, 35),
                BackColor = Color.FromArgb(30, 31, 34),
                Dock = DockStyle.Top
            };
            newsTickerPanel.Paint += NewsTickerPanel_Paint;
            this.Controls.Add(newsTickerPanel);

            // Header Panel
            headerPanel = new Panel
            {
                Location = new Point(0, 35),
                Size = new Size(1000, 70),
                BackColor = secondaryBg,
                Dock = DockStyle.Top
            };
            this.Controls.Add(headerPanel);

            // Model label
            modelLabel = new Label
            {
                Location = new Point(20, 23),
                Size = new Size(60, 25),
                Text = "Model:",
                ForeColor = textColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10)
            };
            headerPanel.Controls.Add(modelLabel);

            // Model selector
            modelSelector = new ComboBox
            {
                Location = new Point(85, 20),
                Size = new Size(220, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = inputBg,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 10)
            };
            modelSelector.SelectedIndexChanged += ModelSelector_SelectedIndexChanged;
            headerPanel.Controls.Add(modelSelector);

            // Settings button
            settingsButton = new Button
            {
                Location = new Point(315, 18),
                Size = new Size(110, 34),
                Text = "? Settings",
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                BackColor = inputBg,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 10)
            };
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 84, 95);
            settingsButton.Click += SettingsButton_Click;
            headerPanel.Controls.Add(settingsButton);

            // Load Prompt button
            loadPromptButton = new Button
            {
                Location = new Point(435, 18),
                Size = new Size(115, 34),
                Text = "?? Load",
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                BackColor = inputBg,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 10)
            };
            loadPromptButton.FlatAppearance.BorderSize = 0;
            loadPromptButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 84, 95);
            loadPromptButton.Click += LoadPromptButton_Click;
            headerPanel.Controls.Add(loadPromptButton);

            // Clear button
            clearButton = new Button
            {
                Location = new Point(560, 18),
                Size = new Size(110, 34),
                Text = "?? Clear",
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                BackColor = inputBg,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 10)
            };
            clearButton.FlatAppearance.BorderSize = 0;
            clearButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 84, 95);
            clearButton.Click += ClearButton_Click;
            headerPanel.Controls.Add(clearButton);

            // Connection indicator (dot)
            connectionIndicator = new Label
            {
                Location = new Point(815, 25),
                Size = new Size(12, 12),
                BackColor = Color.Gray,
                Text = ""
            };
            headerPanel.Controls.Add(connectionIndicator);

            // Status label
            statusLabel = new Label
            {
                Location = new Point(835, 23),
                Size = new Size(145, 25),
                Text = "Initializing...",
                ForeColor = textColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            headerPanel.Controls.Add(statusLabel);

            // Chat Panel (container for chat display with padding)
            chatPanel = new Panel
            {
                Location = new Point(15, 80),
                Size = new Size(950, 450),
                BackColor = secondaryBg,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
                Padding = new Padding(5)
            };
            this.Controls.Add(chatPanel);

            // Chat display
            chatDisplay = new RichTextBox
            {
                Location = new Point(5, 5),
                Size = new Size(940, 440),
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = secondaryBg,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 10.5f),
                ScrollBars = RichTextBoxScrollBars.Vertical,
                BorderStyle = BorderStyle.None,
                DetectUrls = true
            };
            chatDisplay.LinkClicked += ChatDisplay_LinkClicked;
            chatPanel.Controls.Add(chatDisplay);

            // Image Panel (right side with Marvin image)
            imagePanel = new Panel
            {
                Location = new Point(980, 80),
                Size = new Size(305, 450),
                BackColor = secondaryBg,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right,
                Padding = new Padding(5)
            };
            this.Controls.Add(imagePanel);

            // Marvin Background Picture Box
            marvinPictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = secondaryBg
            };
            
            
            imagePanel.Controls.Add(marvinPictureBox);

            // Marvin Eyes Picture Box (overlay)
            marvinEyesPictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            
            // Load the default eyes image
            LoadMarvinEyes("marvin-normal-eyes.png");
            
            imagePanel.Controls.Add(marvinEyesPictureBox);
            marvinEyesPictureBox.BringToFront();

            // Input Panel
            inputPanel = new Panel
            {
                Location = new Point(15, 540),
                Size = new Size(1270, 100),
                BackColor = inputBg,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Padding = new Padding(10)
            };
            this.Controls.Add(inputPanel);

            // Message input
            messageInput = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(1140, 80),
                Multiline = true,
                Font = new Font("Segoe UI", 11),
                BackColor = inputBg,
                ForeColor = textColor,
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Left
            };
            messageInput.KeyDown += MessageInput_KeyDown;
            inputPanel.Controls.Add(messageInput);

            // Send button
            sendButton = new Button
            {
                Location = new Point(1160, 10),
                Size = new Size(100, 80),
                Text = "Send ?",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = primaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right
            };
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.FlatAppearance.MouseOverBackColor = primaryHover;
            sendButton.Click += SendButton_Click;
            inputPanel.Controls.Add(sendButton);

            // Status Bar at bottom
            statusPanel = new Panel
            {
                Location = new Point(0, 650),
                Size = new Size(1300, 30),
                BackColor = secondaryBg,
                Dock = DockStyle.Bottom
            };
            statusPanel.Paint += (s, e) => {
                // Draw top border
                using (Pen pen = new Pen(Color.FromArgb(70, 70, 70), 1))
                {
                    e.Graphics.DrawLine(pen, 0, 0, statusPanel.Width, 0);
                }
            };
            this.Controls.Add(statusPanel);

            // Status message label on the left
            statusBarLabel = new Label
            {
                Location = new Point(15, 7),
                Size = new Size(300, 16),
                Text = "? Ready",
                ForeColor = accentColor,
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleLeft
            };
            statusPanel.Controls.Add(statusBarLabel);

            // Connection status label
            statusBarConnectionLabel = new Label
            {
                Location = new Point(320, 7),
                Size = new Size(200, 16),
                Text = "",
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleLeft
            };
            statusPanel.Controls.Add(statusBarConnectionLabel);

            // Words per second label
            wpsLabel = new Label
            {
                Location = new Point(530, 7),
                Size = new Size(150, 16),
                Text = "",
                ForeColor = Color.FromArgb(67, 181, 129), // Green accent
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            statusPanel.Controls.Add(wpsLabel);

            // Keyboard shortcuts on the right
            Label footerLabel = new Label
            {
                Location = new Point(850, 7),
                Size = new Size(430, 16),
                Text = "Ctrl+Enter to send • Enter for new line • UP/DOWN arrows for history",
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 8),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            statusPanel.Controls.Add(footerLabel);
            
            // Prevent Enter key from triggering the form's default button (send button)
            this.KeyPreview = true;
        }

        #endregion
    }
}








