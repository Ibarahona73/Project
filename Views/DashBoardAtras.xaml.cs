namespace Project.Views;

public partial class DashBoardAtras : ContentPage
{
	public DashBoardAtras()
	{
		InitializeComponent();
        var page = new DashBoardo();
        Navigation.PushAsync(page);
        

    }
}