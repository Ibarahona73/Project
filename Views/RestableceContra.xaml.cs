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

        // Verificar si el campo de contrase�a est� en blanco
        if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Error", "Por favor, ingrese una contrase�a.", "OK");
            return;
        }

        if (newPassword != confirmPassword)
        {
            await DisplayAlert("Error", "Las contrase�as no coinciden", "OK");
            return;
        }

        // Expresi�n regular para validar la contrase�a
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=]).{8,}$";
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);

        if (!regex.IsMatch(newPassword))
        {
            await DisplayAlert("Error", "La contrase�a no cumple con los requisitos:\nDebe tener al menos 8 caracteres, incluyendo al menos una letra may�scula, una letra min�scula, un n�mero y un car�cter especial.", "OK");
            return;
        }

        // Si todas las validaciones pasan, mostrar un saludo
        await DisplayAlert("�xito", "�Hola!", "OK");
    }
}