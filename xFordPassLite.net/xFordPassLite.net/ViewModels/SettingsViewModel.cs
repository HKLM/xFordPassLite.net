using System.Windows.Input;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

using xFordPassLite.net.Utils;

namespace xFordPassLite.net.ViewModels
{
    public class SettingsViewModel : NewUserViewModel
    {
        public override void Dispose()
        {
            this.Dispose();
        }

        public bool SettingsSection => LoginBox != null && !string.IsNullOrEmpty(LoginBox) && LoginBox != "" && VinBox != null && !string.IsNullOrEmpty(VinBox) && VinBox != "";

        private bool _longRefreshAtStartup;
        /// <summary>
        /// At app startup, Force a full refresh of data from vehicle or use the quicker data from Ford API servers
        /// </summary>
        public bool LongRefreshAtStartup
        {
            get => _longRefreshAtStartup;
            set
            {
                SetProperty(ref _longRefreshAtStartup, value);
                if (ConfigData.LongRefreshAtStartup != value)
                {
                    ConfigData.LongRefreshAtStartup = value;
                    Preferences.Set("LongRefreshAtStartup", value);
                }
            }
        }

        public new Command LoadItemsCommand { get; }
        public new ICommand SaveCommand { get; }

        public SettingsViewModel()
        {
            Title = "App Settings";
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);

            SaveCommand = new AsyncCommand(OnSave);
        }

        void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            LoginBox = ConfigData.USERNAME;
            PassBox = ConfigData.PW;
            VinBox = ConfigData.VIN;
            RegionPick = ConfigData.REGION;
            LongRefreshAtStartup = ConfigData.LongRefreshAtStartup;
            IsBusy = false;
        }
    }
}
