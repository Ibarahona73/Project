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
            DateTime fechaYHoraSeleccionada; int estado;
            string hola;

            if(switche.IsToggled)
            {

                estado = 1;
                hola = estado.ToString();
                Console.WriteLine(estado);
            }
            else
            {
                estado = 0;
                hola = estado.ToString();
                Console.WriteLine(estado);
            }

            const int tipoRecordatorio = 1; // Valor fijo para tipo de recordatorio

            if (string.IsNullOrWhiteSpace(contenido))
			{
				await DisplayAlert("Error", "Por favor ingrese el contenido de la nota", "Aceptar");
				return;
			}

            DateTime fechaSeleccionada = Recuerdo.Date;
            TimeSpan horaSeleccionada = RecuerdoTime.Time;

            fechaYHoraSeleccionada = fechaSeleccionada.Date + horaSeleccionada;

            var nuevaNota = new
			{
                contenido,
                reminderDate = fechaYHoraSeleccionada,
                tipoderecordatorio = tipoRecordatorio,
                estado = hola,
                id_usuario = Preferences.Get("UserId", defaultValue: 0),

            };

            Console.WriteLine(estado);

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
                    Recuerdo.Date = DateTime.Today; // Establecer la fecha en la fecha actual
                    RecuerdoTime.Time = TimeSpan.Zero; // Establecer la hora en 00:00:00

                    // Limpiar el estado del Switch
                    switche.IsToggled = false; // Opcionalmente, puedes establecerlo en false para limpiar su estado


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

        private async void switche_Toggled(object sender, ToggledEventArgs e)
        {
            if (switche.IsToggled)
            {

                await DisplayAlert("Estado: ", "\tActivo \n Nota De Voz", "OK");
            }
            else
            {
                await DisplayAlert("Estado: ", "\tDesactivado \n Nota De Voz", "OK");
            }


        }

        public class ApiResponse
		{
			public string message { get; set; }
		}
	}
}
