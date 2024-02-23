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
        // Verificar que los campos est�n completos
        if (string.IsNullOrWhiteSpace(Email.Text) || string.IsNullOrWhiteSpace(Contra.Text))
        {
            // Mostrar una alerta indicando que los campos deben estar completos
            await DisplayAlert("Campos incompletos", "Por favor complete todos los campos", "Aceptar");
            return; // Salir del m�todo si los campos no est�n completos
        }

        // Verificar las credenciales
        if (Email.Text == "hola@gmail.com" && Contra.Text == "1234")
        {
            // Crear una instancia de la p�gina DashBoardAtras
            DashBoardAtras dashBoardPage = new DashBoardAtras();

            // Obtener el NavigationPage actual
            NavigationPage currentNavigationPage = Application.Current.MainPage as NavigationPage;

            // Verificar si currentNavigationPage no es nulo antes de continuar
            if (currentNavigationPage != null)
            {
                // Deshabilitar el bot�n de retroceso en la barra de navegaci�n
                NavigationPage.SetHasBackButton(dashBoardPage, false);

                // Navegar a la p�gina DashBoardAtras dentro del NavigationPage actual
                await currentNavigationPage.PushAsync(dashBoardPage);
            }
        }
        else
        {
            // Mostrar una alerta indicando que las credenciales son incorrectas
            await DisplayAlert("Credenciales incorrectas", "El correo electr�nico o la contrase�a son incorrectos", "Aceptar");
        }

        // Limpiar el campo de contrase�a independientemente del resultado de las credenciales
        Contra.Text = string.Empty;
    }




    private async void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        // Crear una instancia de la p�gina Register.xaml
        Register registerPage = new();

        // Obtener el NavigationPage actual
        NavigationPage? currentNavigationPage = Application.Current.MainPage as NavigationPage;

        // Verificar si currentNavigationPage no es nulo antes de continuar
        if (currentNavigationPage != null)
        {
            // Navegar a la p�gina Register.xaml dentro del NavigationPage actual
            await currentNavigationPage.PushAsync(registerPage);
        }
    }
}
    

