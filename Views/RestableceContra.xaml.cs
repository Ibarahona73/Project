namespace Project.Views;

public partial class RestableceContra : ContentPage
{
    public RestableceContra()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void BtnEnv_Clicked(object sender, EventArgs e)
    {
        string newPassword = NewPass.Text;
        string confirmPassword = ConfirmPass.Text;

        // Verificar si el campo de contraseña está en blanco
        if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Error", "Por favor, ingrese una contraseña.", "OK");
            return;
        }

        if (newPassword != confirmPassword)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            return;
        }

        // Expresión regular para validar la contraseña
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=]).{8,}$";
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);

        if (!regex.IsMatch(newPassword))
        {
            await DisplayAlert("Error", "La contraseña no cumple con los requisitos:\nDebe tener al menos 8 caracteres, incluyendo al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.", "OK");
            return;
        }

        // Si todas las validaciones pasan, mostrar un saludo
        await DisplayAlert("Éxito", "¡Hola!", "OK");
    }
}