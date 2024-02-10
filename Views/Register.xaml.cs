namespace Project.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

    private async void Procesar_Clicked(object sender, EventArgs e)
    {


        // Crear una instancia de la p�gina LoginPage.xaml
        Login loginPage = new Login();

        // Obtener el NavigationPage actual
        NavigationPage currentNavigationPage = Application.Current.MainPage as NavigationPage;

        // Verificar si currentNavigationPage no es nulo antes de continuar
        if (currentNavigationPage != null)
        {
            // Navegar a la p�gina LoginPage.xaml sin opci�n para volver atr�s
            await currentNavigationPage.Navigation.PushAsync(loginPage);
        }

    }
}