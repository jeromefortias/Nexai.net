using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Musuko.Kaonashi;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DrawingColor = System.Drawing.Color;

namespace Musuko.Kaonashi
{
    public partial class CvManagerForm : Form
    {
        private readonly string _completionHost;
        private readonly int _completionPort;
        private readonly LogService _logService;
        private List<Cv> _cvs = new List<Cv>();
        private Cv? _currentCv = null;

        public CvManagerForm(string completionHost, int completionPort)
        {
            _completionHost = completionHost;
            _completionPort = completionPort;
            _logService = new LogService(completionHost, completionPort);
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Ensure the form is properly initialized
            searchTextBox.Enabled = true;
            searchButton.Enabled = true;
            newButton.Enabled = true;
            
            // Enable all fields and buttons
            detailsPanel.Enabled = true;
            saveButton.Enabled = true;
            cloneButton.Enabled = true;
            deleteButton.Enabled = true;
            generateWordButton.Enabled = true;
            saveAsJsonButton.Enabled = true;
            loadJsonButton.Enabled = true;
            
            statusLabel.Text = "Ready to manage CVs";
            statusLabel.ForeColor = DrawingColor.Blue;
        }

        public void LoadCvDirectly(Cv cv)
        {
            _currentCv = cv;
            PopulateCvDetails(cv);
            statusLabel.Text = "CV loaded successfully";
            statusLabel.ForeColor = DrawingColor.Green;
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await SearchCvsAsync();
        }

        private async Task SearchCvsAsync()
        {
            try
            {
                statusLabel.Text = "Searching CVs...";
                statusLabel.ForeColor = DrawingColor.Blue;

                var searchId = new SearchId
                {
                    Criteria = searchTextBox.Text?.Trim() ?? ""
                };

                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                var response = await client.PostAsync($"http://{_completionHost}:{_completionPort}/cv/search", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _cvs = JsonConvert.DeserializeObject<List<Cv>>(responseContent) ?? new List<Cv>();
                    PopulateDataGridView();
                    statusLabel.Text = $"Found {_cvs.Count} CV(s)";
                    statusLabel.ForeColor = DrawingColor.Green;
                }
                else
                {
                    statusLabel.Text = $"Search failed: {response.StatusCode}";
                    statusLabel.ForeColor = DrawingColor.Red;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Search error: {ex.Message}";
                statusLabel.ForeColor = DrawingColor.Red;
            }
            finally
            {
                // Search button remains always enabled
            }
        }

        private void PopulateDataGridView()
        {
            cvsDataGrid.Rows.Clear();
            foreach (var cv in _cvs)
            {
                cvsDataGrid.Rows.Add(
                    cv.Id,
                    cv.Title ?? "",
                    $"{cv.FirstName} {cv.LastName}",
                    cv.Email,
                    cv.Phone,
                    cv.AddressCity,
                    cv.Date.ToString("yyyy-MM-dd")
                );
            }
        }

        private async void CvsDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _cvs.Count)
            {
                var selectedCv = _cvs[e.RowIndex];
                await LoadCvDetailsAsync(selectedCv.Id);
            }
        }

