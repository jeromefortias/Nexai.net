namespace Musuko.Kaonashi
{
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public partial class CodeViewerForm : Form
    {
        private RichTextBox codeTextBox;
        private Label titleLabel;
        private Button copyButton;
        private Button saveAsButton;
        private Button closeButton;
        private Panel headerPanel;
        private Panel footerPanel;
        
        private string currentLanguage;

        private readonly Color backgroundColor = Color.FromArgb(30, 30, 30);
        private readonly Color secondaryBg = Color.FromArgb(40, 40, 40);
        private readonly Color textColor = Color.FromArgb(230, 230, 230);
        private readonly Color accentColor = Color.FromArgb(88, 101, 242);

        // Parameterless constructor for Visual Studio Designer
        public CodeViewerForm() : this("// Sample code", "csharp")
        {
        }

        public CodeViewerForm(string code, string language)
        {
            currentLanguage = language;
            InitializeComponent();
            IconHelper.SetFormIcon(this);
            SetCodeContent(code, language);
        }

        private void SetCodeContent(string code, string language)
        {
            titleLabel.Text = $"Code Preview - {language.ToUpper()}";
            codeTextBox.Text = code;
            ApplySyntaxHighlighting(language);
        }

        private void ApplySyntaxHighlighting(string language)
        {
            // Simple syntax highlighting for better readability
            string text = codeTextBox.Text;
            codeTextBox.Clear();

            // Keywords by language
            string[] keywords = GetKeywordsForLanguage(language);
            
            // Color scheme
            Color keywordColor = Color.FromArgb(86, 156, 214);      // Blue
            Color stringColor = Color.FromArgb(206, 145, 120);      // Orange
            Color commentColor = Color.FromArgb(106, 153, 85);      // Green
            Color numberColor = Color.FromArgb(181, 206, 168);      // Light green
            Color defaultColor = textColor;

            codeTextBox.Text = text;
            codeTextBox.SelectAll();
            codeTextBox.SelectionColor = defaultColor;
            codeTextBox.DeselectAll();

            // Highlight keywords
            foreach (var keyword in keywords)
            {
                HighlightPattern($@"\b{keyword}\b", keywordColor);
            }

            // Highlight strings
            HighlightPattern(@"""[^""\\]*(?:\\.[^""\\]*)*""", stringColor);
            HighlightPattern(@"'[^'\\]*(?:\\.[^'\\]*)*'", stringColor);

            // Highlight comments
            HighlightPattern(@"//.*$", commentColor, RegexOptions.Multiline);
            HighlightPattern(@"/\*[\s\S]*?\*/", commentColor);
            HighlightPattern(@"#.*$", commentColor, RegexOptions.Multiline);

            // Highlight numbers
            HighlightPattern(@"\b\d+\.?\d*\b", numberColor);

            codeTextBox.Select(0, 0);
        }

        private void HighlightPattern(string pattern, Color color, RegexOptions options = RegexOptions.None)
        {
            try
            {
                var matches = Regex.Matches(codeTextBox.Text, pattern, options);
                foreach (Match match in matches)
                {
                    codeTextBox.Select(match.Index, match.Length);
                    codeTextBox.SelectionColor = color;
                }
            }
            catch { }
        }

        private string[] GetKeywordsForLanguage(string language)
        {
            language = language.ToLower();

            if (language == "csharp" || language == "cs" || language == "c#")
            {
                return new[] {
                    "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char",
                    "checked", "class", "const", "continue", "decimal", "default", "delegate",
                    "do", "double", "else", "enum", "event", "explicit", "extern", "false",
                    "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit",
                    "in", "int", "interface", "internal", "is", "lock", "long", "namespace",
                    "new", "null", "object", "operator", "out", "override", "params", "private",
                    "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
                    "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
                    "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
                    "unsafe", "ushort", "using", "virtual", "void", "volatile", "while", "var",
                    "async", "await", "dynamic", "get", "set", "value", "yield"
                };
            }
            else if (language == "python" || language == "py")
            {
                return new[] {
                    "and", "as", "assert", "async", "await", "break", "class", "continue",
                    "def", "del", "elif", "else", "except", "False", "finally", "for", "from",
                    "global", "if", "import", "in", "is", "lambda", "None", "nonlocal", "not",
                    "or", "pass", "raise", "return", "True", "try", "while", "with", "yield",
                    "self", "print", "range", "len", "str", "int", "float", "list", "dict", "set"
                };
            }
            else if (language == "cpp" || language == "c++" || language == "c")
            {
                return new[] {
                    "alignas", "alignof", "and", "and_eq", "asm", "auto", "bitand", "bitor",
                    "bool", "break", "case", "catch", "char", "char16_t", "char32_t", "class",
                    "compl", "const", "constexpr", "const_cast", "continue", "decltype", "default",
                    "delete", "do", "double", "dynamic_cast", "else", "enum", "explicit", "export",
                    "extern", "false", "float", "for", "friend", "goto", "if", "inline", "int",
                    "long", "mutable", "namespace", "new", "noexcept", "not", "not_eq", "nullptr",
                    "operator", "or", "or_eq", "private", "protected", "public", "register",
                    "reinterpret_cast", "return", "short", "signed", "sizeof", "static",
                    "static_assert", "static_cast", "struct", "switch", "template", "this",
                    "thread_local", "throw", "true", "try", "typedef", "typeid", "typename",
                    "union", "unsigned", "using", "virtual", "void", "volatile", "wchar_t",
                    "while", "xor", "xor_eq", "cout", "cin", "endl", "std", "include"
                };
            }

            return new string[0];
        }

        private void CopyButton_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(codeTextBox.Text))
                {
                    Clipboard.SetText(codeTextBox.Text);
                    copyButton.Text = "? Copied!";
                    copyButton.BackColor = Color.FromArgb(67, 181, 129);
                    
                    var timer = new System.Windows.Forms.Timer();
                    timer.Interval = 2000;
                    timer.Tick += (s, args) =>
                    {
                        copyButton.Text = "?? Copy Code";
                        copyButton.BackColor = accentColor;
                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy code: {ex.Message}", "Copy Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAsButton_Click(object? sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    // Determine file extension and filter based on language
                    string extension = GetFileExtension(currentLanguage);
                    string filter = GetFileFilter(currentLanguage);
                    
                    saveFileDialog.Filter = filter;
                    saveFileDialog.DefaultExt = extension;
                    saveFileDialog.FileName = $"code{extension}";
                    saveFileDialog.Title = "Save Code As";
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(saveFileDialog.FileName, codeTextBox.Text);
                        
                        // Visual feedback
                        saveAsButton.Text = "? Saved!";
                        var originalColor = saveAsButton.BackColor;
                        saveAsButton.BackColor = Color.FromArgb(67, 181, 129);
                        
                        var timer = new System.Windows.Forms.Timer();
                        timer.Interval = 2000;
                        timer.Tick += (s, args) =>
                        {
                            saveAsButton.Text = "?? Save As...";
                            saveAsButton.BackColor = originalColor;
                            timer.Stop();
                            timer.Dispose();
                        };
                        timer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save file: {ex.Message}", "Save Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetFileExtension(string language)
        {
            language = language.ToLower();
            
            if (language == "csharp" || language == "cs" || language == "c#")
                return ".cs";
            else if (language == "python" || language == "py")
                return ".py";
            else if (language == "cpp" || language == "c++")
                return ".cpp";
            else if (language == "c")
                return ".c";
            else if (language == "mixed")
                return ".txt";
            else
                return ".txt";
        }

        private string GetFileFilter(string language)
        {
            language = language.ToLower();
            
            if (language == "csharp" || language == "cs" || language == "c#")
                return "C# Files (*.cs)|*.cs|All Files (*.*)|*.*";
            else if (language == "python" || language == "py")
                return "Python Files (*.py)|*.py|All Files (*.*)|*.*";
            else if (language == "cpp" || language == "c++")
                return "C++ Files (*.cpp)|*.cpp|All Files (*.*)|*.*";
            else if (language == "c")
                return "C Files (*.c)|*.c|All Files (*.*)|*.*";
            else if (language == "mixed")
                return "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            else
                return "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        }
    }
}



