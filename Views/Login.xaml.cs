using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
namespace Project.Views;


public partial class Login : ContentPage
{
    private static readonly HttpClient client = new HttpClient();
    private const string BaseUrl = "http://18.223.158.59:3000/"; // Cambia por tu URL de la API

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
        if (string.IsNullOrWhiteSpace(Email.Text) || string.IsNullOrWhiteSpace(Contra.Text))
        {
            await DisplayAlert("Campos incompletos", "Por favor complete todos los campos", "Aceptar");
            return;
        }

        var usuario = new
        {
            Email = Email.Text,
            Password = Contra.Text
        };

        try
        {
            var json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{BaseUrl}login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LoginResponse>(responseBody);

                if (result != null && result.message == "Login exitoso" && result.usuario != null)
                {
                    DashBoardAtras dashBoardPage = new DashBoardAtras
                    {
                        BindingContext = result.usuario
                    };

                    NavigationPage.SetHasBackButton(dashBoardPage, false);
                    await Navigation.PushAsync(dashBoardPage);
                }
                else
                {
                    await DisplayAlert("Credenciales incorrectas", "El correo electrónico o la contraseña son incorrectos", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Error", "Ocurrió un error al intentar iniciar sesión", "Aceptar");
            }
        }
        catch (HttpRequestException ex)
        {
            await DisplayAlert("Error de Conexión", $"Error al intentar conectar con el servidor: {ex.Message}", "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al intentar iniciar sesión: {ex.Message}", "Aceptar");
        }

        Contra.Text = string.Empty;
    }

    private async void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        Register registerPage = new Register();
        await Navigation.PushAsync(registerPage);
    }

    public class LoginResponse
    {
        public string message { get; set; }
        public Usuario usuario { get; set; }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
    }
}
