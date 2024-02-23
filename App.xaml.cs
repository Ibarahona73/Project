using Project.Views;

namespace Project
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new Login());
            NavigationPage.SetHasNavigationBar(this, false);


            //Para mostrar El menu
            //MainPage = new Views.DashBoardo();
        }
    }
}
