using System;
using System.Drawing;
using System.Windows.Forms;

namespace Musuko.Kaonashi
{
    partial class NewsViewerForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView newsDataGridView;
        private Panel headerPanel;
        private Label titleLabel;
        private Panel footerPanel;
        private Button refreshButton;
        private Button closeButton;
        private Label statusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                newsService?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Form colors
            Color primaryBg = Color.FromArgb(40, 42, 54);
            Color secondaryBg = Color.FromArgb(44, 47, 51);
            Color accentColor = Color.FromArgb(98, 114, 164);
            Color textColor = Color.FromArgb(248, 248, 242);
            
            // Form
            this.Text = "News Viewer";
            this.Size = new Size(900, 600);
            this.BackColor = primaryBg;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(600, 400);
            
            // DataGridView - Add FIRST so it's at the bottom of Z-order
            newsDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = primaryBg,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(60, 63, 65)
            };
            
            // Configure DataGridView style
            newsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = secondaryBg;
            newsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            newsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            newsDataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = secondaryBg;
            newsDataGridView.ColumnHeadersHeight = 35;
            
            newsDataGridView.DefaultCellStyle.BackColor = primaryBg;
            newsDataGridView.DefaultCellStyle.ForeColor = textColor;
            newsDataGridView.DefaultCellStyle.SelectionBackColor = accentColor;
            newsDataGridView.DefaultCellStyle.SelectionForeColor = textColor;
            newsDataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            newsDataGridView.RowTemplate.Height = 30;
            
            newsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(45, 47, 59);
            
            // Add columns
            var titleColumn = new DataGridViewTextBoxColumn
            {
                Name = "TitleColumn",
                HeaderText = "Title",
                FillWeight = 60,
                DataPropertyName = "Title"
            };
            newsDataGridView.Columns.Add(titleColumn);
            
            var ratingColumn = new DataGridViewTextBoxColumn
            {
                Name = "RatingColumn",
                HeaderText = "Rating",
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };
            newsDataGridView.Columns.Add(ratingColumn);
            
            var urlColumn = new DataGridViewTextBoxColumn
            {
                Name = "UrlColumn",
                HeaderText = "URL",
                FillWeight = 30,
                DataPropertyName = "Url"
            };
            newsDataGridView.Columns.Add(urlColumn);
            
            // Add double-click event
            newsDataGridView.CellDoubleClick += NewsDataGridView_CellDoubleClick;
            
            // Add DataGridView first
            this.Controls.Add(newsDataGridView);
            
            // Header Panel - Add AFTER DataGridView so it appears on top
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = secondaryBg
            };
            this.Controls.Add(headerPanel);
            
            // Title Label
            titleLabel = new Label
            {
                Text = "Latest News",
                Location = new Point(20, 18),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = textColor,
                AutoSize = false
            };
            headerPanel.Controls.Add(titleLabel);
            
            // Footer Panel - Add AFTER DataGridView so it appears on top
            footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = secondaryBg
            };
            this.Controls.Add(footerPanel);
            
            // Status Label
            statusLabel = new Label
            {
                Location = new Point(20, 15),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(150, 150, 150),
                Text = "Ready",
                AutoSize = false
            };
            footerPanel.Controls.Add(statusLabel);
            
            // Refresh Button
            refreshButton = new Button
            {
                Location = new Point(670, 10),
                Size = new Size(100, 30),
                Text = "Refresh",
                Font = new Font("Segoe UI", 10),
                BackColor = accentColor,
                ForeColor = textColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.Click += RefreshButton_Click;
            footerPanel.Controls.Add(refreshButton);
            
            // Close Button
            closeButton = new Button
            {
                Location = new Point(780, 10),
                Size = new Size(100, 30),
                Text = "Close",
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(60, 63, 65),
                ForeColor = textColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += CloseButton_Click;
            footerPanel.Controls.Add(closeButton);
        }
    }
}

