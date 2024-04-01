using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;

namespace Project.Views
{
	public partial class ValidacionCod : ContentPage
	{
		private const string apiUrl = "http://3.129.71.4:3000/verificationcode";
		private string correoRecuperacion;

		public ValidacionCod(string email)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			correoRecuperacion = email;
		}

		private async void BtnEnv_Clicked(object sender, EventArgs e)
		{
			string codigoRecuperacion = CodiRec.Text.Trim();

			try
			{
				using (HttpClient client = new HttpClient())
				{
					// Crear el objeto JSON con las claves "Email" y "codigo_recuperacion"
					var jsonObject = new
					{
						Email = correoRecuperacion,
						codigo_recuperacion = codigoRecuperacion
					};

					// Serializar el objeto JSON utilizando Newtonsoft.Json
					var json = JsonConvert.SerializeObject(jsonObject);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					// Enviar la solicitud POST a la API con la ruta correcta
					HttpResponseMessage response = await client.PostAsync(apiUrl, content);

					if (response.IsSuccessStatusCode)
					{
						// Procesar la respuesta si la solicitud fue exitosa
						string responseContent = await response.Content.ReadAsStringAsync();

						// Parsear la respuesta JSON para obtener el mensaje de la API
						dynamic responseData = JsonConvert.DeserializeObject(responseContent);
						string mensaje = responseData.message;

						await DisplayAlert("Mensaje de la API", mensaje, "OK");

						// Si el código es válido, navegar a la página RestableceContra
						RestableceContra restablecerPage = new RestableceContra(correoRecuperacion, codigoRecuperacion);
						await Navigation.PushAsync(restablecerPage);
					}
					else
					{
						// Mostrar mensaje si la solicitud no fue exitosa
						await DisplayAlert("Error", "No se pudo validar el código de recuperación.", "OK");
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
