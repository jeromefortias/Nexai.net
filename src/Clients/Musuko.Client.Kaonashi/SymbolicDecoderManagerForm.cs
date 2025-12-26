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

    public partial class SymbolicDecoderManagerForm : Form
    {
        private readonly string completionHost;
        private readonly int completionPort;
        private readonly LogService logService;
        private readonly HttpClient httpClient;
        private SymbolicDecoder? currentDecoder;

        // Parameterless constructor for designer support
        public SymbolicDecoderManagerForm()
        {
            completionHost = "localhost";
            completionPort = 50824;
            logService = new LogService(completionHost, completionPort);
            httpClient = new HttpClient();
            InitializeComponent();
            IconHelper.SetFormIcon(this);
        }

        public SymbolicDecoderManagerForm(string host, int port)
        {
            completionHost = host;
            completionPort = port;
            logService = new LogService(host, port);
            httpClient = new HttpClient();
            InitializeComponent();
            IconHelper.SetFormIcon(this);
            
            if (!DesignMode)
            {
                _ = LogActionAsync("Symbolic Decoder Manager opened");
            }
        }

        private async Task LogActionAsync(string action)
        {
            try
            {
                await logService.LogAsync("SymbolicDecoderManager", action);
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
                UpdateStatus("Please enter search criteria (use * for last 10 decoders)", Color.Orange);
                return;
            }

            await SearchDecodersAsync(criteria);
        }

        private async Task SearchDecodersAsync(string criteria)
        {
            try
            {
                string statusMessage = criteria == "*" 
                    ? "Loading last 10 decoders..." 
                    : "Searching...";
                UpdateStatus(statusMessage, Color.DodgerBlue);
                searchButton.Enabled = false;

                var searchId = new SearchId
                {
                    Id = string.Empty,
                    Criteria = criteria
                };

                var url = $"http://{completionHost}:{completionPort}/symbolicdecoder/search";
                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var decoders = JsonConvert.DeserializeObject<List<SymbolicDecoder>>(responseBody);
                    
                    if (decoders != null && decoders.Count > 0)
                    {
                        decodersDataGrid.DataSource = decoders;
                        ConfigureDataGridColumns();
                        
                        string resultMessage = criteria == "*" 
                            ? $"Loaded last {decoders.Count} decoder(s)" 
                            : $"Found {decoders.Count} decoder(s)";
                        UpdateStatus(resultMessage, Color.Green);
                        
                        await LogActionAsync($"Search successful: {decoders.Count} decoders found for '{criteria}'");
                    }
                    else
                    {
                        decodersDataGrid.DataSource = null;
                        UpdateStatus("No decoders found", Color.Orange);
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
            if (decodersDataGrid.Columns["Id"] != null)
                decodersDataGrid.Columns["Id"]!.Visible = false;

            if (decodersDataGrid.Columns["Date"] != null)
            {
                decodersDataGrid.Columns["Date"]!.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                decodersDataGrid.Columns["Date"]!.Width = 130;
            }

            if (decodersDataGrid.Columns["Name"] != null)
                decodersDataGrid.Columns["Name"]!.Width = 200;

            if (decodersDataGrid.Columns["SystemPrompt"] != null)
                decodersDataGrid.Columns["SystemPrompt"]!.Width = 300;
        }

        private async void DecodersDataGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var decoder = decodersDataGrid.Rows[e.RowIndex].DataBoundItem as SymbolicDecoder;
                if (decoder != null)
                {
                    await LoadDecoderDetailsAsync(decoder.Id);
                }
            }
        }

        private async Task LoadDecoderDetailsAsync(string decoderId)
        {
            try
            {
                UpdateStatus("Loading decoder details...", Color.DodgerBlue);

                var searchId = new SearchId
                {
                    Id = decoderId,
                    Criteria = string.Empty
                };

                var url = $"http://{completionHost}:{completionPort}/symbolicdecoder/load";
                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    currentDecoder = JsonConvert.DeserializeObject<SymbolicDecoder>(responseBody);
                    
                    if (currentDecoder != null)
                    {
                        PopulateDecoderDetails(currentDecoder);
                        UpdateStatus("Decoder loaded", Color.Green);
                        await LogActionAsync($"Decoder loaded: {currentDecoder.Name}");
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
                UpdateStatus($"Error loading decoder: {ex.Message}", Color.Red);
                await LogActionAsync($"Load error: {ex.Message}");
            }
        }

        private void PopulateDecoderDetails(SymbolicDecoder decoder)
        {
            // Display decoder ID
            decoderIdLabel.Text = $"[ID: {decoder.Id}]";
            
            nameTextBox.Text = decoder.Name;
            systemPromptTextBox.Text = decoder.SystemPrompt;
            userPromptTextBox.Text = decoder.UserPrompt;
            preCompletionTextBox.Text = decoder.PreCompletion;
            postCompletionTextBox.Text = decoder.PostCompletion;
            
            // List fields - convert to comma-separated strings
            shouldTextBox.Text = string.Join(", ", decoder.Should ?? new List<string>());
            shouldNotTextBox.Text = string.Join(", ", decoder.ShouldNot ?? new List<string>());
            mustTextBox.Text = string.Join(", ", decoder.Must ?? new List<string>());
            mustNotTextBox.Text = string.Join(", ", decoder.MustNot ?? new List<string>());

            detailsPanel.Enabled = true;
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            if (currentDecoder == null)
            {
                UpdateStatus("No decoder selected", Color.Orange);
                return;
            }

            await SaveDecoderAsync();
        }

        private async Task SaveDecoderAsync()
        {
            try
            {
                UpdateStatus("Saving decoder...", Color.DodgerBlue);
                saveButton.Enabled = false;

                // Update current decoder with form values
                currentDecoder!.Name = nameTextBox.Text?.Trim() ?? string.Empty;
                currentDecoder.SystemPrompt = systemPromptTextBox.Text?.Trim() ?? string.Empty;
                currentDecoder.UserPrompt = userPromptTextBox.Text?.Trim() ?? string.Empty;
                currentDecoder.PreCompletion = preCompletionTextBox.Text?.Trim() ?? string.Empty;
                currentDecoder.PostCompletion = postCompletionTextBox.Text?.Trim() ?? string.Empty;
                
                // Convert comma-separated strings back to lists
                currentDecoder.Should = shouldTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentDecoder.ShouldNot = shouldNotTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentDecoder.Must = mustTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentDecoder.MustNot = mustNotTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                currentDecoder.Date = DateTime.Now;

                var url = $"http://{completionHost}:{completionPort}/symbolicdecoder/save";
                var json = JsonConvert.SerializeObject(currentDecoder, Formatting.Indented);
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
                        UpdateStatus($"Decoder saved successfully: {serviceReturn.Message}", Color.Green);
                        await LogActionAsync($"Decoder saved: {currentDecoder.Name}");
                        
                        // Refresh search results if there was a previous search
                        if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
                        {
                            await SearchDecodersAsync(searchTextBox.Text.Trim());
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
                UpdateStatus($"Error saving decoder: {ex.Message}", Color.Red);
                await LogActionAsync($"Save error: {ex.Message}");
            }
            finally
            {
                saveButton.Enabled = true;
            }
        }

        private void NewButton_Click(object? sender, EventArgs e)
        {
            currentDecoder = new SymbolicDecoder
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName
            };

            PopulateDecoderDetails(currentDecoder);
            UpdateStatus("New decoder created", Color.Green);
        }

        private void CloneButton_Click(object? sender, EventArgs e)
        {
            if (currentDecoder == null)
            {
                UpdateStatus("No decoder to clone", Color.Orange);
                return;
            }

            // Create a clone with a new ID and updated date
            var clonedDecoder = new SymbolicDecoder
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                Name = currentDecoder.Name + " (Copy)",
                SystemPrompt = currentDecoder.SystemPrompt,
                UserPrompt = currentDecoder.UserPrompt,
                PreCompletion = currentDecoder.PreCompletion,
                PostCompletion = currentDecoder.PostCompletion,
                Should = new List<string>(currentDecoder.Should ?? new List<string>()),
                ShouldNot = new List<string>(currentDecoder.ShouldNot ?? new List<string>()),
                Must = new List<string>(currentDecoder.Must ?? new List<string>()),
                MustNot = new List<string>(currentDecoder.MustNot ?? new List<string>())
            };

            currentDecoder = clonedDecoder;
            PopulateDecoderDetails(currentDecoder);
            UpdateStatus("Decoder cloned successfully", Color.Green);
        }

        private void NewWindowButton_Click(object? sender, EventArgs e)
        {
            var newWindow = new SymbolicDecoderManagerForm(completionHost, completionPort);
            newWindow.Show();
            UpdateStatus("New window opened", Color.Green);
        }

        private void SaveAsButton_Click(object? sender, EventArgs e)
        {
            if (currentDecoder == null)
            {
                UpdateStatus("No decoder to save", Color.Orange);
                return;
            }

            try
            {
                // Collect data from form
                var decoderToSave = new SymbolicDecoder
                {
                    Id = string.Empty, // Set ID to empty as requested
                    Date = DateTime.Now,
                    MachineName = Environment.MachineName,
                    UserName = Environment.UserName,
                    Name = nameTextBox.Text?.Trim() ?? "",
                    SystemPrompt = systemPromptTextBox.Text?.Trim() ?? "",
                    UserPrompt = userPromptTextBox.Text?.Trim() ?? "",
                    PreCompletion = preCompletionTextBox.Text?.Trim() ?? "",
                    PostCompletion = postCompletionTextBox.Text?.Trim() ?? "",
                    Should = shouldTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    ShouldNot = shouldNotTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    Must = mustTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    MustNot = mustNotTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>()
                };

                // Open SaveFileDialog
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Save Decoder As JSON";
                    saveFileDialog.DefaultExt = "json";
                    
                    // Suggest filename based on decoder name
                    string suggestedFileName = string.IsNullOrWhiteSpace(decoderToSave.Name) 
                        ? "decoder.json" 
                        : $"{decoderToSave.Name.Replace(" ", "_")}.json";
                    saveFileDialog.FileName = suggestedFileName;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Serialize decoder to JSON
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(decoderToSave, Newtonsoft.Json.Formatting.Indented);
                        
                        // Save to file
                        System.IO.File.WriteAllText(saveFileDialog.FileName, json);
                        
                        UpdateStatus($"Decoder saved to: {System.IO.Path.GetFileName(saveFileDialog.FileName)}", Color.Green);
                        logService.Log("OK", $"Decoder saved as JSON: {saveFileDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error saving decoder: {ex.Message}", Color.Red);
                logService.Log("ERROR", $"Error saving decoder as JSON: {ex.Message}");
                MessageBox.Show($"Failed to save decoder:\n{ex.Message}", "Save Error", 
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
                    openFileDialog.Title = "Load Decoder From JSON File";
                    openFileDialog.DefaultExt = "json";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Read file content
                        string jsonContent = System.IO.File.ReadAllText(openFileDialog.FileName);
                        
                        // Deserialize decoder from JSON
                        var loadedDecoder = Newtonsoft.Json.JsonConvert.DeserializeObject<SymbolicDecoder>(jsonContent);
                        
                        if (loadedDecoder == null)
                        {
                            UpdateStatus("Failed to load decoder: Invalid JSON format", Color.Red);
                            MessageBox.Show("The selected file does not contain a valid decoder.", 
                                          "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Always generate a new ID when loading from file
                        // This ensures loaded decoders are treated as new/cloned decoders
                        loadedDecoder.Id = Guid.NewGuid().ToString();
                        
                        // Update metadata
                        loadedDecoder.Date = DateTime.Now;
                        loadedDecoder.MachineName = Environment.MachineName;
                        loadedDecoder.UserName = Environment.UserName;

                        // Update current decoder
                        currentDecoder = loadedDecoder;
                        
                        // Populate the form with loaded data
                        PopulateDecoderDetails(loadedDecoder);
                        
                        UpdateStatus($"Decoder loaded from: {System.IO.Path.GetFileName(openFileDialog.FileName)} (New ID generated)", Color.Green);
                        logService.Log("OK", $"Decoder loaded from JSON with new ID: {openFileDialog.FileName}");
                    }
                }
            }
            catch (Newtonsoft.Json.JsonException jsonEx)
            {
                UpdateStatus($"Invalid JSON file: {jsonEx.Message}", Color.Red);
                logService.Log("ERROR", $"JSON parse error loading decoder: {jsonEx.Message}");
                MessageBox.Show($"Failed to parse JSON file:\n{jsonEx.Message}", 
                               "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading decoder: {ex.Message}", Color.Red);
                logService.Log("ERROR", $"Error loading decoder from JSON: {ex.Message}");
                MessageBox.Show($"Failed to load decoder:\n{ex.Message}", 
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
