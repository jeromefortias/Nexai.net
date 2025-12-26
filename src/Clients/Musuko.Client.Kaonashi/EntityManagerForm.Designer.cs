using System;
using System.Drawing;
using System.Windows.Forms;

namespace Musuko.Kaonashi
{
    partial class EntityManagerForm
    {
        private System.ComponentModel.IContainer components = null;

        // UI Components
        private Panel searchPanel;
        private TextBox searchTextBox;
        private Button searchButton;
        private DataGridView entitiesDataGrid;
        private Panel detailsPanel;
        private TextBox nameTextBox;
        private TextBox alternativeNamesTextBox;
        private TextBox typeTextBox;
        private TextBox shortDescriptionTextBox;
        private TextBox longDescriptionTextBox;
        private TextBox relatedEntitiesTextBox;
        private NumericUpDown joyNumeric;
        private NumericUpDown fearNumeric;
        private NumericUpDown angerNumeric;
        private NumericUpDown sadnessNumeric;
        private NumericUpDown disgustNumeric;
        private DateTimePicker dateFromPicker;
        private DateTimePicker dateToPicker;
        private TextBox howToTextBox;
        private TextBox withWhatTextBox;
        private TextBox withoutWhatTextBox;
        private TextBox whereTextBox;
        private TextBox whenTextBox;
        private TextBox commentTextBox;
        private Button saveButton;
        private Button saveAsButton;
        private Button loadButton;
        private Button newButton;
        private Button cloneButton;
        private Button newWindowButton;
        private Button autogenerateButton;
        private Panel statusPanel;
        private Label statusLabel;
        private Label entityIdLabel;

        private void InitializeComponent()
        {
            // Modern color scheme
            var backgroundColor = Color.FromArgb(240, 244, 248);
            var panelColor = Color.White;
            var accentColor = Color.FromArgb(52, 152, 219);
            var textColor = Color.FromArgb(44, 62, 80);

            // Form
            Text = "Entity Manager";
            Size = new Size(1600, 1100);
            MinimumSize = new Size(1400, 1000);
            BackColor = backgroundColor;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9F);

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
                Font = new Font("Segoe UI", 9.5F),
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
                Text = "Search Entities:",
                Location = new Point(0, 5),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = textColor
            };

            searchTextBox = new TextBox
            {
                Location = new Point(125, 3),
                Size = new Size(400, 28),
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Enter search criteria (* for last 10 entities)"
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
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += SearchButton_Click;

            searchPanel.Controls.AddRange(new Control[] { searchLabel, searchTextBox, searchButton });

            // DataGrid Panel (Middle)
            var gridPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 250,
                BackColor = backgroundColor,
                Padding = new Padding(20, 10, 20, 10)
            };

