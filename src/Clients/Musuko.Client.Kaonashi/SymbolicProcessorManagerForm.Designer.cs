using System;
using System.Drawing;
using System.Windows.Forms;

namespace Musuko.Kaonashi
{
    partial class SymbolicProcessorManagerForm
    {
        private System.ComponentModel.IContainer components = null;

        // UI Components
        private Panel searchPanel;
        private TextBox searchTextBox;
        private Button searchButton;
        private DataGridView processorsDataGrid;
        private Panel detailsPanel;
        private TextBox nameTextBox;
        private TextBox systemPromptTextBox;
        private TextBox inboundShouldTextBox;
        private TextBox inboundShouldNotTextBox;
        private TextBox inboundMustTextBox;
        private TextBox inboundMustNotTextBox;
        private TextBox outboundShouldTextBox;
        private TextBox outboundShouldNotTextBox;
        private TextBox outboundMustTextBox;
        private TextBox outboundMustNotTextBox;
        private Button saveButton;
        private Button saveAsButton;
        private Button loadButton;
        private Button newButton;
        private Button cloneButton;
        private Button newWindowButton;
        private Panel statusPanel;
        private Label statusLabel;
        private Label processorIdLabel;

        private void InitializeComponent()
        {
            // Modern color scheme
            var backgroundColor = Color.FromArgb(240, 244, 248);
            var panelColor = Color.White;
            var accentColor = Color.FromArgb(52, 152, 219);
            var textColor = Color.FromArgb(44, 62, 80);

            // Form
            Text = "Symbolic Processor Manager";
            Size = new Size(1600, 1100);
            MinimumSize = new Size(1400, 1000);
            BackColor = backgroundColor;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Tahoma", 9F);

            // Status Panel (Bottom)
            statusPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 35,
                BackColor = Color.FromArgb(52, 73, 94),
                Padding = new Padding(10, 8, 10, 8)
            };

            statusLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Ready",
                ForeColor = Color.White,
                Font = new Font("Tahoma", 9.5F),
                TextAlign = ContentAlignment.MiddleLeft
            };
            statusPanel.Controls.Add(statusLabel);

