namespace Project.Views;

public partial class OlvContra : ContentPage
{
	public OlvContra()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void BtnEnvRec_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CorreoRes.Text))

        {
            await DisplayAlert("Error", "Por favor, llene todos los campos.", "OK");
            return; // Si hay campos vacíos, termina aquí y no continúa con las validaciones.
        }

        // Obtener los valores de los campos
        string correoRec = CorreoRes.Text.Trim();


        // Validar el correo electrónico
        bool esValido = ValidarCorreoElectronico(correoRec);

        if (!esValido)
        {
            await DisplayAlert("Error", "Inserte un correo válido (Gmail, Hotmail, Outlook)", "OK");
            return;
        }
        ValidacionCod Restablecer = new ValidacionCod();
        NavigationPage currentNavigationPage = Application.Current.MainPage as NavigationPage;
        await currentNavigationPage.Navigation.PushAsync(Restablecer);
    }

    private bool ValidarCorreoElectronico(string correo)
    {
        // Expresión regular para validar el correo electrónico
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+@(gmail|hotmail|outlook)\.(com|es)$");
        return regex.IsMatch(correo);
    }
}