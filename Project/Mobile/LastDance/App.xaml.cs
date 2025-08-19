using LastDance.ViewModels;

namespace LastDance
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoadClients());
        }
    }
}