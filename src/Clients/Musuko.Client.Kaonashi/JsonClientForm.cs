namespace Musuko.Kaonashi
{
    using System;
    using System.Drawing;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class JsonClientForm : Form
    {
        private Panel headerPanel;
        private Panel requestPanel;
        private Panel responsePanel;
        private Label titleLabel;
        private Label urlLabel;
        private TextBox urlTextBox;
        private ComboBox methodComboBox;
        private Label requestBodyLabel;
        private RichTextBox requestBodyTextBox;
        private Label responseLabel;
        private RichTextBox responseTextBox;
        private Button sendButton;
        private Button clearButton;
        private Button formatRequestButton;
        private Button formatResponseButton;
        private Button saveRequestAsButton;
        private Button loadRequestButton;
        private Button saveResponseAsButton;
        private Panel footerPanel;
        private Label statusLabel;
        private readonly HttpClient httpClient;
        private readonly LogService logService;

        public JsonClientForm()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            var config = AppConfig.Load();
            logService = new LogService(config.CompletionHost, config.CompletionPort);
            InitializeComponent();
        }

        private async void SendButton_Click(object? sender, EventArgs e)
        {
            string url = urlTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Please enter a URL", "Missing URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                sendButton.Enabled = false;
                string method = methodComboBox.SelectedItem?.ToString() ?? "GET";
                statusLabel.Text = $"? Sending {method} request to: {url}";
                statusLabel.ForeColor = Color.FromArgb(255, 165, 0);

                string requestBody = requestBodyTextBox.Text.Trim();

                HttpResponseMessage response;
                DateTime startTime = DateTime.Now;

                if (method == "GET")
                {
                    response = await httpClient.GetAsync(url);
                }
                else if (method == "POST")
                {
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(url, content);
                }
                else if (method == "PUT")
                {
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    response = await httpClient.PutAsync(url, content);
                }
                else if (method == "DELETE")
                {
                    response = await httpClient.DeleteAsync(url);
                }
                else if (method == "PATCH")
                {
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                    {
                        Content = content
                    };
                    response = await httpClient.SendAsync(request);
                }
                else
                {
                    response = await httpClient.GetAsync(url);
                }

                TimeSpan elapsed = DateTime.Now - startTime;

                string responseBody = await response.Content.ReadAsStringAsync();
                
                // Try to format as JSON if possible
                try
                {
                    var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                    responseBody = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                }
                catch
                {
                    // Not valid JSON, display as-is
                }

                responseTextBox.Text = responseBody;
                ApplyJsonSyntaxHighlighting(responseTextBox);

                string statusIcon = response.IsSuccessStatusCode ? "?" : "??";
                statusLabel.Text = $"{statusIcon} {method} {url} | Status: {(int)response.StatusCode} {response.ReasonPhrase} | ? {elapsed.TotalMilliseconds:F0}ms";
                statusLabel.ForeColor = response.IsSuccessStatusCode ? Color.FromArgb(67, 181, 129) : Color.FromArgb(220, 80, 80);
                
                if (response.IsSuccessStatusCode)
                {
                    logService.Log("INFO", $"REST {method} to {url} - Status: {(int)response.StatusCode}, Time: {elapsed.TotalMilliseconds:F0}ms");
                }
                else
                {
                    logService.Log("WARNING", $"REST {method} to {url} - Status: {(int)response.StatusCode} {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                responseTextBox.Text = $"? Error: {ex.Message}\n\n{ex.StackTrace}";
                responseTextBox.ForeColor = Color.FromArgb(220, 80, 80);
                statusLabel.Text = $"? Failed to call {url} | Error: {ex.Message}";
                statusLabel.ForeColor = Color.FromArgb(220, 80, 80);
                logService.Log("ERROR", $"REST {methodComboBox.SelectedItem?.ToString() ?? "GET"} to {url} failed: {ex.Message}");
            }
            finally
            {
                sendButton.Enabled = true;
            }
        }

        private void FormatRequestButton_Click(object? sender, EventArgs e)
        {
            try
            {
                string text = requestBodyTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(text)) return;

                var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(text);
                string formatted = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                requestBodyTextBox.Text = formatted;
                ApplyJsonSyntaxHighlighting(requestBodyTextBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Invalid JSON: {ex.Message}", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatResponseButton_Click(object? sender, EventArgs e)
        {
            try
            {
                string text = responseTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(text)) return;

                var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(text);
                string formatted = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                responseTextBox.Text = formatted;
                ApplyJsonSyntaxHighlighting(responseTextBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Invalid JSON: {ex.Message}", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            requestBodyTextBox.Clear();
            responseTextBox.Clear();
            urlTextBox.Clear();
            statusLabel.Text = "? Ready to send requests";
            statusLabel.ForeColor = Color.FromArgb(180, 180, 180);
        }

        private void SaveRequestAsButton_Click(object? sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON Request Files (*.jsonreq)|*.jsonreq|All Files (*.*)|*.*";
                    saveFileDialog.DefaultExt = "jsonreq";
                    saveFileDialog.FileName = "request.jsonreq";
                    saveFileDialog.Title = "Save Request";
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Create request object
                        var request = new
                        {
                            url = urlTextBox.Text,
                            method = methodComboBox.SelectedItem?.ToString() ?? "GET",
                            body = requestBodyTextBox.Text
                        };

                        // Serialize to JSON with formatting
                        string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(request, Newtonsoft.Json.Formatting.Indented);

                        // Write to file
                        System.IO.File.WriteAllText(saveFileDialog.FileName, jsonContent);

                        // Set file to read-only
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(saveFileDialog.FileName);
                        fileInfo.IsReadOnly = true;

                        statusLabel.Text = $"?? Request saved to: {System.IO.Path.GetFileName(saveFileDialog.FileName)}";
                        statusLabel.ForeColor = Color.FromArgb(67, 181, 129);

                        // Visual feedback
                        saveRequestAsButton.Text = "? Saved!";
                        var timer = new System.Windows.Forms.Timer();
                        timer.Interval = 2000;
                        timer.Tick += (s, args) =>
                        {
                            saveRequestAsButton.Text = "?? Save";
                            timer.Stop();
                            timer.Dispose();
                        };
                        timer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save request: {ex.Message}", "Save Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRequestButton_Click(object? sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "JSON Request Files (*.jsonreq)|*.jsonreq|All Files (*.*)|*.*";
                    openFileDialog.Title = "Load Request";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Read file content
                        string fileContent = System.IO.File.ReadAllText(openFileDialog.FileName);

                        // Deserialize JSON
                        var request = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(fileContent);

                        // Load into form
                        if (request != null)
                        {
                            if (request.url != null)
                                urlTextBox.Text = request.url.ToString();

                            if (request.method != null)
                            {
                                string method = request.method.ToString();
                                int index = methodComboBox.Items.IndexOf(method);
                                if (index >= 0)
                                    methodComboBox.SelectedIndex = index;
                            }

                            if (request.body != null)
                            {
                                requestBodyTextBox.Text = request.body.ToString();
                                ApplyJsonSyntaxHighlighting(requestBodyTextBox);
                            }

                            statusLabel.Text = $"?? Request loaded from: {System.IO.Path.GetFileName(openFileDialog.FileName)}";
                            statusLabel.ForeColor = Color.FromArgb(255, 165, 0);

                            // Visual feedback
                            loadRequestButton.Text = "? Loaded!";
                            var timer = new System.Windows.Forms.Timer();
                            timer.Interval = 2000;
                            timer.Tick += (s, args) =>
                            {
                                loadRequestButton.Text = "?? Load";
                                timer.Stop();
                                timer.Dispose();
                            };
                            timer.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load request: {ex.Message}", "Load Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveResponseAsButton_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(responseTextBox.Text))
                {
                    MessageBox.Show("No response to save!", "Info", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                    saveFileDialog.DefaultExt = "json";
                    saveFileDialog.FileName = $"response_{DateTime.Now:yyyy-MM-dd_HHmmss}.json";
                    saveFileDialog.Title = "Save Response As";
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(saveFileDialog.FileName, responseTextBox.Text);
                        
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(saveFileDialog.FileName);
                        fileInfo.IsReadOnly = true;
                        
                        statusLabel.Text = $"?? Response saved to: {System.IO.Path.GetFileName(saveFileDialog.FileName)}";
                        statusLabel.ForeColor = Color.FromArgb(156, 120, 206);
                        
                        saveResponseAsButton.Text = "? Saved!";
                        var originalColor = saveResponseAsButton.BackColor;
                        saveResponseAsButton.BackColor = Color.FromArgb(67, 181, 129);
                        
                        var timer = new System.Windows.Forms.Timer();
                        timer.Interval = 2000;
                        timer.Tick += (s, args) =>
                        {
                            saveResponseAsButton.Text = "?? Save as";
                            saveResponseAsButton.BackColor = originalColor;
                            timer.Stop();
                            timer.Dispose();
                        };
                        timer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save response: {ex.Message}", "Save Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyJsonSyntaxHighlighting(RichTextBox textBox)
        {
            int currentPosition = textBox.SelectionStart;
            textBox.SelectAll();
            textBox.SelectionColor = Color.White;

            // Highlight strings (property names and values)
            HighlightPattern(textBox, @"""[^""\\]*(?:\\.[^""\\]*)*"":", Color.FromArgb(156, 220, 254)); // Property names
            HighlightPattern(textBox, @"""[^""\\]*(?:\\.[^""\\]*)*""", Color.FromArgb(206, 145, 120)); // String values

            // Highlight numbers
            HighlightPattern(textBox, @"\b\d+\.?\d*\b", Color.FromArgb(181, 206, 168));

            // Highlight booleans and null
            HighlightPattern(textBox, @"\b(true|false|null)\b", Color.FromArgb(86, 156, 214));

            // Highlight brackets and braces
            HighlightPattern(textBox, @"[\{\}\[\]]", Color.FromArgb(255, 215, 0));

            textBox.SelectionStart = currentPosition;
            textBox.SelectionLength = 0;
        }

        private void HighlightPattern(RichTextBox textBox, string pattern, Color color)
        {
            Regex regex = new Regex(pattern);
            foreach (Match match in regex.Matches(textBox.Text))
            {
                textBox.Select(match.Index, match.Length);
                textBox.SelectionColor = color;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                httpClient?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