            entitiesDataGrid = new DataGridView
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
                Font = new Font("Segoe UI", 9.5F),
                EnableHeadersVisualStyles = false
            };
            
            entitiesDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            entitiesDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            entitiesDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            entitiesDataGrid.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            entitiesDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 252);
            entitiesDataGrid.CellClick += EntitiesDataGrid_CellClick;

            gridPanel.Controls.Add(entitiesDataGrid);

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
                Enabled = true  // Keep enabled so buttons remain accessible
            };

            // Details Panel Title
            var detailsTitle = new Label
            {
                Text = "Entity Details",
                Location = new Point(10, 10),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = accentColor
            };
            detailsPanel.Controls.Add(detailsTitle);

            // Entity ID Label
            entityIdLabel = new Label
            {
                Text = "",
                Location = new Point(220, 13),
                Size = new Size(600, 25),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 100, 100)
            };
            detailsPanel.Controls.Add(entityIdLabel);

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
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
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
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
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
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
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
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
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
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
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
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            newWindowButton.FlatAppearance.BorderSize = 0;
            newWindowButton.Click += NewWindowButton_Click;
            detailsPanel.Controls.Add(newWindowButton);
            currentY += buttonSpacing;

            // Autogenerate Button
            autogenerateButton = new Button
            {
                Text = "?? Autogenerate",
                Location = new Point(buttonX, currentY),
                Size = new Size(buttonWidth, buttonHeight),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            autogenerateButton.FlatAppearance.BorderSize = 0;
            autogenerateButton.Click += AutogenerateButton_Click;
            detailsPanel.Controls.Add(autogenerateButton);

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
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = textColor
                };
                
                control.Location = new Point(fieldX, yPos);
                control.Size = new Size(width, 28);
                control.Font = new Font("Segoe UI", 10F);
                
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                }

                detailsPanel.Controls.AddRange(new Control[] { label, control });
                yPos += rowHeight;
            }

            // Row 1: Name and Type
            nameTextBox = new TextBox();
            AddField("Name:", nameTextBox, 700);

            typeTextBox = new TextBox();
            typeTextBox.Location = new Point(920, 55);
            typeTextBox.Size = new Size(300, 28);
            typeTextBox.Font = new Font("Segoe UI", 10F);
            typeTextBox.BorderStyle = BorderStyle.FixedSingle;
            var typeLabel = new Label
            {
                Text = "Type:",
                Location = new Point(850, 58),
                Size = new Size(60, 25),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = textColor
            };
            detailsPanel.Controls.AddRange(new Control[] { typeLabel, typeTextBox });

            // Row 2: Alternative Names
            alternativeNamesTextBox = new TextBox();
            AddField("Alternative Names:", alternativeNamesTextBox, 1020);

            // Row 3: Short Description
            shortDescriptionTextBox = new TextBox();
            AddField("Short Description:", shortDescriptionTextBox, 1020);

            // Row 4-5: Long Description (Multiline)
            var longDescLabel = new Label
            {
                Text = "Long Description:",
                Location = new Point(labelX, yPos + 3),
                Size = new Size(160, 25),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = textColor
            };
            longDescriptionTextBox = new TextBox
            {
                Location = new Point(fieldX, yPos),
                Size = new Size(1020, 100),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            detailsPanel.Controls.AddRange(new Control[] { longDescLabel, longDescriptionTextBox });
            yPos += 110;

            // Row 6: Related Entities
            relatedEntitiesTextBox = new TextBox();
            AddField("Related Entity IDs:", relatedEntitiesTextBox, 1020);

            // Row 7: Emotional Scores (All 5)
            var emotionLabel = new Label
            {
                Text = "Emotional Scores:",
                Location = new Point(labelX, yPos + 3),
                Size = new Size(160, 25),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = textColor
            };
            detailsPanel.Controls.Add(emotionLabel);

            // Joy
            var joyLabel = new Label
            {
                Text = "Joy:",
                Location = new Point(fieldX, yPos + 3),
                Size = new Size(50, 25),
                Font = new Font("Segoe UI", 9F),
                ForeColor = textColor
            };
            joyNumeric = new NumericUpDown
            {
                Location = new Point(fieldX + 55, yPos),
                Size = new Size(80, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 100
            };

            // Fear
            var fearLabel = new Label
            {
                Text = "Fear:",
                Location = new Point(fieldX + 150, yPos + 3),
                Size = new Size(50, 25),
                Font = new Font("Segoe UI", 9F),
                ForeColor = textColor
            };
            fearNumeric = new NumericUpDown
            {
                Location = new Point(fieldX + 205, yPos),
                Size = new Size(80, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 100
            };

            // Anger
            var angerLabel = new Label
            {
                Text = "Anger:",
                Location = new Point(fieldX + 300, yPos + 3),
                Size = new Size(50, 25),
                Font = new Font("Segoe UI", 9F),
                ForeColor = textColor
            };
            angerNumeric = new NumericUpDown
            {
                Location = new Point(fieldX + 360, yPos),
                Size = new Size(80, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 100
            };

            // Sadness
            var sadnessLabel = new Label
            {
                Text = "Sadness:",
                Location = new Point(fieldX + 455, yPos + 3),
                Size = new Size(70, 25),
                Font = new Font("Segoe UI", 9F),
                ForeColor = textColor
            };
            sadnessNumeric = new NumericUpDown
            {
                Location = new Point(fieldX + 530, yPos),
                Size = new Size(80, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 100
            };

            // Disgust
            var disgustLabel = new Label
            {
                Text = "Disgust:",
                Location = new Point(fieldX + 625, yPos + 3),
                Size = new Size(70, 25),
                Font = new Font("Segoe UI", 9F),
                ForeColor = textColor
            };
            disgustNumeric = new NumericUpDown
            {
                Location = new Point(fieldX + 700, yPos),
                Size = new Size(80, 28),
                Font = new Font("Segoe UI", 10F),
                Minimum = 0,
                Maximum = 100
            };

            detailsPanel.Controls.AddRange(new Control[] { 
                joyLabel, joyNumeric, 
                fearLabel, fearNumeric,
                angerLabel, angerNumeric,
                sadnessLabel, sadnessNumeric,
                disgustLabel, disgustNumeric
            });
            yPos += rowHeight;

            // Row 8: Date Range
            var dateFromLabel = new Label
            {
                Text = "Date From:",
                Location = new Point(labelX, yPos + 3),
                Size = new Size(160, 25),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = textColor
            };
            dateFromPicker = new DateTimePicker
            {
                Location = new Point(fieldX, yPos),
                Size = new Size(200, 28),
                Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short
            };

            var dateToLabel = new Label
            {
                Text = "Date To:",
                Location = new Point(fieldX + 250, yPos + 3),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = textColor
            };
            dateToPicker = new DateTimePicker
            {
                Location = new Point(fieldX + 340, yPos),
                Size = new Size(200, 28),
                Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short
            };
            detailsPanel.Controls.AddRange(new Control[] { 
                dateFromLabel, dateFromPicker, 
                dateToLabel, dateToPicker 
            });
            yPos += rowHeight;

            // Row 9: HowTo
            howToTextBox = new TextBox();
            AddField("How To:", howToTextBox, 1020);

            // Row 10: WithWhat
            withWhatTextBox = new TextBox();
            AddField("With What:", withWhatTextBox, 1020);

            // Row 11: WithoutWhat
            withoutWhatTextBox = new TextBox();
            AddField("Without What:", withoutWhatTextBox, 1020);

            // Row 12: Where
            whereTextBox = new TextBox();
            AddField("Where:", whereTextBox, 1020);

            // Row 13: When
            whenTextBox = new TextBox();
            AddField("When:", whenTextBox, 1020);

            // Row 14: Comment
            commentTextBox = new TextBox();
            AddField("Comment:", commentTextBox, 1020);


            detailsScrollPanel.Controls.Add(detailsPanel);

            // Add all panels to form
            Controls.Add(detailsScrollPanel);
            Controls.Add(gridPanel);
            Controls.Add(searchPanel);
            Controls.Add(statusPanel);
        }
    }
}

