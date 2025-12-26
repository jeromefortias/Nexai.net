using System;
using System.Drawing;
using System.Windows.Forms;

namespace Musuko.Kaonashi
{
    public partial class SettingsForm : Form
    {
        private Panel headerPanel;
        private Panel contentPanel;
        private Panel footerPanel;
        private Label titleLabel;
        private Label hostLabel;
        private TextBox hostTextBox;
        private Label portLabel;
        private NumericUpDown portNumericUpDown;
        private Label completionHostLabel;
        private TextBox completionHostTextBox;
        private Label completionPortLabel;
        private NumericUpDown completionPortNumericUpDown;
        private Label modelLabel;
        private TextBox modelTextBox;
        private Label systemPromptLabel;
        private TextBox systemPromptTextBox;
        private CheckBox hideThinkTagsCheckBox;
        private CheckBox saveInteractionsCheckBox;
        private Label promptFolderLabel;
        private TextBox promptFolderTextBox;
        private Button browseFolderButton;
        private Button saveButton;
        private Button cancelButton;
        private Label infoLabel;

        private AppConfig config;

        // Modern color scheme matching MainForm
        private readonly Color backgroundColor = Color.FromArgb(54, 57, 63);
        private readonly Color secondaryBg = Color.FromArgb(47, 49, 54);
        private readonly Color inputBg = Color.FromArgb(64, 68, 75);
        private readonly Color textColor = Color.FromArgb(220, 221, 222);
        private readonly Color primaryColor = Color.FromArgb(88, 101, 242);
        private readonly Color primaryHover = Color.FromArgb(71, 82, 196);

        // Parameterless constructor for Visual Studio Designer
        public SettingsForm() : this(new AppConfig())
        {
        }

        public SettingsForm(AppConfig currentConfig)
        {
            config = new AppConfig
            {
                OllamaHost = currentConfig.OllamaHost,
                OllamaPort = currentConfig.OllamaPort,
                CompletionHost = currentConfig.CompletionHost,
                CompletionPort = currentConfig.CompletionPort,
                DefaultModel = currentConfig.DefaultModel,
                SystemPrompt = currentConfig.SystemPrompt,
                HideThinkTags = currentConfig.HideThinkTags,
                SaveInteractions = currentConfig.SaveInteractions,
                PromptFolder = currentConfig.PromptFolder
            };

            InitializeComponent();
            IconHelper.SetFormIcon(this);
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            hostTextBox.Text = config.OllamaHost;
            portNumericUpDown.Value = config.OllamaPort;
            completionHostTextBox.Text = config.CompletionHost;
            completionPortNumericUpDown.Value = config.CompletionPort;
            modelTextBox.Text = config.DefaultModel;
            systemPromptTextBox.Text = config.SystemPrompt;
            hideThinkTagsCheckBox.Checked = config.HideThinkTags;
            saveInteractionsCheckBox.Checked = config.SaveInteractions;
            promptFolderTextBox.Text = config.PromptFolder;
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(hostTextBox.Text))
            {
                MessageBox.Show("Please enter a valid host address.", "Invalid Host", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                hostTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(modelTextBox.Text))
            {
                MessageBox.Show("Please enter a default model name.", "Invalid Model", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                modelTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(systemPromptTextBox.Text))
            {
                MessageBox.Show("Please enter a system prompt.", "Invalid System Prompt", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                systemPromptTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(completionHostTextBox.Text))
            {
                MessageBox.Show("Please enter a valid completion host address.", "Invalid Completion Host", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                completionHostTextBox.Focus();
                return;
            }

            // Update config
            config.OllamaHost = hostTextBox.Text.Trim();
            config.OllamaPort = (int)portNumericUpDown.Value;
            config.CompletionHost = completionHostTextBox.Text.Trim();
            config.CompletionPort = (int)completionPortNumericUpDown.Value;
            config.DefaultModel = modelTextBox.Text.Trim();
            config.SystemPrompt = systemPromptTextBox.Text.Trim();
            config.HideThinkTags = hideThinkTagsCheckBox.Checked;
            config.SaveInteractions = saveInteractionsCheckBox.Checked;
            config.PromptFolder = promptFolderTextBox.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BrowseFolderButton_Click(object? sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select default folder for prompts";
                folderDialog.ShowNewFolderButton = true;
                
                // Set initial directory if one is already specified
                if (!string.IsNullOrWhiteSpace(promptFolderTextBox.Text) && 
                    Directory.Exists(promptFolderTextBox.Text))
                {
                    folderDialog.SelectedPath = promptFolderTextBox.Text;
                }

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    promptFolderTextBox.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public AppConfig GetUpdatedConfig()
        {
            return config;
        }
    }
}

