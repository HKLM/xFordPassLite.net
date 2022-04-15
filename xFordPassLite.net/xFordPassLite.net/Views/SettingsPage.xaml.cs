using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using xFordPassLite.net.ViewModels;

namespace xFordPassLite.net.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel _viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel();
       }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            BindingContext = _viewModel;
            RegionPicker.SelectedIndex = RegionToIndex(_viewModel.RegionPick);
        }

        private void RegionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                _viewModel.RegionPick = (string)picker.Items[selectedIndex];
            }
        }

        public int RegionToIndex(string InRegion)
        {
            if (InRegion == "US")
                return 0;
            else if (InRegion == "EU")
                return 1;
            else if (InRegion == "AU")
                return 2;
            else
                return 0;
        }

    }
}
