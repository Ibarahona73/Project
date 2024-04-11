using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Project.Views
{
	public partial class ListaRecordatorio : ContentPage
	{
		public List<RecordatorioImagenModel> Recordatorios { get; set; }

		public ListaRecordatorio()
		{
			InitializeComponent();
			BindingContext = this;
			Recordatorios = new List<RecordatorioImagenModel>();

			MostrarRecordatoriosConImagenes();
		}

		private async void MostrarRecordatoriosConImagenes()
		{
			try
			{
				HttpClient client = new HttpClient();
				var response = await client.GetAsync("http://3.129.71.4:3000/recordatorios-con-imagenes/1");
				response.EnsureSuccessStatusCode();

				var responseBody = await response.Content.ReadAsStringAsync();
				Recordatorios = JsonSerializer.Deserialize<List<RecordatorioImagenModel>>(responseBody);

				// Notificar al ListView que la colección ha cambiado
				OnPropertyChanged(nameof(Recordatorios));
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al obtener los recordatorios con imágenes: {ex.Message}");
			}
		}

		public class RecordatorioImagenModel
		{
			public RutaArchivoModel RutaArchivo { get; set; }
			public string Description { get; set; }
			public DateTime ReminderDate { get; set; }
			public int TipoRecordatorio { get; set; }
			public DateTime CreateDate { get; set; }
		}

		public class RutaArchivoModel
		{
			public string Type { get; set; }
			public byte[] Data { get; set; }
		}
	}

	public class Base64ToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is byte[] imageData)
			{
				try
				{
					// Convertir byte[] a ImageSource
					ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageData));
					return imageSource;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error al convertir imagen: {ex.Message}");
					return null;
				}
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
