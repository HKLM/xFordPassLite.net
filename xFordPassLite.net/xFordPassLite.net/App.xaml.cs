using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using xFordPassLite.net.Services;
using xFordPassLite.net.Utils;

namespace xFordPassLite.net
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        /// <summary>
        /// The version number of this program
        /// </summary>
        public const string APP_VERSION = "0.1.2-beta";

        public App()
        {
            InitializeComponent();

            DependencyService.Register<FordDataStore>();
            MainPage = new AppShell();

            ConfigData.USERNAME = Preferences.Get("USERNAME", "");
            ConfigData.PW = Preferences.Get("PW", "");
            ConfigData.VIN = Preferences.Get("VIN", "");
            ConfigData.REGION = Preferences.Get("RegionCode", ConfigData.DefaultRegion);
            ConfigData.DebugMode = Preferences.Get("DebugMode", false);
            ConfigData.LongRefreshAtStartup = Preferences.Get("LongRefreshAtStartup", false);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
