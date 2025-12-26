using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Musuko.Kaonashi
{
    partial class JsonClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // Form settings
            Text = "JSON REST Client";
            Size = new Size(1400, 900);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(24, 26, 27);
            MinimumSize = new Size(900, 650);

            // Header Panel with gradient effect
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(32, 34, 37)
            };
            headerPanel.Paint += (s, e) => {
                // Add subtle bottom border
                using (Pen pen = new Pen(Color.FromArgb(0, 122, 204), 2))
                {
                    e.Graphics.DrawLine(pen, 0, headerPanel.Height - 2, headerPanel.Width, headerPanel.Height - 2);
                }
            };
            Controls.Add(headerPanel);

            // Title with gradient background
            var titlePanel = new Panel
            {
                Location = new Point(20, 15),
                Size = new Size(280, 50),
                BackColor = Color.FromArgb(0, 122, 204)
            };
            titlePanel.Paint += (s, e) => {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    titlePanel.ClientRectangle,
                    Color.FromArgb(0, 122, 204),
                    Color.FromArgb(0, 180, 240),
                    45f))
                {
                    e.Graphics.FillRectangle(brush, titlePanel.ClientRectangle);
                }
            };
            headerPanel.Controls.Add(titlePanel);

            titleLabel = new Label
            {
                Text = "?? JSON REST Client",
                Location = new Point(10, 10),
                Size = new Size(260, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };
            titlePanel.Controls.Add(titleLabel);

            // Request Panel (Left side) with border
            requestPanel = new Panel
            {
                Location = new Point(15, 95),
                Size = new Size(680, 735),
                BackColor = Color.FromArgb(32, 34, 37),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            requestPanel.Paint += (s, e) => {
                // Draw border
                using (Pen pen = new Pen(Color.FromArgb(50, 50, 50), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, requestPanel.Width - 1, requestPanel.Height - 1);
                }
            };
            Controls.Add(requestPanel);

            // URL Label
            urlLabel = new Label
            {
                Text = "?? Endpoint",
                Location = new Point(20, 20),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 180, 240)
            };
            requestPanel.Controls.Add(urlLabel);

            // Method ComboBox with better styling
            methodComboBox = new ComboBox
            {
                Location = new Point(20, 55),
                Size = new Size(110, 35),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 47, 49),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            methodComboBox.Items.AddRange(new object[] { "GET", "POST", "PUT", "DELETE", "PATCH" });
            methodComboBox.SelectedIndex = 0;
            requestPanel.Controls.Add(methodComboBox);

            // URL TextBox with better styling
            urlTextBox = new TextBox
            {
                Location = new Point(140, 55),
                Size = new Size(390, 35),
                BackColor = Color.FromArgb(45, 47, 49),
                ForeColor = Color.FromArgb(220, 220, 220),
                Font = new Font("Consolas", 11),
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            urlTextBox.Enter += (s, e) => urlTextBox.BackColor = Color.FromArgb(55, 57, 59);
            urlTextBox.Leave += (s, e) => urlTextBox.BackColor = Color.FromArgb(45, 47, 49);
            requestPanel.Controls.Add(urlTextBox);

            // Send Button with gradient and hover effect
            sendButton = new Button
            {
                Location = new Point(540, 52),
                Size = new Size(120, 40),
                Text = "? Send",
                BackColor = Color.FromArgb(0, 180, 240),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.Click += SendButton_Click;
            sendButton.MouseEnter += (s, e) => sendButton.BackColor = Color.FromArgb(0, 150, 200);
            sendButton.MouseLeave += (s, e) => sendButton.BackColor = Color.FromArgb(0, 180, 240);
            requestPanel.Controls.Add(sendButton);

            // Request Body Label
            requestBodyLabel = new Label
            {
                Text = "?? Request Body",
                Location = new Point(20, 110),
                Size = new Size(180, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 180, 240)
            };
            requestPanel.Controls.Add(requestBodyLabel);

            // Save Request As Button
            saveRequestAsButton = new Button
            {
                Location = new Point(210, 112),
                Size = new Size(85, 28),
                Text = "?? Save",
                BackColor = Color.FromArgb(156, 120, 206),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            saveRequestAsButton.FlatAppearance.BorderSize = 0;
            saveRequestAsButton.Click += SaveRequestAsButton_Click;
            saveRequestAsButton.MouseEnter += (s, e) => saveRequestAsButton.BackColor = Color.FromArgb(146, 110, 196);
            saveRequestAsButton.MouseLeave += (s, e) => saveRequestAsButton.BackColor = Color.FromArgb(156, 120, 206);
            requestPanel.Controls.Add(saveRequestAsButton);

            // Load Request Button
            loadRequestButton = new Button
            {
                Location = new Point(300, 112),
                Size = new Size(75, 28),
                Text = "?? Load",
                BackColor = Color.FromArgb(255, 165, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            loadRequestButton.FlatAppearance.BorderSize = 0;
            loadRequestButton.Click += LoadRequestButton_Click;
            loadRequestButton.MouseEnter += (s, e) => loadRequestButton.BackColor = Color.FromArgb(245, 155, 0);
            loadRequestButton.MouseLeave += (s, e) => loadRequestButton.BackColor = Color.FromArgb(255, 165, 0);
            requestPanel.Controls.Add(loadRequestButton);

            // Format Request Button
            formatRequestButton = new Button
            {
                Location = new Point(380, 112),
                Size = new Size(85, 28),
                Text = "? Format",
                BackColor = Color.FromArgb(67, 181, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            formatRequestButton.FlatAppearance.BorderSize = 0;
            formatRequestButton.Click += FormatRequestButton_Click;
            formatRequestButton.MouseEnter += (s, e) => formatRequestButton.BackColor = Color.FromArgb(57, 171, 119);
            formatRequestButton.MouseLeave += (s, e) => formatRequestButton.BackColor = Color.FromArgb(67, 181, 129);
            requestPanel.Controls.Add(formatRequestButton);

            // Clear Button
            clearButton = new Button
            {
                Location = new Point(470, 112),
                Size = new Size(80, 28),
                Text = "?? Clear",
                BackColor = Color.FromArgb(220, 80, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            clearButton.FlatAppearance.BorderSize = 0;
            clearButton.Click += ClearButton_Click;
            clearButton.MouseEnter += (s, e) => clearButton.BackColor = Color.FromArgb(200, 60, 60);
            clearButton.MouseLeave += (s, e) => clearButton.BackColor = Color.FromArgb(220, 80, 80);
            requestPanel.Controls.Add(clearButton);

            // Request Body TextBox with better styling
            requestBodyTextBox = new RichTextBox
            {
                Location = new Point(20, 150),
                Size = new Size(640, 565),
                BackColor = Color.FromArgb(28, 30, 32),
                ForeColor = Color.FromArgb(220, 220, 220),
                Font = new Font("Consolas", 10.5f),
                BorderStyle = BorderStyle.None,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AcceptsTab = true,
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.Both
            };
            requestBodyTextBox.TextChanged += RequestBodyTextBox_TextChanged;
            requestBodyTextBox.Enter += (s, e) => requestBodyTextBox.BackColor = Color.FromArgb(35, 37, 39);
            requestBodyTextBox.Leave += (s, e) => requestBodyTextBox.BackColor = Color.FromArgb(28, 30, 32);
            requestPanel.Controls.Add(requestBodyTextBox);

            // Response Panel (Right side) with border
            responsePanel = new Panel
            {
                Location = new Point(705, 95),
                Size = new Size(680, 735),
                BackColor = Color.FromArgb(32, 34, 37),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            responsePanel.Paint += (s, e) => {
                // Draw border
                using (Pen pen = new Pen(Color.FromArgb(50, 50, 50), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, responsePanel.Width - 1, responsePanel.Height - 1);
                }
            };
            Controls.Add(responsePanel);

            // Response Label
            responseLabel = new Label
            {
                Text = "?? Response",
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 180, 240)
            };
            responsePanel.Controls.Add(responseLabel);

            // Save Response As Button
            saveResponseAsButton = new Button
            {
                Location = new Point(430, 18),
                Size = new Size(120, 30),
                Text = "?? Save as",
                BackColor = Color.FromArgb(156, 120, 206),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            saveResponseAsButton.FlatAppearance.BorderSize = 0;
            saveResponseAsButton.Click += SaveResponseAsButton_Click;
            saveResponseAsButton.MouseEnter += (s, e) => saveResponseAsButton.BackColor = Color.FromArgb(146, 110, 196);
            saveResponseAsButton.MouseLeave += (s, e) => saveResponseAsButton.BackColor = Color.FromArgb(156, 120, 206);
            responsePanel.Controls.Add(saveResponseAsButton);

            // Format Response Button
            formatResponseButton = new Button
            {
                Location = new Point(560, 18),
                Size = new Size(100, 30),
                Text = "? Format",
                BackColor = Color.FromArgb(67, 181, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            formatResponseButton.FlatAppearance.BorderSize = 0;
            formatResponseButton.Click += FormatResponseButton_Click;
            formatResponseButton.MouseEnter += (s, e) => formatResponseButton.BackColor = Color.FromArgb(57, 171, 119);
            formatResponseButton.MouseLeave += (s, e) => formatResponseButton.BackColor = Color.FromArgb(67, 181, 129);
            responsePanel.Controls.Add(formatResponseButton);

            // Response TextBox with better styling
            responseTextBox = new RichTextBox
            {
                Location = new Point(20, 60),
                Size = new Size(640, 655),
                BackColor = Color.FromArgb(28, 30, 32),
                ForeColor = Color.FromArgb(220, 220, 220),
                Font = new Font("Consolas", 10.5f),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.Both
            };
            responsePanel.Controls.Add(responseTextBox);

            // Footer Panel
            footerPanel = new Panel
            {
                Location = new Point(0, 840),
                Size = new Size(1400, 50),
                BackColor = Color.FromArgb(28, 30, 32),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            footerPanel.Paint += (s, e) => {
                // Draw top border
                using (Pen pen = new Pen(Color.FromArgb(50, 50, 50), 1))
                {
                    e.Graphics.DrawLine(pen, 0, 0, footerPanel.Width, 0);
                }
            };
            Controls.Add(footerPanel);

            // Status Label in footer
            statusLabel = new Label
            {
                Location = new Point(20, 15),
                Size = new Size(1360, 25),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(180, 180, 180),
                Text = "? Ready to send requests",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            footerPanel.Controls.Add(statusLabel);
        }

        #endregion

        private void RequestBodyTextBox_TextChanged(object? sender, EventArgs e)
        {
            // Optional: Auto-format on paste or after typing
        }
    }
}

