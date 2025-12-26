using System;
using System.Drawing;
using System.Windows.Forms;

namespace Musuko.Kaonashi
{
    partial class CodeViewerForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.Text = "Code Viewer";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);
            this.MinimumSize = new Size(600, 400);

            // Header Panel
            headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(900, 50),
                BackColor = secondaryBg,
                Dock = DockStyle.Top
            };
            this.Controls.Add(headerPanel);

            // Title Label
            titleLabel = new Label
            {
                Location = new Point(20, 15),
                Size = new Size(700, 25),
                Text = "Code Preview",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = textColor,
                TextAlign = ContentAlignment.MiddleLeft
            };
            headerPanel.Controls.Add(titleLabel);

            // Code TextBox
            codeTextBox = new RichTextBox
            {
                Location = new Point(15, 60),
                Size = new Size(870, 490),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                BackColor = secondaryBg,
                ForeColor = textColor,
                Font = new Font("Consolas", 10),
                BorderStyle = BorderStyle.None,
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.Both
            };
            this.Controls.Add(codeTextBox);

            // Footer Panel
            footerPanel = new Panel
            {
                Location = new Point(0, 560),
                Size = new Size(900, 60),
                BackColor = secondaryBg,
                Dock = DockStyle.Bottom
            };
            this.Controls.Add(footerPanel);

            // Copy Button
            copyButton = new Button
            {
                Location = new Point(420, 15),
                Size = new Size(140, 35),
                Text = "?? Copy Code",
                BackColor = accentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            copyButton.FlatAppearance.BorderSize = 0;
            copyButton.Click += CopyButton_Click;
            footerPanel.Controls.Add(copyButton);

            // Save As Button
            saveAsButton = new Button
            {
                Location = new Point(580, 15),
                Size = new Size(140, 35),
                Text = "?? Save As...",
                BackColor = Color.FromArgb(67, 181, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            saveAsButton.FlatAppearance.BorderSize = 0;
            saveAsButton.Click += SaveAsButton_Click;
            footerPanel.Controls.Add(saveAsButton);

            // Close Button
            closeButton = new Button
            {
                Location = new Point(740, 15),
                Size = new Size(140, 35),
                Text = "Close",
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = textColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Close();
            footerPanel.Controls.Add(closeButton);
        }

        #endregion
    }
}








