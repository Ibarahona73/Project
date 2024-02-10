namespace Project.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

    private async void Procesar_Clicked(object sender, EventArgs e)
    {


        // Crear una instancia de la página LoginPage.xaml
        Login loginPage = new Login();

        // Obtener el NavigationPage actual
        NavigationPage currentNavigationPage = Application.Current.MainPage as NavigationPage;

        // Verificar si currentNavigationPage no es nulo antes de continuar
        if (currentNavigationPage != null)
        {
            // Navegar a la página LoginPage.xaml sin opción para volver atrás
            await currentNavigationPage.Navigation.PushAsync(loginPage);
        }

    }
}