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

    public partial class SymbolicProcessorManagerForm : Form
    {
        private readonly string completionHost;
        private readonly int completionPort;
        private readonly LogService logService;
        private readonly HttpClient httpClient;
        private SymbolicProcessor? currentProcessor;

        // Parameterless constructor for designer support
        public SymbolicProcessorManagerForm()
        {
            completionHost = "localhost";
            completionPort = 50824;
            logService = new LogService(completionHost, completionPort);
            httpClient = new HttpClient();
            InitializeComponent();
            IconHelper.SetFormIcon(this);
        }

        public SymbolicProcessorManagerForm(string host, int port)
        {
            completionHost = host;
            completionPort = port;
            logService = new LogService(host, port);
            httpClient = new HttpClient();
            InitializeComponent();
            IconHelper.SetFormIcon(this);
            
            if (!DesignMode)
            {
                _ = LogActionAsync("Symbolic Processor Manager opened");
            }
        }

        private async Task LogActionAsync(string action)
        {
            try
            {
                await logService.LogAsync("SymbolicProcessorManager", action);
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
                UpdateStatus("Please enter search criteria (use * for last 10 processors)", Color.Orange);
                return;
            }

            await SearchProcessorsAsync(criteria);
        }

        private async Task SearchProcessorsAsync(string criteria)
        {
            try
            {
                string statusMessage = criteria == "*" 
                    ? "Loading last 10 processors..." 
                    : "Searching...";
                UpdateStatus(statusMessage, Color.DodgerBlue);
                searchButton.Enabled = false;

                var searchId = new SearchId
                {
                    Id = string.Empty,
                    Criteria = criteria
                };

                var url = $"http://{completionHost}:{completionPort}/symbolicprocessor/search";
                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var processors = JsonConvert.DeserializeObject<List<SymbolicProcessor>>(responseBody);
                    
                    if (processors != null && processors.Count > 0)
                    {
                        processorsDataGrid.DataSource = processors;
                        ConfigureDataGridColumns();
                        
                        string resultMessage = criteria == "*" 
                            ? $"Loaded last {processors.Count} processor(s)" 
                            : $"Found {processors.Count} processor(s)";
                        UpdateStatus(resultMessage, Color.Green);
                        
                        await LogActionAsync($"Search successful: {processors.Count} processors found for '{criteria}'");
                    }
                    else
                    {
                        processorsDataGrid.DataSource = null;
                        UpdateStatus("No processors found", Color.Orange);
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
            if (processorsDataGrid.Columns["Id"] != null)
                processorsDataGrid.Columns["Id"]!.Visible = false;

            if (processorsDataGrid.Columns["Date"] != null)
            {
                processorsDataGrid.Columns["Date"]!.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                processorsDataGrid.Columns["Date"]!.Width = 130;
            }

            if (processorsDataGrid.Columns["Name"] != null)
                processorsDataGrid.Columns["Name"]!.Width = 200;

            if (processorsDataGrid.Columns["SystemPrompt"] != null)
                processorsDataGrid.Columns["SystemPrompt"]!.Width = 300;
        }

        private async void ProcessorsDataGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var processor = processorsDataGrid.Rows[e.RowIndex].DataBoundItem as SymbolicProcessor;
                if (processor != null)
                {
                    await LoadProcessorDetailsAsync(processor.Id);
                }
            }
        }

        private async Task LoadProcessorDetailsAsync(string processorId)
        {
            try
            {
                UpdateStatus("Loading processor details...", Color.DodgerBlue);

                var searchId = new SearchId
                {
                    Id = processorId,
                    Criteria = string.Empty
                };

                var url = $"http://{completionHost}:{completionPort}/symbolicprocessor/load";
                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    currentProcessor = JsonConvert.DeserializeObject<SymbolicProcessor>(responseBody);
                    
                    if (currentProcessor != null)
                    {
                        PopulateProcessorDetails(currentProcessor);
                        UpdateStatus("Processor loaded", Color.Green);
                        await LogActionAsync($"Processor loaded: {currentProcessor.Name}");
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
                UpdateStatus($"Error loading processor: {ex.Message}", Color.Red);
                await LogActionAsync($"Load error: {ex.Message}");
            }
        }

        private void PopulateProcessorDetails(SymbolicProcessor processor)
        {
            // Display processor ID
            processorIdLabel.Text = $"[ID: {processor.Id}]";
            
            nameTextBox.Text = processor.Name;
            systemPromptTextBox.Text = processor.SystemPrompt;
            
            // List fields - convert to comma-separated strings
            inboundShouldTextBox.Text = string.Join(", ", processor.InboundShould ?? new List<string>());
            inboundShouldNotTextBox.Text = string.Join(", ", processor.InboundShouldNot ?? new List<string>());
            inboundMustTextBox.Text = string.Join(", ", processor.InboundMust ?? new List<string>());
            inboundMustNotTextBox.Text = string.Join(", ", processor.InboundMustNot ?? new List<string>());
            outboundShouldTextBox.Text = string.Join(", ", processor.OutbounShould ?? new List<string>());
            outboundShouldNotTextBox.Text = string.Join(", ", processor.OutboundShouldNot ?? new List<string>());
            outboundMustTextBox.Text = string.Join(", ", processor.OutboundMust ?? new List<string>());
            outboundMustNotTextBox.Text = string.Join(", ", processor.OutboundMustNot ?? new List<string>());

            detailsPanel.Enabled = true;
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            if (currentProcessor == null)
            {
                UpdateStatus("No processor selected", Color.Orange);
                return;
            }

            await SaveProcessorAsync();
        }

        private async Task SaveProcessorAsync()
        {
            try
            {
                UpdateStatus("Saving processor...", Color.DodgerBlue);
                saveButton.Enabled = false;

                // Update current processor with form values
                currentProcessor!.Name = nameTextBox.Text?.Trim() ?? string.Empty;
                currentProcessor.SystemPrompt = systemPromptTextBox.Text?.Trim() ?? string.Empty;
                
                // Convert comma-separated strings back to lists
                currentProcessor.InboundShould = inboundShouldTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentProcessor.InboundShouldNot = inboundShouldNotTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentProcessor.InboundMust = inboundMustTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentProcessor.InboundMustNot = inboundMustNotTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentProcessor.OutbounShould = outboundShouldTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentProcessor.OutboundShouldNot = outboundShouldNotTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentProcessor.OutboundMust = outboundMustTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentProcessor.OutboundMustNot = outboundMustNotTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                currentProcessor.Date = DateTime.Now;

                var url = $"http://{completionHost}:{completionPort}/symbolicprocessor/save";
                var json = JsonConvert.SerializeObject(currentProcessor, Formatting.Indented);
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
                        UpdateStatus($"Processor saved successfully: {serviceReturn.Message}", Color.Green);
                        await LogActionAsync($"Processor saved: {currentProcessor.Name}");
                        
                        // Refresh search results if there was a previous search
                        if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
                        {
                            await SearchProcessorsAsync(searchTextBox.Text.Trim());
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
                UpdateStatus($"Error saving processor: {ex.Message}", Color.Red);
                await LogActionAsync($"Save error: {ex.Message}");
            }
            finally
            {
                saveButton.Enabled = true;
            }
        }

        private void NewButton_Click(object? sender, EventArgs e)
        {
            currentProcessor = new SymbolicProcessor
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName
            };

            PopulateProcessorDetails(currentProcessor);
            UpdateStatus("New processor created", Color.Green);
        }

        private void CloneButton_Click(object? sender, EventArgs e)
        {
            if (currentProcessor == null)
            {
                UpdateStatus("No processor to clone", Color.Orange);
                return;
            }

            // Create a clone with a new ID and updated date
            var clonedProcessor = new SymbolicProcessor
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                Name = currentProcessor.Name + " (Copy)",
                SystemPrompt = currentProcessor.SystemPrompt,
                InboundShould = new List<string>(currentProcessor.InboundShould ?? new List<string>()),
                InboundShouldNot = new List<string>(currentProcessor.InboundShouldNot ?? new List<string>()),
                InboundMust = new List<string>(currentProcessor.InboundMust ?? new List<string>()),
                InboundMustNot = new List<string>(currentProcessor.InboundMustNot ?? new List<string>()),
                OutbounShould = new List<string>(currentProcessor.OutbounShould ?? new List<string>()),
                OutboundShouldNot = new List<string>(currentProcessor.OutboundShouldNot ?? new List<string>()),
                OutboundMust = new List<string>(currentProcessor.OutboundMust ?? new List<string>()),
                OutboundMustNot = new List<string>(currentProcessor.OutboundMustNot ?? new List<string>())
            };

            currentProcessor = clonedProcessor;
            PopulateProcessorDetails(currentProcessor);
            UpdateStatus("Processor cloned successfully", Color.Green);
        }

        private void NewWindowButton_Click(object? sender, EventArgs e)
        {
            var newWindow = new SymbolicProcessorManagerForm(completionHost, completionPort);
            newWindow.Show();
            UpdateStatus("New window opened", Color.Green);
        }

        private void SaveAsButton_Click(object? sender, EventArgs e)
        {
            if (currentProcessor == null)
            {
                UpdateStatus("No processor to save", Color.Orange);
                return;
            }

            try
            {
                // Collect data from form
                var processorToSave = new SymbolicProcessor
                {
                    Id = string.Empty, // Set ID to empty as requested
                    Date = DateTime.Now,
                    MachineName = Environment.MachineName,
                    UserName = Environment.UserName,
                    Name = nameTextBox.Text?.Trim() ?? "",
                    SystemPrompt = systemPromptTextBox.Text?.Trim() ?? "",
                    InboundShould = inboundShouldTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    InboundShouldNot = inboundShouldNotTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    InboundMust = inboundMustTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    InboundMustNot = inboundMustNotTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    OutbounShould = outboundShouldTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    OutboundShouldNot = outboundShouldNotTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    OutboundMust = outboundMustTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    OutboundMustNot = outboundMustNotTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>()
                };

                // Open SaveFileDialog
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Save Processor As JSON";
                    saveFileDialog.DefaultExt = "json";
                    
                    // Suggest filename based on processor name
                    string suggestedFileName = string.IsNullOrWhiteSpace(processorToSave.Name) 
                        ? "processor.json" 
                        : $"{processorToSave.Name.Replace(" ", "_")}.json";
                    saveFileDialog.FileName = suggestedFileName;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Serialize processor to JSON
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(processorToSave, Newtonsoft.Json.Formatting.Indented);
                        
                        // Save to file
                        System.IO.File.WriteAllText(saveFileDialog.FileName, json);
                        
                        UpdateStatus($"Processor saved to: {System.IO.Path.GetFileName(saveFileDialog.FileName)}", Color.Green);
                        logService.Log("OK", $"Processor saved as JSON: {saveFileDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error saving processor: {ex.Message}", Color.Red);
                logService.Log("ERROR", $"Error saving processor as JSON: {ex.Message}");
                MessageBox.Show($"Failed to save processor:\n{ex.Message}", "Save Error", 
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
                    openFileDialog.Title = "Load Processor From JSON File";
                    openFileDialog.DefaultExt = "json";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Read file content
                        string jsonContent = System.IO.File.ReadAllText(openFileDialog.FileName);
                        
                        // Deserialize processor from JSON
                        var loadedProcessor = Newtonsoft.Json.JsonConvert.DeserializeObject<SymbolicProcessor>(jsonContent);
                        
                        if (loadedProcessor == null)
                        {
                            UpdateStatus("Failed to load processor: Invalid JSON format", Color.Red);
                            MessageBox.Show("The selected file does not contain a valid processor.", 
                                          "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Always generate a new ID when loading from file
                        // This ensures loaded processors are treated as new/cloned processors
                        loadedProcessor.Id = Guid.NewGuid().ToString();
                        
                        // Update metadata
                        loadedProcessor.Date = DateTime.Now;
                        loadedProcessor.MachineName = Environment.MachineName;
                        loadedProcessor.UserName = Environment.UserName;

                        // Update current processor
                        currentProcessor = loadedProcessor;
                        
                        // Populate the form with loaded data
                        PopulateProcessorDetails(loadedProcessor);
                        
                        UpdateStatus($"Processor loaded from: {System.IO.Path.GetFileName(openFileDialog.FileName)} (New ID generated)", Color.Green);
                        logService.Log("OK", $"Processor loaded from JSON with new ID: {openFileDialog.FileName}");
                    }
                }
            }
            catch (Newtonsoft.Json.JsonException jsonEx)
            {
                UpdateStatus($"Invalid JSON file: {jsonEx.Message}", Color.Red);
                logService.Log("ERROR", $"JSON parse error loading processor: {jsonEx.Message}");
                MessageBox.Show($"Failed to parse JSON file:\n{jsonEx.Message}", 
                               "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading processor: {ex.Message}", Color.Red);
                logService.Log("ERROR", $"Error loading processor from JSON: {ex.Message}");
                MessageBox.Show($"Failed to load processor:\n{ex.Message}", 
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
