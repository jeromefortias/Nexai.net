namespace Musuko.Kaonashi
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Diagnostics;


    public partial class NewsViewerForm : Form
    {
        private NewsService newsService;
        private List<News> newsList = new List<News>();

        public NewsViewerForm(string completionHost, int completionPort)
        {
            newsService = new NewsService(completionHost, completionPort);
            InitializeComponent();
            IconHelper.SetFormIcon(this);
            _ = LoadNewsAsync();
        }

        private async System.Threading.Tasks.Task LoadNewsAsync()
        {
            try
            {
                statusLabel.Text = "Loading news...";
                var news = await newsService.GetNewsAsync("test");
                if (news != null && news.Count > 0)
                {
                    newsList = news;
                    PopulateDataGridView();
                    statusLabel.Text = $"Loaded {news.Count} news items";
                }
                else
                {
                    statusLabel.Text = "No news available";
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Error loading news: {ex.Message}";
                MessageBox.Show($"Failed to load news: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateDataGridView()
        {
            newsDataGridView.Rows.Clear();
            
            foreach (var news in newsList)
            {
                int rowIndex = newsDataGridView.Rows.Add();
                var row = newsDataGridView.Rows[rowIndex];
                
                // Set cell values
                row.Cells["TitleColumn"].Value = news.Title;
                row.Cells["RatingColumn"].Value = news.Rating;
                row.Cells["UrlColumn"].Value = news.Url;
                
                // Color-code rating cell
                var ratingCell = row.Cells["RatingColumn"];
                if (news.Rating > 0)
                {
                    ratingCell.Style.BackColor = Color.FromArgb(67, 181, 129); // Green
                    ratingCell.Style.ForeColor = Color.White;
                }
                else if (news.Rating < 0)
                {
                    ratingCell.Style.BackColor = Color.FromArgb(237, 66, 69); // Red
                    ratingCell.Style.ForeColor = Color.White;
                }
                
                // Store the news object in the Tag for easy access
                row.Tag = news;
            }
        }

        private void NewsDataGridView_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            // Ignore header row clicks
            if (e.RowIndex < 0)
                return;

            var row = newsDataGridView.Rows[e.RowIndex];
            if (row.Tag is News news)
            {
                // Open URL if not empty
                if (!string.IsNullOrWhiteSpace(news.Url))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = news.Url,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not open URL: {news.Url}\n\nError: {ex.Message}", 
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("This news item does not have a URL.", "No URL", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void RefreshButton_Click(object? sender, EventArgs e)
        {
            _ = LoadNewsAsync();
        }

        private void CloseButton_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}

