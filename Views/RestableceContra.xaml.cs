using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;

namespace Project.Views
{
	public partial class RestableceContra : ContentPage
	{
		private const string apiUrl = "http://3.129.71.4:3000/newpassword";
		private string correoRecuperacion;
		private string codigoRecuperacion;

		public RestableceContra(string email, string codigoRecuperacion)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			this.correoRecuperacion = email;
			this.codigoRecuperacion = codigoRecuperacion;
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

			try
			{
				using (HttpClient client = new HttpClient())
				{
					// Crear el objeto JSON con las claves "Email", "codigo_recuperacion" y "nuevaContrasena"
					var jsonObject = new
					{
						Email = correoRecuperacion,
						codigo_recuperacion = codigoRecuperacion,
						nuevaContrasena = newPassword
					};

					// Serializar el objeto JSON utilizando Newtonsoft.Json
					var json = JsonConvert.SerializeObject(jsonObject);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					// Enviar la solicitud PUT a la API con la ruta correcta
					HttpResponseMessage response = await client.PutAsync(apiUrl, content);

					if (response.IsSuccessStatusCode)
					{
						// Procesar la respuesta si la solicitud fue exitosa
						string responseContent = await response.Content.ReadAsStringAsync();

						// Parsear la respuesta JSON para obtener el mensaje de la API
						dynamic responseData = JsonConvert.DeserializeObject(responseContent);
						string mensaje = responseData.message;

						await DisplayAlert("Mensaje de la API", mensaje, "OK");

						// Navegar a la página de inicio de sesión (LoginPage)
						await Navigation.PopToRootAsync(); // Para volver a la página de inicio
					}
					else
					{
						// Mostrar mensaje si la solicitud no fue exitosa
						await DisplayAlert("Error", "No se pudo actualizar la contraseña.", "OK");
					}
				}
			}
			catch (Exception ex)
			{
				// Manejar cualquier excepción que pueda ocurrir durante la solicitud
				await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
			}
		}

	}

}
