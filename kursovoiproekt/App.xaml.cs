using kursovoiproekt.Services;
using kursovoiproekt.Views;
using System.Windows;
using kursovoiproekt.Converters;

namespace kursovoiproekt
{
    public partial class App : Application
    {
        public static NavigationService NavigationService { get; } = new NavigationService();

        protected override void OnStartup(StartupEventArgs e)
        {

        }
    }
}
