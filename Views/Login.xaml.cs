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
        // Verificar que los campos estén completos
        if (string.IsNullOrWhiteSpace(Email.Text) || string.IsNullOrWhiteSpace(Contra.Text))
        {
            // Mostrar una alerta indicando que los campos deben estar completos
            await DisplayAlert("Campos incompletos", "Por favor complete todos los campos", "Aceptar");
            return; // Salir del método si los campos no están completos
        }

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
        else
        {
            // Mostrar una alerta indicando que las credenciales son incorrectas
            await DisplayAlert("Credenciales incorrectas", "El correo electrónico o la contraseña son incorrectos", "Aceptar");
        }

        // Limpiar el campo de contraseña independientemente del resultado de las credenciales
        Contra.Text = string.Empty;
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
    

