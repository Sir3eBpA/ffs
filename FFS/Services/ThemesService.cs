using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AdonisUI;

namespace FFS.Services
{
    public enum Theme { Light, Dark }

    public static class ThemesService
    {
        public static event Action<Theme> ThemeChanged;

        [SupportedOSPlatform("windows7.0")]
        public static void ApplyTheme(Theme theme)
        {
            Uri themeResource = theme == Theme.Light ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme;
            ResourceLocator.SetColorScheme(Application.Current.Resources, themeResource);
            ThemeChanged?.Invoke(theme);
        }
    }
}
