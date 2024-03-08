namespace Project.Views;

public partial class ValidacionCod : ContentPage
{
	public ValidacionCod()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void BtnEnv_Clicked(object sender, EventArgs e)
    {
        RestableceContra NewContra = new RestableceContra();
        NavigationPage currentNavigationPage = Application.Current.MainPage as NavigationPage;
        await currentNavigationPage.Navigation.PushAsync(NewContra);
    }
}