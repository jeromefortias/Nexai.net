using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Musuko.Kaonashi
{
    /// <summary>
    /// Main form for the Localhost.AI Kaonashi client application.
    /// Provides a Windows Forms-based chat interface for interacting with AI models via Ollama.
    /// Features include: chat history, model selection, code block detection, emotion detection,
    /// news ticker, and various management forms for entities, caches, CVs, etc.
    /// </summary>
    public partial class MainForm : Form
    {
        // UI Components - Panels and containers
        private Panel headerPanel;
        private Panel chatPanel;
        private Panel inputPanel;
        private Panel statusPanel;
        private Panel imagePanel;
        private PictureBox marvinPictureBox;  // Main character image
        private PictureBox marvinEyesPictureBox;  // Eyes that change based on emotion
        private RichTextBox chatDisplay;  // Main chat display area
        private TextBox messageInput;  // User input text box
        private Button sendButton;
        private Button clearButton;
        private Button settingsButton;
        private Button loadPromptButton;
        private ComboBox modelSelector;  // Dropdown for selecting AI model
        private Label statusLabel;
        private Label modelLabel;
        private Label titleLabel;
        private Label connectionIndicator;  // Visual indicator for connection status
        private Label statusBarLabel;
        private Label statusBarConnectionLabel;
        private Label wpsLabel;
        private Panel newsTickerPanel;  // Panel for scrolling news ticker
        
        // News ticker components
        private System.Windows.Forms.Timer newsScrollTimer;  // Timer for scrolling animation
        private System.Windows.Forms.Timer newsRefreshTimer;  // Timer for refreshing news
        private NewsService newsService;  // Service for fetching news
        
        // Core services and data
        private OllamaService ollamaService;  // Service for communicating with Ollama API
        private ChatHistory chatHistory;  // Maintains conversation history
        private AppConfig config;  // Application configuration
        private LogService logService;  // Service for logging operations
        private bool isProcessing = false;  // Flag to prevent concurrent requests
        
        // Message history for UP/DOWN arrow navigation
        private List<string> messageHistory = new List<string>();
        private int historyIndex = -1;
        
        // Current user prompt for saving with completion
        private string? currentUserPrompt = null;
        
        // Buffer for handling think tags across streaming chunks
        // Think tags (<think>) are hidden from display
        private System.Text.StringBuilder streamBuffer = new System.Text.StringBuilder();
        
        // Buffer for accumulating assistant response for emotion detection
        private System.Text.StringBuilder assistantResponseBuffer = new System.Text.StringBuilder();
        
        // Track detected code blocks to avoid duplicate popups
        private List<(string code, string language)> detectedCodeBlocks = new List<(string, string)>();
        
        // News banner functionality
        private List<News> newsItems = new List<News>();
        private int newsScrollPosition = 0;  // Current scroll position for news ticker
        private int newsTextWidth = 0;  // Total width of news text

        // Modern color scheme
        private readonly Color primaryColor = Color.FromArgb(88, 101, 242);      // Discord-like purple
        private readonly Color primaryHover = Color.FromArgb(71, 82, 196);
        private readonly Color backgroundColor = Color.FromArgb(54, 57, 63);      // Dark background
        private readonly Color secondaryBg = Color.FromArgb(47, 49, 54);          // Darker panels
        private readonly Color inputBg = Color.FromArgb(64, 68, 75);              // Input background
        private readonly Color textColor = Color.FromArgb(220, 221, 222);         // Light text
        private readonly Color accentColor = Color.FromArgb(67, 181, 129);        // Green accent
        private readonly Color warningColor = Color.FromArgb(250, 166, 26);       // Orange
        private readonly Color errorColor = Color.FromArgb(237, 66, 69);          // Red

        /// <summary>
        /// Initializes a new instance of the MainForm.
        /// Sets up UI components, loads configuration, and initializes services.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            IconHelper.SetFormIcon(this);
            LoadConfiguration();
            InitializeOllamaService();
            InitializeNewsBanner();
            
            // Asynchronously load available models from Ollama
            _ = LoadAvailableModels();
        }

        /// <summary>
        /// Loads application configuration from file and initializes log service.
        /// </summary>
        private void LoadConfiguration()
        {
            config = AppConfig.Load();
            logService = new LogService(config.CompletionHost, config.CompletionPort);
        }

        /// <summary>
        /// Reloads configuration and reinitializes services.
        /// Called when settings are updated.
        /// </summary>
        public void ReloadConfiguration()
        {
            LoadConfiguration();
            InitializeOllamaService();
            InitializeNewsBanner();
            _ = LoadAvailableModels();
        }

        /// <summary>
        /// Initializes the Ollama service with connection settings.
        /// Also initializes a new chat history instance.
        /// </summary>
        private void InitializeOllamaService()
        {
            ollamaService = new OllamaService(config.OllamaHost, config.OllamaPort, 
                                             config.CompletionHost, config.CompletionPort);
            chatHistory = new ChatHistory();
        }

        private void InitializeNewsBanner()
        {
            try
            {
                // Initialize news service
                newsService = new NewsService(config.CompletionHost, config.CompletionPort);
                
                // Initialize scroll timer (for scrolling animation)
                newsScrollTimer = new System.Windows.Forms.Timer();
                newsScrollTimer.Interval = 50; // 50ms for smooth scrolling
                newsScrollTimer.Tick += NewsScrollTimer_Tick;
                
                // Initialize refresh timer (for loading new news)
                newsRefreshTimer = new System.Windows.Forms.Timer();
                newsRefreshTimer.Interval = 300000; // 5 minutes
                newsRefreshTimer.Tick += NewsRefreshTimer_Tick;
                
                // Load initial news and start timers
                _ = LoadNewsForBanner();
                
                // Add form closing event to cleanup resources
                this.FormClosing += MainForm_FormClosing;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to initialize news banner: {ex.Message}");
            }
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            CleanupNewsBanner();
        }

        private void CreateSampleNewsData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("CreateSampleNewsData: Starting...");
                
                // Create sample news data with different ratings for testing color coding
                newsItems = new List<News>
                {
                    new News { Title = "Tech Stocks Surge", Rating = 5, Url = "https://example.com/tech" },
                    new News { Title = "Market Crash Concerns", Rating = -3, Url = "https://example.com/market" },
                    new News { Title = "New AI Breakthrough", Rating = 8, Url = "https://example.com/ai" },
                    new News { Title = "Climate Change Report", Rating = -2, Url = "https://example.com/climate" },
                    new News { Title = "Neutral Business News", Rating = 0, Url = "https://example.com/business" }
                };
                
                System.Diagnostics.Debug.WriteLine($"CreateSampleNewsData: Created {newsItems.Count} news items");
                System.Diagnostics.Debug.WriteLine($"CreateSampleNewsData: newsScrollTimer is null: {newsScrollTimer == null}");
                System.Diagnostics.Debug.WriteLine($"CreateSampleNewsData: newsTickerPanel is null: {newsTickerPanel == null}");
                
                // Start the scroll timer
                newsScrollTimer?.Start();
                newsRefreshTimer?.Start();
                
                // Force panel redraw
                newsTickerPanel?.Invalidate();
                
                System.Diagnostics.Debug.WriteLine("CreateSampleNewsData: Completed successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create sample news data: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads available AI models from Ollama and populates the model selector dropdown.
        /// Updates UI status indicators based on connection and model availability.
        /// </summary>
        private async Task LoadAvailableModels()
        {
            try
            {
                // Update UI to show connecting state
                statusLabel.Text = "Connecting...";
                statusLabel.ForeColor = warningColor;
                connectionIndicator.BackColor = warningColor;
                statusBarLabel.Text = "?? Connecting to Ollama...";
                statusBarLabel.ForeColor = warningColor;
                
                // Test connection to Ollama service
                bool connected = await ollamaService.TestConnectionAsync();
                
                if (!connected)
                {
                    // Connection failed - update UI and log error
                    statusLabel.Text = "Disconnected";
                    statusLabel.ForeColor = errorColor;
                    connectionIndicator.BackColor = errorColor;
                    statusBarLabel.Text = "? Cannot connect to Ollama";
                    statusBarLabel.ForeColor = errorColor;
                    statusBarConnectionLabel.Text = $"{config.OllamaHost}:{config.OllamaPort}";
                    AppendSystemMessage($"? Cannot connect to Ollama at {config.OllamaHost}:{config.OllamaPort}. Please check if Ollama is running.");
                    await logService.LogAsync("ERROR", $"Cannot connect to Ollama at {config.OllamaHost}:{config.OllamaPort}");
                    return;
                }

                // Connection successful - fetch available models
                var models = await ollamaService.GetAvailableModelsAsync();
                
                modelSelector.Items.Clear();
                
                if (models.Length > 0)
                {
                    foreach (var model in models)
                    {
                        modelSelector.Items.Add(model);
                    }
                    
                    // Select last used model, or default model, or first available
                    int selectedIndex = -1;
                    
                    if (!string.IsNullOrEmpty(config.LastModelUsed))
                    {
                        selectedIndex = Array.IndexOf(models, config.LastModelUsed);
                    }
                    
                    if (selectedIndex < 0)
                    {
                        selectedIndex = Array.IndexOf(models, config.DefaultModel);
                    }
                    
                    modelSelector.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
                    
                    statusLabel.Text = "Connected";
                    statusLabel.ForeColor = accentColor;
                    connectionIndicator.BackColor = accentColor;
                    statusBarLabel.Text = "? Ready";
                    statusBarLabel.ForeColor = accentColor;
                    statusBarConnectionLabel.Text = $"Models: http://{config.OllamaHost}:{config.OllamaPort} • Completion: http://{config.CompletionHost}:{config.CompletionPort} • {models.Length} model(s)";
                    AppendSystemMessage($"? Connected to Ollama at {config.OllamaHost}:{config.OllamaPort}");
                    AppendSystemMessage($"?? {models.Length} model(s) available");
                    await logService.LogAsync("OK", $"Connected to Ollama - {models.Length} model(s) available");
                }
                else
                {
                    statusLabel.Text = "No models";
                    statusLabel.ForeColor = warningColor;
                    connectionIndicator.BackColor = warningColor;
                    statusBarLabel.Text = "? No models found";
                    statusBarLabel.ForeColor = warningColor;
                    statusBarConnectionLabel.Text = $"Models: http://{config.OllamaHost}:{config.OllamaPort} • Completion: http://{config.CompletionHost}:{config.CompletionPort}";
                    AppendSystemMessage("? No models found. Use 'ollama pull <model-name>' to download models.");
                    await logService.LogAsync("WARNING", "No models found in Ollama");
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error";
                statusLabel.ForeColor = errorColor;
                connectionIndicator.BackColor = errorColor;
                statusBarLabel.Text = $"? Error: {ex.Message}";
                statusBarLabel.ForeColor = errorColor;
                AppendSystemMessage($"? Error: {ex.Message}");
                await logService.LogAsync("ERROR", $"Error loading models: {ex.Message}");
            }
        }

        private async void SendButton_Click(object? sender, EventArgs e)
        {
            await SendMessageAsync();
        }

        private async void MessageInput_KeyDown(object? sender, KeyEventArgs e)
        {
            // Only Ctrl+Enter sends the message
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await SendMessageAsync();
            }
            // Regular Enter just adds a new line (default behavior)
            else if (e.KeyCode == Keys.Enter && !e.Control)
            {
                // Allow default behavior - adds new line in multiline textbox
                // Do nothing, let it pass through
            }
            else if (e.KeyCode == Keys.Up)
            {
                e.SuppressKeyPress = true;
                NavigateMessageHistory(-1);
            }
            else if (e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                NavigateMessageHistory(1);
            }
        }

        private void NavigateMessageHistory(int direction)
        {
            if (messageHistory.Count == 0)
                return;

            // Moving up (backward in history)
            if (direction < 0)
            {
                if (historyIndex < messageHistory.Count - 1)
                {
                    historyIndex++;
                    messageInput.Text = messageHistory[messageHistory.Count - 1 - historyIndex];
                    messageInput.SelectionStart = messageInput.Text.Length;
                }
            }
            // Moving down (forward in history)
            else if (direction > 0)
            {
                if (historyIndex > 0)
                {
                    historyIndex--;
                    messageInput.Text = messageHistory[messageHistory.Count - 1 - historyIndex];
                    messageInput.SelectionStart = messageInput.Text.Length;
                }
                else if (historyIndex == 0)
                {
                    historyIndex = -1;
                    messageInput.Clear();
                }
            }
        }

        /// <summary>
        /// Sends a chat message to the AI model.
        /// Handles special commands, processes the message through the completion service,
        /// and displays the response with code block detection and emotion analysis.
        /// </summary>
        private async Task SendMessageAsync()
        {
            // Prevent concurrent requests
            if (isProcessing)
                return;

            string message = messageInput.Text.Trim();
            if (string.IsNullOrEmpty(message))
                return;

            // Check for special commands (management forms, web browser commands, etc.)
            if (message.Equals("/json", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                var jsonClientForm = new JsonClientForm();
                jsonClientForm.Show();
                return;
            }
            
            // Check for entity manager command
            if (message.Equals("/entities", StringComparison.OrdinalIgnoreCase) ||
                message.Equals("/entity", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                var entityManagerForm = new EntityManagerForm(config.CompletionHost, config.CompletionPort);
                entityManagerForm.Show();
                return;
            }
            
            // Check for emotion commands
            if (message.Equals("/sad", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                LoadMarvinEyes("marvin-sad-eyes.png");
                return;
            }
            
            if (message.Equals("/normal", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                LoadMarvinEyes("marvin-normal-eyes.png");
                return;
            }
            
            if (message.Equals("/annoyed", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                LoadMarvinEyes("marvin-annoyed-eyes.png");
                return;
            }
            
            if (message.Equals("/coquin", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                LoadMarvinEyes("marvin-coquin-eyes.png");
                return;
            }
            
            if (message.Equals("/interrogation", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                LoadMarvinEyes("marvin-interogation-eyes.png");
                return;
            }
            
            if (message.Equals("/thinking", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                LoadMarvinEyes("marvin-thinking-1.png");
                return;
            }
            
            // Check for news viewer command
            if (message.Equals("/news", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                var newsViewerForm = new NewsViewerForm(config.CompletionHost, config.CompletionPort);
                newsViewerForm.Show();
                return;
            }
            
            // Check for cache manager command
            if (message.Equals("/cache", StringComparison.OrdinalIgnoreCase) || 
                message.Equals("/caches", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                var cacheManagerForm = new CacheManagerForm(config.CompletionHost, config.CompletionPort);
                cacheManagerForm.Show();
                return;
            }
            
            // Check for symbolic encoder manager command
            if (message.Equals("/encode", StringComparison.OrdinalIgnoreCase) || 
                message.Equals("/encoder", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                var symbolicEncoderManagerForm = new SymbolicEncoderManagerForm(config.CompletionHost, config.CompletionPort);
                symbolicEncoderManagerForm.Show();
                return;
            }
            
            // Check for symbolic decoder manager command
            if (message.Equals("/decode", StringComparison.OrdinalIgnoreCase) || 
                message.Equals("/decoder", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                var symbolicDecoderManagerForm = new SymbolicDecoderManagerForm(config.CompletionHost, config.CompletionPort);
                symbolicDecoderManagerForm.Show();
                return;
            }
            
            // Check for symbolic processor manager command
            if (message.Equals("/process", StringComparison.OrdinalIgnoreCase) || 
                message.Equals("/processor", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                var symbolicProcessorManagerForm = new SymbolicProcessorManagerForm(config.CompletionHost, config.CompletionPort);
                symbolicProcessorManagerForm.Show();
                return;
            }
            
            // Check for Kibana command
            if (message.Equals("/kibana", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "http://localhost:5601/app/home#/",
                        UseShellExecute = true
                    });
                    statusLabel.Text = "Opening Kibana in web browser...";
                    statusLabel.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Failed to open Kibana: {ex.Message}";
                    statusLabel.ForeColor = Color.Red;
                }
                return;
            }
            
            // Check for Orleans monitoring command
            if (message.Equals("/orleans", StringComparison.OrdinalIgnoreCase) || 
                message.Equals("/monitor", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "http://localhost:9090/",
                        UseShellExecute = true
                    });
                    statusLabel.Text = "Opening Orleans monitoring dashboard...";
                    statusLabel.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Failed to open Orleans monitoring: {ex.Message}";
                    statusLabel.ForeColor = Color.Red;
                }
                return;
            }
            
            // Check for Nexai website command
            if (message.Equals("/nexai", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://www.Musuko/",
                        UseShellExecute = true
                    });
                    statusLabel.Text = "Opening Nexai website...";
                    statusLabel.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Failed to open Nexai website: {ex.Message}";
                    statusLabel.ForeColor = Color.Red;
                }
                return;
            }
            
            // Check for default web browser command
            if (message.Equals("/web", StringComparison.OrdinalIgnoreCase) ||
                message.Equals("/www", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "http://",
                        UseShellExecute = true
                    });
                    statusLabel.Text = "Opening default web browser...";
                    statusLabel.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Failed to open web browser: {ex.Message}";
                    statusLabel.ForeColor = Color.Red;
                }
                return;
            }
            
            // Check for help command
            if (message.Equals("/help", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                var helpMessage = @"Available Commands:

?? MANAGEMENT FORMS:
/entities or /entity - Open Entity Manager
/encode or /encoder - Open Symbolic Encoder Manager
/decode or /decoder - Open Symbolic Decoder Manager
/process or /processor - Open Symbolic Processor Manager
/cv - Open CV Manager
/cache - Open Cache Manager
/news - Open News Viewer
/json - Open JSON Client

?? WEB BROWSER COMMANDS:
/web or /www - Open default web browser
/kibana - Open Kibana dashboard (localhost:5601)
/orleans or /monitor - Open Orleans monitoring (localhost:9090)
/nexai - Open Nexai website (Musuko)
/youtube - Open YouTube channel (@lelabia)

?? APPLICATION COMMANDS:
/settings - Open Settings
/word - Launch Microsoft Word (Windows 10/11)
/excel - Launch Microsoft Excel (Windows 10/11)
/mail or /outlook - Launch Microsoft Outlook (Windows 10/11)
/clear - Clear chat history
/exit or /bye - Exit application

Type any command and press Enter to execute it.";
                
                chatDisplay.AppendText($"\n?? Kaonashi: {helpMessage}\n");
                chatDisplay.ScrollToCaret();
                statusLabel.Text = "Help displayed";
                statusLabel.ForeColor = Color.Blue;
                return;
            }
            
            // Check for settings command
            if (message.Equals("/settings", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                try
                {
                    var settingsForm = new SettingsForm(config);
                    settingsForm.Show();
                    statusLabel.Text = "Settings form opened";
                    statusLabel.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Failed to open settings: {ex.Message}";
                    statusLabel.ForeColor = Color.Red;
                }
                return;
            }
            
            // Check for Word command
            if (message.Equals("/word", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                try
                {
                    // Check if running on Windows 10 or 11
                    var osVersion = Environment.OSVersion;
                    var isWindows10Or11 = osVersion.Platform == PlatformID.Win32NT && 
                                         (osVersion.Version.Major == 10);
                    
                    if (isWindows10Or11)
                    {
                        // Try to launch Microsoft Word
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "winword.exe",
                            UseShellExecute = true
                        });
                        statusLabel.Text = "Microsoft Word launched";
                        statusLabel.ForeColor = Color.Green;
                    }
                    else
                    {
                        statusLabel.Text = "Word command only available on Windows 10/11";
                        statusLabel.ForeColor = Color.Orange;
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Failed to launch Word: {ex.Message}";
                    statusLabel.ForeColor = Color.Red;
                }
                return;
            }
            
            // Check for Excel command
            if (message.Equals("/excel", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                try
                {
                    // Check if running on Windows 10 or 11
                    var osVersion = Environment.OSVersion;
                    var isWindows10Or11 = osVersion.Platform == PlatformID.Win32NT && 
                                         (osVersion.Version.Major == 10);
                    
                    if (isWindows10Or11)
                    {
                        // Try to launch Microsoft Excel
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "excel.exe",
                            UseShellExecute = true
                        });
                        statusLabel.Text = "Microsoft Excel launched";
                        statusLabel.ForeColor = Color.Green;
                    }
                    else
                    {
                        statusLabel.Text = "Excel command only available on Windows 10/11";
                        statusLabel.ForeColor = Color.Orange;
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Failed to launch Excel: {ex.Message}";
                    statusLabel.ForeColor = Color.Red;
                }
                return;
            }
            
            // Check for Outlook command
            if (message.Equals("/mail", StringComparison.OrdinalIgnoreCase) || 
                message.Equals("/outlook", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                try
                {
                    // Check if running on Windows 10 or 11
                    var osVersion = Environment.OSVersion;
                    var isWindows10Or11 = osVersion.Platform == PlatformID.Win32NT && 
                                         (osVersion.Version.Major == 10);
                    
                    if (isWindows10Or11)
                    {
                        // Try to launch Microsoft Outlook
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "outlook.exe",
                            UseShellExecute = true
                        });
                        statusLabel.Text = "Microsoft Outlook launched";
                        statusLabel.ForeColor = Color.Green;
                    }
                    else
                    {
                        statusLabel.Text = "Outlook command only available on Windows 10/11";
                        statusLabel.ForeColor = Color.Orange;
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Failed to launch Outlook: {ex.Message}";
                    statusLabel.ForeColor = Color.Red;
                }
                return;
            }
            
            // Check for YouTube command
            if (message.Equals("/youtube", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://www.youtube.com/@lelabia",
                        UseShellExecute = true
                    });
                    statusLabel.Text = "Opening YouTube channel...";
                    statusLabel.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Failed to open YouTube: {ex.Message}";
                    statusLabel.ForeColor = Color.Red;
                }
                return;
            }
            
            // Check for CV manager command
            if (message.Equals("/cv", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                var cvManagerForm = new CvManagerForm(config.CompletionHost, config.CompletionPort);
                cvManagerForm.Show();
                return;
            }
            
            // Check for exit command
            if (message.Equals("/exit", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                // Close the application
                Application.Exit();
                return;
            }
            
            // Check for bye command
            if (message.Equals("/bye", StringComparison.OrdinalIgnoreCase))
            {
                messageInput.Clear();
                // Close the application
                Application.Exit();
                return;
            }

            if (modelSelector.SelectedItem == null)
            {
                MessageBox.Show("Please select a model first.", "No Model Selected", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedModel = modelSelector.SelectedItem.ToString()!;

            // Add message to history for UP arrow navigation
            messageHistory.Add(message);
            historyIndex = -1;

            isProcessing = true;
            sendButton.Enabled = false;
            messageInput.Enabled = false;
            
            string completionUrl = $"http://{config.CompletionHost}:{config.CompletionPort}/v1/chat/completions";
            
            statusLabel.Text = "Thinking...";
            statusLabel.ForeColor = primaryColor;
            connectionIndicator.BackColor = primaryColor;
            statusBarLabel.Text = $"? Sending chat request...";
            statusBarLabel.ForeColor = primaryColor;
            statusBarConnectionLabel.Text = $"POST {completionUrl} (model: {selectedModel})";

            AppendUserMessage(message);
            messageInput.Clear();
            
            // Store user prompt for later saving with completion
            if (config.SaveInteractions)
            {
                currentUserPrompt = message;
            }

            DateTime startTime = DateTime.Now;

            try
            {
                // Prepare for assistant response
                AppendAssistantMessageStart();
                streamBuffer.Clear();
                assistantResponseBuffer.Clear();
                detectedCodeBlocks.Clear();
                
                // Reset to normal eyes at start of response
                LoadMarvinEyes("marvin-normal-eyes.png");

                await ollamaService.SendChatMessageAsync(
                    message, 
                    selectedModel, 
                    chatHistory,
                    (chunk) =>
                    {
                        // Update UI on the UI thread
                        this.Invoke((MethodInvoker)delegate
                        {
                            AppendAssistantChunk(chunk);
                        });
                    },
                    config.SystemPrompt
                );

                // Process any remaining content in buffer
                FlushStreamBuffer();
                
                // Detect and show code blocks in popup
                string fullResponse = assistantResponseBuffer.ToString();
                DetectAndShowCodeBlocks(fullResponse);
                
                // Update the chat display with formatted code blocks
                UpdateLastAssistantMessage(fullResponse);
                
                // Save interaction (prompt + completion) as JSON if enabled
                if (config.SaveInteractions && !string.IsNullOrEmpty(currentUserPrompt))
                {
                    SaveInteractionToJsonFile(currentUserPrompt, fullResponse);
                    currentUserPrompt = null; // Clear after saving
                }
                
                AppendAssistantMessageEnd();
                
                TimeSpan elapsed = DateTime.Now - startTime;
                
                statusLabel.Text = "Connected";
                statusLabel.ForeColor = accentColor;
                connectionIndicator.BackColor = accentColor;
                statusBarLabel.Text = "? Chat completion successful";
                statusBarLabel.ForeColor = accentColor;
                statusBarConnectionLabel.Text = $"POST {completionUrl} | Status: 200 OK | ? {elapsed.TotalSeconds:F1}s";
                await logService.LogAsync("DONE", $"Chat completion successful - Model: {selectedModel}, Time: {elapsed.TotalSeconds:F1}s");
                
                // Reset to ready after a short delay
                var resetTimer = new System.Windows.Forms.Timer();
                resetTimer.Interval = 3000;
                resetTimer.Tick += (s, args) =>
                {
                    statusBarLabel.Text = "? Ready";
                    statusBarConnectionLabel.Text = $"Models: http://{config.OllamaHost}:{config.OllamaPort} • Completion: http://{config.CompletionHost}:{config.CompletionPort}";
                    resetTimer.Stop();
                    resetTimer.Dispose();
                };
                resetTimer.Start();
            }
            catch (Exception ex)
            {
                AppendSystemMessage($"? Error: {ex.Message}");
                statusLabel.Text = "Error";
                statusLabel.ForeColor = errorColor;
                connectionIndicator.BackColor = errorColor;
                statusBarLabel.Text = $"? Chat completion failed";
                statusBarLabel.ForeColor = errorColor;
                statusBarConnectionLabel.Text = $"POST {completionUrl} | Error: {ex.Message}";
                await logService.LogAsync("ERROR", $"Chat completion failed - Model: {selectedModel}, Error: {ex.Message}");
            }
            finally
            {
                isProcessing = false;
                sendButton.Enabled = true;
                messageInput.Enabled = true;
                messageInput.Focus();
            }
        }

        /// <summary>
        /// Appends a user message to the chat display with formatted styling.
        /// </summary>
        /// <param name="message">The user's message text.</param>
        private void AppendUserMessage(string message)
        {
            chatDisplay.SelectionStart = chatDisplay.TextLength;
            chatDisplay.SelectionLength = 0;
            
            // Display user label with primary color
            chatDisplay.SelectionColor = primaryColor;
            chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 11, FontStyle.Bold);
            chatDisplay.AppendText("? You\n");
            
            // Display message text
            chatDisplay.SelectionColor = textColor;
            chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Regular);
            chatDisplay.AppendText(message + "\n\n");
            
            chatDisplay.ScrollToCaret();
        }

        /// <summary>
        /// Appends the assistant message header to the chat display.
        /// Called at the start of assistant response streaming.
        /// </summary>
        private void AppendAssistantMessageStart()
        {
            chatDisplay.SelectionStart = chatDisplay.TextLength;
            chatDisplay.SelectionLength = 0;
            
            // Display assistant label with accent color
            chatDisplay.SelectionColor = accentColor;
            chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 11, FontStyle.Bold);
            chatDisplay.AppendText("? Assistant\n");
            
            // Set text color for message content
            chatDisplay.SelectionColor = textColor;
            chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Regular);
        }

        /// <summary>
        /// Processes a chunk of assistant response during streaming.
        /// Handles think tag filtering, accumulates text for emotion detection,
        /// and buffers content for final formatted display.
        /// </summary>
        /// <param name="chunk">A chunk of text from the streaming response.</param>
        private void AppendAssistantChunk(string chunk)
        {
            string displayText = "";
            
            if (config.HideThinkTags)
            {
                // Add chunk to buffer for think tag processing
                streamBuffer.Append(chunk);
                
                // Process complete think tags from buffer
                // Think tags (<think>...</think>) are filtered out
                string processed = ProcessThinkTagsFromBuffer();
                displayText = processed;
                
                // Don't display during streaming - we'll format and display at the end
                // Just accumulate in the buffer
            }
            else
            {
                displayText = chunk;
                // Don't display during streaming - we'll format and display at the end
                // Just accumulate in the buffer
            }
            
            // Accumulate response for emotion detection and final display
            if (!string.IsNullOrEmpty(displayText))
            {
                assistantResponseBuffer.Append(displayText);
                DetectEmotionAndUpdateEyes();  // Update character eyes based on detected emotions
            }
        }

        /// <summary>
        /// Detects emotional keywords in the assistant response and updates the character's eyes accordingly.
        /// This provides visual feedback about the emotional tone of the conversation.
        /// </summary>
        private void DetectEmotionAndUpdateEyes()
        {
            string response = assistantResponseBuffer.ToString().ToLower();
            
            // Check for happy/positive keywords
            if (response.Contains("happy") || response.Contains("fun") || response.Contains("great"))
            {
                LoadMarvinEyes("marvin-happy-eyes.png");
            }
            // Additional emotion detection can be added here
        }

        /// <summary>
        /// Detects code blocks in the assistant response and shows them in a code viewer popup.
        /// Supports multiple programming languages (C#, Python, C++, etc.).
        /// </summary>
        /// <param name="response">The full assistant response text to analyze.</param>
        private void DetectAndShowCodeBlocks(string response)
        {
            try
            {
                // Regex to match markdown code blocks with language specification
                var codeBlockPattern = @"```(\w+)\s*\n([\s\S]*?)```";
                var matches = System.Text.RegularExpressions.Regex.Matches(response, codeBlockPattern);

                var supportedLanguages = new[] { "csharp", "cs", "c#", "python", "py", "cpp", "c++", "c" };
                var codeBlocksToShow = new List<(string code, string language)>();

                // Collect all code blocks first
                var matchList = matches.Cast<System.Text.RegularExpressions.Match>().ToList();

                foreach (var match in matchList)
                {
                    if (match.Groups.Count >= 3)
                    {
                        string language = match.Groups[1].Value.ToLower().Trim();
                        string code = match.Groups[2].Value.Trim();

                        // Check if it's a supported language
                        if (supportedLanguages.Contains(language) && !string.IsNullOrWhiteSpace(code))
                        {
                            // Check if we haven't shown this code block already
                            if (!detectedCodeBlocks.Any(cb => cb.code == code && cb.language == language))
                            {
                                detectedCodeBlocks.Add((code, language));
                                codeBlocksToShow.Add((code, language));
                            }
                        }
                    }
                }

                // If we have code blocks to show, merge them and show in one popup
                if (codeBlocksToShow.Count > 0)
                {
                    // Merge all code blocks with separators
                    var mergedCode = new System.Text.StringBuilder();
                    for (int i = 0; i < codeBlocksToShow.Count; i++)
                    {
                        var (code, language) = codeBlocksToShow[i];
                        
                        // Add separator comment
                        string separator = GetCommentSeparator(language, i + 1, codeBlocksToShow.Count);
                        mergedCode.AppendLine(separator);
                        mergedCode.AppendLine();
                        mergedCode.AppendLine(code);
                        
                        if (i < codeBlocksToShow.Count - 1)
                        {
                            mergedCode.AppendLine();
                            mergedCode.AppendLine();
                        }
                    }

                    // Determine the primary language (first one or "mixed" if multiple)
                    string displayLanguage = codeBlocksToShow.Count == 1 
                        ? codeBlocksToShow[0].language 
                        : "Mixed";

                    // Show single popup with all code
                    ShowCodeViewer(mergedCode.ToString().TrimEnd(), displayLanguage);
                }
            }
            catch (Exception ex)
            {
                // Silently fail - don't interrupt the chat flow
                Console.WriteLine($"Code detection error: {ex.Message}");
            }
        }

        private string GetCommentSeparator(string language, int index, int total)
        {
            string languageName = language.ToUpper();
            string header = total > 1 
                ? $"Code Block {index} of {total} - {languageName}" 
                : $"{languageName} Code";

            // Use appropriate comment syntax for the language
            language = language.ToLower();
            if (language == "csharp" || language == "cs" || language == "c#" || 
                language == "cpp" || language == "c++" || language == "c")
            {
                return $"// ========================================\n// {header}\n// ========================================";
            }
            else if (language == "python" || language == "py")
            {
                return $"# ========================================\n# {header}\n# ========================================";
            }
            else
            {
                return $"// ========================================\n// {header}\n// ========================================";
            }
        }

        private void ShowCodeViewer(string code, string language)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    var codeViewer = new CodeViewerForm(code, language);
                    codeViewer.Show(this);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error showing code viewer: {ex.Message}");
            }
        }

        private void FormatLastAssistantMessage(string response)
        {
            try
            {
                // Find the last assistant message in the chat display
                string currentText = chatDisplay.Text;
                int lastAssistantIndex = currentText.LastIndexOf("?? Assistant:\n");
                
                if (lastAssistantIndex >= 0)
                {
                    // Remove everything from the assistant marker to the end
                    chatDisplay.Select(lastAssistantIndex, chatDisplay.TextLength - lastAssistantIndex);
                    chatDisplay.SelectedText = "";
                    
                    // Re-append the assistant header
                    chatDisplay.SelectionStart = chatDisplay.TextLength;
                    chatDisplay.SelectionLength = 0;
                    chatDisplay.SelectionColor = accentColor;
                    chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 11, FontStyle.Bold);
                    chatDisplay.AppendText("?? Assistant:\n");
                    
                    // Parse and format the response with code blocks
                    FormatResponseWithCodeBlocks(response);
                    
                    chatDisplay.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error formatting last assistant message: {ex.Message}");
            }
        }

        private void FormatResponseWithCodeBlocks(string response)
        {
            // Regex to match markdown code blocks
            var codeBlockPattern = @"```(\w+)\s*\n([\s\S]*?)```";
            var matches = System.Text.RegularExpressions.Regex.Matches(response, codeBlockPattern);
            
            int lastIndex = 0;
            Color codeBackgroundColor = Color.FromArgb(40, 40, 40);
            Color codeTextColor = Color.FromArgb(220, 220, 220);
            
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                // Append text before code block (regular text)
                if (match.Index > lastIndex)
                {
                    string textBefore = response.Substring(lastIndex, match.Index - lastIndex);
                    chatDisplay.SelectionColor = textColor;
                    chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Regular);
                    chatDisplay.SelectionBackColor = backgroundColor;
                    chatDisplay.AppendText(textBefore);
                }
                
                // Append code block with special formatting
                if (match.Groups.Count >= 3)
                {
                    string language = match.Groups[1].Value.ToUpper();
                    string code = match.Groups[2].Value;
                    
                    // Add language label
                    chatDisplay.SelectionColor = Color.FromArgb(88, 101, 242);
                    chatDisplay.SelectionFont = new Font("Consolas", 9, FontStyle.Bold);
                    chatDisplay.SelectionBackColor = codeBackgroundColor;
                    chatDisplay.AppendText($"\n+- {language} Code -----\n");
                    
                    // Add code with monospace font
                    chatDisplay.SelectionColor = codeTextColor;
                    chatDisplay.SelectionFont = new Font("Consolas", 10, FontStyle.Regular);
                    chatDisplay.SelectionBackColor = codeBackgroundColor;
                    chatDisplay.AppendText(code);
                    
                    // Add bottom border
                    chatDisplay.SelectionColor = Color.FromArgb(88, 101, 242);
                    chatDisplay.SelectionFont = new Font("Consolas", 9, FontStyle.Bold);
                    chatDisplay.SelectionBackColor = codeBackgroundColor;
                    chatDisplay.AppendText("\n+-----------------\n");
                }
                
                lastIndex = match.Index + match.Length;
            }
            
            // Append remaining text after last code block
            if (lastIndex < response.Length)
            {
                string textAfter = response.Substring(lastIndex);
                chatDisplay.SelectionColor = textColor;
                chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Regular);
                chatDisplay.SelectionBackColor = backgroundColor;
                chatDisplay.AppendText(textAfter);
            }
        }

        private void UpdateLastAssistantMessage(string cleanedResponse)
        {
            try
            {
                // Since we don't display anything during streaming anymore,
                // we just need to append the formatted response after the "? Assistant\n" header
                // that was already added by AppendAssistantMessageStart()
                
                // Just append the formatted response directly
                AppendFormattedResponse(cleanedResponse);
                chatDisplay.ScrollToCaret();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating last assistant message: {ex.Message}");
            }
        }

        private void AppendFormattedResponse(string response)
        {
            // Parse response and format code blocks with syntax highlighting
            var codeBlockPattern = @"```(\w+)?\s*\n([\s\S]*?)```";
            var matches = System.Text.RegularExpressions.Regex.Matches(response, codeBlockPattern);
            
            int lastIndex = 0;
            
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                // Append text before code block with markdown formatting
                if (match.Index > lastIndex)
                {
                    string textBefore = response.Substring(lastIndex, match.Index - lastIndex);
                    AppendMarkdownFormattedText(textBefore);
                }
                
                // Format the code block
                if (match.Groups.Count >= 3)
                {
                    string language = match.Groups[1].Success ? match.Groups[1].Value.ToLower().Trim() : "text";
                    string code = match.Groups[2].Value.TrimEnd();
                    
                    if (string.IsNullOrWhiteSpace(language))
                        language = "text";
                    
                    // Add code block header
                    chatDisplay.SelectionColor = Color.FromArgb(100, 100, 100);
                    chatDisplay.SelectionFont = new Font("Consolas", 9, FontStyle.Italic);
                    chatDisplay.AppendText($"\n+- {language.ToUpper()} Code -------------\n");
                    
                    // Apply syntax highlighting to the code
                    ApplyCodeSyntaxHighlighting(code, language);
                    
                    // Add code block footer
                    chatDisplay.SelectionColor = Color.FromArgb(100, 100, 100);
                    chatDisplay.SelectionFont = new Font("Consolas", 9, FontStyle.Italic);
                    chatDisplay.AppendText("\n+--------------------------------\n");
                }
                
                lastIndex = match.Index + match.Length;
            }
            
            // Append remaining text after last code block with markdown formatting
            if (lastIndex < response.Length)
            {
                string textAfter = response.Substring(lastIndex);
                AppendMarkdownFormattedText(textAfter);
            }
        }

        private void AppendMarkdownFormattedText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            // Process markdown elements: headers, bold, italic, inline code, lists
            var lines = text.Split('\n');
            
            foreach (var line in lines)
            {
                ProcessMarkdownLine(line);
                if (line != lines[lines.Length - 1])
                    chatDisplay.AppendText("\n");
            }
        }

        private void ProcessMarkdownLine(string line)
        {
            // Check for headers (# Header)
            var headerMatch = System.Text.RegularExpressions.Regex.Match(line, @"^(#{1,6})\s+(.+)$");
            if (headerMatch.Success)
            {
                int headerLevel = headerMatch.Groups[1].Value.Length;
                string headerText = headerMatch.Groups[2].Value;
                
                chatDisplay.SelectionColor = accentColor;
                chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 
                    10.5f + (7 - headerLevel) * 1.5f, FontStyle.Bold);
                chatDisplay.AppendText(headerText);
                return;
            }

            // Check for bullet points
            var bulletMatch = System.Text.RegularExpressions.Regex.Match(line, @"^(\s*)[-*+]\s+(.+)$");
            if (bulletMatch.Success)
            {
                string indent = bulletMatch.Groups[1].Value;
                string bulletText = bulletMatch.Groups[2].Value;
                
                chatDisplay.SelectionColor = accentColor;
                chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Regular);
                chatDisplay.AppendText(indent + "• ");
                
                ProcessInlineMarkdown(bulletText);
                return;
            }

            // Check for numbered lists
            var numberedMatch = System.Text.RegularExpressions.Regex.Match(line, @"^(\s*)(\d+)\.\s+(.+)$");
            if (numberedMatch.Success)
            {
                string indent = numberedMatch.Groups[1].Value;
                string number = numberedMatch.Groups[2].Value;
                string listText = numberedMatch.Groups[3].Value;
                
                chatDisplay.SelectionColor = accentColor;
                chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Bold);
                chatDisplay.AppendText(indent + number + ". ");
                
                ProcessInlineMarkdown(listText);
                return;
            }

            // Regular text with inline markdown
            ProcessInlineMarkdown(line);
        }

        private void ProcessInlineMarkdown(string text)
        {
            // Pattern for: **bold**, __bold__, *italic*, _italic_, `code`
            // Process in order: code first (to avoid conflicts), then bold, then italic
            
            var pattern = @"(`[^`]+`)|(\*\*[^*]+\*\*)|(__[^_]+__)|(\*[^*]+\*)|(_[^_]+_)";
            var matches = System.Text.RegularExpressions.Regex.Matches(text, pattern);
            
            int lastIndex = 0;
            
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                // Append text before this match
                if (match.Index > lastIndex)
                {
                    string textBefore = text.Substring(lastIndex, match.Index - lastIndex);
                    chatDisplay.SelectionColor = textColor;
                    chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Regular);
                    chatDisplay.AppendText(textBefore);
                }
                
                string matchedText = match.Value;
                
                // Inline code: `code`
                if (matchedText.StartsWith("`") && matchedText.EndsWith("`"))
                {
                    string code = matchedText.Substring(1, matchedText.Length - 2);
                    chatDisplay.SelectionColor = Color.FromArgb(206, 145, 120);
                    chatDisplay.SelectionBackColor = Color.FromArgb(45, 45, 45);
                    chatDisplay.SelectionFont = new Font("Consolas", 10, FontStyle.Regular);
                    chatDisplay.AppendText(code);
                    chatDisplay.SelectionBackColor = chatDisplay.BackColor; // Reset background
                }
                // Bold: **text** or __text__
                else if ((matchedText.StartsWith("**") && matchedText.EndsWith("**")) ||
                         (matchedText.StartsWith("__") && matchedText.EndsWith("__")))
                {
                    string boldText = matchedText.Substring(2, matchedText.Length - 4);
                    chatDisplay.SelectionColor = textColor;
                    chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Bold);
                    chatDisplay.AppendText(boldText);
                }
                // Italic: *text* or _text_
                else if ((matchedText.StartsWith("*") && matchedText.EndsWith("*")) ||
                         (matchedText.StartsWith("_") && matchedText.EndsWith("_")))
                {
                    string italicText = matchedText.Substring(1, matchedText.Length - 2);
                    chatDisplay.SelectionColor = textColor;
                    chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Italic);
                    chatDisplay.AppendText(italicText);
                }
                
                lastIndex = match.Index + match.Length;
            }
            
            // Append remaining text
            if (lastIndex < text.Length)
            {
                string textAfter = text.Substring(lastIndex);
                chatDisplay.SelectionColor = textColor;
                chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 10.5f, FontStyle.Regular);
                chatDisplay.AppendText(textAfter);
            }
        }

        private void ApplyCodeSyntaxHighlighting(string code, string language)
        {
            // Color scheme (matching CodeViewerForm)
            Color keywordColor = Color.FromArgb(86, 156, 214);      // Blue
            Color stringColor = Color.FromArgb(206, 145, 120);      // Orange
            Color commentColor = Color.FromArgb(106, 153, 85);      // Green
            Color numberColor = Color.FromArgb(181, 206, 168);      // Light green
            Color codeTextColor = Color.FromArgb(220, 220, 220);    // Light gray for code
            
            // Get keywords for the language
            string[] keywords = GetKeywordsForLanguage(language);
            
            // Split code into lines and process each line
            string[] lines = code.Split('\n');
            
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int lineStartPos = chatDisplay.TextLength;
                
                // Append the line first
                chatDisplay.SelectionColor = codeTextColor;
                chatDisplay.SelectionFont = new Font("Consolas", 10);
                chatDisplay.AppendText(line);
                
                if (i < lines.Length - 1)
                {
                    chatDisplay.AppendText("\n");
                }
                
                // Now apply highlighting to this line
                int lineLength = line.Length;
                
                // Highlight keywords
                foreach (var keyword in keywords)
                {
                    var keywordPattern = $@"\b{Regex.Escape(keyword)}\b";
                    var keywordMatches = Regex.Matches(line, keywordPattern);
                    foreach (System.Text.RegularExpressions.Match m in keywordMatches)
                    {
                        chatDisplay.Select(lineStartPos + m.Index, m.Length);
                        chatDisplay.SelectionColor = keywordColor;
                    }
                }
                
                // Highlight strings
                var stringMatches = Regex.Matches(line, @"""[^""\\]*(?:\\.[^""\\]*)*""|'[^'\\]*(?:\\.[^'\\]*)*'");
                foreach (System.Text.RegularExpressions.Match m in stringMatches)
                {
                    chatDisplay.Select(lineStartPos + m.Index, m.Length);
                    chatDisplay.SelectionColor = stringColor;
                }
                
                // Highlight comments
                var commentMatches = Regex.Matches(line, @"//.*$|#.*$");
                foreach (System.Text.RegularExpressions.Match m in commentMatches)
                {
                    chatDisplay.Select(lineStartPos + m.Index, m.Length);
                    chatDisplay.SelectionColor = commentColor;
                }
                
                // Highlight numbers
                var numberMatches = Regex.Matches(line, @"\b\d+\.?\d*\b");
                foreach (System.Text.RegularExpressions.Match m in numberMatches)
                {
                    chatDisplay.Select(lineStartPos + m.Index, m.Length);
                    chatDisplay.SelectionColor = numberColor;
                }
            }
            
            // Reset selection to end
            chatDisplay.SelectionStart = chatDisplay.TextLength;
            chatDisplay.SelectionLength = 0;
        }

        private string[] GetKeywordsForLanguage(string language)
        {
            language = language.ToLower();

            if (language == "csharp" || language == "cs" || language == "c#")
            {
                return new[] { "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", 
                    "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", 
                    "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", 
                    "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", 
                    "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", 
                    "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", 
                    "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", 
                    "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "var", "virtual", 
                    "void", "volatile", "while", "async", "await", "get", "set", "value", "partial", "where" };
            }
            else if (language == "python" || language == "py")
            {
                return new[] { "False", "None", "True", "and", "as", "assert", "async", "await", "break", 
                    "class", "continue", "def", "del", "elif", "else", "except", "finally", "for", "from", 
                    "global", "if", "import", "in", "is", "lambda", "nonlocal", "not", "or", "pass", "raise", 
                    "return", "try", "while", "with", "yield", "self", "print", "range", "len", "str", "int", 
                    "float", "list", "dict", "tuple", "set" };
            }
            else if (language == "cpp" || language == "c++" || language == "c")
            {
                return new[] { "alignas", "alignof", "and", "and_eq", "asm", "auto", "bitand", "bitor", 
                    "bool", "break", "case", "catch", "char", "char16_t", "char32_t", "class", "compl", 
                    "const", "constexpr", "const_cast", "continue", "decltype", "default", "delete", "do", 
                    "double", "dynamic_cast", "else", "enum", "explicit", "export", "extern", "false", "float", 
                    "for", "friend", "goto", "if", "inline", "int", "long", "mutable", "namespace", "new", 
                    "noexcept", "not", "not_eq", "nullptr", "operator", "or", "or_eq", "private", "protected", 
                    "public", "register", "reinterpret_cast", "return", "short", "signed", "sizeof", "static", 
                    "static_assert", "static_cast", "struct", "switch", "template", "this", "thread_local", 
                    "throw", "true", "try", "typedef", "typeid", "typename", "union", "unsigned", "using", 
                    "virtual", "void", "volatile", "wchar_t", "while", "xor", "xor_eq", "override", "final", 
                    "include", "define", "ifdef", "ifndef", "endif", "pragma" };
            }
            
            return new string[0];
        }

        private void LoadMarvinEyes(string eyesFileName)
        {
            try
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", eyesFileName);
                if (File.Exists(imagePath))
                {
                    // Dispose old image if exists
                    if (marvinEyesPictureBox.Image != null)
                    {
                        var oldImage = marvinEyesPictureBox.Image;
                        marvinEyesPictureBox.Image = null;
                        oldImage.Dispose();
                    }
                    
                    marvinEyesPictureBox.Image = Image.FromFile(imagePath);
                }
            }
            catch
            {
                // Image loading failed, leave current image
            }
        }

        private void SaveInteractionToJsonFile(string prompt, string completion)
        {
            try
            {
                // Get current datetime in format: YYYY-MM-DD_HHmmss
                string dateTimeString = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
                string fileName = $"{dateTimeString}.completion.json";
                
                // Determine save directory
                string directory;
                if (!string.IsNullOrWhiteSpace(config.PromptFolder) && Directory.Exists(config.PromptFolder))
                {
                    // Use configured prompt folder
                    directory = config.PromptFolder;
                }
                else
                {
                    // Fallback to default AppData folder
                    directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Localhost.AI.Kaonashi");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }
                
                string filePath = Path.Combine(directory, fileName);
                
                // Get local IP address
                string localIp = GetLocalIPAddress();
                
                // Create JSON structure
                var interaction = new
                {
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    modelName = modelSelector.SelectedItem?.ToString() ?? config.DefaultModel,
                    userName = Environment.UserName,
                    machineName = Environment.MachineName,
                    ip = localIp,
                    prompt = prompt,
                    completion = completion
                };
                
                // Serialize to JSON with formatting
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(interaction, Newtonsoft.Json.Formatting.Indented);
                
                // Write to file
                File.WriteAllText(filePath, jsonContent);
                
                System.Diagnostics.Debug.WriteLine($"Saved interaction to: {filePath}");
            }
            catch (Exception ex)
            {
                // Log error for debugging
                System.Diagnostics.Debug.WriteLine($"Failed to save interaction: {ex.Message}");
                MessageBox.Show($"Failed to save interaction: {ex.Message}\n\nPlease check the prompt folder path in settings.", 
                               "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                string localIP = "";
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
                
                return !string.IsNullOrEmpty(localIP) ? localIP : "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        /// <summary>
        /// Processes the stream buffer to remove think tags (<think>...</think>).
        /// Think tags contain internal reasoning that should not be displayed to users.
        /// Handles incomplete tags that may span multiple chunks.
        /// </summary>
        /// <returns>Processed text with think tags removed.</returns>
        private string ProcessThinkTagsFromBuffer()
        {
            string bufferContent = streamBuffer.ToString();
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            
            int searchStart = 0;
            
            while (searchStart < bufferContent.Length)
            {
                // Find opening think tag
                int thinkStart = bufferContent.IndexOf("<think>", searchStart, StringComparison.OrdinalIgnoreCase);
                
                if (thinkStart == -1)
                {
                    // No more think tags found
                    // Check if there might be an incomplete tag at the end
                    string remaining = bufferContent.Substring(searchStart);
                    
                    // Look for potential incomplete tag at the very end
                    bool hasIncompleteTag = false;
                    for (int i = remaining.Length - 1; i >= Math.Max(0, remaining.Length - 7); i--)
                    {
                        string suffix = remaining.Substring(i);
                        if ("<think>".StartsWith(suffix, StringComparison.OrdinalIgnoreCase))
                        {
                            // Found potential incomplete opening tag - keep it in buffer for next chunk
                            output.Append(remaining.Substring(0, i));
                            streamBuffer.Clear();
                            streamBuffer.Append(suffix);
                            hasIncompleteTag = true;
                            break;
                        }
                    }
                    
                    if (!hasIncompleteTag)
                    {
                        // No incomplete tag, output everything and clear buffer
                        output.Append(remaining);
                        streamBuffer.Clear();
                    }
                    
                    return output.ToString();
                }
                
                // Add content before <think> tag (this is what we want to display)
                output.Append(bufferContent.Substring(searchStart, thinkStart - searchStart));
                
                // Look for closing tag
                int thinkEnd = bufferContent.IndexOf("</think>", thinkStart, StringComparison.OrdinalIgnoreCase);
                
                if (thinkEnd == -1)
                {
                    // Incomplete think block, keep everything from <think> onwards in buffer
                    streamBuffer.Clear();
                    streamBuffer.Append(bufferContent.Substring(thinkStart));
                    return output.ToString();
                }
                
                // Skip the entire think block (including both tags and content)
                // Move searchStart to right after </think>
                searchStart = thinkEnd + "</think>".Length;
            }
            
            // Clear buffer - everything has been processed
            streamBuffer.Clear();
            return output.ToString();
        }

        private void FlushStreamBuffer()
        {
            if (streamBuffer.Length > 0)
            {
                // Remove any remaining incomplete think tags
                string remaining = streamBuffer.ToString();
                remaining = Regex.Replace(remaining, @"<think>.*", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                
                if (!string.IsNullOrEmpty(remaining))
                {
                    // Add to assistant response buffer instead of displaying directly
                    assistantResponseBuffer.Append(remaining);
                }
                
                streamBuffer.Clear();
            }
        }

        private void AppendAssistantMessageEnd()
        {
            chatDisplay.AppendText("\n\n");
            chatDisplay.ScrollToCaret();
        }

        private void AppendSystemMessage(string message)
        {
            chatDisplay.SelectionStart = chatDisplay.TextLength;
            chatDisplay.SelectionLength = 0;
            
            chatDisplay.SelectionColor = Color.FromArgb(150, 150, 150);
            chatDisplay.SelectionFont = new Font(chatDisplay.Font.FontFamily, 9.5f, FontStyle.Italic);
            chatDisplay.AppendText($"{message}\n");
            
            chatDisplay.ScrollToCaret();
        }

        private void LoadPromptButton_Click(object? sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    openFileDialog.Title = "Load Prompt from File";
                    openFileDialog.RestoreDirectory = true;

                    // Set initial directory to the configured prompt folder if it exists
                    if (!string.IsNullOrWhiteSpace(config.PromptFolder) && 
                        Directory.Exists(config.PromptFolder))
                    {
                        openFileDialog.InitialDirectory = config.PromptFolder;
                    }

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileContent = File.ReadAllText(openFileDialog.FileName);
                        messageInput.Text = fileContent;
                        messageInput.Focus();
                        messageInput.SelectionStart = messageInput.Text.Length;
                        logService.Log("INFO", $"Loaded prompt from file: {Path.GetFileName(openFileDialog.FileName)}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load file: {ex.Message}", "Load Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                logService.Log("ERROR", $"Error loading prompt file: {ex.Message}");
            }
        }

        private void ModelSelector_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (modelSelector.SelectedItem != null)
            {
                string selectedModel = modelSelector.SelectedItem.ToString()!;
                config.LastModelUsed = selectedModel;
                config.Save();
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to clear the chat history?", 
                                        "Clear Chat", 
                                        MessageBoxButtons.YesNo, 
                                        MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                chatDisplay.Clear();
                chatHistory.Clear();
                messageHistory.Clear();
                historyIndex = -1;
                AppendSystemMessage("?? Chat history cleared.\n");
                logService.Log("INFO", "Chat history cleared");
            }
        }

        private void SettingsButton_Click(object? sender, EventArgs e)
        {
            using var settingsForm = new SettingsForm(config);
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                config = settingsForm.GetUpdatedConfig();
                config.Save();
                
                // Update service with new settings
                ollamaService.UpdateConnectionSettings(config.OllamaHost, config.OllamaPort,
                                                      config.CompletionHost, config.CompletionPort);
                logService = new LogService(config.CompletionHost, config.CompletionPort);
                
                // Reload models
                _ = LoadAvailableModels();
                
                logService.Log("INFO", "Settings updated and saved");
            }
        }

        private void ChatDisplay_LinkClicked(object? sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = e.LinkText,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open link: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NewsTickerPanel_Paint(object? sender, PaintEventArgs e)
        {
            if (newsTickerPanel == null) return;
            
            System.Diagnostics.Debug.WriteLine($"NewsTickerPanel_Paint: Called, newsItems count: {newsItems?.Count ?? 0}");
            
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            
            // Create professional stock market gradient background
            var rect = new Rectangle(0, 0, newsTickerPanel.Width, newsTickerPanel.Height);
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(rect, 
                Color.FromArgb(25, 25, 30), Color.FromArgb(15, 15, 20), 0f))
            {
                g.FillRectangle(brush, rect);
            }
            
            // Add subtle top border
            using (var pen = new Pen(Color.FromArgb(60, 60, 65), 1))
            {
                g.DrawLine(pen, 0, 0, newsTickerPanel.Width, 0);
            }
            
            // Add subtle bottom border
            using (var pen = new Pen(Color.FromArgb(35, 35, 40), 1))
            {
                g.DrawLine(pen, 0, newsTickerPanel.Height - 1, newsTickerPanel.Width, newsTickerPanel.Height - 1);
            }
            
            if (newsItems != null && newsItems.Count > 0)
            {
                // Draw professional stock market header
                var headerFont = new Font("Segoe UI", 7, FontStyle.Bold);
                var headerBrush = new SolidBrush(Color.FromArgb(160, 160, 165));
                g.DrawString("MARKET NEWS", headerFont, headerBrush, 8, 1);
                headerFont.Dispose();
                headerBrush.Dispose();
                
                // Draw live indicator
                var liveFont = new Font("Segoe UI", 7, FontStyle.Bold);
                var liveBrush = new SolidBrush(Color.FromArgb(46, 204, 113));
                g.DrawString("? LIVE", liveFont, liveBrush, newsTickerPanel.Width - 45, 1);
                liveFont.Dispose();
                liveBrush.Dispose();
                
                // Calculate total width needed for all news items
                int totalWidth = 0;
                var newsTexts = new List<(string text, Color color, int width, string symbol)>();
                
                foreach (var news in newsItems)
                {
                    // Determine color and symbol based on rating (professional stock colors)
                    Color newsColor;
                    string symbol;
                    string changeIndicator;
                    
                    if (news.Rating < 0)
                    {
                        newsColor = Color.FromArgb(239, 83, 80); // Professional red
                        symbol = "?";
                        changeIndicator = $"{news.Rating}";
                    }
                    else if (news.Rating > 0)
                    {
                        newsColor = Color.FromArgb(46, 204, 113); // Professional green
                        symbol = "?";
                        changeIndicator = $"+{news.Rating}";
                    }
                    else
                    {
                        newsColor = Color.FromArgb(189, 195, 199); // Professional gray
                        symbol = "?";
                        changeIndicator = $"{news.Rating}";
                    }
                    
                    // Format news text like professional stock ticker
                    var newsText = $"{symbol} {news.Title} {changeIndicator}";
                    var textSize = g.MeasureString(newsText + "  |  ", new Font("Segoe UI", 9, FontStyle.Regular));
                    var textWidth = (int)textSize.Width;
                    
                    newsTexts.Add((newsText, newsColor, textWidth, symbol));
                    totalWidth += textWidth;
                }
                
                // Add separator space (100px per separator)
                totalWidth += 100 * newsItems.Count;
                newsTextWidth = totalWidth;
                
                // Draw scrolling news items
                var x = newsTickerPanel.Width - newsScrollPosition;
                var y = 14; // Start below header
                
                // Reset position if scrolled past
                if (x < -totalWidth)
                {
                    x = newsTickerPanel.Width;
                    newsScrollPosition = 0;
                }
                
                // Draw each news item with professional styling
                int currentX = x;
                var font = new Font("Segoe UI", 9, FontStyle.Regular);
                
                foreach (var (text, color, width, symbol) in newsTexts)
                {
                    // Add subtle background highlight for active items
                    if (symbol != "?")
                    {
                        using (var highlightBrush = new SolidBrush(Color.FromArgb(color.R, color.G, color.B, 15)))
                        {
                            g.FillRectangle(highlightBrush, currentX - 2, y - 1, width + 4, 12);
                        }
                    }
                    
                    var brush = new SolidBrush(color);
                    
                    // Draw text with subtle shadow for depth
                    using (var shadowBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 80)))
                    {
                        g.DrawString(text, font, shadowBrush, currentX + 1, y + 1);
                    }
                    g.DrawString(text, font, brush, currentX, y);
                    
                    currentX += width;
                    
                    // Draw professional separator
                    var separatorBrush = new SolidBrush(Color.FromArgb(80, 80, 85));
                    g.DrawString("  |  ", font, separatorBrush, currentX, y);
                    currentX += 100;
                    
                    brush.Dispose();
                    separatorBrush.Dispose();
                }
                
                // If content is shorter than panel width, repeat it
                if (totalWidth < newsTickerPanel.Width)
                {
                    currentX = x + totalWidth;
                    foreach (var (text, color, width, symbol) in newsTexts)
                    {
                        // Add subtle background highlight
                        if (symbol != "?")
                        {
                            using (var highlightBrush = new SolidBrush(Color.FromArgb(color.R, color.G, color.B, 15)))
                            {
                                g.FillRectangle(highlightBrush, currentX - 2, y - 1, width + 4, 12);
                            }
                        }
                        
                        var brush = new SolidBrush(color);
                        
                        // Draw with shadow
                        using (var shadowBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 80)))
                        {
                            g.DrawString(text, font, shadowBrush, currentX + 1, y + 1);
                        }
                        g.DrawString(text, font, brush, currentX, y);
                        
                        currentX += width;
                        
                        var separatorBrush = new SolidBrush(Color.FromArgb(80, 80, 85));
                        g.DrawString("  |  ", font, separatorBrush, currentX, y);
                        currentX += 100;
                        
                        brush.Dispose();
                        separatorBrush.Dispose();
                    }
                }
                
                font.Dispose();
            }
            else
            {
                // Show professional loading message
                var font = new Font("Segoe UI", 9, FontStyle.Regular);
                var brush = new SolidBrush(Color.FromArgb(140, 140, 145));
                var text = newsItems == null ? "? Loading market data..." : "? No market data available";
                
                // Center the message
                var textSize = g.MeasureString(text, font);
                var x = (newsTickerPanel.Width - textSize.Width) / 2;
                var y = (newsTickerPanel.Height - textSize.Height) / 2;
                
                g.DrawString(text, font, brush, x, y);
                font.Dispose();
                brush.Dispose();
            }
        }

        private async Task LoadNewsForBanner()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"LoadNewsForBanner: Starting... Connecting to http://{config.CompletionHost}:{config.CompletionPort}/news");
                if (newsService == null) 
                {
                    System.Diagnostics.Debug.WriteLine("LoadNewsForBanner: newsService is null");
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine("LoadNewsForBanner: Calling GetNewsAsync...");
                var news = await newsService.GetNewsAsync("test");
                System.Diagnostics.Debug.WriteLine($"LoadNewsForBanner: Got {news?.Count ?? 0} news items");
                
                if (news != null && news.Count > 0)
                {
                    newsItems = news;
                    System.Diagnostics.Debug.WriteLine($"LoadNewsForBanner: Set newsItems, starting timers");
                    
                    // Start the scroll timer
                    newsScrollTimer?.Start();
                    newsRefreshTimer?.Start();
                    
                    // Force panel redraw
                    newsTickerPanel?.Invalidate();
                }
                else
                {
                    newsItems = new List<News>(); // Clear news items
                    System.Diagnostics.Debug.WriteLine("LoadNewsForBanner: No news items, clearing list");
                    newsTickerPanel?.Invalidate();
                }
            }
            catch (Exception ex)
            {
                newsItems = new List<News>(); // Clear news items on error
                newsTickerPanel?.Invalidate();
                System.Diagnostics.Debug.WriteLine($"Failed to load news for banner: {ex.Message}");
            }
        }

        private void NewsScrollTimer_Tick(object? sender, EventArgs e)
        {
            newsScrollPosition += 4; // Scroll speed (2x faster)
            newsTickerPanel?.Invalidate();
        }

        private void NewsRefreshTimer_Tick(object? sender, EventArgs e)
        {
            _ = LoadNewsForBanner();
        }

        private void CleanupNewsBanner()
        {
            try
            {
                newsScrollTimer?.Stop();
                newsScrollTimer?.Dispose();
                newsRefreshTimer?.Stop();
                newsRefreshTimer?.Dispose();
                newsService?.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cleaning up news banner: {ex.Message}");
            }
        }

        public string GetSelectedModel()
        {
            return modelSelector.SelectedItem?.ToString() ?? "llama3.2";
        }
    }
}

