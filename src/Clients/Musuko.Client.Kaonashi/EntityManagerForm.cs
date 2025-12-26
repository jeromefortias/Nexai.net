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

    public partial class EntityManagerForm : Form
    {
        private readonly string completionHost;
        private readonly int completionPort;
        private readonly LogService logService;
        private readonly HttpClient httpClient;
        private Entity? currentEntity;

        // Parameterless constructor for designer support
        public EntityManagerForm()
        {
            completionHost = "localhost";
            completionPort = 50824;
            logService = new LogService(completionHost, completionPort);
            httpClient = new HttpClient();
            InitializeComponent();
            IconHelper.SetFormIcon(this);
        }

        public EntityManagerForm(string host, int port)
        {
            completionHost = host;
            completionPort = port;
            logService = new LogService(host, port);
            httpClient = new HttpClient();
            InitializeComponent();
            IconHelper.SetFormIcon(this);
            
            // Initially disable only the input fields, but keep buttons enabled
            EnableAllDetailControls(false);
            
            // But always keep these buttons enabled (New, Load, New Window can be used anytime)
            newButton.Enabled = true;
            loadButton.Enabled = true;
            newWindowButton.Enabled = true;
            
            if (!DesignMode)
            {
                _ = LogActionAsync("Entity Manager opened");
            }
        }

        private async Task LogActionAsync(string action)
        {
            try
            {
                await logService.LogAsync("EntityManager", action);
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
                UpdateStatus("Please enter search criteria (use * for last 10 entities)", Color.Orange);
                return;
            }

            await SearchEntitiesAsync(criteria);
        }

        private async Task SearchEntitiesAsync(string criteria)
        {
            try
            {
                string statusMessage = criteria == "*" 
                    ? "Loading last 10 entities..." 
                    : "Searching...";
                UpdateStatus(statusMessage, Color.DodgerBlue);
                searchButton.Enabled = false;

                var searchId = new SearchId
                {
                    Id = string.Empty,
                    Criteria = criteria
                };

                var url = $"http://{completionHost}:{completionPort}/entities/search";
                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var entities = JsonConvert.DeserializeObject<List<Entity>>(responseBody);
                    
                    if (entities != null && entities.Count > 0)
                    {
                        entitiesDataGrid.DataSource = entities;
                        ConfigureDataGridColumns();
                        
                        string resultMessage = criteria == "*" 
                            ? $"Loaded last {entities.Count} entity(ies)" 
                            : $"Found {entities.Count} entity(ies)";
                        UpdateStatus(resultMessage, Color.Green);
                        
                        await LogActionAsync($"Search successful: {entities.Count} entities found for '{criteria}'");
                    }
                    else
                    {
                        entitiesDataGrid.DataSource = null;
                        UpdateStatus("No entities found", Color.Orange);
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
            if (entitiesDataGrid.Columns["Id"] != null)
                entitiesDataGrid.Columns["Id"]!.Visible = false;

            if (entitiesDataGrid.Columns["Date"] != null)
            {
                entitiesDataGrid.Columns["Date"]!.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                entitiesDataGrid.Columns["Date"]!.Width = 130;
            }

            if (entitiesDataGrid.Columns["Name"] != null)
                entitiesDataGrid.Columns["Name"]!.Width = 200;

            if (entitiesDataGrid.Columns["ShortDescription"] != null)
                entitiesDataGrid.Columns["ShortDescription"]!.Width = 300;
        }

        private async void EntitiesDataGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var entity = entitiesDataGrid.Rows[e.RowIndex].DataBoundItem as Entity;
                if (entity != null)
                {
                    await LoadEntityDetailsAsync(entity.Id);
                }
            }
        }

        private async Task LoadEntityDetailsAsync(string entityId)
        {
            try
            {
                UpdateStatus("Loading entity details...", Color.DodgerBlue);

                var searchId = new SearchId
                {
                    Id = entityId,
                    Criteria = string.Empty
                };

                var url = $"http://{completionHost}:{completionPort}/entities/load";
                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    currentEntity = JsonConvert.DeserializeObject<Entity>(responseBody);
                    
                    if (currentEntity != null)
                    {
                        PopulateEntityDetails(currentEntity);
                        UpdateStatus("Entity loaded", Color.Green);
                        await LogActionAsync($"Entity loaded: {currentEntity.Name}");
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
                UpdateStatus($"Error loading entity: {ex.Message}", Color.Red);
                await LogActionAsync($"Load error: {ex.Message}");
            }
        }

        private void PopulateEntityDetails(Entity entity)
        {
            // Display entity ID
            entityIdLabel.Text = $"[ID: {entity.Id}]";
            
            nameTextBox.Text = entity.Name;
            alternativeNamesTextBox.Text = string.Join(", ", entity.AlternativeNames ?? new List<string>());
            typeTextBox.Text = entity.Type;
            shortDescriptionTextBox.Text = entity.ShortDescription;
            longDescriptionTextBox.Text = entity.LongDescription;
            relatedEntitiesTextBox.Text = string.Join(", ", entity.RelatedEntities ?? new List<string>());
            
            // Emotional scores
            joyNumeric.Value = entity.Joy;
            fearNumeric.Value = entity.Fear;
            angerNumeric.Value = entity.Anger;
            sadnessNumeric.Value = entity.Sadness;
            disgustNumeric.Value = entity.Disgust;
            
            // Date range
            if (entity.DateFrom.HasValue)
                dateFromPicker.Value = entity.DateFrom.Value;
            else
                dateFromPicker.Value = DateTime.Now;

            if (entity.DateTo.HasValue)
                dateToPicker.Value = entity.DateTo.Value;
            else
                dateToPicker.Value = DateTime.Now;

            // Context fields
            howToTextBox.Text = entity.HowTo ?? string.Empty;
            withWhatTextBox.Text = entity.WithWhat ?? string.Empty;
            withoutWhatTextBox.Text = entity.WithoutWhat ?? string.Empty;
            whereTextBox.Text = entity.Where ?? string.Empty;
            whenTextBox.Text = entity.When ?? string.Empty;
            commentTextBox.Text = entity.Comment;

            // Enable all input fields when entity is loaded
            EnableAllDetailControls(true);
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            if (currentEntity == null)
            {
                UpdateStatus("No entity selected", Color.Orange);
                return;
            }

            await SaveEntityAsync();
        }

        private async Task SaveEntityAsync()
        {
            try
            {
                UpdateStatus("Saving entity...", Color.DodgerBlue);
                saveButton.Enabled = false;

                // Update current entity with form values
                currentEntity!.Name = nameTextBox.Text?.Trim() ?? string.Empty;
                currentEntity.AlternativeNames = alternativeNamesTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                currentEntity.Type = typeTextBox.Text?.Trim() ?? string.Empty;
                currentEntity.ShortDescription = shortDescriptionTextBox.Text?.Trim() ?? string.Empty;
                currentEntity.LongDescription = longDescriptionTextBox.Text?.Trim() ?? string.Empty;
                currentEntity.RelatedEntities = relatedEntitiesTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();
                
                // Emotional scores
                currentEntity.Joy = (int)joyNumeric.Value;
                currentEntity.Fear = (int)fearNumeric.Value;
                currentEntity.Anger = (int)angerNumeric.Value;
                currentEntity.Sadness = (int)sadnessNumeric.Value;
                currentEntity.Disgust = (int)disgustNumeric.Value;
                
                // Date range
                currentEntity.DateFrom = dateFromPicker.Value;
                currentEntity.DateTo = dateToPicker.Value;
                
                // Context fields
                currentEntity.HowTo = howToTextBox.Text?.Trim();
                currentEntity.WithWhat = withWhatTextBox.Text?.Trim();
                currentEntity.WithoutWhat = withoutWhatTextBox.Text?.Trim();
                currentEntity.Where = whereTextBox.Text?.Trim();
                currentEntity.When = whenTextBox.Text?.Trim();
                currentEntity.Comment = commentTextBox.Text?.Trim() ?? string.Empty;
                currentEntity.Date = DateTime.Now;

                var url = $"http://{completionHost}:{completionPort}/entities/save";
                var json = JsonConvert.SerializeObject(currentEntity, Formatting.Indented);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                UpdateStatus($"Sending POST to: {url}", Color.Blue);
                
                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                UpdateStatus($"Response: {response.StatusCode} - Body: {(responseBody != null ? responseBody.Substring(0, Math.Min(100, responseBody.Length)) : "null")}", Color.Blue);

                // Log the JSON being sent for debugging
                System.Diagnostics.Debug.WriteLine("=== SAVE ENTITY REQUEST ===");
                System.Diagnostics.Debug.WriteLine($"URL: {url}");
                System.Diagnostics.Debug.WriteLine($"JSON Payload:\n{json}");
                System.Diagnostics.Debug.WriteLine($"Response Status: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Response Body: {responseBody}");
                System.Diagnostics.Debug.WriteLine("===========================");

                if (response.IsSuccessStatusCode)
                {
                    var serviceReturn = JsonConvert.DeserializeObject<ServiceReturn>(responseBody);
                    
                    if (serviceReturn != null && serviceReturn.Success)
                    {
                        UpdateStatus($"Entity saved successfully: {serviceReturn.Message}", Color.Green);
                        await LogActionAsync($"Entity saved: {currentEntity.Name}");
                        
                        // Refresh search results if there was a previous search
                        if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
                        {
                            await SearchEntitiesAsync(searchTextBox.Text.Trim());
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
                UpdateStatus($"Error saving entity: {ex.Message}", Color.Red);
                await LogActionAsync($"Save error: {ex.Message}");
            }
            finally
            {
                saveButton.Enabled = true;
            }
        }

        private void NewButton_Click(object? sender, EventArgs e)
        {
            // Create a new entity with default values
            currentEntity = new Entity
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                Name = string.Empty,
                AlternativeNames = new List<string>(),
                Type = "OTHER_ENTITY",
                ShortDescription = string.Empty,
                LongDescription = string.Empty,
                RelatedEntities = new List<string>(),
                Joy = 0,
                Fear = 0,
                Anger = 0,
                Sadness = 0,
                Disgust = 0,
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now.AddYears(1000),
                HowTo = null,
                WithWhat = null,
                WithoutWhat = null,
                Where = null,
                When = null,
                Comment = string.Empty
            };

            // Clear and populate all fields
            PopulateEntityDetails(currentEntity);
            
            // All input fields are already enabled by PopulateEntityDetails
            UpdateStatus("New entity created - All fields are ready for input", Color.Green);
            _ = LogActionAsync("New entity created");
        }

        private void EnableAllDetailControls(bool enabled)
        {
            // Enable all input controls in the details panel
            nameTextBox.Enabled = enabled;
            alternativeNamesTextBox.Enabled = enabled;
            typeTextBox.Enabled = enabled;
            shortDescriptionTextBox.Enabled = enabled;
            longDescriptionTextBox.Enabled = enabled;
            relatedEntitiesTextBox.Enabled = enabled;
            joyNumeric.Enabled = enabled;
            fearNumeric.Enabled = enabled;
            angerNumeric.Enabled = enabled;
            sadnessNumeric.Enabled = enabled;
            disgustNumeric.Enabled = enabled;
            dateFromPicker.Enabled = enabled;
            dateToPicker.Enabled = enabled;
            howToTextBox.Enabled = enabled;
            withWhatTextBox.Enabled = enabled;
            withoutWhatTextBox.Enabled = enabled;
            whereTextBox.Enabled = enabled;
            whenTextBox.Enabled = enabled;
            commentTextBox.Enabled = enabled;
            
            // Enable buttons that require an entity
            saveButton.Enabled = enabled;
            saveAsButton.Enabled = enabled;
            cloneButton.Enabled = enabled;
            autogenerateButton.Enabled = enabled;
        }

        private void CloneButton_Click(object? sender, EventArgs e)
        {
            if (currentEntity == null)
            {
                UpdateStatus("No entity to clone", Color.Orange);
                return;
            }

            // Create a clone with a new ID and updated date
            var clonedEntity = new Entity
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                Name = currentEntity.Name + " (Copy)",
                AlternativeNames = new List<string>(currentEntity.AlternativeNames ?? new List<string>()),
                Type = currentEntity.Type,
                ShortDescription = currentEntity.ShortDescription,
                LongDescription = currentEntity.LongDescription,
                RelatedEntities = new List<string>(currentEntity.RelatedEntities ?? new List<string>()),
                Joy = currentEntity.Joy,
                Fear = currentEntity.Fear,
                Anger = currentEntity.Anger,
                Sadness = currentEntity.Sadness,
                Disgust = currentEntity.Disgust,
                DateFrom = currentEntity.DateFrom,
                DateTo = currentEntity.DateTo,
                HowTo = currentEntity.HowTo,
                WithWhat = currentEntity.WithWhat,
                WithoutWhat = currentEntity.WithoutWhat,
                Where = currentEntity.Where,
                When = currentEntity.When,
                Comment = currentEntity.Comment
            };

            currentEntity = clonedEntity;
            PopulateEntityDetails(currentEntity);
            UpdateStatus("Entity cloned successfully", Color.Green);
        }

        private void NewWindowButton_Click(object? sender, EventArgs e)
        {
            var newWindow = new EntityManagerForm(completionHost, completionPort);
            newWindow.Show();
            UpdateStatus("New window opened", Color.Green);
        }

        private void SaveAsButton_Click(object? sender, EventArgs e)
        {
            if (currentEntity == null)
            {
                UpdateStatus("No entity to save", Color.Orange);
                return;
            }

            try
            {
                // Collect data from form
                var entityToSave = new Entity
                {
                    Id = string.Empty, // Set ID to empty as requested
                    Date = DateTime.Now,
                    MachineName = Environment.MachineName,
                    UserName = Environment.UserName,
                    Name = nameTextBox.Text?.Trim() ?? "",
                    Type = typeTextBox.Text?.Trim() ?? "OTHER_ENTITY",
                    AlternativeNames = alternativeNamesTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    ShortDescription = shortDescriptionTextBox.Text?.Trim() ?? "",
                    LongDescription = longDescriptionTextBox.Text?.Trim() ?? "",
                    RelatedEntities = relatedEntitiesTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>(),
                    Joy = (int)joyNumeric.Value,
                    Fear = (int)fearNumeric.Value,
                    Anger = (int)angerNumeric.Value,
                    Sadness = (int)sadnessNumeric.Value,
                    Disgust = (int)disgustNumeric.Value,
                    DateFrom = dateFromPicker.Value,
                    DateTo = dateToPicker.Value,
                    HowTo = howToTextBox.Text?.Trim(),
                    WithWhat = withWhatTextBox.Text?.Trim(),
                    WithoutWhat = withoutWhatTextBox.Text?.Trim(),
                    Where = whereTextBox.Text?.Trim(),
                    When = whenTextBox.Text?.Trim(),
                    Comment = commentTextBox.Text?.Trim()
                };

                // Open SaveFileDialog
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Save Entity As JSON";
                    saveFileDialog.DefaultExt = "json";
                    
                    // Suggest filename based on entity name
                    string suggestedFileName = string.IsNullOrWhiteSpace(entityToSave.Name) 
                        ? "entity.json" 
                        : $"{entityToSave.Name.Replace(" ", "_")}.json";
                    saveFileDialog.FileName = suggestedFileName;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Serialize entity to JSON
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(entityToSave, Newtonsoft.Json.Formatting.Indented);
                        
                        // Save to file
                        System.IO.File.WriteAllText(saveFileDialog.FileName, json);
                        
                        UpdateStatus($"Entity saved to: {System.IO.Path.GetFileName(saveFileDialog.FileName)}", Color.Green);
                        logService.Log("OK", $"Entity saved as JSON: {saveFileDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error saving entity: {ex.Message}", Color.Red);
                logService.Log("ERROR", $"Error saving entity as JSON: {ex.Message}");
                MessageBox.Show($"Failed to save entity:\n{ex.Message}", "Save Error", 
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
                    openFileDialog.Title = "Load Entity From JSON File";
                    openFileDialog.DefaultExt = "json";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Read file content
                        string jsonContent = System.IO.File.ReadAllText(openFileDialog.FileName);
                        
                        // Deserialize entity from JSON
                        var loadedEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<Entity>(jsonContent);
                        
                        if (loadedEntity == null)
                        {
                            UpdateStatus("Failed to load entity: Invalid JSON format", Color.Red);
                            MessageBox.Show("The selected file does not contain a valid entity.", 
                                          "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Always generate a new ID when loading from file
                        // This ensures loaded entities are treated as new/cloned entities
                        loadedEntity.Id = Guid.NewGuid().ToString();
                        
                        // Update metadata
                        loadedEntity.Date = DateTime.Now;
                        loadedEntity.MachineName = Environment.MachineName;
                        loadedEntity.UserName = Environment.UserName;

                        // Update current entity
                        currentEntity = loadedEntity;
                        
                        // Populate the form with loaded data
                        PopulateEntityDetails(loadedEntity);
                        
                        UpdateStatus($"Entity loaded from: {System.IO.Path.GetFileName(openFileDialog.FileName)} (New ID generated)", Color.Green);
                        logService.Log("OK", $"Entity loaded from JSON with new ID: {openFileDialog.FileName}");
                    }
                }
            }
            catch (Newtonsoft.Json.JsonException jsonEx)
            {
                UpdateStatus($"Invalid JSON file: {jsonEx.Message}", Color.Red);
                logService.Log("ERROR", $"JSON parse error loading entity: {jsonEx.Message}");
                MessageBox.Show($"Failed to parse JSON file:\n{jsonEx.Message}", 
                               "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading entity: {ex.Message}", Color.Red);
                logService.Log("ERROR", $"Error loading entity from JSON: {ex.Message}");
                MessageBox.Show($"Failed to load entity:\n{ex.Message}", 
                               "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void AutogenerateButton_Click(object? sender, EventArgs e)
        {
            if (currentEntity == null)
            {
                UpdateStatus("No entity loaded. Create a new entity first.", Color.Orange);
                return;
            }

            var prompt = commentTextBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(prompt))
            {
                UpdateStatus("Please enter a description in the Comment field to autogenerate", Color.Orange);
                return;
            }

            await AutogenerateEntityAsync(prompt);
        }

        private async Task AutogenerateEntityAsync(string userPrompt)
        {
            try
            {
                UpdateStatus("Generating entity information from LLM...", Color.DodgerBlue);
                autogenerateButton.Enabled = false;

                var systemPrompt = @"You are an AI assistant that generates structured entity information in JSON format. 
Based on the user's description, generate comprehensive entity details including:
- Name: A clear, concise name for the entity
- Type: One of: PERSON, LOCATION, CONTRAT, ORG, DATE, TIME, MONEY, QUANTITY, PRODUCT, EVENT, APPLICATION, AGENT, OTHER_ENTITY
- AlternativeNames: Array of alternative names or aliases
- ShortDescription: A brief one-sentence description
- LongDescription: A detailed multi-sentence description
- RelatedEntities: Array of related entity names
- Joy, Fear, Anger, Sadness, Disgust: Emotional scores (0-100) associated with this entity
- HowTo: Instructions or guidance related to the entity
- WithWhat: What tools, resources, or items are associated
- WithoutWhat: What should be avoided or is not needed
- Where: Location or context information
- When: Temporal information or timing

Respond ONLY with valid JSON matching this structure, no other text:
{
  ""Name"": """",
  ""Type"": """",
  ""AlternativeNames"": [],
  ""ShortDescription"": """",
  ""LongDescription"": """",
  ""RelatedEntities"": [],
  ""Joy"": 0,
  ""Fear"": 0,
  ""Anger"": 0,
  ""Sadness"": 0,
  ""Disgust"": 0,
  ""HowTo"": """",
  ""WithWhat"": """",
  ""WithoutWhat"": """",
  ""Where"": """",
  ""When"": """"
}";

                var requestBody = new
                {
                    model = "llama3.2:latest",
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = userPrompt }
                    },
                    stream = false,
                    temperature = 0.7
                };

               var url = $"http://{completionHost}:{completionPort}/v1/chat/completions";
               var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
               var content = new StringContent(json, Encoding.UTF8, "application/json");

               UpdateStatus($"Calling LLM at {url}...", Color.Blue);

                var response = await httpClient.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    UpdateStatus($"LLM call failed: {response.StatusCode}", Color.Red);
                    await LogActionAsync($"Autogenerate failed: {response.StatusCode}");
                    return;
                }

                // Parse the LLM response
                dynamic? completionResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                string? llmResponse = completionResponse?.message?.content;

                if (string.IsNullOrWhiteSpace(llmResponse))
                {
                    UpdateStatus("LLM returned empty response", Color.Red);
                    return;
                }

                // Extract JSON from response (might be wrapped in markdown code blocks)
                llmResponse = llmResponse.Trim();
                if (llmResponse.StartsWith("```json"))
                    llmResponse = llmResponse.Substring(7);
                else if (llmResponse.StartsWith("```"))
                    llmResponse = llmResponse.Substring(3);
                
                if (llmResponse.EndsWith("```"))
                    llmResponse = llmResponse.Substring(0, llmResponse.Length - 3);
                
                llmResponse = llmResponse.Trim();

                // Parse the entity JSON
                var generatedData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(llmResponse);

                // Update current entity with generated data
                if (generatedData?.Name != null)
                    nameTextBox.Text = generatedData.Name.ToString();
                
                if (generatedData?.Type != null)
                    typeTextBox.Text = generatedData.Type.ToString();
                
                if (generatedData?.AlternativeNames != null)
                {
                    var altNames = new List<string>();
                    foreach (var name in generatedData.AlternativeNames)
                        altNames.Add(name.ToString());
                    alternativeNamesTextBox.Text = string.Join(", ", altNames);
                }
                
                if (generatedData?.ShortDescription != null)
                    shortDescriptionTextBox.Text = generatedData.ShortDescription.ToString();
                
                if (generatedData?.LongDescription != null)
                    longDescriptionTextBox.Text = generatedData.LongDescription.ToString();
                
                if (generatedData?.RelatedEntities != null)
                {
                    var relEntities = new List<string>();
                    foreach (var entity in generatedData.RelatedEntities)
                        relEntities.Add(entity.ToString());
                    relatedEntitiesTextBox.Text = string.Join(", ", relEntities);
                }

                // Emotional scores
                if (generatedData?.Joy != null)
                    joyNumeric.Value = Math.Min(100, Math.Max(0, (int)generatedData.Joy));
                if (generatedData?.Fear != null)
                    fearNumeric.Value = Math.Min(100, Math.Max(0, (int)generatedData.Fear));
                if (generatedData?.Anger != null)
                    angerNumeric.Value = Math.Min(100, Math.Max(0, (int)generatedData.Anger));
                if (generatedData?.Sadness != null)
                    sadnessNumeric.Value = Math.Min(100, Math.Max(0, (int)generatedData.Sadness));
                if (generatedData?.Disgust != null)
                    disgustNumeric.Value = Math.Min(100, Math.Max(0, (int)generatedData.Disgust));

                // Context fields
                if (generatedData?.HowTo != null)
                    howToTextBox.Text = generatedData.HowTo.ToString();
                if (generatedData?.WithWhat != null)
                    withWhatTextBox.Text = generatedData.WithWhat.ToString();
                if (generatedData?.WithoutWhat != null)
                    withoutWhatTextBox.Text = generatedData.WithoutWhat.ToString();
                if (generatedData?.Where != null)
                    whereTextBox.Text = generatedData.Where.ToString();
                if (generatedData?.When != null)
                    whenTextBox.Text = generatedData.When.ToString();

                UpdateStatus("Entity information generated successfully!", Color.Green);
                await LogActionAsync($"Autogenerated entity: {nameTextBox.Text}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error autogenerating: {ex.Message}", Color.Red);
                await LogActionAsync($"Autogenerate error: {ex.Message}");
            }
            finally
            {
                autogenerateButton.Enabled = true;
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

