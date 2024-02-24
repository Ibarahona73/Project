namespace Project.Views;

public partial class DashBoardo : Shell
{
	public DashBoardo()
    {
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);


    }

    private async void Csesion_Clicked(object sender, EventArgs e)
    {
        Login InicioS = new Login();

        // Obtener el NavigationPage actual
        NavigationPage currentNavigationPage = Application.Current.MainPage as NavigationPage;

        // Verificar si currentNavigationPage no es nulo antes de continuar
        if (currentNavigationPage != null)
        {
            // Deshabilitar el bot�n de retroceso en la barra de navegaci�n
            NavigationPage.SetHasBackButton(InicioS, false);

            // Navegar a la p�gina DashBoardAtras dentro del NavigationPage actual
            await currentNavigationPage.PushAsync(InicioS);
        }
    }
}