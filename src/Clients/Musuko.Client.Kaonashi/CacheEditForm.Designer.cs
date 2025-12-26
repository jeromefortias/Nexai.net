namespace Musuko.Kaonashi
{
    partial class CacheEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            headerPanel = new Panel();
            titleLabel = new Label();
            mainTabControl = new TabControl();
            basicTabPage = new TabPage();
            generatedSystemPromptLabel = new Label();
            generatedSystemPromptTextBox = new TextBox();
            parentCacheIdLabel = new Label();
            parentCacheIdTextBox = new TextBox();
            chatModeLabel = new Label();
            chatModeComboBox = new ComboBox();
            modelLabel = new Label();
            modelTextBox = new TextBox();
            languageLabel = new Label();
            languageTextBox = new TextBox();
            completionLabel = new Label();
            completionTextBox = new TextBox();
            promptLabel = new Label();
            promptTextBox = new TextBox();
            commentLabel = new Label();
            commentTextBox = new TextBox();
            userNameLabel = new Label();
            userNameTextBox = new TextBox();
            machineNameLabel = new Label();
            machineNameTextBox = new TextBox();
            dateLabel = new Label();
            dateTextBox = new TextBox();
            idLabel = new Label();
            idTextBox = new TextBox();
            tagsTabPage = new TabPage();
            tagInputPanel = new Panel();
            tagInputTextBox = new TextBox();
            tagInputLabel = new Label();
            tagsPanel = new Panel();
            generatedTagsGroupBox = new GroupBox();
            removeGeneratedTagsButton = new Button();
            addGeneratedTagsButton = new Button();
            generatedTagsListBox = new ListBox();
            tagsShouldNotGroupBox = new GroupBox();
            removeTagsShouldNotButton = new Button();
            addTagsShouldNotButton = new Button();
            tagsShouldNotListBox = new ListBox();
            tagsMustNotGroupBox = new GroupBox();
            removeTagsMustNotButton = new Button();
            addTagsMustNotButton = new Button();
            tagsMustNotListBox = new ListBox();
            tagsShouldGroupBox = new GroupBox();
            removeTagsShouldButton = new Button();
            addTagsShouldButton = new Button();
            tagsShouldListBox = new ListBox();
            tagsMustGroupBox = new GroupBox();
            removeTagsMustButton = new Button();
            addTagsMustButton = new Button();
            tagsMustListBox = new ListBox();
            footerPanel = new Panel();
            statusLabel = new Label();
            addChildButton = new Button();
            cancelButton = new Button();
            saveButton = new Button();
            headerPanel.SuspendLayout();
            mainTabControl.SuspendLayout();
            basicTabPage.SuspendLayout();
            tagsTabPage.SuspendLayout();
            tagInputPanel.SuspendLayout();
            tagsPanel.SuspendLayout();
            generatedTagsGroupBox.SuspendLayout();
            tagsShouldNotGroupBox.SuspendLayout();
            tagsMustNotGroupBox.SuspendLayout();
            tagsShouldGroupBox.SuspendLayout();
            tagsMustGroupBox.SuspendLayout();
            footerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(255, 255, 255);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Margin = new Padding(4, 3, 4, 3);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(1392, 58);
            headerPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(33, 37, 41);
            titleLabel.Location = new Point(18, 14);
            titleLabel.Margin = new Padding(4, 0, 4, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(123, 25);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Cache Editor";
            // 
            // mainTabControl
            // 
            mainTabControl.Controls.Add(basicTabPage);
            mainTabControl.Controls.Add(tagsTabPage);
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Location = new Point(0, 58);
            mainTabControl.Margin = new Padding(4, 3, 4, 3);
            mainTabControl.Name = "mainTabControl";
            mainTabControl.SelectedIndex = 0;
            mainTabControl.Size = new Size(1392, 652);
            mainTabControl.TabIndex = 1;
            // 
            // basicTabPage
            // 
            basicTabPage.BackColor = Color.FromArgb(248, 249, 250);
            basicTabPage.Controls.Add(generatedSystemPromptLabel);
            basicTabPage.Controls.Add(generatedSystemPromptTextBox);
            basicTabPage.Controls.Add(parentCacheIdLabel);
            basicTabPage.Controls.Add(parentCacheIdTextBox);
            basicTabPage.Controls.Add(chatModeLabel);
            basicTabPage.Controls.Add(chatModeComboBox);
            basicTabPage.Controls.Add(modelLabel);
            basicTabPage.Controls.Add(modelTextBox);
            basicTabPage.Controls.Add(languageLabel);
            basicTabPage.Controls.Add(languageTextBox);
            basicTabPage.Controls.Add(completionLabel);
            basicTabPage.Controls.Add(completionTextBox);
            basicTabPage.Controls.Add(promptLabel);
            basicTabPage.Controls.Add(promptTextBox);
            basicTabPage.Controls.Add(commentLabel);
            basicTabPage.Controls.Add(commentTextBox);
            basicTabPage.Controls.Add(userNameLabel);
            basicTabPage.Controls.Add(userNameTextBox);
            basicTabPage.Controls.Add(machineNameLabel);
            basicTabPage.Controls.Add(machineNameTextBox);
            basicTabPage.Controls.Add(dateLabel);
            basicTabPage.Controls.Add(dateTextBox);
            basicTabPage.Controls.Add(idLabel);
            basicTabPage.Controls.Add(idTextBox);
            basicTabPage.Location = new Point(4, 24);
            basicTabPage.Margin = new Padding(4, 3, 4, 3);
            basicTabPage.Name = "basicTabPage";
            basicTabPage.Padding = new Padding(4, 3, 4, 3);
            basicTabPage.Size = new Size(1384, 624);
            basicTabPage.TabIndex = 0;
            basicTabPage.Text = "Basic Properties";
            // 
            // generatedSystemPromptLabel
            // 
            generatedSystemPromptLabel.AutoSize = true;
            generatedSystemPromptLabel.ForeColor = Color.FromArgb(33, 37, 41);
            generatedSystemPromptLabel.Location = new Point(23, 554);
            generatedSystemPromptLabel.Margin = new Padding(4, 0, 4, 0);
            generatedSystemPromptLabel.Name = "generatedSystemPromptLabel";
            generatedSystemPromptLabel.Size = new Size(148, 15);
            generatedSystemPromptLabel.TabIndex = 23;
            generatedSystemPromptLabel.Text = "Generated System Prompt:";
            // 
            // generatedSystemPromptTextBox
            // 
            generatedSystemPromptTextBox.BackColor = Color.FromArgb(255, 255, 255);
            generatedSystemPromptTextBox.BorderStyle = BorderStyle.FixedSingle;
            generatedSystemPromptTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            generatedSystemPromptTextBox.Location = new Point(23, 572);
            generatedSystemPromptTextBox.Margin = new Padding(4, 3, 4, 3);
            generatedSystemPromptTextBox.Multiline = true;
            generatedSystemPromptTextBox.Name = "generatedSystemPromptTextBox";
            generatedSystemPromptTextBox.ScrollBars = ScrollBars.Vertical;
            generatedSystemPromptTextBox.Size = new Size(1341, 23);
            generatedSystemPromptTextBox.TabIndex = 24;
            // 
            // parentCacheIdLabel
            // 
            parentCacheIdLabel.AutoSize = true;
            parentCacheIdLabel.ForeColor = Color.FromArgb(33, 37, 41);
            parentCacheIdLabel.Location = new Point(23, 508);
            parentCacheIdLabel.Margin = new Padding(4, 0, 4, 0);
            parentCacheIdLabel.Name = "parentCacheIdLabel";
            parentCacheIdLabel.Size = new Size(94, 15);
            parentCacheIdLabel.TabIndex = 21;
            parentCacheIdLabel.Text = "Parent Cache ID:";
            // 
            // parentCacheIdTextBox
            // 
            parentCacheIdTextBox.BackColor = Color.FromArgb(255, 255, 255);
            parentCacheIdTextBox.BorderStyle = BorderStyle.FixedSingle;
            parentCacheIdTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            parentCacheIdTextBox.Location = new Point(23, 526);
            parentCacheIdTextBox.Margin = new Padding(4, 3, 4, 3);
            parentCacheIdTextBox.Name = "parentCacheIdTextBox";
            parentCacheIdTextBox.Size = new Size(233, 23);
            parentCacheIdTextBox.TabIndex = 22;
            // 
            // chatModeLabel
            // 
            chatModeLabel.AutoSize = true;
            chatModeLabel.ForeColor = Color.FromArgb(33, 37, 41);
            chatModeLabel.Location = new Point(537, 462);
            chatModeLabel.Margin = new Padding(4, 0, 4, 0);
            chatModeLabel.Name = "chatModeLabel";
            chatModeLabel.Size = new Size(69, 15);
            chatModeLabel.TabIndex = 19;
            chatModeLabel.Text = "Chat Mode:";
            // 
            // chatModeComboBox
            // 
            chatModeComboBox.BackColor = Color.FromArgb(255, 255, 255);
            chatModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            chatModeComboBox.ForeColor = Color.FromArgb(33, 37, 41);
            chatModeComboBox.FormattingEnabled = true;
            chatModeComboBox.Items.AddRange(new object[] { "completion", "chat", "diagnostic" });
            chatModeComboBox.Location = new Point(537, 480);
            chatModeComboBox.Margin = new Padding(4, 3, 4, 3);
            chatModeComboBox.Name = "chatModeComboBox";
            chatModeComboBox.Size = new Size(174, 23);
            chatModeComboBox.TabIndex = 20;
            // 
            // modelLabel
            // 
            modelLabel.AutoSize = true;
            modelLabel.ForeColor = Color.FromArgb(33, 37, 41);
            modelLabel.Location = new Point(280, 462);
            modelLabel.Margin = new Padding(4, 0, 4, 0);
            modelLabel.Name = "modelLabel";
            modelLabel.Size = new Size(44, 15);
            modelLabel.TabIndex = 17;
            modelLabel.Text = "Model:";
            // 
            // modelTextBox
            // 
            modelTextBox.BackColor = Color.FromArgb(255, 255, 255);
            modelTextBox.BorderStyle = BorderStyle.FixedSingle;
            modelTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            modelTextBox.Location = new Point(280, 480);
            modelTextBox.Margin = new Padding(4, 3, 4, 3);
            modelTextBox.Name = "modelTextBox";
            modelTextBox.Size = new Size(233, 23);
            modelTextBox.TabIndex = 18;
            // 
            // languageLabel
            // 
            languageLabel.AutoSize = true;
            languageLabel.ForeColor = Color.FromArgb(33, 37, 41);
            languageLabel.Location = new Point(23, 462);
            languageLabel.Margin = new Padding(4, 0, 4, 0);
            languageLabel.Name = "languageLabel";
            languageLabel.Size = new Size(62, 15);
            languageLabel.TabIndex = 15;
            languageLabel.Text = "Language:";
            // 
            // languageTextBox
            // 
            languageTextBox.BackColor = Color.FromArgb(255, 255, 255);
            languageTextBox.BorderStyle = BorderStyle.FixedSingle;
            languageTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            languageTextBox.Location = new Point(23, 480);
            languageTextBox.Margin = new Padding(4, 3, 4, 3);
            languageTextBox.Name = "languageTextBox";
            languageTextBox.Size = new Size(116, 23);
            languageTextBox.TabIndex = 16;
            // 
            // completionLabel
            // 
            completionLabel.AutoSize = true;
            completionLabel.ForeColor = Color.FromArgb(33, 37, 41);
            completionLabel.Location = new Point(23, 231);
            completionLabel.Margin = new Padding(4, 0, 4, 0);
            completionLabel.Name = "completionLabel";
            completionLabel.Size = new Size(73, 15);
            completionLabel.TabIndex = 13;
            completionLabel.Text = "Completion:";
            // 
            // completionTextBox
            // 
            completionTextBox.BackColor = Color.FromArgb(255, 255, 255);
            completionTextBox.BorderStyle = BorderStyle.FixedSingle;
            completionTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            completionTextBox.Location = new Point(23, 249);
            completionTextBox.Margin = new Padding(4, 3, 4, 3);
            completionTextBox.Multiline = true;
            completionTextBox.Name = "completionTextBox";
            completionTextBox.ScrollBars = ScrollBars.Vertical;
            completionTextBox.Size = new Size(1341, 230);
            completionTextBox.TabIndex = 14;
            // 
            // promptLabel
            // 
            promptLabel.AutoSize = true;
            promptLabel.ForeColor = Color.FromArgb(33, 37, 41);
            promptLabel.Location = new Point(23, 138);
            promptLabel.Margin = new Padding(4, 0, 4, 0);
            promptLabel.Name = "promptLabel";
            promptLabel.Size = new Size(50, 15);
            promptLabel.TabIndex = 11;
            promptLabel.Text = "Prompt:";
            // 
            // promptTextBox
            // 
            promptTextBox.BackColor = Color.FromArgb(255, 255, 255);
            promptTextBox.BorderStyle = BorderStyle.FixedSingle;
            promptTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            promptTextBox.Location = new Point(23, 157);
            promptTextBox.Margin = new Padding(4, 3, 4, 3);
            promptTextBox.Multiline = true;
            promptTextBox.Name = "promptTextBox";
            promptTextBox.ScrollBars = ScrollBars.Vertical;
            promptTextBox.Size = new Size(1341, 92);
            promptTextBox.TabIndex = 12;
            // 
            // commentLabel
            // 
            commentLabel.AutoSize = true;
            commentLabel.ForeColor = Color.FromArgb(33, 37, 41);
            commentLabel.Location = new Point(23, 92);
            commentLabel.Margin = new Padding(4, 0, 4, 0);
            commentLabel.Name = "commentLabel";
            commentLabel.Size = new Size(64, 15);
            commentLabel.TabIndex = 9;
            commentLabel.Text = "Comment:";
            // 
            // commentTextBox
            // 
            commentTextBox.BackColor = Color.FromArgb(255, 255, 255);
            commentTextBox.BorderStyle = BorderStyle.FixedSingle;
            commentTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            commentTextBox.Location = new Point(23, 111);
            commentTextBox.Margin = new Padding(4, 3, 4, 3);
            commentTextBox.Name = "commentTextBox";
            commentTextBox.Size = new Size(1341, 23);
            commentTextBox.TabIndex = 10;
            // 
            // userNameLabel
            // 
            userNameLabel.AutoSize = true;
            userNameLabel.ForeColor = Color.FromArgb(33, 37, 41);
            userNameLabel.Location = new Point(537, 46);
            userNameLabel.Margin = new Padding(4, 0, 4, 0);
            userNameLabel.Name = "userNameLabel";
            userNameLabel.Size = new Size(68, 15);
            userNameLabel.TabIndex = 7;
            userNameLabel.Text = "User Name:";
            // 
            // userNameTextBox
            // 
            userNameTextBox.BackColor = Color.FromArgb(255, 255, 255);
            userNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            userNameTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            userNameTextBox.Location = new Point(537, 65);
            userNameTextBox.Margin = new Padding(4, 3, 4, 3);
            userNameTextBox.Name = "userNameTextBox";
            userNameTextBox.Size = new Size(233, 23);
            userNameTextBox.TabIndex = 8;
            // 
            // machineNameLabel
            // 
            machineNameLabel.AutoSize = true;
            machineNameLabel.ForeColor = Color.FromArgb(33, 37, 41);
            machineNameLabel.Location = new Point(280, 46);
            machineNameLabel.Margin = new Padding(4, 0, 4, 0);
            machineNameLabel.Name = "machineNameLabel";
            machineNameLabel.Size = new Size(91, 15);
            machineNameLabel.TabIndex = 5;
            machineNameLabel.Text = "Machine Name:";
            // 
            // machineNameTextBox
            // 
            machineNameTextBox.BackColor = Color.FromArgb(255, 255, 255);
            machineNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            machineNameTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            machineNameTextBox.Location = new Point(280, 65);
            machineNameTextBox.Margin = new Padding(4, 3, 4, 3);
            machineNameTextBox.Name = "machineNameTextBox";
            machineNameTextBox.Size = new Size(233, 23);
            machineNameTextBox.TabIndex = 6;
            // 
            // dateLabel
            // 
            dateLabel.AutoSize = true;
            dateLabel.ForeColor = Color.FromArgb(33, 37, 41);
            dateLabel.Location = new Point(140, 46);
            dateLabel.Margin = new Padding(4, 0, 4, 0);
            dateLabel.Name = "dateLabel";
            dateLabel.Size = new Size(34, 15);
            dateLabel.TabIndex = 3;
            dateLabel.Text = "Date:";
            // 
            // dateTextBox
            // 
            dateTextBox.BackColor = Color.FromArgb(255, 255, 255);
            dateTextBox.BorderStyle = BorderStyle.FixedSingle;
            dateTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            dateTextBox.Location = new Point(140, 65);
            dateTextBox.Margin = new Padding(4, 3, 4, 3);
            dateTextBox.Name = "dateTextBox";
            dateTextBox.Size = new Size(116, 23);
            dateTextBox.TabIndex = 4;
            // 
            // idLabel
            // 
            idLabel.AutoSize = true;
            idLabel.ForeColor = Color.FromArgb(33, 37, 41);
            idLabel.Location = new Point(23, 46);
            idLabel.Margin = new Padding(4, 0, 4, 0);
            idLabel.Name = "idLabel";
            idLabel.Size = new Size(20, 15);
            idLabel.TabIndex = 1;
            idLabel.Text = "Id:";
            // 
            // idTextBox
            // 
            idTextBox.BackColor = Color.FromArgb(255, 255, 255);
            idTextBox.BorderStyle = BorderStyle.FixedSingle;
            idTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            idTextBox.Location = new Point(23, 65);
            idTextBox.Margin = new Padding(4, 3, 4, 3);
            idTextBox.Name = "idTextBox";
            idTextBox.ReadOnly = true;
            idTextBox.Size = new Size(116, 23);
            idTextBox.TabIndex = 2;
            // 
            // tagsTabPage
            // 
            tagsTabPage.BackColor = Color.FromArgb(248, 249, 250);
            tagsTabPage.Controls.Add(tagInputPanel);
            tagsTabPage.Controls.Add(tagsPanel);
            tagsTabPage.Location = new Point(4, 24);
            tagsTabPage.Margin = new Padding(4, 3, 4, 3);
            tagsTabPage.Name = "tagsTabPage";
            tagsTabPage.Padding = new Padding(4, 3, 4, 3);
            tagsTabPage.Size = new Size(1392, 722);
            tagsTabPage.TabIndex = 1;
            tagsTabPage.Text = "Tags";
            // 
            // tagInputPanel
            // 
            tagInputPanel.Controls.Add(tagInputTextBox);
            tagInputPanel.Controls.Add(tagInputLabel);
            tagInputPanel.Dock = DockStyle.Top;
            tagInputPanel.Location = new Point(4, 3);
            tagInputPanel.Margin = new Padding(4, 3, 4, 3);
            tagInputPanel.Name = "tagInputPanel";
            tagInputPanel.Size = new Size(1384, 46);
            tagInputPanel.TabIndex = 1;
            // 
            // tagInputTextBox
            // 
            tagInputTextBox.BackColor = Color.FromArgb(255, 255, 255);
            tagInputTextBox.BorderStyle = BorderStyle.FixedSingle;
            tagInputTextBox.ForeColor = Color.FromArgb(33, 37, 41);
            tagInputTextBox.Location = new Point(140, 12);
            tagInputTextBox.Margin = new Padding(4, 3, 4, 3);
            tagInputTextBox.Name = "tagInputTextBox";
            tagInputTextBox.Size = new Size(350, 23);
            tagInputTextBox.TabIndex = 1;
            // 
            // tagInputLabel
            // 
            tagInputLabel.AutoSize = true;
            tagInputLabel.ForeColor = Color.FromArgb(33, 37, 41);
            tagInputLabel.Location = new Point(23, 15);
            tagInputLabel.Margin = new Padding(4, 0, 4, 0);
            tagInputLabel.Name = "tagInputLabel";
            tagInputLabel.Size = new Size(98, 15);
            tagInputLabel.TabIndex = 0;
            tagInputLabel.Text = "Enter Tag to Add:";
            // 
            // tagsPanel
            // 
            tagsPanel.Controls.Add(generatedTagsGroupBox);
            tagsPanel.Controls.Add(tagsShouldNotGroupBox);
            tagsPanel.Controls.Add(tagsMustNotGroupBox);
            tagsPanel.Controls.Add(tagsShouldGroupBox);
            tagsPanel.Controls.Add(tagsMustGroupBox);
            tagsPanel.Dock = DockStyle.Fill;
            tagsPanel.Location = new Point(4, 3);
            tagsPanel.Margin = new Padding(4, 3, 4, 3);
            tagsPanel.Name = "tagsPanel";
            tagsPanel.Size = new Size(1384, 716);
            tagsPanel.TabIndex = 0;
            // 
            // generatedTagsGroupBox
            // 
            generatedTagsGroupBox.Controls.Add(removeGeneratedTagsButton);
            generatedTagsGroupBox.Controls.Add(addGeneratedTagsButton);
            generatedTagsGroupBox.Controls.Add(generatedTagsListBox);
            generatedTagsGroupBox.ForeColor = Color.FromArgb(33, 37, 41);
            generatedTagsGroupBox.Location = new Point(23, 438);
            generatedTagsGroupBox.Margin = new Padding(4, 3, 4, 3);
            generatedTagsGroupBox.Name = "generatedTagsGroupBox";
            generatedTagsGroupBox.Padding = new Padding(4, 3, 4, 3);
            generatedTagsGroupBox.Size = new Size(467, 92);
            generatedTagsGroupBox.TabIndex = 4;
            generatedTagsGroupBox.TabStop = false;
            generatedTagsGroupBox.Text = "Generated Tags";
            // 
            // removeGeneratedTagsButton
            // 
            removeGeneratedTagsButton.BackColor = Color.FromArgb(220, 53, 69);
            removeGeneratedTagsButton.FlatStyle = FlatStyle.Flat;
            removeGeneratedTagsButton.ForeColor = Color.White;
            removeGeneratedTagsButton.Location = new Point(373, 58);
            removeGeneratedTagsButton.Margin = new Padding(4, 3, 4, 3);
            removeGeneratedTagsButton.Name = "removeGeneratedTagsButton";
            removeGeneratedTagsButton.Size = new Size(70, 27);
            removeGeneratedTagsButton.TabIndex = 2;
            removeGeneratedTagsButton.Text = "Remove";
            removeGeneratedTagsButton.UseVisualStyleBackColor = false;
            removeGeneratedTagsButton.Click += RemoveTagButton_Click;
            // 
            // addGeneratedTagsButton
            // 
            addGeneratedTagsButton.BackColor = Color.FromArgb(40, 167, 69);
            addGeneratedTagsButton.FlatStyle = FlatStyle.Flat;
            addGeneratedTagsButton.ForeColor = Color.White;
            addGeneratedTagsButton.Location = new Point(292, 58);
            addGeneratedTagsButton.Margin = new Padding(4, 3, 4, 3);
            addGeneratedTagsButton.Name = "addGeneratedTagsButton";
            addGeneratedTagsButton.Size = new Size(70, 27);
            addGeneratedTagsButton.TabIndex = 1;
            addGeneratedTagsButton.Text = "Add";
            addGeneratedTagsButton.UseVisualStyleBackColor = false;
            addGeneratedTagsButton.Click += AddTagButton_Click;
            // 
            // generatedTagsListBox
            // 
            generatedTagsListBox.BackColor = Color.FromArgb(255, 255, 255);
            generatedTagsListBox.BorderStyle = BorderStyle.FixedSingle;
            generatedTagsListBox.ForeColor = Color.FromArgb(33, 37, 41);
            generatedTagsListBox.FormattingEnabled = true;
            generatedTagsListBox.ItemHeight = 15;
            generatedTagsListBox.Location = new Point(12, 23);
            generatedTagsListBox.Margin = new Padding(4, 3, 4, 3);
            generatedTagsListBox.Name = "generatedTagsListBox";
            generatedTagsListBox.Size = new Size(443, 32);
            generatedTagsListBox.TabIndex = 0;
            // 
            // tagsShouldNotGroupBox
            // 
            tagsShouldNotGroupBox.Controls.Add(removeTagsShouldNotButton);
            tagsShouldNotGroupBox.Controls.Add(addTagsShouldNotButton);
            tagsShouldNotGroupBox.Controls.Add(tagsShouldNotListBox);
            tagsShouldNotGroupBox.ForeColor = Color.FromArgb(33, 37, 41);
            tagsShouldNotGroupBox.Location = new Point(537, 346);
            tagsShouldNotGroupBox.Margin = new Padding(4, 3, 4, 3);
            tagsShouldNotGroupBox.Name = "tagsShouldNotGroupBox";
            tagsShouldNotGroupBox.Padding = new Padding(4, 3, 4, 3);
            tagsShouldNotGroupBox.Size = new Size(467, 92);
            tagsShouldNotGroupBox.TabIndex = 3;
            tagsShouldNotGroupBox.TabStop = false;
            tagsShouldNotGroupBox.Text = "Tags Should Not";
            // 
            // removeTagsShouldNotButton
            // 
            removeTagsShouldNotButton.BackColor = Color.FromArgb(220, 53, 69);
            removeTagsShouldNotButton.FlatStyle = FlatStyle.Flat;
            removeTagsShouldNotButton.ForeColor = Color.White;
            removeTagsShouldNotButton.Location = new Point(373, 58);
            removeTagsShouldNotButton.Margin = new Padding(4, 3, 4, 3);
            removeTagsShouldNotButton.Name = "removeTagsShouldNotButton";
            removeTagsShouldNotButton.Size = new Size(70, 27);
            removeTagsShouldNotButton.TabIndex = 2;
            removeTagsShouldNotButton.Text = "Remove";
            removeTagsShouldNotButton.UseVisualStyleBackColor = false;
            removeTagsShouldNotButton.Click += RemoveTagButton_Click;
            // 
            // addTagsShouldNotButton
            // 
            addTagsShouldNotButton.BackColor = Color.FromArgb(40, 167, 69);
            addTagsShouldNotButton.FlatStyle = FlatStyle.Flat;
            addTagsShouldNotButton.ForeColor = Color.White;
            addTagsShouldNotButton.Location = new Point(292, 58);
            addTagsShouldNotButton.Margin = new Padding(4, 3, 4, 3);
            addTagsShouldNotButton.Name = "addTagsShouldNotButton";
            addTagsShouldNotButton.Size = new Size(70, 27);
            addTagsShouldNotButton.TabIndex = 1;
            addTagsShouldNotButton.Text = "Add";
            addTagsShouldNotButton.UseVisualStyleBackColor = false;
            addTagsShouldNotButton.Click += AddTagButton_Click;
            // 
            // tagsShouldNotListBox
            // 
            tagsShouldNotListBox.BackColor = Color.FromArgb(255, 255, 255);
            tagsShouldNotListBox.BorderStyle = BorderStyle.FixedSingle;
            tagsShouldNotListBox.ForeColor = Color.FromArgb(33, 37, 41);
            tagsShouldNotListBox.FormattingEnabled = true;
            tagsShouldNotListBox.ItemHeight = 15;
            tagsShouldNotListBox.Location = new Point(12, 23);
            tagsShouldNotListBox.Margin = new Padding(4, 3, 4, 3);
            tagsShouldNotListBox.Name = "tagsShouldNotListBox";
            tagsShouldNotListBox.Size = new Size(443, 32);
            tagsShouldNotListBox.TabIndex = 0;
            // 
            // tagsMustNotGroupBox
            // 
            tagsMustNotGroupBox.Controls.Add(removeTagsMustNotButton);
            tagsMustNotGroupBox.Controls.Add(addTagsMustNotButton);
            tagsMustNotGroupBox.Controls.Add(tagsMustNotListBox);
            tagsMustNotGroupBox.ForeColor = Color.FromArgb(33, 37, 41);
            tagsMustNotGroupBox.Location = new Point(23, 346);
            tagsMustNotGroupBox.Margin = new Padding(4, 3, 4, 3);
            tagsMustNotGroupBox.Name = "tagsMustNotGroupBox";
            tagsMustNotGroupBox.Padding = new Padding(4, 3, 4, 3);
            tagsMustNotGroupBox.Size = new Size(467, 92);
            tagsMustNotGroupBox.TabIndex = 2;
            tagsMustNotGroupBox.TabStop = false;
            tagsMustNotGroupBox.Text = "Tags Must Not";
            // 
            // removeTagsMustNotButton
            // 
            removeTagsMustNotButton.BackColor = Color.FromArgb(220, 53, 69);
            removeTagsMustNotButton.FlatStyle = FlatStyle.Flat;
            removeTagsMustNotButton.ForeColor = Color.White;
            removeTagsMustNotButton.Location = new Point(373, 58);
            removeTagsMustNotButton.Margin = new Padding(4, 3, 4, 3);
            removeTagsMustNotButton.Name = "removeTagsMustNotButton";
            removeTagsMustNotButton.Size = new Size(70, 27);
            removeTagsMustNotButton.TabIndex = 2;
            removeTagsMustNotButton.Text = "Remove";
            removeTagsMustNotButton.UseVisualStyleBackColor = false;
            removeTagsMustNotButton.Click += RemoveTagButton_Click;
            // 
            // addTagsMustNotButton
            // 
            addTagsMustNotButton.BackColor = Color.FromArgb(40, 167, 69);
            addTagsMustNotButton.FlatStyle = FlatStyle.Flat;
            addTagsMustNotButton.ForeColor = Color.White;
            addTagsMustNotButton.Location = new Point(292, 58);
            addTagsMustNotButton.Margin = new Padding(4, 3, 4, 3);
            addTagsMustNotButton.Name = "addTagsMustNotButton";
            addTagsMustNotButton.Size = new Size(70, 27);
            addTagsMustNotButton.TabIndex = 1;
            addTagsMustNotButton.Text = "Add";
            addTagsMustNotButton.UseVisualStyleBackColor = false;
            addTagsMustNotButton.Click += AddTagButton_Click;
            // 
            // tagsMustNotListBox
            // 
            tagsMustNotListBox.BackColor = Color.FromArgb(255, 255, 255);
            tagsMustNotListBox.BorderStyle = BorderStyle.FixedSingle;
            tagsMustNotListBox.ForeColor = Color.FromArgb(33, 37, 41);
            tagsMustNotListBox.FormattingEnabled = true;
            tagsMustNotListBox.ItemHeight = 15;
            tagsMustNotListBox.Location = new Point(12, 23);
            tagsMustNotListBox.Margin = new Padding(4, 3, 4, 3);
            tagsMustNotListBox.Name = "tagsMustNotListBox";
            tagsMustNotListBox.Size = new Size(443, 32);
            tagsMustNotListBox.TabIndex = 0;
            // 
            // tagsShouldGroupBox
            // 
            tagsShouldGroupBox.Controls.Add(removeTagsShouldButton);
            tagsShouldGroupBox.Controls.Add(addTagsShouldButton);
            tagsShouldGroupBox.Controls.Add(tagsShouldListBox);
            tagsShouldGroupBox.ForeColor = Color.FromArgb(33, 37, 41);
            tagsShouldGroupBox.Location = new Point(537, 254);
            tagsShouldGroupBox.Margin = new Padding(4, 3, 4, 3);
            tagsShouldGroupBox.Name = "tagsShouldGroupBox";
            tagsShouldGroupBox.Padding = new Padding(4, 3, 4, 3);
            tagsShouldGroupBox.Size = new Size(467, 92);
            tagsShouldGroupBox.TabIndex = 1;
            tagsShouldGroupBox.TabStop = false;
            tagsShouldGroupBox.Text = "Tags Should";
            // 
            // removeTagsShouldButton
            // 
            removeTagsShouldButton.BackColor = Color.FromArgb(220, 53, 69);
            removeTagsShouldButton.FlatStyle = FlatStyle.Flat;
            removeTagsShouldButton.ForeColor = Color.White;
            removeTagsShouldButton.Location = new Point(373, 58);
            removeTagsShouldButton.Margin = new Padding(4, 3, 4, 3);
            removeTagsShouldButton.Name = "removeTagsShouldButton";
            removeTagsShouldButton.Size = new Size(70, 27);
            removeTagsShouldButton.TabIndex = 2;
            removeTagsShouldButton.Text = "Remove";
            removeTagsShouldButton.UseVisualStyleBackColor = false;
            removeTagsShouldButton.Click += RemoveTagButton_Click;
            // 
            // addTagsShouldButton
            // 
            addTagsShouldButton.BackColor = Color.FromArgb(40, 167, 69);
            addTagsShouldButton.FlatStyle = FlatStyle.Flat;
            addTagsShouldButton.ForeColor = Color.White;
            addTagsShouldButton.Location = new Point(292, 58);
            addTagsShouldButton.Margin = new Padding(4, 3, 4, 3);
            addTagsShouldButton.Name = "addTagsShouldButton";
            addTagsShouldButton.Size = new Size(70, 27);
            addTagsShouldButton.TabIndex = 1;
            addTagsShouldButton.Text = "Add";
            addTagsShouldButton.UseVisualStyleBackColor = false;
            addTagsShouldButton.Click += AddTagButton_Click;
            // 
            // tagsShouldListBox
            // 
            tagsShouldListBox.BackColor = Color.FromArgb(255, 255, 255);
            tagsShouldListBox.BorderStyle = BorderStyle.FixedSingle;
            tagsShouldListBox.ForeColor = Color.FromArgb(33, 37, 41);
            tagsShouldListBox.FormattingEnabled = true;
            tagsShouldListBox.ItemHeight = 15;
            tagsShouldListBox.Location = new Point(12, 23);
            tagsShouldListBox.Margin = new Padding(4, 3, 4, 3);
            tagsShouldListBox.Name = "tagsShouldListBox";
            tagsShouldListBox.Size = new Size(443, 32);
            tagsShouldListBox.TabIndex = 0;
            // 
            // tagsMustGroupBox
            // 
            tagsMustGroupBox.Controls.Add(removeTagsMustButton);
            tagsMustGroupBox.Controls.Add(addTagsMustButton);
            tagsMustGroupBox.Controls.Add(tagsMustListBox);
            tagsMustGroupBox.ForeColor = Color.FromArgb(33, 37, 41);
            tagsMustGroupBox.Location = new Point(23, 254);
            tagsMustGroupBox.Margin = new Padding(4, 3, 4, 3);
            tagsMustGroupBox.Name = "tagsMustGroupBox";
            tagsMustGroupBox.Padding = new Padding(4, 3, 4, 3);
            tagsMustGroupBox.Size = new Size(467, 92);
            tagsMustGroupBox.TabIndex = 0;
            tagsMustGroupBox.TabStop = false;
            tagsMustGroupBox.Text = "Tags Must";
            // 
            // removeTagsMustButton
            // 
            removeTagsMustButton.BackColor = Color.FromArgb(220, 53, 69);
            removeTagsMustButton.FlatStyle = FlatStyle.Flat;
            removeTagsMustButton.ForeColor = Color.White;
            removeTagsMustButton.Location = new Point(373, 58);
            removeTagsMustButton.Margin = new Padding(4, 3, 4, 3);
            removeTagsMustButton.Name = "removeTagsMustButton";
            removeTagsMustButton.Size = new Size(70, 27);
            removeTagsMustButton.TabIndex = 2;
            removeTagsMustButton.Text = "Remove";
            removeTagsMustButton.UseVisualStyleBackColor = false;
            removeTagsMustButton.Click += RemoveTagButton_Click;
            // 
            // addTagsMustButton
            // 
            addTagsMustButton.BackColor = Color.FromArgb(40, 167, 69);
            addTagsMustButton.FlatStyle = FlatStyle.Flat;
            addTagsMustButton.ForeColor = Color.White;
            addTagsMustButton.Location = new Point(292, 58);
            addTagsMustButton.Margin = new Padding(4, 3, 4, 3);
            addTagsMustButton.Name = "addTagsMustButton";
            addTagsMustButton.Size = new Size(70, 27);
            addTagsMustButton.TabIndex = 1;
            addTagsMustButton.Text = "Add";
            addTagsMustButton.UseVisualStyleBackColor = false;
            addTagsMustButton.Click += AddTagButton_Click;
            // 
            // tagsMustListBox
            // 
            tagsMustListBox.BackColor = Color.FromArgb(255, 255, 255);
            tagsMustListBox.BorderStyle = BorderStyle.FixedSingle;
            tagsMustListBox.ForeColor = Color.FromArgb(33, 37, 41);
            tagsMustListBox.FormattingEnabled = true;
            tagsMustListBox.ItemHeight = 15;
            tagsMustListBox.Location = new Point(12, 23);
            tagsMustListBox.Margin = new Padding(4, 3, 4, 3);
            tagsMustListBox.Name = "tagsMustListBox";
            tagsMustListBox.Size = new Size(443, 32);
            tagsMustListBox.TabIndex = 0;
            // 
            // footerPanel
            // 
            footerPanel.BackColor = Color.FromArgb(255, 255, 255);
            footerPanel.Controls.Add(statusLabel);
            footerPanel.Controls.Add(addChildButton);
            footerPanel.Controls.Add(cancelButton);
            footerPanel.Controls.Add(saveButton);
            footerPanel.Dock = DockStyle.Bottom;
            footerPanel.Location = new Point(0, 710);
            footerPanel.Margin = new Padding(4, 3, 4, 3);
            footerPanel.Name = "footerPanel";
            footerPanel.Size = new Size(1392, 115);
            footerPanel.TabIndex = 2;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.ForeColor = Color.FromArgb(33, 37, 41);
            statusLabel.Location = new Point(23, 17);
            statusLabel.Margin = new Padding(4, 0, 4, 0);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(39, 15);
            statusLabel.TabIndex = 2;
            statusLabel.Text = "Ready";
            // 
            // addChildButton
            // 
            addChildButton.BackColor = Color.FromArgb(0, 123, 255);
            addChildButton.FlatStyle = FlatStyle.Flat;
            addChildButton.ForeColor = Color.White;
            addChildButton.Location = new Point(735, 58);
            addChildButton.Margin = new Padding(4, 3, 4, 3);
            addChildButton.Name = "addChildButton";
            addChildButton.Size = new Size(88, 35);
            addChildButton.TabIndex = 3;
            addChildButton.Text = "Add Child";
            addChildButton.UseVisualStyleBackColor = false;
            addChildButton.Click += AddChildButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.BackColor = Color.FromArgb(108, 117, 125);
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.ForeColor = Color.White;
            cancelButton.Location = new Point(840, 58);
            cancelButton.Margin = new Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(88, 35);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = false;
            cancelButton.Click += CancelButton_Click;
            // 
            // saveButton
            // 
            saveButton.BackColor = Color.FromArgb(40, 167, 69);
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.ForeColor = Color.White;
            saveButton.Location = new Point(945, 58);
            saveButton.Margin = new Padding(4, 3, 4, 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(88, 35);
            saveButton.TabIndex = 0;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = false;
            saveButton.Click += SaveButton_Click;
            // 
            // CacheEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(1392, 825);
            Controls.Add(mainTabControl);
            Controls.Add(footerPanel);
            Controls.Add(headerPanel);
            Margin = new Padding(4, 3, 4, 3);
            MaximumSize = new Size(1408, 864);
            MinimumSize = new Size(1408, 864);
            Name = "CacheEditForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cache Editor";
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            mainTabControl.ResumeLayout(false);
            basicTabPage.ResumeLayout(false);
            basicTabPage.PerformLayout();
            tagsTabPage.ResumeLayout(false);
            tagInputPanel.ResumeLayout(false);
            tagInputPanel.PerformLayout();
            tagsPanel.ResumeLayout(false);
            generatedTagsGroupBox.ResumeLayout(false);
            tagsShouldNotGroupBox.ResumeLayout(false);
            tagsMustNotGroupBox.ResumeLayout(false);
            tagsShouldGroupBox.ResumeLayout(false);
            tagsMustGroupBox.ResumeLayout(false);
            footerPanel.ResumeLayout(false);
            footerPanel.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage basicTabPage;
        private System.Windows.Forms.TabPage tagsTabPage;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button addChildButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.TextBox dateTextBox;
        private System.Windows.Forms.Label machineNameLabel;
        private System.Windows.Forms.TextBox machineNameTextBox;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Label commentLabel;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.Label promptLabel;
        private System.Windows.Forms.TextBox promptTextBox;
        private System.Windows.Forms.Label completionLabel;
        private System.Windows.Forms.TextBox completionTextBox;
        private System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.TextBox languageTextBox;
        private System.Windows.Forms.Label modelLabel;
        private System.Windows.Forms.TextBox modelTextBox;
        private System.Windows.Forms.Label chatModeLabel;
        private System.Windows.Forms.ComboBox chatModeComboBox;
        private System.Windows.Forms.Label parentCacheIdLabel;
        private System.Windows.Forms.TextBox parentCacheIdTextBox;
        private System.Windows.Forms.Label generatedSystemPromptLabel;
        private System.Windows.Forms.TextBox generatedSystemPromptTextBox;
        private System.Windows.Forms.Panel tagsPanel;
        private System.Windows.Forms.GroupBox tagsMustGroupBox;
        private System.Windows.Forms.ListBox tagsMustListBox;
        private System.Windows.Forms.Button addTagsMustButton;
        private System.Windows.Forms.Button removeTagsMustButton;
        private System.Windows.Forms.GroupBox tagsShouldGroupBox;
        private System.Windows.Forms.ListBox tagsShouldListBox;
        private System.Windows.Forms.Button addTagsShouldButton;
        private System.Windows.Forms.Button removeTagsShouldButton;
        private System.Windows.Forms.GroupBox tagsMustNotGroupBox;
        private System.Windows.Forms.ListBox tagsMustNotListBox;
        private System.Windows.Forms.Button addTagsMustNotButton;
        private System.Windows.Forms.Button removeTagsMustNotButton;
        private System.Windows.Forms.GroupBox tagsShouldNotGroupBox;
        private System.Windows.Forms.ListBox tagsShouldNotListBox;
        private System.Windows.Forms.Button addTagsShouldNotButton;
        private System.Windows.Forms.Button removeTagsShouldNotButton;
        private System.Windows.Forms.GroupBox generatedTagsGroupBox;
        private System.Windows.Forms.ListBox generatedTagsListBox;
        private System.Windows.Forms.Button addGeneratedTagsButton;
        private System.Windows.Forms.Button removeGeneratedTagsButton;
        private System.Windows.Forms.Panel tagInputPanel;
        private System.Windows.Forms.Label tagInputLabel;
        private System.Windows.Forms.TextBox tagInputTextBox;
    }
}
