namespace Musuko.Kaonashi
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Newtonsoft.Json;

    public partial class SymbolicEncoderManagerForm : Form
    {
        private readonly string completionHost;
        private readonly int completionPort;
        private readonly LogService logService;
        private readonly HttpClient httpClient;
        private SymbolicEncoder? currentEncoder;

        // Parameterless constructor for designer support
        public SymbolicEncoderManagerForm()
        {
            completionHost = "localhost";
            completionPort = 50824;
            logService = new LogService(completionHost, completionPort);
            httpClient = new HttpClient();
            InitializeComponent();
            IconHelper.SetFormIcon(this);
        }

        public SymbolicEncoderManagerForm(string host, int port)
        {
            completionHost = host;
            completionPort = port;
            logService = new LogService(host, port);
            httpClient = new HttpClient();
            InitializeComponent();
            IconHelper.SetFormIcon(this);
            
            if (!DesignMode)
            {
                _ = LogActionAsync("Symbolic Encoder Manager opened");
                InitializeForm();
            }
        }

        private void InitializeForm()
        {
            // Ensure the New button is always enabled and visible
            newButton.Enabled = true;
            newButton.Visible = true;
            
            // Set initial status
            UpdateStatus("Ready - Click 'New' to create a new encoder", Color.Blue);
        }

        private async Task LogActionAsync(string action)
        {
            try
            {
                await logService.LogAsync("SymbolicEncoderManager", action);
            }
            catch
            {
                // Silently fail if logging is unavailable
            }
        }

        private async void SearchButton_Click(object? sender, EventArgs e)
        {
            var criteria = searchTextBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(criteria))
            {
                UpdateStatus("Please enter search criteria (use * for last 10 encoders)", Color.Orange);
                return;
            }

            await SearchEncodersAsync(criteria);
        }

        private async Task SearchEncodersAsync(string criteria)
        {
            try
            {
                string statusMessage = criteria == "*" 
                    ? "Loading last 10 encoders..." 
                    : "Searching...";
                UpdateStatus(statusMessage, Color.DodgerBlue);
                searchButton.Enabled = false;

                var searchId = new SearchId
                {
                    Id = string.Empty,
                    Criteria = criteria
                };

                var url = $"http://{completionHost}:{completionPort}/symbolicencoder/search";
                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var encoders = JsonConvert.DeserializeObject<List<SymbolicEncoder>>(responseBody);
                    
                    if (encoders != null && encoders.Count > 0)
                    {
                        encodersDataGrid.DataSource = encoders;
                        ConfigureDataGridColumns();
                        
                        string resultMessage = criteria == "*" 
                            ? $"Loaded last {encoders.Count} encoder(s)" 
                            : $"Found {encoders.Count} encoder(s)";
                        UpdateStatus(resultMessage, Color.Green);
                        
                        await LogActionAsync($"Search successful: {encoders.Count} encoders found for '{criteria}'");
                    }
                    else
                    {
                        encodersDataGrid.DataSource = null;
                        UpdateStatus("No encoders found", Color.Orange);
                        await LogActionAsync($"Search returned no results for '{criteria}'");
                    }
                }
                else
                {
                    UpdateStatus($"Search failed: {response.StatusCode}", Color.Red);
                    await LogActionAsync($"Search failed: {response.StatusCode} - {responseBody?.Substring(0, Math.Min(100, responseBody.Length))}");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
                await LogActionAsync($"Search error: {ex.Message}");
            }
            finally
            {
                searchButton.Enabled = true;
            }
        }

        private void ConfigureDataGridColumns()
        {
            if (encodersDataGrid.Columns["Id"] != null)
                encodersDataGrid.Columns["Id"]!.Visible = false;

            if (encodersDataGrid.Columns["Date"] != null)
            {
                encodersDataGrid.Columns["Date"]!.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                encodersDataGrid.Columns["Date"]!.Width = 130;
            }

            if (encodersDataGrid.Columns["Name"] != null)
                encodersDataGrid.Columns["Name"]!.Width = 200;

            if (encodersDataGrid.Columns["SystemPrompt"] != null)
                encodersDataGrid.Columns["SystemPrompt"]!.Width = 300;
        }

        private async void EncodersDataGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var encoder = encodersDataGrid.Rows[e.RowIndex].DataBoundItem as SymbolicEncoder;
                if (encoder != null)
                {
                    await LoadEncoderDetailsAsync(encoder.Id);
                }
            }
        }

        private async Task LoadEncoderDetailsAsync(string encoderId)
        {
            try
            {
                UpdateStatus("Loading encoder details...", Color.DodgerBlue);

                var searchId = new SearchId
                {
                    Id = encoderId,
                    Criteria = string.Empty
                };

                var url = $"http://{completionHost}:{completionPort}/symbolicencoder/load";
                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    currentEncoder = JsonConvert.DeserializeObject<SymbolicEncoder>(responseBody);
                    
                    if (currentEncoder != null)
                    {
                        PopulateEncoderDetails(currentEncoder);
                        UpdateStatus("Encoder loaded", Color.Green);
                        await LogActionAsync($"Encoder loaded: {currentEncoder.Name}");
                    }
                }
                else
                {
                    UpdateStatus($"Load failed: {response.StatusCode}", Color.Red);
                    await LogActionAsync($"Load failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading encoder: {ex.Message}", Color.Red);
                await LogActionAsync($"Load error: {ex.Message}");
            }
        }

        private void PopulateEncoderDetails(SymbolicEncoder encoder)
        {
            // Display encoder ID
            encoderIdLabel.Text = $"[ID: {encoder.Id}]";
            
            nameTextBox.Text = encoder.Name;
            systemPromptTextBox.Text = encoder.SystemPrompt;
            
            // Populate list fields
            regexTextBox.Text = string.Join("\r\n", encoder.Regex ?? new List<string>());
            termsTextBox.Text = string.Join("\r\n", encoder.Terms ?? new List<string>());
            shouldTextBox.Text = string.Join("\r\n", encoder.Should ?? new List<string>());
            shouldNotTextBox.Text = string.Join("\r\n", encoder.ShouldNot ?? new List<string>());
            mustTextBox.Text = string.Join("\r\n", encoder.Must ?? new List<string>());
            mustNotTextBox.Text = string.Join("\r\n", encoder.MustNot ?? new List<string>());
            
            commentTextBox.Text = encoder.Comment;

            // Enable the form first, then explicitly enable the save button
            detailsPanel.Enabled = true;
            saveButton.Enabled = true;
            
            // Force refresh to ensure the UI updates
            detailsPanel.Refresh();
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            if (currentEncoder == null)
            {
                UpdateStatus("No encoder selected", Color.Orange);
                return;
            }

            await SaveEncoderAsync();
        }

        private async Task SaveEncoderAsync()
        {
            try
            {
                UpdateStatus("Saving encoder...", Color.DodgerBlue);
                saveButton.Enabled = false;

                // Update current encoder with form values
                currentEncoder!.Name = nameTextBox.Text?.Trim() ?? string.Empty;
                currentEncoder.SystemPrompt = systemPromptTextBox.Text?.Trim() ?? string.Empty;
                
                // Update list fields
                currentEncoder.Regex = regexTextBox.Text?
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                currentEncoder.Terms = termsTextBox.Text?
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                currentEncoder.Should = shouldTextBox.Text?
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                currentEncoder.ShouldNot = shouldNotTextBox.Text?
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                currentEncoder.Must = mustTextBox.Text?
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                currentEncoder.MustNot = mustNotTextBox.Text?
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                currentEncoder.Comment = commentTextBox.Text?.Trim() ?? string.Empty;
                currentEncoder.Date = DateTime.Now;

                var url = $"http://{completionHost}:{completionPort}/symbolicencoder/save";
                var json = JsonConvert.SerializeObject(currentEncoder, Formatting.Indented);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                UpdateStatus($"Sending POST to: {url}", Color.Blue);
                
                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                UpdateStatus($"Response: {response.StatusCode} - Body: {(responseBody != null ? responseBody.Substring(0, Math.Min(100, responseBody.Length)) : "null")}", Color.Blue);

                if (response.IsSuccessStatusCode)
                {
                    var serviceReturn = JsonConvert.DeserializeObject<ServiceReturn>(responseBody);
                    
                    if (serviceReturn != null && serviceReturn.Success)
                    {
                        UpdateStatus($"Encoder saved successfully: {serviceReturn.Message}", Color.Green);
                        await LogActionAsync($"Encoder saved: {currentEncoder.Name}");
                        
                        // Refresh search results if there was a previous search
                        if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
                        {
                            await SearchEncodersAsync(searchTextBox.Text.Trim());
                        }
                    }
                    else
                    {
                        UpdateStatus($"Save failed: {serviceReturn?.Message ?? "Unknown error"}", Color.Red);
                        await LogActionAsync($"Save failed: {serviceReturn?.Message}");
                    }
                }
                else
                {
                    UpdateStatus($"Save failed: {response.StatusCode}", Color.Red);
                    await LogActionAsync($"Save failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error saving encoder: {ex.Message}", Color.Red);
                await LogActionAsync($"Save error: {ex.Message}");
            }
            finally
            {
                saveButton.Enabled = true;
            }
        }

        private void NewButton_Click(object? sender, EventArgs e)
        {
            currentEncoder = new SymbolicEncoder
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName
            };

            PopulateEncoderDetails(currentEncoder);
            UpdateStatus("New encoder created - Form is now editable", Color.Green);
        }

        private void CloneButton_Click(object? sender, EventArgs e)
        {
            if (currentEncoder == null)
            {
                UpdateStatus("No encoder to clone", Color.Orange);
                return;
            }

            // Create a clone with a new ID and updated date
            var clonedEncoder = new SymbolicEncoder
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                Name = currentEncoder.Name + " (Copy)",
                SystemPrompt = currentEncoder.SystemPrompt,
                Regex = new List<string>(currentEncoder.Regex ?? new List<string>()),
                Terms = new List<string>(currentEncoder.Terms ?? new List<string>()),
                Should = new List<string>(currentEncoder.Should ?? new List<string>()),
                ShouldNot = new List<string>(currentEncoder.ShouldNot ?? new List<string>()),
                Must = new List<string>(currentEncoder.Must ?? new List<string>()),
                MustNot = new List<string>(currentEncoder.MustNot ?? new List<string>()),
                Comment = currentEncoder.Comment
            };

            currentEncoder = clonedEncoder;
            PopulateEncoderDetails(currentEncoder);
            UpdateStatus("Encoder cloned successfully", Color.Green);
        }

        private void NewWindowButton_Click(object? sender, EventArgs e)
        {
            var newWindow = new SymbolicEncoderManagerForm(completionHost, completionPort);
            newWindow.Show();
            UpdateStatus("New window opened", Color.Green);
        }

        private void SaveAsButton_Click(object? sender, EventArgs e)
        {
            if (currentEncoder == null)
            {
                UpdateStatus("No encoder to save", Color.Orange);
                return;
            }

            try
            {
                // Collect data from form
                var encoderToSave = new SymbolicEncoder
                {
                    Id = string.Empty, // Set ID to empty as requested
                    Date = DateTime.Now,
                    MachineName = Environment.MachineName,
                    UserName = Environment.UserName,
                    Name = nameTextBox.Text?.Trim() ?? "",
                    SystemPrompt = systemPromptTextBox.Text?.Trim() ?? "",
                    Regex = regexTextBox.Text?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    Terms = termsTextBox.Text?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    Should = shouldTextBox.Text?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    ShouldNot = shouldNotTextBox.Text?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    Must = mustTextBox.Text?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    MustNot = mustNotTextBox.Text?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    Comment = commentTextBox.Text?.Trim() ?? ""
                };

                // Open SaveFileDialog
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Save Symbolic Encoder As JSON";
                    saveFileDialog.DefaultExt = "json";
                    
                    // Suggest filename based on encoder name
                    string suggestedFileName = string.IsNullOrWhiteSpace(encoderToSave.Name) 
                        ? "symbolic_encoder.json" 
                        : $"{encoderToSave.Name.Replace(" ", "_")}.json";
                    saveFileDialog.FileName = suggestedFileName;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Serialize encoder to JSON
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(encoderToSave, Newtonsoft.Json.Formatting.Indented);
                        
                        // Save to file
                        System.IO.File.WriteAllText(saveFileDialog.FileName, json);
                        
                        UpdateStatus($"Encoder saved to: {System.IO.Path.GetFileName(saveFileDialog.FileName)}", Color.Green);
                        logService.Log("OK", $"Encoder saved as JSON: {saveFileDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error saving encoder: {ex.Message}", Color.Red);
                logService.Log("ERROR", $"Error saving encoder as JSON: {ex.Message}");
                MessageBox.Show($"Failed to save encoder:\n{ex.Message}", "Save Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            try
            {
                // Open OpenFileDialog
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                    openFileDialog.Title = "Load Symbolic Encoder From JSON File";
                    openFileDialog.DefaultExt = "json";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Read file content
                        string jsonContent = System.IO.File.ReadAllText(openFileDialog.FileName);
                        
                        // Deserialize encoder from JSON
                        var loadedEncoder = Newtonsoft.Json.JsonConvert.DeserializeObject<SymbolicEncoder>(jsonContent);
                        
                        if (loadedEncoder == null)
                        {
                            UpdateStatus("Failed to load encoder: Invalid JSON format", Color.Red);
                            MessageBox.Show("The selected file does not contain a valid symbolic encoder.", 
                                          "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Always generate a new ID when loading from file
                        loadedEncoder.Id = Guid.NewGuid().ToString();
                        
                        // Update metadata
                        loadedEncoder.Date = DateTime.Now;
                        loadedEncoder.MachineName = Environment.MachineName;
                        loadedEncoder.UserName = Environment.UserName;

                        // Update current encoder
                        currentEncoder = loadedEncoder;
                        
                        // Populate the form with loaded data
                        PopulateEncoderDetails(loadedEncoder);
                        
                        UpdateStatus($"Encoder loaded from: {System.IO.Path.GetFileName(openFileDialog.FileName)} (New ID generated)", Color.Green);
                        logService.Log("OK", $"Encoder loaded from JSON with new ID: {openFileDialog.FileName}");
                    }
                }
            }
            catch (Newtonsoft.Json.JsonException jsonEx)
            {
                UpdateStatus($"Invalid JSON file: {jsonEx.Message}", Color.Red);
                logService.Log("ERROR", $"JSON parse error loading encoder: {jsonEx.Message}");
                MessageBox.Show($"Failed to parse JSON file:\n{jsonEx.Message}", 
                               "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading encoder: {ex.Message}", Color.Red);
                logService.Log("ERROR", $"Error loading encoder from JSON: {ex.Message}");
                MessageBox.Show($"Failed to load encoder:\n{ex.Message}", 
                               "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(string message, Color color)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(() => UpdateStatus(message, color));
                return;
            }

            statusLabel.Text = message;
            statusLabel.ForeColor = color;
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
