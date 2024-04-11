using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;


namespace Project.Views
{
	public partial class ListaRecordatorio : ContentPage, INotifyPropertyChanged
	{
		private ObservableCollection<RecordatorioItem> _imagenes;
		private ObservableCollection<RecordatorioItem> _audios;
		private ObservableCollection<RecordatorioItem> _otros;

		public ObservableCollection<RecordatorioItem> Imagenes
		{
			get { return _imagenes; }
			set
			{
				_imagenes = value;
				OnPropertyChanged(nameof(Imagenes));
			}
		}

		public ObservableCollection<RecordatorioItem> Audios
		{
			get { return _audios; }
			set
			{
				_audios = value;
				OnPropertyChanged(nameof(Audios));
			}
		}

		public ObservableCollection<RecordatorioItem> Otros
		{
			get { return _otros; }
			set
			{
				_otros = value;
				OnPropertyChanged(nameof(Otros));
			}
		}

		public ListaRecordatorio()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			Imagenes = new ObservableCollection<RecordatorioItem>();
			Audios = new ObservableCollection<RecordatorioItem>();
			Otros = new ObservableCollection<RecordatorioItem>();

			this.Appearing += async (sender, args) =>
			{
				await ObtenerRecordatorios();
			};
		}

		private async Task ObtenerRecordatorios()
		{
			try
			{
				var idUsuario = 1; // Cambiar por el ID del usuario

				using (var client = new HttpClient())
				{
					var url = $"http://3.129.71.4:3000/recordatorios/{idUsuario}";
					var response = await client.GetAsync(url);

					if (response.IsSuccessStatusCode)
					{
						var content = await response.Content.ReadAsStringAsync();
						Console.WriteLine("Contenido de la respuesta:");
						Console.WriteLine(content);

						var recordatorios = JsonConvert.DeserializeObject<List<RecordatorioItem>>(content);

						Imagenes.Clear();
						Audios.Clear();
						Otros.Clear();

						foreach (var recordatorio in recordatorios)
						{
							switch (recordatorio.Tipo.ToLowerInvariant())
							{
								case "imagen":
									Imagenes.Add(recordatorio);
									break;
								case "audio":
									Audios.Add(recordatorio);
									break;
								default:
									Otros.Add(recordatorio);
									break;
							}
						}
					}
					else
					{
						Console.WriteLine($"Respuesta del servidor: {response.StatusCode} - {response.ReasonPhrase}");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al obtener los recordatorios: {ex.Message}");
			}
		}


		public class RecordatorioItem
		{
			public string Tipo { get; set; }
			public string RutaArchivo { get; set; }
			public string Description { get; set; }
			public DateTime ReminderDate { get; set; }
			// Agrega más propiedades según los datos que quieras mostrar

			public string Detalle
			{
				get
				{
					// Construir el detalle que quieres mostrar en la lista
					if (!string.IsNullOrEmpty(Description))
					{
						return $"{Description} - {ReminderDate}";
					}
					else
					{
						return $"{RutaArchivo} - {ReminderDate}";
					}
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
