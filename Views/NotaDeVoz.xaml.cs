using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.AudioRecorder;


namespace Project.Views
{
	public partial class NotaDeVoz : ContentPage
	{
		AudioRecorderService recorder = new AudioRecorderService();
		Plugin.AudioRecorder.AudioPlayer players;
		string filePath;
		byte[] audi;

		public NotaDeVoz()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			recorder.TotalAudioTimeout = TimeSpan.FromSeconds(10); // Cambiado a 10 segundos para tu requerimiento
			recorder.StopRecordingOnSilence = false;
			players = new Plugin.AudioRecorder.AudioPlayer();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await DisplayAlert("Instrucciones", "Tienes como límite hacer una nota de voz de 10 segundos", "OK");
		}

		private async void Grabar_Clicked(object sender, EventArgs e)
		{
			micro.Source = "mic_on.png";
			var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
			if (status != PermissionStatus.Granted)
			{
				status = await Permissions.RequestAsync<Permissions.Microphone>();
				if (status != PermissionStatus.Granted)
				{
					await DisplayAlert("Permiso Requerido", "Se requieren permisos de micrófono para grabar audio.", "Aceptar");
					return;
				}
			}
			else
			{
				await recorder.StartRecording();
				Grabar.IsEnabled = false;
				Stop.IsEnabled = true;
			}
		}

		private async void Stop_Clicked(object sender, EventArgs e)
		{
			micro.Source = "mic_off.png";
            await recorder.StopRecording();
			filePath = recorder.GetAudioFilePath();
			audi = ConvertAudioToBase64(filePath);
			Grabar.IsEnabled = true;
			Stop.IsEnabled = false;
		}

		private async void rep_Clicked(object sender, EventArgs e)
		{
			if (filePath == null)
			{
				await DisplayAlert("Error", "No hay audio grabado", "OK");
			}
			else
			{
				players.Play(filePath);
			}
		}

		private byte[] ConvertAudioToBase64(string filePath)
		{
			byte[] audio = File.ReadAllBytes(filePath);
			return audio;
		}

		private async void GDatos_Clicked(object sender, EventArgs e)
		{
            DateTime fechaYHoraSeleccionada;


            if (audi == null || audi.Length == 0)
			{
				await DisplayAlert("Error", "No se grabó ningún audio", "OK");
				return;
			}

			DateTime fechaSeleccionada = Recuerdo.Date;
            TimeSpan horaSeleccionada = RecuerdoTime.Time;
            DateTime soloFecha = new DateTime(fechaSeleccionada.Year, fechaSeleccionada.Month, fechaSeleccionada.Day);
            fechaYHoraSeleccionada = fechaSeleccionada.Date + horaSeleccionada;

            try
			{
				var notaDeVoz = new
				{
					rutaArchivo = Convert.ToBase64String(audi),
					reminderDate = fechaYHoraSeleccionada,
                    tiporecordatorio = 1,
					id_usuario = 1
				};

				var json = JsonConvert.SerializeObject(notaDeVoz);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				using (var client = new HttpClient())
				{
					var uri = new Uri("http://3.129.71.4:3000/crearnotasvoz"); // Reemplaza con la URL de tu API
					var response = await client.PostAsync(uri, content);

					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();
						var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
						await DisplayAlert("Aviso", result.message.ToString(), "OK");
					}
					else
					{
						await DisplayAlert("Error", "Hubo un error al enviar la nota de voz", "OK");
					}
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"Se produjo un error al agregar la nota de voz: {ex.Message}", "OK");
			}
		}
	}
}
