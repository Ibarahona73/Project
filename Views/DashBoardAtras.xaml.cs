

namespace Project.Views
{
	public partial class DashBoardAtras : ContentPage
	{
		public DashBoardAtras()
		{
			InitializeComponent();
			var page = new DashBoardo();
			Navigation.PushAsync(page);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			// Recuperar los datos del usuario desde Preferences
			int userId = Preferences.Get("UserId", defaultValue: 0);
			string userName = Preferences.Get("UserName", string.Empty);
			string userEmail = Preferences.Get("UserEmail", string.Empty);

			// Establecer el texto del label con el nombre de usuario
			lblUserName.Text = $"¡Hola, {userName}!";
		}
	}
}
