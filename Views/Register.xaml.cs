using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.Views
{
	public partial class Register : ContentPage
	{
		private readonly HttpClient _httpClient;

		public Register()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			_httpClient = new HttpClient();
		}

		private async void Procesar_Clicked(object sender, EventArgs e)
		{
			// Verificar si alg�n campo est� vac�o
			if (string.IsNullOrWhiteSpace(RegNombre.Text) ||
				string.IsNullOrWhiteSpace(NombreCompleto.Text) ||
				string.IsNullOrWhiteSpace(RegEmail.Text) ||
				string.IsNullOrWhiteSpace(RegContra.Text))
			{
				await DisplayAlert("Error", "Por favor, llene todos los campos.", "OK");
				return;
			}

			// Obtener los valores de los campos
			string nombre = RegNombre.Text.Trim();
			string apellido = NombreCompleto.Text.Trim();
			string correo = RegEmail.Text.Trim();
			string contrasena = RegContra.Text.Trim();

			// Validar el correo electr�nico
			if (!ValidarCorreoElectronico(correo))
			{
				await DisplayAlert("Error", "Inserte un correo v�lido (Gmail, Hotmail, Outlook)", "OK");
				return;
			}

			// Llamar a la API para crear el usuario
			bool success = await CreateUser(nombre, apellido, correo, contrasena);

			if (success)
			{
				await DisplayAlert("�xito", "Usuario creado correctamente.", "OK");
				// Aqu� puedes navegar a otra p�gina, por ejemplo, la p�gina de inicio de sesi�n
				Navigation.PushAsync(new Login());
			}
			else
			{
				await DisplayAlert("Error", "No se pudo crear el usuario. Por favor, int�ntelo de nuevo m�s tarde.", "OK");
			}
		}

		private async Task<bool> CreateUser(string nombre, string apellido, string correo, string contrasena)
		{
			try
			{
				var user = new
				{
					Nombre = nombre,
					Apellido = apellido,
					Email = correo,
					Contrasena = contrasena
				};

				var json = JsonSerializer.Serialize(user);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await _httpClient.PostAsync("http://3.129.71.4:3000/createuser", content);

				if (response.IsSuccessStatusCode)
				{
					// Leer la respuesta como cadena JSON
					var responseContent = await response.Content.ReadAsStringAsync();

					// Deserializar la respuesta para verificar si la creaci�n fue exitosa
					var result = JsonSerializer.Deserialize<ApiResponse>(responseContent);

					if (result != null && result.success)
					{
						return true;
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al crear el usuario: {ex.Message}");
				return false;
			}
		}

		private bool ValidarCorreoElectronico(string correo)
		{
			// Expresi�n regular para validar el correo electr�nico
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+@(gmail|hotmail|outlook)\.(com|es)$");
			return regex.IsMatch(correo);
		}

        private async void ReadAccount_Clicked(object sender, EventArgs e)
        {
            Login LoginPage = new Login();
            await Navigation.PushAsync(LoginPage);

        }
    }



    // Clase auxiliar para deserializar la respuesta de la API
    public class ApiResponse
	{
		public bool success { get; set; }
		public string message { get; set; }
	}

}
