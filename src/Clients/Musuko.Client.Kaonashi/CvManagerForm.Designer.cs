namespace Musuko.Kaonashi
{
    partial class CvManagerForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel detailsPanel;
        private Label statusLabel;
        private Button saveButton;
        private Button cloneButton;
        private Button generateWordButton;
        private Button deleteButton;
        private Button saveAsJsonButton;
        private Button loadJsonButton;

        // Education Tab Controls
        private GroupBox educationGroupBox;
        private Label institutionLabel;
        private TextBox institutionTextBox;
        private Label degreeLabel;
        private TextBox degreeTextBox;
        private Label fieldOfStudyLabel;
        private TextBox fieldOfStudyTextBox;
        private Label startDateLabel;
        private DateTimePicker startDateDateTimePicker;
        private Label endDateLabel;
        private DateTimePicker endDateDateTimePicker;
        private Label educationDescriptionLabel;
        private TextBox educationDescriptionTextBox;
        private Button addEducationButton;
        private ListBox educationListBox;
        private Button removeEducationButton;
        private Button updateEducationButton;
        private Button deleteEducationButton;
        private Button removeExperienceButton;
        private Button updateExperienceButton;
        private Button deleteExperienceButton;

        // Creation Tab Controls
        private GroupBox creationGroupBox;
        private Label creationTitleLabel;
        private TextBox creationTitleTextBox;
        private Label creationDescriptionLabel;
        private TextBox creationDescriptionTextBox;
        private Label creationUrlLabel;
        private TextBox creationUrlTextBox;
        private Button addCreationButton;
        private ListBox creationListBox;
        private Button removeCreationButton;
        private Button updateCreationButton;
        private Button deleteCreationButton;
        private TextBox commentTextBox;
        private Label newCommentLabel;
        private TextBox newCommentTextBox;
        private Button addCommentButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            detailsPanel = new Panel();
            tabControl1 = new TabControl();
            TabHeader = new TabPage();
            basicInfoGroupBox = new GroupBox();
            titleLabel = new Label();
            titleTextBox = new TextBox();
            firstNameLabel = new Label();
            firstNameTextBox = new TextBox();
            lastNameLabel = new Label();
            lastNameTextBox = new TextBox();
            emailLabel = new Label();
            emailTextBox = new TextBox();
            phoneLabel = new Label();
            phoneTextBox = new TextBox();
            linkedInLabel = new Label();
            linkedInTextBox = new TextBox();
            summaryLabel = new Label();
            summaryTextBox = new TextBox();
            addressGroupBox = new GroupBox();
            addressStreetLabel = new Label();
            addressStreetTextBox = new TextBox();
            addressNumberLabel = new Label();
            addressNumberTextBox = new TextBox();
            addressCityLabel = new Label();
            addressCityTextBox = new TextBox();
            addressZipLabel = new Label();
            addressZipTextBox = new TextBox();
            addressCountryLabel = new Label();
            addressCountryTextBox = new TextBox();
            skillsGroupBox = new GroupBox();
            skillsTextBox = new TextBox();
            tabExperiences = new TabPage();
            experienceListBox = new ListBox();
            experienceGroupBox = new GroupBox();
            companyLabel = new Label();
            companyTextBox = new TextBox();
            positionLabel = new Label();
            positionTextBox = new TextBox();
            expStartDateLabel = new Label();
            expStartDateDateTimePicker = new DateTimePicker();
            expEndDateLabel = new Label();
            expEndDateDateTimePicker = new DateTimePicker();
            expDescriptionLabel = new Label();
            expDescriptionTextBox = new TextBox();
            expTechnologiesLabel = new Label();
            expTechnologiesTextBox = new TextBox();
            addExperienceButton = new Button();
            updateExperienceButton = new Button();
            deleteExperienceButton = new Button();
            tabPage3 = new TabPage();
            educationListBox = new ListBox();
            educationGroupBox = new GroupBox();
            institutionLabel = new Label();
            institutionTextBox = new TextBox();
            degreeLabel = new Label();
            degreeTextBox = new TextBox();
            fieldOfStudyLabel = new Label();
            fieldOfStudyTextBox = new TextBox();
            startDateLabel = new Label();
            startDateDateTimePicker = new DateTimePicker();
            endDateLabel = new Label();
            endDateDateTimePicker = new DateTimePicker();
            educationDescriptionLabel = new Label();
            educationDescriptionTextBox = new TextBox();
            addEducationButton = new Button();
            updateEducationButton = new Button();
            deleteEducationButton = new Button();
            tabPage4 = new TabPage();
            creationListBox = new ListBox();
            creationGroupBox = new GroupBox();
            creationTitleLabel = new Label();
            creationTitleTextBox = new TextBox();
            creationDescriptionLabel = new Label();
            creationDescriptionTextBox = new TextBox();
            creationUrlLabel = new Label();
            creationUrlTextBox = new TextBox();
            addCreationButton = new Button();
            updateCreationButton = new Button();
            deleteCreationButton = new Button();
            tabPage5 = new TabPage();
            commentTextBox = new TextBox();
            newCommentLabel = new Label();
            newCommentTextBox = new TextBox();
            addCommentButton = new Button();
            searchPanel = new Panel();
            searchLabel = new Label();
            searchTextBox = new TextBox();
            searchButton = new Button();
            newButton = new Button();
            cvsDataGrid = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            saveButton = new Button();
            cloneButton = new Button();
            generateWordButton = new Button();
            deleteButton = new Button();
            saveAsJsonButton = new Button();
            loadJsonButton = new Button();
            statusLabel = new Label();
            removeEducationButton = new Button();
            removeExperienceButton = new Button();
            removeCreationButton = new Button();
            detailsPanel.SuspendLayout();
            tabControl1.SuspendLayout();
            TabHeader.SuspendLayout();
            basicInfoGroupBox.SuspendLayout();
            addressGroupBox.SuspendLayout();
            skillsGroupBox.SuspendLayout();
            tabExperiences.SuspendLayout();
            experienceGroupBox.SuspendLayout();
            tabPage3.SuspendLayout();
            educationGroupBox.SuspendLayout();
            tabPage4.SuspendLayout();
            creationGroupBox.SuspendLayout();
            tabPage5.SuspendLayout();
            searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cvsDataGrid).BeginInit();
            SuspendLayout();
            // 
            // detailsPanel
            // 
            detailsPanel.AutoScroll = true;
            detailsPanel.BackColor = Color.White;
            detailsPanel.Controls.Add(tabControl1);
            detailsPanel.Controls.Add(searchPanel);
            detailsPanel.Controls.Add(cvsDataGrid);
            detailsPanel.Controls.Add(saveButton);
            detailsPanel.Controls.Add(cloneButton);
            detailsPanel.Controls.Add(generateWordButton);
            detailsPanel.Controls.Add(deleteButton);
            detailsPanel.Controls.Add(saveAsJsonButton);
            detailsPanel.Controls.Add(loadJsonButton);
            detailsPanel.Controls.Add(statusLabel);
            detailsPanel.Dock = DockStyle.Fill;
            detailsPanel.Location = new Point(0, 0);
            detailsPanel.Name = "detailsPanel";
            detailsPanel.Size = new Size(927, 857);
            detailsPanel.TabIndex = 2;
            detailsPanel.Paint += detailsPanel_Paint;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(TabHeader);
            tabControl1.Controls.Add(tabExperiences);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Location = new Point(3, 257);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(909, 543);
            tabControl1.TabIndex = 9;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // TabHeader
            // 
            TabHeader.Controls.Add(basicInfoGroupBox);
            TabHeader.Controls.Add(addressGroupBox);
            TabHeader.Controls.Add(skillsGroupBox);
            TabHeader.Location = new Point(4, 23);
            TabHeader.Name = "TabHeader";
            TabHeader.Padding = new Padding(3);
            TabHeader.Size = new Size(901, 516);
            TabHeader.TabIndex = 0;
            TabHeader.Text = "Header";
            TabHeader.UseVisualStyleBackColor = true;
            // 
            // basicInfoGroupBox
            // 
            basicInfoGroupBox.Controls.Add(titleLabel);
            basicInfoGroupBox.Controls.Add(titleTextBox);
            basicInfoGroupBox.Controls.Add(firstNameLabel);
            basicInfoGroupBox.Controls.Add(firstNameTextBox);
            basicInfoGroupBox.Controls.Add(lastNameLabel);
            basicInfoGroupBox.Controls.Add(lastNameTextBox);
            basicInfoGroupBox.Controls.Add(emailLabel);
            basicInfoGroupBox.Controls.Add(emailTextBox);
            basicInfoGroupBox.Controls.Add(phoneLabel);
            basicInfoGroupBox.Controls.Add(phoneTextBox);
            basicInfoGroupBox.Controls.Add(linkedInLabel);
            basicInfoGroupBox.Controls.Add(linkedInTextBox);
            basicInfoGroupBox.Controls.Add(summaryLabel);
            basicInfoGroupBox.Controls.Add(summaryTextBox);
            basicInfoGroupBox.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            basicInfoGroupBox.Location = new Point(-1, 21);
            basicInfoGroupBox.Name = "basicInfoGroupBox";
            basicInfoGroupBox.Size = new Size(900, 216);
            basicInfoGroupBox.TabIndex = 3;
            basicInfoGroupBox.TabStop = false;
            basicInfoGroupBox.Text = "Basic Information";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(20, 30);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(37, 14);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Title:";
            // 
            // titleTextBox
            // 
            titleTextBox.Location = new Point(100, 28);
            titleTextBox.Name = "titleTextBox";
            titleTextBox.Size = new Size(780, 22);
            titleTextBox.TabIndex = 1;
            // 
            // firstNameLabel
            // 
            firstNameLabel.Location = new Point(20, 60);
            firstNameLabel.Name = "firstNameLabel";
            firstNameLabel.Size = new Size(80, 20);
            firstNameLabel.TabIndex = 2;
            firstNameLabel.Text = "First Name:";
            // 
            // firstNameTextBox
            // 
            firstNameTextBox.Location = new Point(110, 58);
            firstNameTextBox.Name = "firstNameTextBox";
            firstNameTextBox.Size = new Size(200, 22);
            firstNameTextBox.TabIndex = 3;
            // 
            // lastNameLabel
            // 
            lastNameLabel.Location = new Point(330, 60);
            lastNameLabel.Name = "lastNameLabel";
            lastNameLabel.Size = new Size(80, 20);
            lastNameLabel.TabIndex = 4;
            lastNameLabel.Text = "Last Name:";
            // 
            // lastNameTextBox
            // 
            lastNameTextBox.Location = new Point(420, 58);
            lastNameTextBox.Name = "lastNameTextBox";
            lastNameTextBox.Size = new Size(200, 22);
            lastNameTextBox.TabIndex = 5;
            // 
            // emailLabel
            // 
            emailLabel.Location = new Point(20, 90);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(80, 20);
            emailLabel.TabIndex = 6;
            emailLabel.Text = "Email:";
            // 
            // emailTextBox
            // 
            emailTextBox.Location = new Point(110, 88);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.Size = new Size(200, 22);
            emailTextBox.TabIndex = 7;
            // 
            // phoneLabel
            // 
            phoneLabel.Location = new Point(330, 90);
            phoneLabel.Name = "phoneLabel";
            phoneLabel.Size = new Size(80, 20);
            phoneLabel.TabIndex = 8;
            phoneLabel.Text = "Phone:";
            // 
            // phoneTextBox
            // 
            phoneTextBox.Location = new Point(420, 88);
            phoneTextBox.Name = "phoneTextBox";
            phoneTextBox.Size = new Size(200, 22);
            phoneTextBox.TabIndex = 9;
            // 
            // linkedInLabel
            // 
            linkedInLabel.Location = new Point(20, 120);
            linkedInLabel.Name = "linkedInLabel";
            linkedInLabel.Size = new Size(80, 20);
            linkedInLabel.TabIndex = 10;
            linkedInLabel.Text = "LinkedIn:";
            // 
            // linkedInTextBox
            // 
            linkedInTextBox.Location = new Point(110, 118);
            linkedInTextBox.Name = "linkedInTextBox";
            linkedInTextBox.Size = new Size(770, 22);
            linkedInTextBox.TabIndex = 11;
            // 
            // summaryLabel
            // 
            summaryLabel.Location = new Point(20, 155);
            summaryLabel.Name = "summaryLabel";
            summaryLabel.Size = new Size(80, 20);
            summaryLabel.TabIndex = 12;
            summaryLabel.Text = "Summary:";
            // 
            // summaryTextBox
            // 
            summaryTextBox.Location = new Point(110, 153);
            summaryTextBox.Multiline = true;
            summaryTextBox.Name = "summaryTextBox";
            summaryTextBox.ScrollBars = ScrollBars.Vertical;
            summaryTextBox.Size = new Size(770, 50);
            summaryTextBox.TabIndex = 13;
            // 
            // addressGroupBox
            // 
            addressGroupBox.Controls.Add(addressStreetLabel);
            addressGroupBox.Controls.Add(addressStreetTextBox);
            addressGroupBox.Controls.Add(addressNumberLabel);
            addressGroupBox.Controls.Add(addressNumberTextBox);
            addressGroupBox.Controls.Add(addressCityLabel);
            addressGroupBox.Controls.Add(addressCityTextBox);
            addressGroupBox.Controls.Add(addressZipLabel);
            addressGroupBox.Controls.Add(addressZipTextBox);
            addressGroupBox.Controls.Add(addressCountryLabel);
            addressGroupBox.Controls.Add(addressCountryTextBox);
            addressGroupBox.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            addressGroupBox.Location = new Point(1, 243);
            addressGroupBox.Name = "addressGroupBox";
            addressGroupBox.Size = new Size(900, 150);
            addressGroupBox.TabIndex = 4;
            addressGroupBox.TabStop = false;
            addressGroupBox.Text = "Address Information";
            // 
            // addressStreetLabel
            // 
            addressStreetLabel.Location = new Point(20, 30);
            addressStreetLabel.Name = "addressStreetLabel";
            addressStreetLabel.Size = new Size(80, 20);
            addressStreetLabel.TabIndex = 0;
            addressStreetLabel.Text = "Street:";
            // 
            // addressStreetTextBox
            // 
            addressStreetTextBox.Location = new Point(110, 28);
            addressStreetTextBox.Name = "addressStreetTextBox";
            addressStreetTextBox.Size = new Size(300, 22);
            addressStreetTextBox.TabIndex = 1;
            // 
            // addressNumberLabel
            // 
            addressNumberLabel.Location = new Point(430, 30);
            addressNumberLabel.Name = "addressNumberLabel";
            addressNumberLabel.Size = new Size(60, 20);
            addressNumberLabel.TabIndex = 2;
            addressNumberLabel.Text = "Number:";
            // 
            // addressNumberTextBox
            // 
            addressNumberTextBox.Location = new Point(500, 28);
            addressNumberTextBox.Name = "addressNumberTextBox";
            addressNumberTextBox.Size = new Size(100, 22);
            addressNumberTextBox.TabIndex = 3;
            // 
            // addressCityLabel
            // 
            addressCityLabel.Location = new Point(20, 65);
            addressCityLabel.Name = "addressCityLabel";
            addressCityLabel.Size = new Size(80, 20);
            addressCityLabel.TabIndex = 4;
            addressCityLabel.Text = "City:";
            // 
            // addressCityTextBox
            // 
            addressCityTextBox.Location = new Point(110, 63);
            addressCityTextBox.Name = "addressCityTextBox";
            addressCityTextBox.Size = new Size(200, 22);
            addressCityTextBox.TabIndex = 5;
            // 
            // addressZipLabel
            // 
            addressZipLabel.Location = new Point(330, 65);
            addressZipLabel.Name = "addressZipLabel";
            addressZipLabel.Size = new Size(40, 20);
            addressZipLabel.TabIndex = 6;
            addressZipLabel.Text = "ZIP:";
            // 
            // addressZipTextBox
            // 
            addressZipTextBox.Location = new Point(380, 63);
            addressZipTextBox.Name = "addressZipTextBox";
            addressZipTextBox.Size = new Size(100, 22);
            addressZipTextBox.TabIndex = 7;
            // 
            // addressCountryLabel
            // 
            addressCountryLabel.Location = new Point(500, 65);
            addressCountryLabel.Name = "addressCountryLabel";
            addressCountryLabel.Size = new Size(60, 20);
            addressCountryLabel.TabIndex = 8;
            addressCountryLabel.Text = "Country:";
            // 
            // addressCountryTextBox
            // 
            addressCountryTextBox.Location = new Point(570, 63);
            addressCountryTextBox.Name = "addressCountryTextBox";
            addressCountryTextBox.Size = new Size(100, 22);
            addressCountryTextBox.TabIndex = 9;
            // 
            // skillsGroupBox
            // 
            skillsGroupBox.Controls.Add(skillsTextBox);
            skillsGroupBox.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            skillsGroupBox.Location = new Point(3, 399);
            skillsGroupBox.Name = "skillsGroupBox";
            skillsGroupBox.Size = new Size(900, 114);
            skillsGroupBox.TabIndex = 5;
            skillsGroupBox.TabStop = false;
            skillsGroupBox.Text = "Skills";
            // 
            // skillsTextBox
            // 
            skillsTextBox.Location = new Point(20, 21);
            skillsTextBox.Multiline = true;
            skillsTextBox.Name = "skillsTextBox";
            skillsTextBox.Size = new Size(860, 87);
            skillsTextBox.TabIndex = 1;
            // 
            // tabExperiences
            // 
            tabExperiences.Controls.Add(experienceListBox);
            tabExperiences.Controls.Add(experienceGroupBox);
            tabExperiences.Location = new Point(4, 23);
            tabExperiences.Name = "tabExperiences";
            tabExperiences.Padding = new Padding(3);
            tabExperiences.Size = new Size(901, 516);
            tabExperiences.TabIndex = 1;
            tabExperiences.Text = "Experiences";
            tabExperiences.UseVisualStyleBackColor = true;
            // 
            // experienceListBox
            // 
            experienceListBox.FormattingEnabled = true;
            experienceListBox.ItemHeight = 14;
            experienceListBox.Location = new Point(412, 16);
            experienceListBox.Name = "experienceListBox";
            experienceListBox.Size = new Size(482, 382);
            experienceListBox.TabIndex = 1;
            experienceListBox.SelectedIndexChanged += ExperienceListBox_SelectedIndexChanged;
            // 
            // experienceGroupBox
            // 
            experienceGroupBox.Controls.Add(companyLabel);
            experienceGroupBox.Controls.Add(companyTextBox);
            experienceGroupBox.Controls.Add(positionLabel);
            experienceGroupBox.Controls.Add(positionTextBox);
            experienceGroupBox.Controls.Add(expStartDateLabel);
            experienceGroupBox.Controls.Add(expStartDateDateTimePicker);
            experienceGroupBox.Controls.Add(expEndDateLabel);
            experienceGroupBox.Controls.Add(expEndDateDateTimePicker);
            experienceGroupBox.Controls.Add(expDescriptionLabel);
            experienceGroupBox.Controls.Add(expDescriptionTextBox);
            experienceGroupBox.Controls.Add(expTechnologiesLabel);
            experienceGroupBox.Controls.Add(expTechnologiesTextBox);
            experienceGroupBox.Controls.Add(addExperienceButton);
            experienceGroupBox.Controls.Add(updateExperienceButton);
            experienceGroupBox.Controls.Add(deleteExperienceButton);
            experienceGroupBox.Location = new Point(6, 7);
            experienceGroupBox.Name = "experienceGroupBox";
            experienceGroupBox.Size = new Size(400, 424);
            experienceGroupBox.TabIndex = 0;
            experienceGroupBox.TabStop = false;
            experienceGroupBox.Text = "Add Experience";
            // 
            // companyLabel
            // 
            companyLabel.AutoSize = true;
            companyLabel.Location = new Point(20, 30);
            companyLabel.Name = "companyLabel";
            companyLabel.Size = new Size(61, 14);
            companyLabel.TabIndex = 5;
            companyLabel.Text = "Company:";
            // 
            // companyTextBox
            // 
            companyTextBox.Location = new Point(100, 28);
            companyTextBox.Name = "companyTextBox";
            companyTextBox.Size = new Size(214, 22);
            companyTextBox.TabIndex = 6;
            // 
            // positionLabel
            // 
            positionLabel.AutoSize = true;
            positionLabel.Location = new Point(20, 60);
            positionLabel.Name = "positionLabel";
            positionLabel.Size = new Size(53, 14);
            positionLabel.TabIndex = 7;
            positionLabel.Text = "Position:";
            // 
            // positionTextBox
            // 
            positionTextBox.Location = new Point(100, 58);
            positionTextBox.Name = "positionTextBox";
            positionTextBox.Size = new Size(214, 22);
            positionTextBox.TabIndex = 8;
            // 
            // expStartDateLabel
            // 
            expStartDateLabel.AutoSize = true;
            expStartDateLabel.Location = new Point(20, 90);
            expStartDateLabel.Name = "expStartDateLabel";
            expStartDateLabel.Size = new Size(68, 14);
            expStartDateLabel.TabIndex = 9;
            expStartDateLabel.Text = "Start Date:";
            // 
            // expStartDateDateTimePicker
            // 
            expStartDateDateTimePicker.Location = new Point(100, 88);
            expStartDateDateTimePicker.Name = "expStartDateDateTimePicker";
            expStartDateDateTimePicker.Size = new Size(214, 22);
            expStartDateDateTimePicker.TabIndex = 10;
            // 
            // expEndDateLabel
            // 
            expEndDateLabel.AutoSize = true;
            expEndDateLabel.Location = new Point(20, 120);
            expEndDateLabel.Name = "expEndDateLabel";
            expEndDateLabel.Size = new Size(62, 14);
            expEndDateLabel.TabIndex = 11;
            expEndDateLabel.Text = "End Date:";
            // 
            // expEndDateDateTimePicker
            // 
            expEndDateDateTimePicker.Location = new Point(100, 118);
            expEndDateDateTimePicker.Name = "expEndDateDateTimePicker";
            expEndDateDateTimePicker.Size = new Size(214, 22);
            expEndDateDateTimePicker.TabIndex = 12;
            // 
            // expDescriptionLabel
            // 
            expDescriptionLabel.AutoSize = true;
            expDescriptionLabel.Location = new Point(20, 150);
            expDescriptionLabel.Name = "expDescriptionLabel";
            expDescriptionLabel.Size = new Size(71, 14);
            expDescriptionLabel.TabIndex = 13;
            expDescriptionLabel.Text = "Description:";
            // 
            // expDescriptionTextBox
            // 
            expDescriptionTextBox.Location = new Point(100, 148);
            expDescriptionTextBox.Multiline = true;
            expDescriptionTextBox.Name = "expDescriptionTextBox";
            expDescriptionTextBox.Size = new Size(294, 242);
            expDescriptionTextBox.TabIndex = 14;
            // 
            // expTechnologiesLabel
            // 
            expTechnologiesLabel.AutoSize = true;
            expTechnologiesLabel.Location = new Point(20, 398);
            expTechnologiesLabel.Name = "expTechnologiesLabel";
            expTechnologiesLabel.Size = new Size(83, 14);
            expTechnologiesLabel.TabIndex = 15;
            expTechnologiesLabel.Text = "Technologies:";
            // 
            // expTechnologiesTextBox
            // 
            expTechnologiesTextBox.Location = new Point(100, 396);
            expTechnologiesTextBox.Name = "expTechnologiesTextBox";
            expTechnologiesTextBox.Size = new Size(294, 22);
            expTechnologiesTextBox.TabIndex = 16;
            expTechnologiesTextBox.TextChanged += ExpTechnologiesTextBox_TextChanged;
            // 
            // addExperienceButton
            // 
            addExperienceButton.Location = new Point(320, 28);
            addExperienceButton.Name = "addExperienceButton";
            addExperienceButton.Size = new Size(75, 23);
            addExperienceButton.TabIndex = 17;
            addExperienceButton.Text = "Add";
            addExperienceButton.UseVisualStyleBackColor = true;
            addExperienceButton.Click += AddExperienceButton_Click;
            // 
            // updateExperienceButton
            // 
            updateExperienceButton.Location = new Point(320, 58);
            updateExperienceButton.Name = "updateExperienceButton";
            updateExperienceButton.Size = new Size(75, 23);
            updateExperienceButton.TabIndex = 18;
            updateExperienceButton.Text = "Update";
            updateExperienceButton.UseVisualStyleBackColor = true;
            updateExperienceButton.Click += UpdateExperienceButton_Click;
            // 
            // deleteExperienceButton
            // 
            deleteExperienceButton.Location = new Point(320, 88);
            deleteExperienceButton.Name = "deleteExperienceButton";
            deleteExperienceButton.Size = new Size(75, 23);
            deleteExperienceButton.TabIndex = 19;
            deleteExperienceButton.Text = "Delete";
            deleteExperienceButton.UseVisualStyleBackColor = true;
            deleteExperienceButton.Click += DeleteExperienceButton_Click;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(educationListBox);
            tabPage3.Controls.Add(educationGroupBox);
            tabPage3.Location = new Point(4, 23);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(901, 516);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Educations and trainings";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // educationListBox
            // 
            educationListBox.FormattingEnabled = true;
            educationListBox.ItemHeight = 14;
            educationListBox.Location = new Point(440, 17);
            educationListBox.Name = "educationListBox";
            educationListBox.Size = new Size(373, 494);
            educationListBox.TabIndex = 1;
            educationListBox.SelectedIndexChanged += EducationListBox_SelectedIndexChanged;
            // 
            // educationGroupBox
            // 
            educationGroupBox.Controls.Add(institutionLabel);
            educationGroupBox.Controls.Add(institutionTextBox);
            educationGroupBox.Controls.Add(degreeLabel);
            educationGroupBox.Controls.Add(degreeTextBox);
            educationGroupBox.Controls.Add(fieldOfStudyLabel);
            educationGroupBox.Controls.Add(fieldOfStudyTextBox);
            educationGroupBox.Controls.Add(startDateLabel);
            educationGroupBox.Controls.Add(startDateDateTimePicker);
            educationGroupBox.Controls.Add(endDateLabel);
            educationGroupBox.Controls.Add(endDateDateTimePicker);
            educationGroupBox.Controls.Add(educationDescriptionLabel);
            educationGroupBox.Controls.Add(educationDescriptionTextBox);
            educationGroupBox.Controls.Add(addEducationButton);
            educationGroupBox.Controls.Add(updateEducationButton);
            educationGroupBox.Controls.Add(deleteEducationButton);
            educationGroupBox.Location = new Point(6, 7);
            educationGroupBox.Name = "educationGroupBox";
            educationGroupBox.Size = new Size(428, 559);
            educationGroupBox.TabIndex = 0;
            educationGroupBox.TabStop = false;
            educationGroupBox.Text = "Add Education";
            // 
            // institutionLabel
            // 
            institutionLabel.AutoSize = true;
            institutionLabel.Location = new Point(20, 30);
            institutionLabel.Name = "institutionLabel";
            institutionLabel.Size = new Size(67, 14);
            institutionLabel.TabIndex = 0;
            institutionLabel.Text = "Institution:";
            // 
            // institutionTextBox
            // 
            institutionTextBox.Location = new Point(100, 28);
            institutionTextBox.Name = "institutionTextBox";
            institutionTextBox.Size = new Size(214, 22);
            institutionTextBox.TabIndex = 1;
            // 
            // degreeLabel
            // 
            degreeLabel.AutoSize = true;
            degreeLabel.Location = new Point(20, 60);
            degreeLabel.Name = "degreeLabel";
            degreeLabel.Size = new Size(51, 14);
            degreeLabel.TabIndex = 2;
            degreeLabel.Text = "Degree:";
            // 
            // degreeTextBox
            // 
            degreeTextBox.Location = new Point(100, 58);
            degreeTextBox.Name = "degreeTextBox";
            degreeTextBox.Size = new Size(214, 22);
            degreeTextBox.TabIndex = 3;
            // 
            // fieldOfStudyLabel
            // 
            fieldOfStudyLabel.AutoSize = true;
            fieldOfStudyLabel.Location = new Point(20, 90);
            fieldOfStudyLabel.Name = "fieldOfStudyLabel";
            fieldOfStudyLabel.Size = new Size(86, 14);
            fieldOfStudyLabel.TabIndex = 4;
            fieldOfStudyLabel.Text = "Field of Study:";
            // 
            // fieldOfStudyTextBox
            // 
            fieldOfStudyTextBox.Location = new Point(100, 88);
            fieldOfStudyTextBox.Name = "fieldOfStudyTextBox";
            fieldOfStudyTextBox.Size = new Size(214, 22);
            fieldOfStudyTextBox.TabIndex = 5;
            // 
            // startDateLabel
            // 
            startDateLabel.AutoSize = true;
            startDateLabel.Location = new Point(20, 120);
            startDateLabel.Name = "startDateLabel";
            startDateLabel.Size = new Size(68, 14);
            startDateLabel.TabIndex = 6;
            startDateLabel.Text = "Start Date:";
            // 
            // startDateDateTimePicker
            // 
            startDateDateTimePicker.Location = new Point(100, 118);
            startDateDateTimePicker.Name = "startDateDateTimePicker";
            startDateDateTimePicker.Size = new Size(295, 22);
            startDateDateTimePicker.TabIndex = 7;
            // 
            // endDateLabel
            // 
            endDateLabel.AutoSize = true;
            endDateLabel.Location = new Point(20, 150);
            endDateLabel.Name = "endDateLabel";
            endDateLabel.Size = new Size(62, 14);
            endDateLabel.TabIndex = 8;
            endDateLabel.Text = "End Date:";
            // 
            // endDateDateTimePicker
            // 
            endDateDateTimePicker.Location = new Point(100, 148);
            endDateDateTimePicker.Name = "endDateDateTimePicker";
            endDateDateTimePicker.Size = new Size(295, 22);
            endDateDateTimePicker.TabIndex = 9;
            // 
            // educationDescriptionLabel
            // 
            educationDescriptionLabel.AutoSize = true;
            educationDescriptionLabel.Location = new Point(20, 180);
            educationDescriptionLabel.Name = "educationDescriptionLabel";
            educationDescriptionLabel.Size = new Size(71, 14);
            educationDescriptionLabel.TabIndex = 10;
            educationDescriptionLabel.Text = "Description:";
            // 
            // educationDescriptionTextBox
            // 
            educationDescriptionTextBox.Location = new Point(100, 178);
            educationDescriptionTextBox.Multiline = true;
            educationDescriptionTextBox.Name = "educationDescriptionTextBox";
            educationDescriptionTextBox.Size = new Size(322, 325);
            educationDescriptionTextBox.TabIndex = 11;
            // 
            // addEducationButton
            // 
            addEducationButton.Location = new Point(320, 28);
            addEducationButton.Name = "addEducationButton";
            addEducationButton.Size = new Size(75, 23);
            addEducationButton.TabIndex = 12;
            addEducationButton.Text = "Add";
            addEducationButton.UseVisualStyleBackColor = true;
            addEducationButton.Click += AddEducationButton_Click;
            // 
            // updateEducationButton
            // 
            updateEducationButton.Location = new Point(320, 58);
            updateEducationButton.Name = "updateEducationButton";
            updateEducationButton.Size = new Size(75, 23);
            updateEducationButton.TabIndex = 13;
            updateEducationButton.Text = "Update";
            updateEducationButton.UseVisualStyleBackColor = true;
            updateEducationButton.Click += UpdateEducationButton_Click;
            // 
            // deleteEducationButton
            // 
            deleteEducationButton.Location = new Point(320, 88);
            deleteEducationButton.Name = "deleteEducationButton";
            deleteEducationButton.Size = new Size(75, 23);
            deleteEducationButton.TabIndex = 14;
            deleteEducationButton.Text = "Delete";
            deleteEducationButton.UseVisualStyleBackColor = true;
            deleteEducationButton.Click += DeleteEducationButton_Click;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(creationListBox);
            tabPage4.Controls.Add(creationGroupBox);
            tabPage4.Location = new Point(4, 23);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(901, 516);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Creations";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // creationListBox
            // 
            creationListBox.FormattingEnabled = true;
            creationListBox.ItemHeight = 14;
            creationListBox.Location = new Point(412, 16);
            creationListBox.Name = "creationListBox";
            creationListBox.Size = new Size(471, 424);
            creationListBox.TabIndex = 1;
            creationListBox.SelectedIndexChanged += CreationListBox_SelectedIndexChanged;
            // 
            // creationGroupBox
            // 
            creationGroupBox.Controls.Add(creationTitleLabel);
            creationGroupBox.Controls.Add(creationTitleTextBox);
            creationGroupBox.Controls.Add(creationDescriptionLabel);
            creationGroupBox.Controls.Add(creationDescriptionTextBox);
            creationGroupBox.Controls.Add(creationUrlLabel);
            creationGroupBox.Controls.Add(creationUrlTextBox);
            creationGroupBox.Controls.Add(addCreationButton);
            creationGroupBox.Controls.Add(updateCreationButton);
            creationGroupBox.Controls.Add(deleteCreationButton);
            creationGroupBox.Location = new Point(6, 7);
            creationGroupBox.Name = "creationGroupBox";
            creationGroupBox.Size = new Size(400, 424);
            creationGroupBox.TabIndex = 0;
            creationGroupBox.TabStop = false;
            creationGroupBox.Text = "Add Creation";
            // 
            // creationTitleLabel
            // 
            creationTitleLabel.AutoSize = true;
            creationTitleLabel.Location = new Point(20, 30);
            creationTitleLabel.Name = "creationTitleLabel";
            creationTitleLabel.Size = new Size(35, 14);
            creationTitleLabel.TabIndex = 0;
            creationTitleLabel.Text = "Title:";
            // 
            // creationTitleTextBox
            // 
            creationTitleTextBox.Location = new Point(100, 28);
            creationTitleTextBox.Name = "creationTitleTextBox";
            creationTitleTextBox.Size = new Size(214, 22);
            creationTitleTextBox.TabIndex = 1;
            // 
            // creationDescriptionLabel
            // 
            creationDescriptionLabel.AutoSize = true;
            creationDescriptionLabel.Location = new Point(20, 60);
            creationDescriptionLabel.Name = "creationDescriptionLabel";
            creationDescriptionLabel.Size = new Size(71, 14);
            creationDescriptionLabel.TabIndex = 2;
            creationDescriptionLabel.Text = "Description:";
            // 
            // creationDescriptionTextBox
            // 
            creationDescriptionTextBox.Location = new Point(100, 58);
            creationDescriptionTextBox.Multiline = true;
            creationDescriptionTextBox.Name = "creationDescriptionTextBox";
            creationDescriptionTextBox.Size = new Size(214, 332);
            creationDescriptionTextBox.TabIndex = 3;
            // 
            // creationUrlLabel
            // 
            creationUrlLabel.AutoSize = true;
            creationUrlLabel.Location = new Point(20, 398);
            creationUrlLabel.Name = "creationUrlLabel";
            creationUrlLabel.Size = new Size(32, 14);
            creationUrlLabel.TabIndex = 4;
            creationUrlLabel.Text = "URL:";
            // 
            // creationUrlTextBox
            // 
            creationUrlTextBox.Location = new Point(100, 396);
            creationUrlTextBox.Name = "creationUrlTextBox";
            creationUrlTextBox.Size = new Size(279, 22);
            creationUrlTextBox.TabIndex = 5;
            // 
            // addCreationButton
            // 
            addCreationButton.Location = new Point(320, 28);
            addCreationButton.Name = "addCreationButton";
            addCreationButton.Size = new Size(75, 23);
            addCreationButton.TabIndex = 6;
            addCreationButton.Text = "Add";
            addCreationButton.UseVisualStyleBackColor = true;
            addCreationButton.Click += AddCreationButton_Click;
            // 
            // updateCreationButton
            // 
            updateCreationButton.Location = new Point(320, 58);
            updateCreationButton.Name = "updateCreationButton";
            updateCreationButton.Size = new Size(75, 23);
            updateCreationButton.TabIndex = 7;
            updateCreationButton.Text = "Update";
            updateCreationButton.UseVisualStyleBackColor = true;
            updateCreationButton.Click += UpdateCreationButton_Click;
            // 
            // deleteCreationButton
            // 
            deleteCreationButton.Location = new Point(320, 88);
            deleteCreationButton.Name = "deleteCreationButton";
            deleteCreationButton.Size = new Size(75, 23);
            deleteCreationButton.TabIndex = 8;
            deleteCreationButton.Text = "Delete";
            deleteCreationButton.UseVisualStyleBackColor = true;
            deleteCreationButton.Click += DeleteCreationButton_Click;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(commentTextBox);
            tabPage5.Controls.Add(newCommentLabel);
            tabPage5.Controls.Add(newCommentTextBox);
            tabPage5.Controls.Add(addCommentButton);
            tabPage5.Location = new Point(4, 23);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(901, 516);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Note";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // commentTextBox
            // 
            commentTextBox.Location = new Point(5, 7);
            commentTextBox.Multiline = true;
            commentTextBox.Name = "commentTextBox";
            commentTextBox.ReadOnly = true;
            commentTextBox.ScrollBars = ScrollBars.Vertical;
            commentTextBox.Size = new Size(885, 400);
            commentTextBox.TabIndex = 4;
            // 
            // newCommentLabel
            // 
            newCommentLabel.AutoSize = true;
            newCommentLabel.Location = new Point(5, 420);
            newCommentLabel.Name = "newCommentLabel";
            newCommentLabel.Size = new Size(119, 14);
            newCommentLabel.TabIndex = 5;
            newCommentLabel.Text = "Add New Comment:";
            // 
            // newCommentTextBox
            // 
            newCommentTextBox.Location = new Point(5, 440);
            newCommentTextBox.Multiline = true;
            newCommentTextBox.Name = "newCommentTextBox";
            newCommentTextBox.Size = new Size(750, 60);
            newCommentTextBox.TabIndex = 6;
            // 
            // addCommentButton
            // 
            addCommentButton.BackColor = Color.FromArgb(46, 204, 113);
            addCommentButton.FlatStyle = FlatStyle.Flat;
            addCommentButton.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            addCommentButton.ForeColor = Color.White;
            addCommentButton.Location = new Point(765, 440);
            addCommentButton.Name = "addCommentButton";
            addCommentButton.Size = new Size(125, 60);
            addCommentButton.TabIndex = 7;
            addCommentButton.Text = "Add Comment";
            addCommentButton.UseVisualStyleBackColor = false;
            addCommentButton.Click += AddCommentButton_Click;
            // 
            // searchPanel
            // 
            searchPanel.BackColor = Color.FromArgb(240, 240, 240);
            searchPanel.BorderStyle = BorderStyle.FixedSingle;
            searchPanel.Controls.Add(searchLabel);
            searchPanel.Controls.Add(searchTextBox);
            searchPanel.Controls.Add(searchButton);
            searchPanel.Controls.Add(newButton);
            searchPanel.Dock = DockStyle.Top;
            searchPanel.Location = new Point(0, 200);
            searchPanel.Name = "searchPanel";
            searchPanel.Size = new Size(927, 51);
            searchPanel.TabIndex = 7;
            // 
            // searchLabel
            // 
            searchLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            searchLabel.Location = new Point(10, 15);
            searchLabel.Name = "searchLabel";
            searchLabel.Size = new Size(100, 20);
            searchLabel.TabIndex = 0;
            searchLabel.Text = "Search CVs:";
            // 
            // searchTextBox
            // 
            searchTextBox.Font = new Font("Tahoma", 9F);
            searchTextBox.Location = new Point(120, 12);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(400, 22);
            searchTextBox.TabIndex = 1;
            // 
            // searchButton
            // 
            searchButton.BackColor = Color.FromArgb(52, 152, 219);
            searchButton.FlatStyle = FlatStyle.Flat;
            searchButton.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            searchButton.ForeColor = Color.White;
            searchButton.Location = new Point(526, 10);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(80, 30);
            searchButton.TabIndex = 2;
            searchButton.Text = "Search";
            searchButton.UseVisualStyleBackColor = false;
            searchButton.Click += SearchButton_Click;
            // 
            // newButton
            // 
            newButton.BackColor = Color.FromArgb(46, 204, 113);
            newButton.FlatStyle = FlatStyle.Flat;
            newButton.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            newButton.ForeColor = Color.White;
            newButton.Location = new Point(612, 10);
            newButton.Name = "newButton";
            newButton.Size = new Size(80, 30);
            newButton.TabIndex = 3;
            newButton.Text = "New CV";
            newButton.UseVisualStyleBackColor = false;
            newButton.Click += NewButton_Click;
            // 
            // cvsDataGrid
            // 
            cvsDataGrid.AllowUserToAddRows = false;
            cvsDataGrid.AllowUserToDeleteRows = false;
            cvsDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cvsDataGrid.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7 });
            cvsDataGrid.Dock = DockStyle.Top;
            cvsDataGrid.Location = new Point(0, 0);
            cvsDataGrid.MultiSelect = false;
            cvsDataGrid.Name = "cvsDataGrid";
            cvsDataGrid.ReadOnly = true;
            cvsDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cvsDataGrid.Size = new Size(927, 200);
            cvsDataGrid.TabIndex = 8;
            cvsDataGrid.CellClick += CvsDataGrid_CellClick;
            cvsDataGrid.CellDoubleClick += CvsDataGrid_CellDoubleClick;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "ID";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Title";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Name";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Email";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Phone";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "City";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Date";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // saveButton
            // 
            saveButton.BackColor = Color.FromArgb(52, 152, 219);
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            saveButton.ForeColor = Color.White;
            saveButton.Location = new Point(488, 806);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(100, 35);
            saveButton.TabIndex = 3;
            saveButton.Text = "Save CV";
            saveButton.UseVisualStyleBackColor = false;
            saveButton.Click += SaveButton_Click;
            // 
            // cloneButton
            // 
            cloneButton.BackColor = Color.FromArgb(230, 126, 34);
            cloneButton.FlatStyle = FlatStyle.Flat;
            cloneButton.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            cloneButton.ForeColor = Color.White;
            cloneButton.Location = new Point(137, 806);
            cloneButton.Name = "cloneButton";
            cloneButton.Size = new Size(100, 35);
            cloneButton.TabIndex = 5;
            cloneButton.Text = "Clone CV";
            cloneButton.UseVisualStyleBackColor = false;
            cloneButton.Click += CloneButton_Click;
            // 
            // generateWordButton
            // 
            generateWordButton.BackColor = Color.FromArgb(155, 89, 182);
            generateWordButton.FlatStyle = FlatStyle.Flat;
            generateWordButton.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            generateWordButton.ForeColor = Color.White;
            generateWordButton.Location = new Point(746, 806);
            generateWordButton.Name = "generateWordButton";
            generateWordButton.Size = new Size(162, 35);
            generateWordButton.TabIndex = 6;
            generateWordButton.Text = "?? Export as a Word";
            generateWordButton.UseVisualStyleBackColor = false;
            generateWordButton.Click += GenerateWordButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.BackColor = Color.FromArgb(231, 76, 60);
            deleteButton.FlatStyle = FlatStyle.Flat;
            deleteButton.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            deleteButton.ForeColor = Color.White;
            deleteButton.Location = new Point(11, 806);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(120, 35);
            deleteButton.TabIndex = 7;
            deleteButton.Text = "??? Delete CV";
            deleteButton.UseVisualStyleBackColor = false;
            deleteButton.Click += DeleteButton_Click;
            // 
            // saveAsJsonButton
            // 
            saveAsJsonButton.BackColor = Color.FromArgb(52, 152, 219);
            saveAsJsonButton.FlatStyle = FlatStyle.Flat;
            saveAsJsonButton.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            saveAsJsonButton.ForeColor = Color.White;
            saveAsJsonButton.Location = new Point(382, 806);
            saveAsJsonButton.Name = "saveAsJsonButton";
            saveAsJsonButton.Size = new Size(100, 35);
            saveAsJsonButton.TabIndex = 8;
            saveAsJsonButton.Text = "?? Save JSON";
            saveAsJsonButton.UseVisualStyleBackColor = false;
            saveAsJsonButton.Click += SaveAsJsonButton_Click;
            // 
            // loadJsonButton
            // 
            loadJsonButton.BackColor = Color.FromArgb(155, 89, 182);
            loadJsonButton.FlatStyle = FlatStyle.Flat;
            loadJsonButton.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            loadJsonButton.ForeColor = Color.White;
            loadJsonButton.Location = new Point(243, 806);
            loadJsonButton.Name = "loadJsonButton";
            loadJsonButton.Size = new Size(133, 35);
            loadJsonButton.TabIndex = 9;
            loadJsonButton.Text = "?? Load JSON";
            loadJsonButton.UseVisualStyleBackColor = false;
            loadJsonButton.Click += LoadJsonButton_Click;
            // 
            // statusLabel
            // 
            statusLabel.Font = new Font("Tahoma", 9F);
            statusLabel.Location = new Point(234, 254);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(400, 20);
            statusLabel.TabIndex = 6;
            statusLabel.Text = "Ready";
            statusLabel.Click += statusLabel_Click;
            // 
            // removeEducationButton
            // 
            removeEducationButton.Location = new Point(0, 0);
            removeEducationButton.Name = "removeEducationButton";
            removeEducationButton.Size = new Size(75, 23);
            removeEducationButton.TabIndex = 0;
            // 
            // removeExperienceButton
            // 
            removeExperienceButton.Location = new Point(0, 0);
            removeExperienceButton.Name = "removeExperienceButton";
            removeExperienceButton.Size = new Size(75, 23);
            removeExperienceButton.TabIndex = 0;
            // 
            // removeCreationButton
            // 
            removeCreationButton.Location = new Point(0, 0);
            removeCreationButton.Name = "removeCreationButton";
            removeCreationButton.Size = new Size(75, 23);
            removeCreationButton.TabIndex = 0;
            // 
            // CvManagerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(927, 857);
            Controls.Add(detailsPanel);
            Font = new Font("Tahoma", 9F);
            Name = "CvManagerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CV Manager";
            detailsPanel.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            TabHeader.ResumeLayout(false);
            basicInfoGroupBox.ResumeLayout(false);
            basicInfoGroupBox.PerformLayout();
            addressGroupBox.ResumeLayout(false);
            addressGroupBox.PerformLayout();
            skillsGroupBox.ResumeLayout(false);
            skillsGroupBox.PerformLayout();
            tabExperiences.ResumeLayout(false);
            experienceGroupBox.ResumeLayout(false);
            experienceGroupBox.PerformLayout();
            tabPage3.ResumeLayout(false);
            educationGroupBox.ResumeLayout(false);
            educationGroupBox.PerformLayout();
            tabPage4.ResumeLayout(false);
            creationGroupBox.ResumeLayout(false);
            creationGroupBox.PerformLayout();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            searchPanel.ResumeLayout(false);
            searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cvsDataGrid).EndInit();
            ResumeLayout(false);
        }
        private Panel searchPanel;
        private Label searchLabel;
        private TextBox searchTextBox;
        private Button searchButton;
        private Button newButton;
        private DataGridView cvsDataGrid;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private TabControl tabControl1;
        private TabPage TabHeader;
        private TabPage tabExperiences;
        private GroupBox basicInfoGroupBox;
        private Label titleLabel;
        private TextBox titleTextBox;
        private Label firstNameLabel;
        private TextBox firstNameTextBox;
        private Label lastNameLabel;
        private TextBox lastNameTextBox;
        private Label emailLabel;
        private TextBox emailTextBox;
        private Label phoneLabel;
        private TextBox phoneTextBox;
        private Label linkedInLabel;
        private TextBox linkedInTextBox;
        private Label summaryLabel;
        private TextBox summaryTextBox;
        private GroupBox addressGroupBox;
        private Label addressStreetLabel;
        private TextBox addressStreetTextBox;
        private Label addressNumberLabel;
        private TextBox addressNumberTextBox;
        private Label addressCityLabel;
        private TextBox addressCityTextBox;
        private Label addressZipLabel;
        private TextBox addressZipTextBox;
        private Label addressCountryLabel;
        private TextBox addressCountryTextBox;
        private GroupBox skillsGroupBox;
        private TextBox skillsTextBox;
        private ListBox experienceListBox;
        private GroupBox experienceGroupBox;
        private Label companyLabel;
        private TextBox companyTextBox;
        private Label positionLabel;
        private TextBox positionTextBox;
        private Label expStartDateLabel;
        private DateTimePicker expStartDateDateTimePicker;
        private Label expEndDateLabel;
        private DateTimePicker expEndDateDateTimePicker;
        private Label expDescriptionLabel;
        private TextBox expDescriptionTextBox;
        private Label expTechnologiesLabel;
        private TextBox expTechnologiesTextBox;
        private Button addExperienceButton;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
    }
}
