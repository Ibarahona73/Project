using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;

namespace Project.Views
{
	public partial class OlvContra : ContentPage
	{
		private const string apiUrl = "http://3.129.71.4:3000/sendcod";
		private string correoRecuperacion;

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
				return;
			}

			// Obtener el correo electrónico
			string correoRec = CorreoRes.Text.Trim();
			correoRecuperacion = CorreoRes.Text.Trim();

			// Validar el correo electrónico
			if (!IsValidEmail(correoRec))
			{
				await DisplayAlert("Error", "Inserte un correo electrónico válido.", "OK");
				return;
			}

			try
			{
				using (HttpClient client = new HttpClient())
				{
					// Crear el objeto JSON con la clave "email"
					var jsonObject = new
					{
						email = correoRec
					};

					// Serializar el objeto JSON utilizando Newtonsoft.Json
					var json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					// Enviar la solicitud POST a la API
					HttpResponseMessage response = await client.PostAsync(apiUrl, content);

					if (response.IsSuccessStatusCode)
					{
						// Mostrar mensaje de éxito
						await DisplayAlert("Éxito", "Código de recuperación enviado correctamente.", "OK");

						// Navegar a la página de validación de código
						ValidacionCod restablecerPage = new ValidacionCod(correoRecuperacion);
						await Navigation.PushAsync(restablecerPage);
					}
					else
					{
						// Mostrar mensaje si no se pudo enviar el correo
						await DisplayAlert("Error", "No se pudo enviar el código, revise que su correo sea correcto.", "OK");
					}
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
			}
		}

		private bool IsValidEmail(string email)
		{
			// Expresión regular para validar el correo electrónico
			Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
			return regex.IsMatch(email);
		}
	}
}
