using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace Musuko.Kaonashi
{
    public partial class CvRewriteForm : Form
    {
        private Cv _currentCv;
        private string _selectedModel;

        public CvRewriteForm(Cv cv, string selectedModel)
        {
            _currentCv = cv;
            _selectedModel = selectedModel;
            InitializeComponent();
            InitializeCustomComponents();
            PopulateCvInfo();
        }

        private void InitializeCustomComponents()
        {
            // Form properties
            this.Text = "Clone CV with AI";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // CV Info Panel
            var cvInfoPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(560, 120),
                BorderStyle = BorderStyle.FixedSingle
            };

            var cvInfoLabel = new Label
            {
                Text = "Current CV Information:",
                Location = new Point(10, 10),
                Size = new Size(200, 20),
                Font = new Font("Tahoma", 9F, FontStyle.Bold)
            };

            var cvInfoTextBox = new TextBox
            {
                Name = "cvInfoTextBox",
                Location = new Point(10, 35),
                Size = new Size(540, 75),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Text = $"ID: {_currentCv.Id}\r\nTitle: {_currentCv.Title}\r\nName: {_currentCv.FirstName} {_currentCv.LastName}"
            };

            cvInfoPanel.Controls.Add(cvInfoLabel);
            cvInfoPanel.Controls.Add(cvInfoTextBox);

            // Prompt Panel
            var promptPanel = new Panel
            {
                Location = new Point(20, 160),
                Size = new Size(560, 200),
                BorderStyle = BorderStyle.FixedSingle
            };

            var promptLabel = new Label
            {
                Text = "AI Rewrite Prompt:",
                Location = new Point(10, 10),
                Size = new Size(200, 20),
                Font = new Font("Tahoma", 9F, FontStyle.Bold)
            };

            var promptTextBox = new TextBox
            {
                Name = "promptTextBox",
                Location = new Point(10, 35),
                Size = new Size(540, 150),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "Enter your prompt to rewrite the CV (e.g., 'Make it more technical', 'Focus on leadership skills', etc.)"
            };

            promptPanel.Controls.Add(promptLabel);
            promptPanel.Controls.Add(promptTextBox);

            // Model Info
            var modelLabel = new Label
            {
                Text = $"Using Model: {_selectedModel}",
                Location = new Point(20, 380),
                Size = new Size(300, 20),
                Font = new Font("Tahoma", 9F, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            // Buttons
            var okButton = new Button
            {
                Name = "okButton",
                Text = "Clone with AI",
                Location = new Point(400, 420),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 9F, FontStyle.Bold),
                ForeColor = Color.White
            };
            okButton.Click += OkButton_Click;

            var cancelButton = new Button
            {
                Name = "cancelButton",
                Text = "Cancel",
                Location = new Point(510, 420),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(231, 76, 60),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 9F, FontStyle.Bold),
                ForeColor = Color.White
            };
            cancelButton.Click += (s, e) => this.Close();

            // Add controls to form
            this.Controls.Add(cvInfoPanel);
            this.Controls.Add(promptPanel);
            this.Controls.Add(modelLabel);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

        private void PopulateCvInfo()
        {
            var cvInfoTextBox = this.Controls.Find("cvInfoTextBox", true)[0] as TextBox;
            if (cvInfoTextBox != null)
            {
                cvInfoTextBox.Text = $"ID: {_currentCv.Id}\r\nTitle: {_currentCv.Title}\r\nName: {_currentCv.FirstName} {_currentCv.LastName}\r\nEmail: {_currentCv.Email}";
            }
        }

        private async void OkButton_Click(object sender, EventArgs e)
        {
            var promptTextBox = this.Controls.Find("promptTextBox", true)[0] as TextBox;
            var okButton = this.Controls.Find("okButton", true)[0] as Button;

            if (string.IsNullOrWhiteSpace(promptTextBox.Text))
            {
                MessageBox.Show("Please enter a prompt for the AI rewrite.", "Missing Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                okButton.Enabled = false;
                okButton.Text = "Processing...";

                // Create rewrite request
                var rewriteRequest = new CvRewriteRequest
                {
                    id = _currentCv.Id,
                    prompt = promptTextBox.Text.Trim(),
                    model = _selectedModel
                };

                // Call the rewrite API
                var newCv = await CallRewriteApiAsync(rewriteRequest);

                if (newCv != null)
                {
                    // Open new CvManagerForm with the rewritten CV
                    var config = AppConfig.Load();
                    var newCvManagerForm = new CvManagerForm(config.CompletionHost, config.CompletionPort);
                    newCvManagerForm.LoadCvDirectly(newCv);
                    newCvManagerForm.Show();

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rewriting CV: {ex.Message}", "Rewrite Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                okButton.Enabled = true;
                okButton.Text = "Clone with AI";
            }
        }

        private async Task<Cv> CallRewriteApiAsync(CvRewriteRequest request)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Get completion host and port from config
                var config = AppConfig.Load();
                var response = await client.PostAsync($"http://{config.CompletionHost}:{config.CompletionPort}/cv/rewrite", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Cv>(responseContent);
                }
                else
                {
                    throw new Exception($"API call failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to call rewrite API: {ex.Message}", ex);
            }
        }
    }
}
