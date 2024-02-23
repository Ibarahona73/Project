namespace Project.Views;


public partial class Login : ContentPage
{

    public Login()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }


    private void OlvidarPassBtn_Clicked(object sender, EventArgs e)
    {
        
    }


    private async void IngresarBtn_Clicked(object sender, EventArgs e)
    {
        // Verificar las credenciales
        if (Email.Text == "hola@gmail.com" && Contra.Text == "1234")
        {
            // Crear una instancia de la página DashBoardAtras
            DashBoardAtras dashBoardPage = new DashBoardAtras();

            // Obtener el NavigationPage actual
            NavigationPage currentNavigationPage = Application.Current.MainPage as NavigationPage;

            // Verificar si currentNavigationPage no es nulo antes de continuar
            if (currentNavigationPage != null)
            {
                // Deshabilitar el botón de retroceso en la barra de navegación
                NavigationPage.SetHasBackButton(dashBoardPage, false);

                // Navegar a la página DashBoardAtras dentro del NavigationPage actual
                await currentNavigationPage.PushAsync(dashBoardPage);
            }
        }
    }



    private async void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        // Crear una instancia de la página Register.xaml
        Register registerPage = new();

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
    

