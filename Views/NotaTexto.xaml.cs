using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Project.Views
{
	public partial class NotaTexto : ContentPage
	{
		private static readonly HttpClient client = new HttpClient();
		private const string BaseUrl = "http://3.129.71.4:3000/"; // Cambia por tu URL de la API

		public NotaTexto()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		private async void CrearNota_Clicked(object sender, EventArgs e)
		{
			string contenido = ContenidoEntry.Text;
			DateTime reminderDate = ReminderDatePicker.Date;
			const int tipoRecordatorio = 1; // Valor fijo para tipo de recordatorio
			
			if (string.IsNullOrWhiteSpace(contenido))
			{
				await DisplayAlert("Error", "Por favor ingrese el contenido de la nota", "Aceptar");
				return;
			}

			var nuevaNota = new
			{
				contenido,
				reminderDate,
				tipoderecordatorio = tipoRecordatorio,
				id_usuario = Preferences.Get("UserId", defaultValue: 0)
				
			};

			try
			{
				var json = JsonSerializer.Serialize(nuevaNota);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await client.PostAsync($"{BaseUrl}crearnotastexto", content);

				if (response.IsSuccessStatusCode)
				{
					var responseBody = await response.Content.ReadAsStringAsync();
					var result = JsonSerializer.Deserialize<ApiResponse>(responseBody);

					await DisplayAlert("Éxito", result.message, "Aceptar");

					// Limpiar campos después de crear la nota
					ContenidoEntry.Text = string.Empty;
					ReminderDatePicker.Date = DateTime.Today;
				}
				else
				{
					await DisplayAlert("Error", "Ocurrió un error al crear la nota de texto", "Aceptar");
				}
			}
			catch (HttpRequestException ex)
			{
				await DisplayAlert("Error de Conexión", $"Error al conectar con el servidor: {ex.Message}", "Aceptar");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"Error al crear la nota de texto: {ex.Message}", "Aceptar");
			}
		}



		public class ApiResponse
		{
			public string message { get; set; }
		}
	}
}
