namespace Project.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }


    private async void Procesar_Clicked(object sender, EventArgs e)
    {
        string nombre = RegNombre.Text?.Trim();
        string apellido = RegApellido.Text?.Trim();
        string correo = RegEmail.Text.Trim();
        string contrasena = RegContra.Text?.Trim();
        string telefono = RegPhone.Text?.Trim();


        // Verificar si alg�n campo est� vac�o
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) ||
            string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena) ||
            string.IsNullOrEmpty(telefono))
        {
            await DisplayAlert("Error", "Por favor, llene todos los campos.", "OK");

            return; // Si hay campos vac�os, termina aqu� y no contin�a con las validaciones.

        }
        else
        {
            // Si todos los campos est�n llenos, habilitar el bot�n "Procesar"
            Procesar.IsEnabled = true;
        }

        // Validar el correo electr�nico
        bool esValido = ValidarCorreoElectronico(correo);


        if (!esValido)

        {

            await DisplayAlert("Error", "Inserte un correo v�lido (Gmail, Hotmail, Outlook)", "OK");
            return;
        }

        // Validar el tel�fono
        if (telefono.Length != 8 || !EsNumero(telefono))
        {
            await DisplayAlert("Error", "El n�mero de tel�fono debe tener 8 d�gitos y ser num�rico.", "OK");
            return;
        }


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
    private bool ValidarCorreoElectronico(string correo)
    {
        // Expresi�n regular para validar el correo electr�nico
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+@(gmail|hotmail|outlook)\.(com|es)$");
        return regex.IsMatch(correo);
    }


    private bool EsNumero(string str)
    {
        // Verifica cada car�cter en la cadena
        foreach (char c in str)
        {
            // Si el car�cter actual no es un d�gito, devuelve falso
            if (!char.IsDigit(c))
                return false;
        }
        // Si se pas� por todos los caracteres y ninguno fue no num�rico, devuelve verdadero
        return true;
    }
}