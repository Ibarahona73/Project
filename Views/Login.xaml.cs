using Xamarin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Xamarin.Forms;
namespace Project.Views;


public partial class Login : ContentPage
{

    private const string ClientId = "YOUR_CLIENT_ID.apps.googleusercontent.com";
    private const string ClientSecret = "YOUR_CLIENT_SECRET";
    private const string RedirectUri = "YOUR_REDIRECT_URI";

    private GoogleAuthorizationCodeFlow flow;
    public Login()
    {
        InitializeComponent();

        // Configurar el flujo de autorización de Google
        flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret
            },
            Scopes = new[] { "openid", "profile", "email" },
            DataStore = new FileDataStore("GoogleAuthStore")
        });
    }


    private void IngresarBtn_Clicked(object sender, EventArgs e)
    {

    }

    private void OlvidarPassBtn_Clicked(object sender, EventArgs e)
    {

    }

    private async void RegisterBtn_Clicked(object sender, EventArgs e)
    {
        // Crear una instancia de la página Register.xaml
        Register registerPage = new Register();

        // Obtener el NavigationPage actual
        NavigationPage? currentNavigationPage = Application.Current.MainPage as NavigationPage;

        // Verificar si currentNavigationPage no es nulo antes de continuar
        if (currentNavigationPage != null)
        {
            // Navegar a la página Register.xaml dentro del NavigationPage actual
            await currentNavigationPage.PushAsync(registerPage);
        }
    }



    private async void RegGoogle_Clicked(object sender, EventArgs e)
    {
        // Crear solicitud de autorización de Google
        var authUrl = flow.CreateAuthorizationCodeRequest(RedirectUri).Build();

        // Iniciar la autenticación con la cuenta de Google
        var authResult = await WebAuthenticator.AuthenticateAsync(authUrl, new Uri(RedirectUri));

        // Procesar el resultado de la autenticación
        if (authResult.Properties.ContainsKey("code"))
        {
            // Se obtuvo el código de autorización, puedes usarlo para obtener tokens de acceso
            var code = authResult.Properties["code"];

            // Aquí puedes hacer algo con el código de autorización, como intercambiarlo por tokens de acceso.
            // Por ejemplo, puedes redirigir al usuario a la página de registro.
            await Navigation.PushAsync(new Register());
        }
        else if (authResult.Properties.ContainsKey("error"))
        {
            // Manejar el error de autenticación si es necesario
            var error = authResult.Properties["error"];
            await DisplayAlert("Error", error, "OK");
        }
        else
        {
            // Manejar otros casos si es necesario
            await DisplayAlert("Error", "Unknown error occurred.", "OK");
        }
    }
}
