using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using xFordPassLite.net.Utils;
using xFordPassLite.net.ViewModels;

namespace xFordPassLite.net.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActionPage : ContentPage
    {
        ActionViewModel _viewModel;

        public ActionPage()
        {
            InitializeComponent();
            if (ConfigData.USERNAME == "" || ConfigData.USERNAME == null || ConfigData.PW == "" || ConfigData.PW == null || ConfigData.VIN == "" || ConfigData.VIN == null)
            {
                LabelLog.Text = "FORDPASS USERID, PASSWORD, AND VIN ARE REQUIRED";
            }
            else
            {
                _viewModel = new ActionViewModel();
            }
        }

        protected override async void OnAppearing()
        {
            if (ConfigData.USERNAME == "" || ConfigData.USERNAME == null || ConfigData.PW == "" || ConfigData.PW == null || ConfigData.VIN == "" || ConfigData.VIN == null)
            {
                await Shell.Current.GoToAsync("NewUserPage");
            }
            else
            {
                base.OnAppearing();
                BindingContext = _viewModel;
            }
        }
    }
}