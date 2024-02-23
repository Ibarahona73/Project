namespace Project.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();

        //Actuan al evento TextChanged para cuando estos tengan datos
        //sus propiedades cambien

        RegNombre.TextChanged += Campos_TextChanged;
        RegApellido.TextChanged += Campos_TextChanged;
        RegEmail.TextChanged += Campos_TextChanged;
        RegContra.TextChanged += Campos_TextChanged;
        RegPhone.TextChanged += Campos_TextChanged;

        //Deshabilitar el boton si los campos estan vacios
        Procesar.IsEnabled = false;
    }

    private void Campos_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Verificar si algún campo está vacío
        if (string.IsNullOrEmpty(RegNombre.Text?.Trim()) || string.IsNullOrEmpty(RegApellido.Text?.Trim()) ||
            string.IsNullOrEmpty(RegEmail.Text?.Trim()) || string.IsNullOrEmpty(RegContra.Text?.Trim()) ||
            string.IsNullOrEmpty(RegPhone.Text?.Trim()))
        {
            // Si hay campos vacíos, deshabilitar el botón "Procesar"
            Procesar.IsEnabled = false;
        }
        else
        {
            // Si todos los campos están llenos, habilitar el botón "Procesar"
            Procesar.IsEnabled = true;
        }
    }


    private async void Procesar_Clicked(object sender, EventArgs e)
    {
        string nombre = RegNombre.Text?.Trim();
        string apellido = RegApellido.Text?.Trim();
        string correo = RegEmail.Text.Trim();
        string contrasena = RegContra.Text?.Trim();
        string telefono = RegPhone.Text?.Trim();


        // Verificar si algún campo está vacío
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) ||
            string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena) ||
            string.IsNullOrEmpty(telefono))
        {
            await DisplayAlert("Error", "Por favor, llene todos los campos.", "OK");

            return; // Si hay campos vacíos, termina aquí y no continúa con las validaciones.

        }
        else
        {
            // Si todos los campos están llenos, habilitar el botón "Procesar"
            Procesar.IsEnabled = true;
        }

        // Validar el correo electrónico
        bool esValido = ValidarCorreoElectronico(correo);


        if (!esValido)

        {

            await DisplayAlert("Error", "Inserte un correo válido (Gmail, Hotmail, Outlook)", "OK");
            return;
        }

        // Validar el teléfono
        if (telefono.Length != 8 || !EsNumero(telefono))
        {
            await DisplayAlert("Error", "El número de teléfono debe tener 8 dígitos y ser numérico.", "OK");
            return;
        }


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
    private bool ValidarCorreoElectronico(string correo)
    {
        // Expresión regular para validar el correo electrónico
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+@(gmail|hotmail|outlook)\.(com|es)$");
        return regex.IsMatch(correo);
    }


    private bool EsNumero(string str)
    {
        // Verifica cada carácter en la cadena
        foreach (char c in str)
        {
            // Si el carácter actual no es un dígito, devuelve falso
            if (!char.IsDigit(c))
                return false;
        }
        // Si se pasó por todos los caracteres y ninguno fue no numérico, devuelve verdadero
        return true;
    }
}