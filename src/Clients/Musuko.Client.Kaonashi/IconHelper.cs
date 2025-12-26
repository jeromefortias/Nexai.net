namespace Musuko.Kaonashi
{
    using System;
    using System.Drawing;
    using System.IO;

    public static class IconHelper
    {
        private static Icon? _cachedIcon = null;

        public static Icon? GetApplicationIcon()
        {
            if (_cachedIcon != null)
                return _cachedIcon;

            try
            {
                // Try multiple possible paths
                string[] possiblePaths = new[]
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "Appli.ico"),
                    Path.Combine(Directory.GetCurrentDirectory(), "images", "Appli.ico"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "images", "Appli.ico"),
                    "images\\Appli.ico",
                    "Appli.ico"
                };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        _cachedIcon = new Icon(path);
                        return _cachedIcon;
                    }
                }

                System.Diagnostics.Debug.WriteLine("Warning: Appli.ico not found in any expected location");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading application icon: {ex.Message}");
                return null;
            }
        }

        public static void SetFormIcon(System.Windows.Forms.Form form)
        {
            var icon = GetApplicationIcon();
            if (icon != null)
            {
                form.Icon = icon;
            }
        }
    }
}

