using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace Project.Views
{
	public partial class Imagen : ContentPage
	{
		private static readonly HttpClient client = new HttpClient();
		private const string BaseUrl = "http://3.129.71.4:3000/"; // Cambia por tu URL de la API
		private string imagePath;

		public Imagen()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		private async void TomarFoto_Clicked(object sender, EventArgs e)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("Error", "La cámara no está disponible.", "Aceptar");
				return;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = "Sample",
				Name = "test.jpg"
			});

			if (file == null)
				return;

			ImagenPreview.Source = ImageSource.FromStream(() =>
			{
				var stream = file.GetStream();
				return stream;
			});

			imagePath = file.Path;
		}

		private async void CrearImagen_Clicked(object sender, EventArgs e)
		{
            DateTime fechaYHoraSeleccionada;
            int estado;

            if (switche.IsToggled)
            {

                estado = 1;
                Console.WriteLine(estado);
            }
            else
            {
                estado = 0;
                Console.WriteLine(estado);
            }

            if (string.IsNullOrWhiteSpace(imagePath))
			{
				await DisplayAlert("Error", "Por favor seleccione una imagen", "Aceptar");
				return;
			}

            string descripcion = DescripcionEntry.Text;
            DateTime fechaSeleccionada = Recuerdo.Date;
            TimeSpan horaSeleccionada = RecuerdoTime.Time;
            fechaYHoraSeleccionada = fechaSeleccionada.Date + horaSeleccionada;

            try
			{
				// Leer el contenido del archivo como un arreglo de bytes
				byte[] imageData = File.ReadAllBytes(imagePath);

				// Convertir el arreglo de bytes a una cadena Base64
				string imageDataBase64 = Convert.ToBase64String(imageData);

				var nuevaImagen = new
				{
                    rutaArchivo = imageDataBase64, // Enviar la imagen como Base64
                    description = descripcion,
                    reminderDate = fechaYHoraSeleccionada,
                    tiporecordatorio = 1,
                    id_usuario = Preferences.Get("UserId", defaultValue: 0)
                };

				var json = JsonSerializer.Serialize(nuevaImagen);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await client.PostAsync($"{BaseUrl}crearimagen", content);

				if (response.IsSuccessStatusCode)
				{
					var responseBody = await response.Content.ReadAsStringAsync();
					var result = JsonSerializer.Deserialize<ApiResponse>(responseBody);

					await DisplayAlert("Éxito", result.message, "Aceptar");

                    // Limpiar campos después de crear la imagen
                    DescripcionEntry.Text = string.Empty;
                    Recuerdo.Date = DateTime.Today;
                    RecuerdoTime.Time = TimeSpan.Zero;
                    ImagenPreview.Source = null;
                    imagePath = null;
                }
				else
				{
					await DisplayAlert("Error", $"Ocurrió un error al crear la imagen", "Aceptar");
				}
			}
			catch (HttpRequestException ex)
			{
				await DisplayAlert("Error de Conexión", $"Error al conectar con el servidor: {ex.Message}", "Aceptar");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"Error al crear la imagen: {ex.Message}", "Aceptar");
			}
		}


		public class ApiResponse
		{
			public string message { get; set; }
		}
	}
}
