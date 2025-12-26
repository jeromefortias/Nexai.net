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
    public partial class CacheManagerForm : Form
    {
        private readonly string baseUrl;
        private readonly HttpClient httpClient;
        private List<Cache> cacheItems = new List<Cache>();

        public CacheManagerForm(string host, int port)
        {
            InitializeComponent();
            baseUrl = $"http://{host}:{port}";
            httpClient = new HttpClient();
            
            // Configure DataGridView
            ConfigureDataGridView();
            
            // Add double-click event for editing cache objects
            cacheDataGridView.CellDoubleClick += CacheDataGridView_CellDoubleClick;
            
            // Add new cache button event
            newCacheButton.Click += NewCacheButton_Click;
            
            // Set form properties
            this.Text = "Cache Manager";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Wire up form closing event
            this.FormClosing += CacheManagerForm_FormClosing;
        }

        private void ConfigureDataGridView()
        {
            cacheDataGridView.AutoGenerateColumns = false;
            cacheDataGridView.AllowUserToAddRows = false;
            cacheDataGridView.AllowUserToDeleteRows = false;
            cacheDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cacheDataGridView.MultiSelect = false;
            cacheDataGridView.ReadOnly = true;

            // Add columns
            cacheDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 100,
                Visible = false
            });

            cacheDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Prompt",
                HeaderText = "Prompt",
                DataPropertyName = "prompt",
                Width = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            cacheDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Completion",
                HeaderText = "Completion",
                DataPropertyName = "completion",
                Width = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            cacheDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Model",
                HeaderText = "Model",
                DataPropertyName = "model",
                Width = 120
            });

            cacheDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Language",
                HeaderText = "Language",
                DataPropertyName = "language",
                Width = 80
            });

            cacheDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ChatMode",
                HeaderText = "Chat Mode",
                DataPropertyName = "chatmode",
                Width = 100
            });

            cacheDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Date",
                HeaderText = "Date",
                DataPropertyName = "Date",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm" }
            });
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await SearchCaches();
        }

        private async void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                await SearchCaches();
            }
        }

        private async Task SearchCaches()
        {
            try
            {
                searchButton.Enabled = false;
                statusLabel.Text = "Searching...";
                statusLabel.ForeColor = Color.Blue;

                var searchId = new SearchId
                {
                    Criteria = searchTextBox.Text.Trim()
                };

                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/caches/search", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                cacheItems = JsonConvert.DeserializeObject<List<Cache>>(responseJson) ?? new List<Cache>();

                cacheDataGridView.DataSource = cacheItems;

                statusLabel.Text = $"Found {cacheItems.Count} cache items";
                statusLabel.ForeColor = Color.Green;

                // Enable load and save buttons if we have results
                loadButton.Enabled = cacheItems.Count > 0;
                saveButton.Enabled = cacheItems.Count > 0;
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Search error: {ex.Message}";
                statusLabel.ForeColor = Color.Red;
                MessageBox.Show($"Search failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                searchButton.Enabled = true;
            }
        }

        private async void LoadButton_Click(object sender, EventArgs e)
        {
            await LoadSelectedCache();
        }

        private async Task LoadSelectedCache()
        {
            if (cacheDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a cache item to load.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                loadButton.Enabled = false;
                statusLabel.Text = "Loading...";
                statusLabel.ForeColor = Color.Blue;

                var selectedCache = cacheDataGridView.SelectedRows[0].DataBoundItem as Cache;
                if (selectedCache == null)
                {
                    statusLabel.Text = "Invalid selection";
                    statusLabel.ForeColor = Color.Red;
                    return;
                }

                var searchId = new SearchId
                {
                    Id = selectedCache.Id
                };

                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/caches/load", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var loadedCache = JsonConvert.DeserializeObject<Cache>(responseJson);

                if (loadedCache != null)
                {
                    // Update the selected row with loaded data
                    var index = cacheDataGridView.SelectedRows[0].Index;
                    cacheItems[index] = loadedCache;
                    cacheDataGridView.DataSource = null;
                    cacheDataGridView.DataSource = cacheItems;
                    cacheDataGridView.Rows[index].Selected = true;

                    statusLabel.Text = "Cache loaded successfully";
                    statusLabel.ForeColor = Color.Green;
                }
                else
                {
                    statusLabel.Text = "Failed to load cache";
                    statusLabel.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Load error: {ex.Message}";
                statusLabel.ForeColor = Color.Red;
                MessageBox.Show($"Load failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loadButton.Enabled = true;
            }
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            await SaveSelectedCache();
        }

        private async Task SaveSelectedCache()
        {
            if (cacheDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a cache item to save.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                saveButton.Enabled = false;
                statusLabel.Text = "Saving...";
                statusLabel.ForeColor = Color.Blue;

                var selectedCache = cacheDataGridView.SelectedRows[0].DataBoundItem as Cache;
                if (selectedCache == null)
                {
                    statusLabel.Text = "Invalid selection";
                    statusLabel.ForeColor = Color.Red;
                    return;
                }

                var json = JsonConvert.SerializeObject(selectedCache);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/caches/save", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var serviceReturn = JsonConvert.DeserializeObject<ServiceReturn>(responseJson);

                if (serviceReturn != null && serviceReturn.Success)
                {
                    statusLabel.Text = $"Cache saved successfully. ID: {serviceReturn.ReturnedId}";
                    statusLabel.ForeColor = Color.Green;
                }
                else
                {
                    statusLabel.Text = $"Save failed: {serviceReturn?.Message ?? "Unknown error"}";
                    statusLabel.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Save error: {ex.Message}";
                statusLabel.ForeColor = Color.Red;
                MessageBox.Show($"Save failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                saveButton.Enabled = true;
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Clear();
            cacheDataGridView.DataSource = null;
            cacheItems.Clear();
            statusLabel.Text = "Ready";
            statusLabel.ForeColor = Color.Black;
            loadButton.Enabled = false;
            saveButton.Enabled = false;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchTextBox.Text))
            {
                _ = SearchCaches();
            }
        }

        private void CacheManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            httpClient?.Dispose();
        }

        private void CacheDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < cacheItems.Count)
                {
                    var selectedCache = cacheItems[e.RowIndex];
                    
                    // Parse the base URL to extract host and port
                    var uri = new Uri(baseUrl);
                    var host = uri.Host;
                    var port = uri.Port;
                    
                    // Open the edit form
                    var editForm = new CacheEditForm(host, port, selectedCache);
                    editForm.ShowDialog();
                    
                    // Refresh the data after editing
                    if (!string.IsNullOrEmpty(searchTextBox.Text))
                    {
                        _ = SearchCaches();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening cache editor: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NewCacheButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Parse the base URL to extract host and port
                var uri = new Uri(baseUrl);
                var host = uri.Host;
                var port = uri.Port;
                
                // Open the edit form for a new cache
                var editForm = new CacheEditForm(host, port, null);
                editForm.ShowDialog();
                
                // Refresh the data after creating new cache
                if (!string.IsNullOrEmpty(searchTextBox.Text))
                {
                    _ = SearchCaches();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating new cache: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
