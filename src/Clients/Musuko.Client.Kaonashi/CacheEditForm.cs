using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Musuko.Kaonashi
{
    public partial class CacheEditForm : Form
    {
        private readonly string baseUrl;
        private readonly HttpClient httpClient;
        private Cache cacheObject;
        private bool isNewCache;

        public CacheEditForm(string host, int port, Cache? cache = null)
        {
            InitializeComponent();
            baseUrl = $"http://{host}:{port}";
            httpClient = new HttpClient();
            
            // Set form properties
            this.Text = cache == null ? "New Cache" : "Edit Cache";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Initialize cache object
            if (cache == null)
            {
                cacheObject = new Cache();
                isNewCache = true;
            }
            else
            {
                cacheObject = cache;
                isNewCache = false;
            }
            
            // Load cache data into form
            LoadCacheData();
            
            // Wire up form closing event
            this.FormClosing += CacheEditForm_FormClosing;
        }

        private void LoadCacheData()
        {
            try
            {
                // Basic properties
                idTextBox.Text = cacheObject.Id;
                dateTextBox.Text = cacheObject.Date.ToString("yyyy-MM-dd HH:mm:ss");
                machineNameTextBox.Text = cacheObject.MachineName ?? "";
                userNameTextBox.Text = cacheObject.UserName ?? "";
                commentTextBox.Text = cacheObject.Comment ?? "";
                
                // Cache-specific properties
                promptTextBox.Text = cacheObject.prompt ?? "";
                completionTextBox.Text = cacheObject.completion ?? "";
                languageTextBox.Text = cacheObject.language ?? "fr";
                modelTextBox.Text = cacheObject.model ?? "mistral-small3.1";
                chatModeComboBox.Text = cacheObject.chatmode ?? "completion";
                parentCacheIdTextBox.Text = cacheObject.ParentCacheId ?? "";
                generatedSystemPromptTextBox.Text = cacheObject.generatedSystemPrompt ?? "";
                
                // Tags
                LoadTagsToListBox(tagsMustListBox, cacheObject.tagsMust);
                LoadTagsToListBox(tagsShouldListBox, cacheObject.tagsShould);
                LoadTagsToListBox(tagsMustNotListBox, cacheObject.tagsMustNot);
                LoadTagsToListBox(tagsShouldNotListBox, cacheObject.tagsShouldNot);
                LoadTagsToListBox(generatedTagsListBox, cacheObject.generatedTags);
                
                // Enable/disable the "Add Child" button based on whether we have a valid ID
                addChildButton.Enabled = !string.IsNullOrEmpty(cacheObject.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cache data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTagsToListBox(ListBox listBox, List<string> tags)
        {
            listBox.Items.Clear();
            foreach (var tag in tags)
            {
                listBox.Items.Add(tag);
            }
        }

        private void SaveCacheData()
        {
            try
            {
                // Basic properties
                cacheObject.Id = idTextBox.Text;
                if (DateTime.TryParse(dateTextBox.Text, out DateTime date))
                {
                    cacheObject.Date = date;
                }
                cacheObject.MachineName = machineNameTextBox.Text;
                cacheObject.UserName = userNameTextBox.Text;
                cacheObject.Comment = commentTextBox.Text;
                
                // Cache-specific properties
                cacheObject.prompt = promptTextBox.Text;
                cacheObject.completion = completionTextBox.Text;
                cacheObject.language = languageTextBox.Text;
                cacheObject.model = modelTextBox.Text;
                cacheObject.chatmode = chatModeComboBox.Text;
                cacheObject.ParentCacheId = parentCacheIdTextBox.Text;
                cacheObject.generatedSystemPrompt = generatedSystemPromptTextBox.Text;
                
                // Tags
                cacheObject.tagsMust = GetTagsFromListBox(tagsMustListBox);
                cacheObject.tagsShould = GetTagsFromListBox(tagsShouldListBox);
                cacheObject.tagsMustNot = GetTagsFromListBox(tagsMustNotListBox);
                cacheObject.tagsShouldNot = GetTagsFromListBox(tagsShouldNotListBox);
                cacheObject.generatedTags = GetTagsFromListBox(generatedTagsListBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving cache data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<string> GetTagsFromListBox(ListBox listBox)
        {
            var tags = new List<string>();
            foreach (string item in listBox.Items)
            {
                tags.Add(item);
            }
            return tags;
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCacheData();
                
                var json = JsonConvert.SerializeObject(cacheObject);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await httpClient.PostAsync($"{baseUrl}/caches/save", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var serviceReturn = JsonConvert.DeserializeObject<ServiceReturn>(responseContent);
                    
                    if (serviceReturn?.Success == true)
                    {
                        MessageBox.Show($"Cache saved successfully!\nID: {serviceReturn.ReturnedId}", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Update the ID if it's a new cache
                        if (isNewCache && !string.IsNullOrEmpty(serviceReturn.ReturnedId))
                        {
                            idTextBox.Text = serviceReturn.ReturnedId;
                            cacheObject.Id = serviceReturn.ReturnedId;
                            isNewCache = false;
                        }
                        
                        statusLabel.Text = "Cache saved successfully";
                        statusLabel.ForeColor = Color.Green;
                    }
                    else
                    {
                        MessageBox.Show($"Failed to save cache: {serviceReturn?.Message}", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        statusLabel.Text = $"Save failed: {serviceReturn?.Message}";
                        statusLabel.ForeColor = Color.Red;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"HTTP Error: {response.StatusCode}\n{errorContent}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = $"HTTP Error: {response.StatusCode}";
                    statusLabel.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving cache: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = $"Error: {ex.Message}";
                statusLabel.ForeColor = Color.Red;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddChildButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current cache ID to use as ParentCacheId
                var parentCacheId = idTextBox.Text.Trim();
                
                // Create a new cache object with the current cache ID as ParentCacheId
                var childCache = new Cache
                {
                    ParentCacheId = string.IsNullOrEmpty(parentCacheId) ? null : parentCacheId
                };
                
                // Parse the base URL to extract host and port
                var uri = new Uri(baseUrl);
                var host = uri.Host;
                var port = uri.Port;
                
                // Open a new CacheEditForm for the child cache
                var childForm = new CacheEditForm(host, port, childCache);
                childForm.Show();
                
                statusLabel.Text = "Child cache form opened successfully.";
                statusLabel.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening child cache form: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddTagButton_Click(object sender, EventArgs e)
        {
            var tag = tagInputTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(tag))
            {
                if (sender == addTagsMustButton)
                    tagsMustListBox.Items.Add(tag);
                else if (sender == addTagsShouldButton)
                    tagsShouldListBox.Items.Add(tag);
                else if (sender == addTagsMustNotButton)
                    tagsMustNotListBox.Items.Add(tag);
                else if (sender == addTagsShouldNotButton)
                    tagsShouldNotListBox.Items.Add(tag);
                else if (sender == addGeneratedTagsButton)
                    generatedTagsListBox.Items.Add(tag);
                
                tagInputTextBox.Clear();
            }
        }

        private void RemoveTagButton_Click(object sender, EventArgs e)
        {
            ListBox listBox = null;
            if (sender == removeTagsMustButton)
                listBox = tagsMustListBox;
            else if (sender == removeTagsShouldButton)
                listBox = tagsShouldListBox;
            else if (sender == removeTagsMustNotButton)
                listBox = tagsMustNotListBox;
            else if (sender == removeTagsShouldNotButton)
                listBox = tagsShouldNotListBox;
            else if (sender == removeGeneratedTagsButton)
                listBox = generatedTagsListBox;
            
            if (listBox != null && listBox.SelectedItem != null)
            {
                listBox.Items.Remove(listBox.SelectedItem);
            }
        }

        private void CacheEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            httpClient?.Dispose();
        }

        // Property to get the updated cache object
        public Cache UpdatedCache => cacheObject;
    }
}