            // Search Panel (Top)
            searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = panelColor,
                Padding = new Padding(20, 20, 20, 15)
            };

            var searchLabel = new Label
            {
                Text = "Search Processors:",
                Location = new Point(0, 5),
                Size = new Size(120, 25),
                Font = new Font("Tahoma", 10F, FontStyle.Bold),
                ForeColor = textColor
            };

            searchTextBox = new TextBox
            {
                Location = new Point(125, 3),
                Size = new Size(400, 28),
                Font = new Font("Tahoma", 11F),
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Enter search criteria (* for last 10 processors)"
            };
            searchTextBox.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    SearchButton_Click(searchButton, EventArgs.Empty);
                }
            };

            searchButton = new Button
            {
                Text = "?? Search",
                Location = new Point(535, 2),
                Size = new Size(120, 30),
                BackColor = accentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += SearchButton_Click;

            // New Button in search panel
            var searchNewButton = new Button
            {
                Text = "? New",
                Location = new Point(665, 2),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            searchNewButton.FlatAppearance.BorderSize = 0;
            searchNewButton.Click += NewButton_Click;

            searchPanel.Controls.AddRange(new Control[] { searchLabel, searchTextBox, searchButton, searchNewButton });

            // DataGrid Panel (Middle)
            var gridPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 250,
                BackColor = backgroundColor,
                Padding = new Padding(20, 10, 20, 10)
            };

            processorsDataGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                Font = new Font("Tahoma", 9.5F),
                EnableHeadersVisualStyles = false
            };
            
            processorsDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            processorsDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            processorsDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            processorsDataGrid.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            processorsDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 252);
            processorsDataGrid.CellClick += ProcessorsDataGrid_CellClick;

            gridPanel.Controls.Add(processorsDataGrid);

            // Details Panel (Scrollable)
            var detailsScrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = backgroundColor,
                Padding = new Padding(20, 10, 20, 10),
                AutoScroll = true
            };

            detailsPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1540, 950),
                BackColor = panelColor,
                Padding = new Padding(30),
                Enabled = false
            };

            // Details Panel Title
            var detailsTitle = new Label
            {
                Text = "Processor Details",
                Location = new Point(10, 10),
                Size = new Size(200, 30),
                Font = new Font("Tahoma", 14F, FontStyle.Bold),
                ForeColor = accentColor
            };
            detailsPanel.Controls.Add(detailsTitle);

            // Processor ID Label
            processorIdLabel = new Label
            {
                Text = "",
                Location = new Point(220, 13),
                Size = new Size(600, 25),
                Font = new Font("Tahoma", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 100, 100)
            };
            detailsPanel.Controls.Add(processorIdLabel);

            // Buttons in top right corner, vertically aligned
            int buttonX = 1310;
            int buttonWidth = 180;
            int buttonHeight = 45;
            int buttonSpacing = 55;
            int currentY = 10;

            // Save Button (Top)
            saveButton = new Button
            {
                Text = "?? Save",
                Location = new Point(buttonX, currentY),
                Size = new Size(buttonWidth, buttonHeight),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.Click += SaveButton_Click;
            detailsPanel.Controls.Add(saveButton);
            currentY += buttonSpacing;

            // Save As Button
            saveAsButton = new Button
            {
                Text = "?? Save As...",
                Location = new Point(buttonX, currentY),
                Size = new Size(buttonWidth, buttonHeight),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            saveAsButton.FlatAppearance.BorderSize = 0;
            saveAsButton.Click += SaveAsButton_Click;
            detailsPanel.Controls.Add(saveAsButton);
            currentY += buttonSpacing;

            // Load Button
            loadButton = new Button
            {
                Text = "?? Load File...",
                Location = new Point(buttonX, currentY),
                Size = new Size(buttonWidth, buttonHeight),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            loadButton.FlatAppearance.BorderSize = 0;
            loadButton.Click += LoadButton_Click;
            detailsPanel.Controls.Add(loadButton);
            currentY += buttonSpacing;

            // New Button
            newButton = new Button
            {
                Text = "? New",
                Location = new Point(buttonX, currentY),
                Size = new Size(buttonWidth, buttonHeight),
                BackColor = accentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            newButton.FlatAppearance.BorderSize = 0;
            newButton.Click += NewButton_Click;
            detailsPanel.Controls.Add(newButton);
            currentY += buttonSpacing;

            // Clone Button
            cloneButton = new Button
            {
                Text = "?? Clone",
                Location = new Point(buttonX, currentY),
                Size = new Size(buttonWidth, buttonHeight),
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            cloneButton.FlatAppearance.BorderSize = 0;
            cloneButton.Click += CloneButton_Click;
            detailsPanel.Controls.Add(cloneButton);
            currentY += buttonSpacing;

            // New Window Button
            newWindowButton = new Button
            {
                Text = "?? New Window",
                Location = new Point(buttonX, currentY),
                Size = new Size(buttonWidth, buttonHeight),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            newWindowButton.FlatAppearance.BorderSize = 0;
            newWindowButton.Click += NewWindowButton_Click;
            detailsPanel.Controls.Add(newWindowButton);

            int labelX = 30;
            int fieldX = 200;
            int fieldWidth = 500;
            int yPos = 55;
            int rowHeight = 45;

            // Helper to create label-textbox pairs
            void AddField(string labelText, Control control, int width = 500)
            {
                var label = new Label
                {
                    Text = labelText,
                    Location = new Point(labelX, yPos + 3),
                    Size = new Size(160, 25),
                    Font = new Font("Tahoma", 9.5F, FontStyle.Bold),
                    ForeColor = textColor
                };
                
                control.Location = new Point(fieldX, yPos);
                control.Size = new Size(width, 28);
                control.Font = new Font("Tahoma", 10F);
                
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                }

                detailsPanel.Controls.AddRange(new Control[] { label, control });
                yPos += rowHeight;
            }

            // Row 1: Name
            nameTextBox = new TextBox();
            AddField("Name:", nameTextBox, 1020);

            // Row 2: System Prompt (Multiline)
            var systemPromptLabel = new Label
            {
                Text = "System Prompt:",
                Location = new Point(labelX, yPos + 3),
                Size = new Size(160, 25),
                Font = new Font("Tahoma", 9.5F, FontStyle.Bold),
                ForeColor = textColor
            };
            systemPromptTextBox = new TextBox
            {
                Location = new Point(fieldX, yPos),
                Size = new Size(1020, 100),
                Font = new Font("Tahoma", 10F),
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            detailsPanel.Controls.AddRange(new Control[] { systemPromptLabel, systemPromptTextBox });
            yPos += 110;

            // Row 3: Inbound Should (List)
            inboundShouldTextBox = new TextBox();
            AddField("Inbound Should (comma-separated):", inboundShouldTextBox, 1020);

            // Row 4: Inbound Should Not (List)
            inboundShouldNotTextBox = new TextBox();
            AddField("Inbound Should Not (comma-separated):", inboundShouldNotTextBox, 1020);

            // Row 5: Inbound Must (List)
            inboundMustTextBox = new TextBox();
            AddField("Inbound Must (comma-separated):", inboundMustTextBox, 1020);

            // Row 6: Inbound Must Not (List)
            inboundMustNotTextBox = new TextBox();
            AddField("Inbound Must Not (comma-separated):", inboundMustNotTextBox, 1020);

            // Row 7: Outbound Should (List)
            outboundShouldTextBox = new TextBox();
            AddField("Outbound Should (comma-separated):", outboundShouldTextBox, 1020);

            // Row 8: Outbound Should Not (List)
            outboundShouldNotTextBox = new TextBox();
            AddField("Outbound Should Not (comma-separated):", outboundShouldNotTextBox, 1020);

            // Row 9: Outbound Must (List)
            outboundMustTextBox = new TextBox();
            AddField("Outbound Must (comma-separated):", outboundMustTextBox, 1020);

            // Row 10: Outbound Must Not (List)
            outboundMustNotTextBox = new TextBox();
            AddField("Outbound Must Not (comma-separated):", outboundMustNotTextBox, 1020);

            detailsScrollPanel.Controls.Add(detailsPanel);

            // Add all panels to form
            Controls.Add(detailsScrollPanel);
            Controls.Add(gridPanel);
            Controls.Add(searchPanel);
            Controls.Add(statusPanel);
        }
    }
}