        private async void CvsDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _cvs.Count)
            {
                var selectedCv = _cvs[e.RowIndex];
                await LoadCvDetailsAsync(selectedCv.Id);
            }
        }

        private async Task LoadCvDetailsAsync(string cvId)
        {
            try
            {
                statusLabel.Text = "Loading CV details...";
                statusLabel.ForeColor = DrawingColor.Blue;

                var searchId = new SearchId
                {
                    Id = cvId
                };

                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                var response = await client.PostAsync($"http://{_completionHost}:{_completionPort}/cv/load", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _currentCv = JsonConvert.DeserializeObject<Cv>(responseContent);

                    if (_currentCv != null)
                    {
                        PopulateCvDetails(_currentCv);
                        statusLabel.Text = $"Loaded CV: {_currentCv.FirstName} {_currentCv.LastName}";
                        statusLabel.ForeColor = DrawingColor.Green;
                    }
                }
                else
                {
                    statusLabel.Text = $"Load failed: {response.StatusCode}";
                    statusLabel.ForeColor = DrawingColor.Red;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Load error: {ex.Message}";
                statusLabel.ForeColor = DrawingColor.Red;
            }
        }

        private void PopulateCvDetails(Cv cv)
        {
            // Clear all form fields first
            ClearEducationForm();
            ClearExperienceForm();
            ClearCreationForm();

            // Basic Information
            titleTextBox.Text = cv.Title ?? "";
            firstNameTextBox.Text = cv.FirstName ?? "";
            lastNameTextBox.Text = cv.LastName ?? "";
            emailTextBox.Text = cv.Email ?? "";
            phoneTextBox.Text = cv.Phone ?? "";
            summaryTextBox.Text = cv.Summary ?? "";
            linkedInTextBox.Text = cv.LinkedInProfile ?? "";

            // Address Information
            addressStreetTextBox.Text = cv.AddressStreet ?? "";
            addressNumberTextBox.Text = cv.AddressNumber ?? "";
            addressCityTextBox.Text = cv.AddressCity ?? "";
            addressZipTextBox.Text = cv.AddressZip ?? "";
            addressCountryTextBox.Text = cv.AddressCountry ?? "";

            // Skills
            skillsTextBox.Text = string.Join(", ", cv.Skills ?? new List<string>());

            // Comment (readonly display)
            commentTextBox.Text = cv.Comment ?? "";
            
            // Clear the new comment input field
            newCommentTextBox.Clear();

            // Load Education data
            educationListBox.Items.Clear();
            if (cv.Educations != null)
            {
                foreach (var education in cv.Educations)
                {
                    educationListBox.Items.Add($"{education.Degree} - {education.Institution}");
                }
            }

            // Load Experience data
            experienceListBox.Items.Clear();
            if (cv.Experiences != null)
            {
                foreach (var experience in cv.Experiences)
                {
                    experienceListBox.Items.Add($"{experience.Position} at {experience.Company}");
                }
            }

            // Load Creation data
            creationListBox.Items.Clear();
            if (cv.Creations != null)
            {
                foreach (var creation in cv.Creations)
                {
                    creationListBox.Items.Add(creation.Title);
                }
            }

            // Enable the details panel and buttons
            detailsPanel.Enabled = true;
            saveButton.Enabled = true;
            cloneButton.Enabled = true;
            deleteButton.Enabled = true;
            generateWordButton.Enabled = true;
            saveAsJsonButton.Enabled = true;
            loadJsonButton.Enabled = true;
            
            // Refresh all tabs
            RefreshAllTabs();
            
            detailsPanel.Refresh();
        }

        private void RefreshAllTabs()
        {
            // Refresh Education tab
            if (educationListBox != null)
            {
                educationListBox.Refresh();
            }
            
            // Refresh Experience tab
            if (experienceListBox != null)
            {
                experienceListBox.Refresh();
            }
            
            // Refresh Creation tab
            if (creationListBox != null)
            {
                creationListBox.Refresh();
            }
            
            // Refresh main tab control
            if (tabControl1 != null)
            {
                tabControl1.Refresh();
            }
            
            // Refresh individual tab pages
            if (TabHeader != null)
            {
                TabHeader.Refresh();
            }
            
            if (tabExperiences != null)
            {
                tabExperiences.Refresh();
            }
            
            if (tabPage3 != null)
            {
                tabPage3.Refresh();
            }
            
            if (tabPage4 != null)
            {
                tabPage4.Refresh();
            }
            
            if (tabPage5 != null)
            {
                tabPage5.Refresh();
            }
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            await SaveCvAsync();
        }

        private async Task SaveCvAsync()
        {
            try
            {
                if (_currentCv == null)
                {
                    _currentCv = new Cv();
                }

                // Update CV with form data
                _currentCv.Title = titleTextBox.Text?.Trim() ?? "";
                _currentCv.FirstName = firstNameTextBox.Text?.Trim() ?? "";
                _currentCv.LastName = lastNameTextBox.Text?.Trim() ?? "";
                _currentCv.Email = emailTextBox.Text?.Trim() ?? "";
                _currentCv.Phone = phoneTextBox.Text?.Trim() ?? "";
                _currentCv.Summary = summaryTextBox.Text?.Trim() ?? "";
                _currentCv.LinkedInProfile = linkedInTextBox.Text?.Trim() ?? "";

                _currentCv.AddressStreet = addressStreetTextBox.Text?.Trim() ?? "";
                _currentCv.AddressNumber = addressNumberTextBox.Text?.Trim() ?? "";
                _currentCv.AddressCity = addressCityTextBox.Text?.Trim() ?? "";
                _currentCv.AddressZip = addressZipTextBox.Text?.Trim() ?? "";
                _currentCv.AddressCountry = addressCountryTextBox.Text?.Trim() ?? "";

                // Parse skills from comma-separated string
                _currentCv.Skills = skillsTextBox.Text?
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList() ?? new List<string>();

                // Comment
                _currentCv.Comment = commentTextBox.Text?.Trim() ?? "";

                _currentCv.Date = DateTime.Now;
                _currentCv.MachineName = Environment.MachineName;
                _currentCv.UserName = Environment.UserName;

                var json = JsonConvert.SerializeObject(_currentCv);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                var response = await client.PostAsync($"http://{_completionHost}:{_completionPort}/cv/save", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var serviceReturn = JsonConvert.DeserializeObject<ServiceReturn>(responseContent);

                    if (serviceReturn?.Success == true)
                    {
                        _currentCv.Id = serviceReturn.ReturnedId ?? _currentCv.Id;
                        statusLabel.Text = "CV saved successfully";
                        statusLabel.ForeColor = DrawingColor.Green;
                        await _logService.LogAsync("INFO", "CV saved successfully");
                    }
                    else
                    {
                        statusLabel.Text = $"Save failed: {serviceReturn?.Message ?? "Unknown error"}";
                        statusLabel.ForeColor = DrawingColor.Red;
                    }
                }
                else
                {
                    statusLabel.Text = $"Save failed: {response.StatusCode}";
                    statusLabel.ForeColor = DrawingColor.Red;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Save error: {ex.Message}";
                statusLabel.ForeColor = DrawingColor.Red;
            }
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            _currentCv = new Cv();
            ClearForm();
            detailsPanel.Enabled = true;
            saveButton.Enabled = true;
            cloneButton.Enabled = false;
            deleteButton.Enabled = false; // Can't delete a new CV
            generateWordButton.Enabled = true;
            saveAsJsonButton.Enabled = true;
            loadJsonButton.Enabled = true;
            statusLabel.Text = "New CV created - ready to edit";
            statusLabel.ForeColor = DrawingColor.Blue;
        }

        private void ClearForm()
        {
            titleTextBox.Clear();
            firstNameTextBox.Clear();
            lastNameTextBox.Clear();
            emailTextBox.Clear();
            phoneTextBox.Clear();
            summaryTextBox.Clear();
            linkedInTextBox.Clear();
            addressStreetTextBox.Clear();
            addressNumberTextBox.Clear();
            addressCityTextBox.Clear();
            addressZipTextBox.Clear();
            addressCountryTextBox.Clear();
            skillsTextBox.Clear();
            commentTextBox.Clear();
            newCommentTextBox.Clear();
            
            // Clear tab fields
            ClearEducationForm();
            ClearExperienceForm();
            ClearCreationForm();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Edit functionality is now integrated into the main form with tabs
            // This method is kept for compatibility but does nothing
            statusLabel.Text = "Edit functionality is now integrated into the main form";
            statusLabel.ForeColor = DrawingColor.Blue;
        }

        private void CloneButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null)
            {
                // Get the selected model from the main form
                var mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
                string selectedModel = mainForm?.GetSelectedModel() ?? "llama3.2";

                // Open the CV rewrite form
                var rewriteForm = new CvRewriteForm(_currentCv, selectedModel);
                rewriteForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No CV selected to clone.", "Clone CV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CvManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up resources if needed
        }

        private void statusLabel_Click(object sender, EventArgs e)
        {

        }

        private void GenerateWordButton_Click(object sender, EventArgs e)
        {
            if (_currentCv == null)
            {
                MessageBox.Show("Please load or create a CV first.", "No CV Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                GenerateWordDocument(_currentCv);
                statusLabel.Text = "Word document generated successfully!";
                statusLabel.ForeColor = DrawingColor.Green;
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Error generating Word document: {ex.Message}";
                statusLabel.ForeColor = DrawingColor.Red;
                MessageBox.Show($"Error generating Word document: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateWordDocument(Cv cv)
        {
            try
            {
                // Create default file name
                string defaultFileName = $"CV_{cv.FirstName}_{cv.LastName}_{DateTime.Now:yyyyMMdd_HHmmss}.docx";

                // Show SaveFileDialog to let user choose location
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.FileName = defaultFileName;
                    saveFileDialog.Title = "Save CV as Word Document";
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        // Create Word document using OpenXML
                        using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                        {
                            // Add main document part
                            MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                            mainPart.Document = new Document();
                            Body body = mainPart.Document.AppendChild(new Body());

                            // Add CV content
                            AddCvContentOpenXml(body, cv);
                        }

                        // Show success message
                        MessageBox.Show($"Word document saved successfully!\n\nLocation: {filePath}", "Document Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Open the document with default application
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = filePath,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to generate Word document: {ex.Message}", ex);
            }
        }

        private void AddCvContentOpenXml(Body body, Cv cv)
        {
            // Add header with name
            AddHeading(body, $"{cv.FirstName} {cv.LastName}", 24, true, JustificationValues.Center);

            // Add title if available
            if (!string.IsNullOrEmpty(cv.Title))
            {
                AddHeading(body, cv.Title, 18, false, JustificationValues.Center);
            }

            // Add contact information
            string contactInfo = "";
            if (!string.IsNullOrEmpty(cv.Email))
                contactInfo += $"Email: {cv.Email}";
            if (!string.IsNullOrEmpty(cv.Phone))
                contactInfo += contactInfo.Length > 0 ? $" | Phone: {cv.Phone}" : $"Phone: {cv.Phone}";

            if (contactInfo.Length > 0)
            {
                AddParagraph(body, contactInfo, 11, false, JustificationValues.Center);
            }

            // Add LinkedIn as hyperlink if available
            if (!string.IsNullOrEmpty(cv.LinkedInProfile))
            {
                AddHyperlinkParagraph(body, $"LinkedIn: {cv.LinkedInProfile}", cv.LinkedInProfile, 11, JustificationValues.Center);
            }

            // Add address
            if (!string.IsNullOrEmpty(cv.AddressStreet) || !string.IsNullOrEmpty(cv.AddressCity))
            {
                string address = "";
                if (!string.IsNullOrEmpty(cv.AddressStreet))
                    address += cv.AddressStreet;
                if (!string.IsNullOrEmpty(cv.AddressNumber))
                    address += address.Length > 0 ? $" {cv.AddressNumber}" : cv.AddressNumber;
                if (!string.IsNullOrEmpty(cv.AddressCity))
                    address += address.Length > 0 ? $", {cv.AddressCity}" : cv.AddressCity;
                if (!string.IsNullOrEmpty(cv.AddressZip))
                    address += address.Length > 0 ? $" {cv.AddressZip}" : cv.AddressZip;
                if (!string.IsNullOrEmpty(cv.AddressCountry))
                    address += address.Length > 0 ? $", {cv.AddressCountry}" : cv.AddressCountry;

                if (address.Length > 0)
                {
                    AddParagraph(body, address, 11, false, JustificationValues.Center);
                }
            }

            // Add line break
            AddParagraph(body, "", 11, false, JustificationValues.Left);

            // Add summary
            if (!string.IsNullOrEmpty(cv.Summary))
            {
                AddHeading(body, "Professional Summary", 14, true, JustificationValues.Left);
                AddParagraph(body, cv.Summary, 11, false, JustificationValues.Left);
            }

            // Add skills
            if (cv.Skills != null && cv.Skills.Count > 0)
            {
                AddHeading(body, "Skills", 14, true, JustificationValues.Left);
                string skillsText = string.Join(", ", cv.Skills);
                AddParagraph(body, skillsText, 11, false, JustificationValues.Left);
            }

            // Add experience
            if (cv.Experiences != null && cv.Experiences.Count > 0)
            {
                AddHeading(body, "Professional Experience", 14, true, JustificationValues.Left);
                foreach (var experience in cv.Experiences)
                {
                    AddExperienceEntryOpenXml(body, experience);
                }
            }

            // Add education
            if (cv.Educations != null && cv.Educations.Count > 0)
            {
                AddHeading(body, "Education", 14, true, JustificationValues.Left);
                foreach (var education in cv.Educations)
                {
                    AddEducationEntryOpenXml(body, education);
                }
            }

            // Add creations
            if (cv.Creations != null && cv.Creations.Count > 0)
            {
                AddHeading(body, "Projects & Creations", 14, true, JustificationValues.Left);
                foreach (var creation in cv.Creations)
                {
                    AddCreationEntryOpenXml(body, creation);
                }
            }
        }

        private void AddHeading(Body body, string text, int fontSize, bool bold, JustificationValues alignment)
        {
            Paragraph paragraph = new Paragraph();
            ParagraphProperties paragraphProperties = new ParagraphProperties();
            paragraphProperties.Justification = new Justification() { Val = alignment };
            paragraph.Append(paragraphProperties);

            Run run = new Run();
            RunProperties runProperties = new RunProperties();
            runProperties.FontSize = new FontSize() { Val = (fontSize * 2).ToString() };
            runProperties.Bold = bold ? new Bold() : null;
            run.Append(runProperties);
            run.Append(new Text(text));

            paragraph.Append(run);
            body.Append(paragraph);
        }

        private void AddParagraph(Body body, string text, int fontSize, bool bold, JustificationValues alignment)
        {
            Paragraph paragraph = new Paragraph();
            ParagraphProperties paragraphProperties = new ParagraphProperties();
            paragraphProperties.Justification = new Justification() { Val = alignment };
            paragraph.Append(paragraphProperties);

            Run run = new Run();
            RunProperties runProperties = new RunProperties();
            runProperties.FontSize = new FontSize() { Val = (fontSize * 2).ToString() };
            runProperties.Bold = bold ? new Bold() : null;
            run.Append(runProperties);
            run.Append(new Text(text));

            paragraph.Append(run);
            body.Append(paragraph);
        }

        private void AddHyperlinkParagraph(Body body, string displayText, string url, int fontSize, JustificationValues alignment)
        {
            Paragraph paragraph = new Paragraph();
            ParagraphProperties paragraphProperties = new ParagraphProperties();
            paragraphProperties.Justification = new Justification() { Val = alignment };
            paragraph.Append(paragraphProperties);

            // Create hyperlink
            Hyperlink hyperlink = new Hyperlink();
            hyperlink.Anchor = url;

            Run run = new Run();
            RunProperties runProperties = new RunProperties();
            runProperties.FontSize = new FontSize() { Val = (fontSize * 2).ToString() };
            runProperties.Color = new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "0563C1" }; // Blue color for links
            runProperties.Underline = new Underline() { Val = UnderlineValues.Single };
            run.Append(runProperties);
            run.Append(new Text(displayText));

            hyperlink.Append(run);
            paragraph.Append(hyperlink);
            body.Append(paragraph);
        }

        private void AddExperienceEntryOpenXml(Body body, Experience experience)
        {
            AddParagraph(body, $"{experience.Position} - {experience.Company}", 12, true, JustificationValues.Left);
            AddParagraph(body, $"{experience.StartDate:MMM yyyy} - {experience.EndDate:MMM yyyy}", 10, false, JustificationValues.Left);

            if (!string.IsNullOrEmpty(experience.Description))
            {
                AddParagraph(body, experience.Description, 11, false, JustificationValues.Left);
            }

            if (experience.Technologies != null && experience.Technologies.Count > 0)
            {
                AddParagraph(body, $"Technologies: {string.Join(", ", experience.Technologies)}", 10, false, JustificationValues.Left);
            }

            AddParagraph(body, "", 11, false, JustificationValues.Left);
        }

        private void AddEducationEntryOpenXml(Body body, Education education)
        {
            AddParagraph(body, $"{education.Degree} - {education.Institution}", 12, true, JustificationValues.Left);
            AddParagraph(body, $"{education.StartDate:MMM yyyy} - {education.EndDate:MMM yyyy}", 10, false, JustificationValues.Left);

            if (!string.IsNullOrEmpty(education.FieldOfStudy))
            {
                AddParagraph(body, $"Field of Study: {education.FieldOfStudy}", 11, false, JustificationValues.Left);
            }

            if (!string.IsNullOrEmpty(education.Description))
            {
                AddParagraph(body, education.Description, 11, false, JustificationValues.Left);
            }

            AddParagraph(body, "", 11, false, JustificationValues.Left);
        }

        private void AddCreationEntryOpenXml(Body body, Creation creation)
        {
            AddParagraph(body, creation.Title, 12, true, JustificationValues.Left);

            if (!string.IsNullOrEmpty(creation.Description))
            {
                AddParagraph(body, creation.Description, 11, false, JustificationValues.Left);
            }

            if (!string.IsNullOrEmpty(creation.Url))
            {
                AddHyperlinkParagraph(body, $"URL: {creation.Url}", creation.Url, 10, JustificationValues.Left);
            }

            AddParagraph(body, "", 11, false, JustificationValues.Left);
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_currentCv == null)
            {
                MessageBox.Show("No CV selected to delete.", "Delete CV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete the CV for {_currentCv.FirstName} {_currentCv.LastName}?\n\nThis action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    await DeleteCvAsync(_currentCv.Id);
                    await _logService.LogAsync("INFO", $"CV deleted: {_currentCv.FirstName} {_currentCv.LastName}");

                    // Clear the form and refresh the list
                    _currentCv = null;
                    ClearForm();
                    await SearchCvsAsync();

                    statusLabel.Text = "CV deleted successfully";
                    statusLabel.ForeColor = DrawingColor.Green;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting CV: {ex.Message}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "Error deleting CV";
                    statusLabel.ForeColor = DrawingColor.Red;
                }
            }
        }

        private async Task DeleteCvAsync(string cvId)
        {
            try
            {
                using var httpClient = new HttpClient();
                var searchId = new SearchId { Id = cvId };
                var json = JsonConvert.SerializeObject(searchId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"http://{_completionHost}:{_completionPort}/cv/delete", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Server error: {response.StatusCode} - {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var serviceReturn = JsonConvert.DeserializeObject<ServiceReturn>(responseContent);

                if (serviceReturn == null || !serviceReturn.Success)
                {
                    throw new Exception(serviceReturn?.Message ?? "Unknown error occurred during deletion");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete CV: {ex.Message}", ex);
            }
        }






        // Education Tab Event Handlers
        private void AddEducationButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null)
            {
                if (_currentCv.Educations == null)
                    _currentCv.Educations = new List<Education>();

                var education = new Education
                {
                    Institution = institutionTextBox.Text?.Trim() ?? "",
                    Degree = degreeTextBox.Text?.Trim() ?? "",
                    FieldOfStudy = fieldOfStudyTextBox.Text?.Trim() ?? "",
                    StartDate = startDateDateTimePicker.Value,
                    EndDate = endDateDateTimePicker.Value,
                    Description = educationDescriptionTextBox.Text?.Trim() ?? ""
                };

                if (educationListBox.SelectedIndex >= 0)
                {
                    // Update existing education
                    _currentCv.Educations[educationListBox.SelectedIndex] = education;
                    educationListBox.Items[educationListBox.SelectedIndex] = $"{education.Degree} - {education.Institution}";
                }
                else
                {
                    // Add new education
                    _currentCv.Educations.Add(education);
                    educationListBox.Items.Add($"{education.Degree} - {education.Institution}");
                }

                ClearEducationForm();
            }
        }

        private void EducationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (educationListBox.SelectedIndex >= 0 && _currentCv?.Educations != null && educationListBox.SelectedIndex < _currentCv.Educations.Count)
            {
                var education = _currentCv.Educations[educationListBox.SelectedIndex];
                institutionTextBox.Text = education.Institution ?? "";
                degreeTextBox.Text = education.Degree ?? "";
                fieldOfStudyTextBox.Text = education.FieldOfStudy ?? "";
                startDateDateTimePicker.Value = education.StartDate;
                endDateDateTimePicker.Value = education.EndDate;
                educationDescriptionTextBox.Text = education.Description ?? "";
            }
        }

        private void ClearEducationForm()
        {
            institutionTextBox.Clear();
            degreeTextBox.Clear();
            fieldOfStudyTextBox.Clear();
            startDateDateTimePicker.Value = DateTime.Now;
            endDateDateTimePicker.Value = DateTime.Now;
            educationDescriptionTextBox.Clear();
            educationListBox.SelectedIndex = -1;
        }

        // Experience Tab Event Handlers
        private void AddExperienceButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null)
            {
                if (_currentCv.Experiences == null)
                    _currentCv.Experiences = new List<Experience>();

                var experience = new Experience
                {
                    Company = companyTextBox.Text?.Trim() ?? "",
                    Position = positionTextBox.Text?.Trim() ?? "",
                    StartDate = expStartDateDateTimePicker.Value,
                    EndDate = expEndDateDateTimePicker.Value,
                    Description = expDescriptionTextBox.Text?.Trim() ?? "",
                    Technologies = expTechnologiesTextBox.Text?.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList() ?? new List<string>()
                };

                if (experienceListBox.SelectedIndex >= 0)
                {
                    // Update existing experience
                    _currentCv.Experiences[experienceListBox.SelectedIndex] = experience;
                    experienceListBox.Items[experienceListBox.SelectedIndex] = $"{experience.Position} at {experience.Company}";
                }
                else
                {
                    // Add new experience
                    _currentCv.Experiences.Add(experience);
                    experienceListBox.Items.Add($"{experience.Position} at {experience.Company}");
                }

                ClearExperienceForm();
            }
        }

        private void ExperienceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (experienceListBox.SelectedIndex >= 0 && _currentCv?.Experiences != null && experienceListBox.SelectedIndex < _currentCv.Experiences.Count)
            {
                var experience = _currentCv.Experiences[experienceListBox.SelectedIndex];
                companyTextBox.Text = experience.Company ?? "";
                positionTextBox.Text = experience.Position ?? "";
                expStartDateDateTimePicker.Value = experience.StartDate;
                expEndDateDateTimePicker.Value = experience.EndDate;
                expDescriptionTextBox.Text = experience.Description ?? "";
                expTechnologiesTextBox.Text = string.Join(", ", experience.Technologies ?? new List<string>());
            }
        }

        private void ClearExperienceForm()
        {
            companyTextBox.Clear();
            positionTextBox.Clear();
            expStartDateDateTimePicker.Value = DateTime.Now;
            expEndDateDateTimePicker.Value = DateTime.Now;
            expDescriptionTextBox.Clear();
            expTechnologiesTextBox.Clear();
            experienceListBox.SelectedIndex = -1;
        }

        // Creation Tab Event Handlers
        private void AddCreationButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null)
            {
                if (_currentCv.Creations == null)
                    _currentCv.Creations = new List<Creation>();

                var creation = new Creation
                {
                    Title = creationTitleTextBox.Text?.Trim() ?? "",
                    Description = creationDescriptionTextBox.Text?.Trim() ?? "",
                    Url = creationUrlTextBox.Text?.Trim() ?? ""
                };

                if (creationListBox.SelectedIndex >= 0)
                {
                    // Update existing creation
                    _currentCv.Creations[creationListBox.SelectedIndex] = creation;
                    creationListBox.Items[creationListBox.SelectedIndex] = creation.Title;
                }
                else
                {
                    // Add new creation
                    _currentCv.Creations.Add(creation);
                    creationListBox.Items.Add(creation.Title);
                }

                ClearCreationForm();
            }
        }

        private void CreationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (creationListBox.SelectedIndex >= 0 && _currentCv?.Creations != null && creationListBox.SelectedIndex < _currentCv.Creations.Count)
            {
                var creation = _currentCv.Creations[creationListBox.SelectedIndex];
                creationTitleTextBox.Text = creation.Title ?? "";
                creationDescriptionTextBox.Text = creation.Description ?? "";
                creationUrlTextBox.Text = creation.Url ?? "";
            }
        }

        private void ClearCreationForm()
        {
            creationTitleTextBox.Clear();
            creationDescriptionTextBox.Clear();
            creationUrlTextBox.Clear();
            creationListBox.SelectedIndex = -1;
        }

        private void detailsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SaveAsJsonButton_Click(object sender, EventArgs e)
        {
            if (_currentCv == null)
            {
                MessageBox.Show("No CV selected to save.", "Save JSON", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Create default file name
                string defaultFileName = $"CV_{_currentCv.FirstName}_{_currentCv.LastName}_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                // Show SaveFileDialog to let user choose location
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.FileName = defaultFileName;
                    saveFileDialog.Title = "Save CV as JSON";
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        // Serialize CV to JSON
                        string json = JsonConvert.SerializeObject(_currentCv, Formatting.Indented);
                        
                        // Write to file
                        File.WriteAllText(filePath, json);

                        // Show success message
                        MessageBox.Show($"CV saved as JSON successfully!\n\nLocation: {filePath}", "JSON Export", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        statusLabel.Text = "CV exported to JSON successfully";
                        statusLabel.ForeColor = DrawingColor.Green;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving CV as JSON: {ex.Message}", "JSON Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error saving CV as JSON";
                statusLabel.ForeColor = DrawingColor.Red;
            }
        }

        private void LoadJsonButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Show OpenFileDialog to let user choose JSON file
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.Title = "Load CV from JSON";
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;

                        // Read JSON file
                        string json = File.ReadAllText(filePath);
                        
                        // Deserialize JSON to CV object
                        Cv loadedCv = JsonConvert.DeserializeObject<Cv>(json);
                        
                        if (loadedCv == null)
                        {
                            MessageBox.Show("Failed to load CV from JSON file. The file may be corrupted or invalid.", "JSON Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Set the loaded CV as current
                        _currentCv = loadedCv;
                        
                        // Populate the form with the loaded CV data
                        PopulateCvDetails(_currentCv);

                        // Show success message
                        MessageBox.Show($"CV loaded from JSON successfully!\n\nFile: {filePath}", "JSON Import", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        statusLabel.Text = "CV loaded from JSON successfully";
                        statusLabel.ForeColor = DrawingColor.Green;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading CV from JSON: {ex.Message}", "JSON Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error loading CV from JSON";
                statusLabel.ForeColor = DrawingColor.Red;
            }
        }

        private void ExpTechnologiesTextBox_TextChanged(object sender, EventArgs e)
        {
            // This method will be called when the technologies text changes
            // The actual splitting logic will be handled when adding the experience
        }

        private void UpdateEducationButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null && educationListBox.SelectedIndex >= 0)
            {
                if (_currentCv.Educations != null && educationListBox.SelectedIndex < _currentCv.Educations.Count)
                {
                    var education = new Education
                    {
                        Institution = institutionTextBox.Text?.Trim() ?? "",
                        Degree = degreeTextBox.Text?.Trim() ?? "",
                        FieldOfStudy = fieldOfStudyTextBox.Text?.Trim() ?? "",
                        StartDate = startDateDateTimePicker.Value,
                        EndDate = endDateDateTimePicker.Value,
                        Description = educationDescriptionTextBox.Text?.Trim() ?? ""
                    };

                    // Update existing education
                    _currentCv.Educations[educationListBox.SelectedIndex] = education;
                    educationListBox.Items[educationListBox.SelectedIndex] = $"{education.Degree} - {education.Institution}";

                    // Clear form after update
                    ClearEducationForm();
                }
            }
        }

        private void UpdateExperienceButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null && experienceListBox.SelectedIndex >= 0)
            {
                if (_currentCv.Experiences != null && experienceListBox.SelectedIndex < _currentCv.Experiences.Count)
                {
                    // Split technologies by comma and clean up each entry
                    var technologies = new List<string>();
                    if (!string.IsNullOrWhiteSpace(expTechnologiesTextBox.Text))
                    {
                        technologies = expTechnologiesTextBox.Text
                            .Split(',')
                            .Select(tech => tech.Trim())
                            .Where(tech => !string.IsNullOrWhiteSpace(tech))
                            .ToList();
                    }

                    var experience = new Experience
                    {
                        Company = companyTextBox.Text?.Trim() ?? "",
                        Position = positionTextBox.Text?.Trim() ?? "",
                        StartDate = expStartDateDateTimePicker.Value,
                        EndDate = expEndDateDateTimePicker.Value,
                        Description = expDescriptionTextBox.Text?.Trim() ?? "",
                        Technologies = technologies
                    };

                    // Update existing experience
                    _currentCv.Experiences[experienceListBox.SelectedIndex] = experience;
                    experienceListBox.Items[experienceListBox.SelectedIndex] = $"{experience.Position} at {experience.Company}";

                    // Clear form after update
                    ClearExperienceForm();
                }
            }
        }

        private void UpdateCreationButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null && creationListBox.SelectedIndex >= 0)
            {
                if (_currentCv.Creations != null && creationListBox.SelectedIndex < _currentCv.Creations.Count)
                {
                    var creation = new Creation
                    {
                        Title = creationTitleTextBox.Text?.Trim() ?? "",
                        Description = creationDescriptionTextBox.Text?.Trim() ?? "",
                        Url = creationUrlTextBox.Text?.Trim() ?? ""
                    };

                    // Update existing creation
                    _currentCv.Creations[creationListBox.SelectedIndex] = creation;
                    creationListBox.Items[creationListBox.SelectedIndex] = creation.Title;

                    // Clear form after update
                    ClearCreationForm();
                }
            }
        }

        private void DeleteEducationButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null && educationListBox.SelectedIndex >= 0)
            {
                if (_currentCv.Educations != null && educationListBox.SelectedIndex < _currentCv.Educations.Count)
                {
                    var result = MessageBox.Show(
                        "Are you sure you want to delete this education entry?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Remove from CV
                        _currentCv.Educations.RemoveAt(educationListBox.SelectedIndex);
                        
                        // Remove from list box
                        educationListBox.Items.RemoveAt(educationListBox.SelectedIndex);
                        
                        // Clear form after deletion
                        ClearEducationForm();
                    }
                }
            }
        }

        private void DeleteExperienceButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null && experienceListBox.SelectedIndex >= 0)
            {
                if (_currentCv.Experiences != null && experienceListBox.SelectedIndex < _currentCv.Experiences.Count)
                {
                    var result = MessageBox.Show(
                        "Are you sure you want to delete this experience entry?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Remove from CV
                        _currentCv.Experiences.RemoveAt(experienceListBox.SelectedIndex);
                        
                        // Remove from list box
                        experienceListBox.Items.RemoveAt(experienceListBox.SelectedIndex);
                        
                        // Clear form after deletion
                        ClearExperienceForm();
                    }
                }
            }
        }

        private void DeleteCreationButton_Click(object sender, EventArgs e)
        {
            if (_currentCv != null && creationListBox.SelectedIndex >= 0)
            {
                if (_currentCv.Creations != null && creationListBox.SelectedIndex < _currentCv.Creations.Count)
                {
                    var result = MessageBox.Show(
                        "Are you sure you want to delete this creation entry?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Remove from CV
                        _currentCv.Creations.RemoveAt(creationListBox.SelectedIndex);
                        
                        // Remove from list box
                        creationListBox.Items.RemoveAt(creationListBox.SelectedIndex);
                        
                        // Clear form after deletion
                        ClearCreationForm();
                    }
                }
            }
        }

        private void AddCommentButton_Click(object sender, EventArgs e)
        {
            if (_currentCv == null)
            {
                MessageBox.Show("No CV selected. Please create or select a CV first.", "No CV Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newComment = newCommentTextBox.Text?.Trim();
            if (string.IsNullOrEmpty(newComment))
            {
                MessageBox.Show("Please enter a comment before adding.", "Empty Comment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

                // Get current timestamp and username
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string username = Environment.UserName;
                
                // Create new comment entry with timestamp and username
                string newCommentEntry = $"[{timestamp}] {username}: {newComment}";
            
            // Add new comment to the top of existing comments
            if (string.IsNullOrEmpty(_currentCv.Comment))
            {
                _currentCv.Comment = newCommentEntry;
            }
            else
            {
                _currentCv.Comment = newCommentEntry + Environment.NewLine + Environment.NewLine + _currentCv.Comment;
            }
            
            // Update the display
            commentTextBox.Text = _currentCv.Comment;
            
            // Clear the input field
            newCommentTextBox.Clear();
            
            // Show success message
            statusLabel.Text = "Comment added successfully";
            statusLabel.ForeColor = DrawingColor.Green;
        }
    }
}
