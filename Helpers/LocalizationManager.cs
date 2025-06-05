using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Kasyno.Helpers
{
    public static class LocalizationManager
    {
        public static void ChangeLanguage(string cultureName)
        {
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            string dictPath = $"Resources/Strings.{cultureName}.xaml";
            var newDict = new ResourceDictionary() { Source = new Uri(dictPath, UriKind.Relative) };

            var oldDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Strings."));
            if (oldDict != null)
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);

            Application.Current.Resources.MergedDictionaries.Add(newDict);
        }
    }
}
