using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

using xFordPassLite.net.Models;
using xFordPassLite.net.Utils;

namespace xFordPassLite.net.ViewModels
{
    public class NewUserViewModel : BaseViewModel
    {
        private DataManager DM;

        public override void Dispose()
        {
            this.Dispose();
        }

        protected string _LogMessage;
        public string LogMessage
        {
            get => _LogMessage;
            set => SetProperty(ref _LogMessage, value);
        }

        #region FordPass Info
        protected string loginBox;
        public string LoginBox
        {
            get => loginBox == null || string.IsNullOrEmpty(loginBox) || loginBox == "" ? ConfigData.USERNAME : loginBox;
            set => SetProperty(ref loginBox, value);
        }
        private string passBox;
        public string PassBox
        {
            get => passBox == null || string.IsNullOrEmpty(passBox) || passBox == "" ? ConfigData.PW : passBox;
            set => SetProperty(ref passBox, value);
        }
        private string vinBox;
        public string VinBox
        {
            get => vinBox == null || string.IsNullOrEmpty(vinBox) || vinBox == "" ? ConfigData.VIN : vinBox;
            set => SetProperty(ref vinBox, value);
        }

        private string regionPick;


        public string RegionPick
        {
            get => regionPick == null || string.IsNullOrEmpty(regionPick) || regionPick == "" ? ConfigData.REGION : regionPick;
            set
            {
                if (value == "US" || value == "EU" || value == "AU")
                {
                    SetProperty(ref regionPick, value);
                    if (DM.REGION != value)
                        DM.REGION = value;
                }
            }
        }

        #endregion

        public virtual Command LoadItemsCommand { get; }

        public ICommand SaveCommand { get; }

        public NewUserViewModel()
        {
            Title = "Login Setup";
            DM = new DataManager();
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
            IsBusy = false;
        }

        public void OnAppearing()
        {
            ExecuteLoadItemsCommand();

            IsBusy = true;
        }

        protected async Task OnSave()
        {
            if (LoginBox == "" || PassBox == "")
            {
                LogMessage = "UserId and Password are required";
                return;
            }
            ELoginStatus fordpassId = await FordXDataStore.ValidateFordPassAsync(LoginBox, PassBox, VinBox);
            switch (fordpassId)
            {
                case ELoginStatus.Sucess:
                    LogMessage = "Saved";
                    DM.USERNAME = LoginBox;
                    DM.PW = PassBox;
                    DM.VIN = VinBox;
                    DM.REGION = RegionPick;
                    ExecuteLoadItemsCommand();
                    break;
                case ELoginStatus.InvalidUserID_or_PW:
                    LogMessage = "Invalid UserId and/or Password";
                    break;
                case ELoginStatus.ValidUserID_BadVIN:
                    LogMessage = "Valid UserId and Password. Invalid VIN";
                    DM.USERNAME = LoginBox;
                    DM.PW = PassBox;
                    break;
                case ELoginStatus.Error:
                    LogMessage = "Other Error. Try again later";
                    break;
                default:
                    break;
            }
        }

    }
}
