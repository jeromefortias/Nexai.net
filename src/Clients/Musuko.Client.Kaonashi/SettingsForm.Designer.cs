using System;
using System.Drawing;
using System.Windows.Forms;

namespace Musuko.Kaonashi
{
    partial class SettingsForm
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
            this.Text = "Settings";
            this.Size = new Size(500, 800);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);

            // Header Panel
            headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(500, 60),
                BackColor = secondaryBg,
                Dock = DockStyle.Top
            };
            this.Controls.Add(headerPanel);

            // Title Label
            titleLabel = new Label
            {
                Location = new Point(20, 15),
                Size = new Size(300, 30),
                Text = "? Connection Settings",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = textColor,
                TextAlign = ContentAlignment.MiddleLeft
            };
            headerPanel.Controls.Add(titleLabel);

            // Content Panel
            contentPanel = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(500, 570),
                BackColor = backgroundColor,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(contentPanel);

            // Info label
            infoLabel = new Label
            {
                Location = new Point(30, 15),
                Size = new Size(440, 30),
                Text = "Configure Ollama connections (Models list & Completion):",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 180, 180)
            };
            contentPanel.Controls.Add(infoLabel);

            // Host label (for model list)
            hostLabel = new Label
            {
                Location = new Point(30, 55),
                Size = new Size(430, 25),
                Text = "Models List Server:",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = primaryColor
            };
            contentPanel.Controls.Add(hostLabel);

            // Host address label
            Label hostAddressLabel = new Label
            {
                Location = new Point(30, 85),
                Size = new Size(120, 30),
                Text = "Host Address:",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10),
                ForeColor = textColor
            };
            contentPanel.Controls.Add(hostAddressLabel);

            // Host textbox
            hostTextBox = new TextBox
            {
                Location = new Point(160, 85),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = inputBg,
                ForeColor = textColor,
                BorderStyle = BorderStyle.FixedSingle
            };
            contentPanel.Controls.Add(hostTextBox);

            // Port label
            portLabel = new Label
            {
                Location = new Point(30, 125),
                Size = new Size(120, 30),
                Text = "Port:",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10),
                ForeColor = textColor
            };
            contentPanel.Controls.Add(portLabel);

            // Port numeric up-down
            portNumericUpDown = new NumericUpDown
            {
                Location = new Point(160, 125),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = inputBg,
                ForeColor = textColor,
                BorderStyle = BorderStyle.FixedSingle,
                Minimum = 1,
                Maximum = 65535,
                Value = 11434
            };
            contentPanel.Controls.Add(portNumericUpDown);

            // Completion server header
            completionHostLabel = new Label
            {
                Location = new Point(30, 170),
                Size = new Size(430, 25),
                Text = "Chat Completion Server:",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = primaryColor
            };
            contentPanel.Controls.Add(completionHostLabel);

            // Completion host address label
            Label completionAddressLabel = new Label
            {
                Location = new Point(30, 200),
                Size = new Size(120, 30),
                Text = "Host Address:",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10),
                ForeColor = textColor
            };
            contentPanel.Controls.Add(completionAddressLabel);

            // Completion host textbox
            completionHostTextBox = new TextBox
            {
                Location = new Point(160, 200),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = inputBg,
                ForeColor = textColor,
                BorderStyle = BorderStyle.FixedSingle
            };
            contentPanel.Controls.Add(completionHostTextBox);

            // Completion port label
            completionPortLabel = new Label
            {
                Location = new Point(30, 240),
                Size = new Size(120, 30),
                Text = "Port:",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10),
                ForeColor = textColor
            };
            contentPanel.Controls.Add(completionPortLabel);

            // Completion port numeric up-down
            completionPortNumericUpDown = new NumericUpDown
            {
                Location = new Point(160, 240),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = inputBg,
                ForeColor = textColor,
                BorderStyle = BorderStyle.FixedSingle,
                Minimum = 1,
                Maximum = 65535,
                Value = 11434
            };
            contentPanel.Controls.Add(completionPortNumericUpDown);

            // Model label
            modelLabel = new Label
            {
                Location = new Point(30, 285),
                Size = new Size(120, 30),
                Text = "Default Model:",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10),
                ForeColor = textColor
            };
            contentPanel.Controls.Add(modelLabel);

            // Model textbox
            modelTextBox = new TextBox
            {
                Location = new Point(160, 285),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = inputBg,
                ForeColor = textColor,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "e.g., llama2, mistral, codellama"
            };
            contentPanel.Controls.Add(modelTextBox);

            // System Prompt label
            systemPromptLabel = new Label
            {
                Location = new Point(30, 330),
                Size = new Size(430, 30),
                Text = "System Prompt:",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10),
                ForeColor = textColor
            };
            contentPanel.Controls.Add(systemPromptLabel);

            // System Prompt textbox
            systemPromptTextBox = new TextBox
            {
                Location = new Point(30, 360),
                Size = new Size(430, 100),
                Font = new Font("Segoe UI", 9.5f),
                BackColor = inputBg,
                ForeColor = textColor,
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "Define the AI's behavior and personality..."
            };
            contentPanel.Controls.Add(systemPromptTextBox);

            // Hide Think Tags CheckBox
            hideThinkTagsCheckBox = new CheckBox
            {
                Location = new Point(30, 475),
                Size = new Size(430, 30),
                Text = "Hide <think> tags in responses (filter AI reasoning tokens)",
                ForeColor = textColor,
                Font = new Font("Segoe UI", 10),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };
            contentPanel.Controls.Add(hideThinkTagsCheckBox);

            // Save Interactions CheckBox
            saveInteractionsCheckBox = new CheckBox
            {
                Location = new Point(30, 515),
                Size = new Size(430, 30),
                Text = "Save all interactions to JSON files (datetime.completion.json)",
                ForeColor = textColor,
                Font = new Font("Segoe UI", 10),
                Checked = false,
                FlatStyle = FlatStyle.Flat
            };
            contentPanel.Controls.Add(saveInteractionsCheckBox);

            // Prompt Folder Label
            promptFolderLabel = new Label
            {
                Location = new Point(30, 560),
                Size = new Size(430, 25),
                Text = "Default Prompt Folder:",
                ForeColor = textColor,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            contentPanel.Controls.Add(promptFolderLabel);

            // Prompt Folder TextBox
            promptFolderTextBox = new TextBox
            {
                Location = new Point(30, 590),
                Size = new Size(335, 30),
                BackColor = inputBg,
                ForeColor = textColor,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            contentPanel.Controls.Add(promptFolderTextBox);

            // Browse Folder Button
            browseFolderButton = new Button
            {
                Location = new Point(375, 590),
                Size = new Size(85, 30),
                Text = "Browse...",
                BackColor = primaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand
            };
            browseFolderButton.FlatAppearance.BorderSize = 0;
            browseFolderButton.Click += BrowseFolderButton_Click;
            contentPanel.Controls.Add(browseFolderButton);

            // Footer Panel
            footerPanel = new Panel
            {
                Location = new Point(0, 700),
                Size = new Size(500, 70),
                BackColor = secondaryBg,
                Dock = DockStyle.Bottom
            };
            this.Controls.Add(footerPanel);

            // Cancel button
            cancelButton = new Button
            {
                Location = new Point(260, 18),
                Size = new Size(100, 35),
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Font = new Font("Segoe UI", 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = inputBg,
                ForeColor = textColor,
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 84, 95);
            cancelButton.Click += CancelButton_Click;
            footerPanel.Controls.Add(cancelButton);

            // Save button
            saveButton = new Button
            {
                Location = new Point(370, 18),
                Size = new Size(100, 35),
                Text = "Save",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = primaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.FlatAppearance.MouseOverBackColor = primaryHover;
            saveButton.Click += SaveButton_Click;
            footerPanel.Controls.Add(saveButton);

            this.AcceptButton = saveButton;
            this.CancelButton = cancelButton;
        }

        #endregion
    }
}








