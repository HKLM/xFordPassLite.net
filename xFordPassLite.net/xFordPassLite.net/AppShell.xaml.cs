using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using xFordPassLite.net.Views;

namespace xFordPassLite.net
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewUserPage), typeof(NewUserPage));
        }
    }
}
