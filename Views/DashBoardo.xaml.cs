namespace Project.Views;

public partial class DashBoardo : Shell
{
	public DashBoardo()
    {
		InitializeComponent();
        

    }

    private async void Csesion_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Error", "Hola Como Estas", "OK");
    }
}