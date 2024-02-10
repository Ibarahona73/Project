namespace Project.Views;

public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();
    }

    private void IngresarBtn_Clicked(object sender, EventArgs e)
    {

    }

    private void OlvidarPassBtn_Clicked(object sender, EventArgs e)
    {

    }

    private async void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        // Crear una instancia de la página Register.xaml
        Register registerPage = new Register();

        // Obtener el NavigationPage actual
        NavigationPage? currentNavigationPage = Application.Current.MainPage as NavigationPage;

        // Verificar si currentNavigationPage no es nulo antes de continuar
        if (currentNavigationPage != null)
        {
            // Navegar a la página Register.xaml dentro del NavigationPage actual
            await currentNavigationPage.PushAsync(registerPage);
        }
    }


}