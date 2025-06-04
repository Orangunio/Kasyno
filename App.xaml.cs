using Kasyno.Models;
using Kasyno.Properties;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Kasyno
{
    /// <summary>  
    /// Interaction logic for App.xaml  
    /// </summary>  
    public partial class App : Application
    {
        public static User User { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string savedCulture = Settings.Default.AppCulture;
            if (string.IsNullOrEmpty(savedCulture))
                savedCulture = "pl"; // domyślny język  

            Kasyno.Helpers.LocalizationManager.ChangeLanguage(savedCulture);
        }
    }

}
