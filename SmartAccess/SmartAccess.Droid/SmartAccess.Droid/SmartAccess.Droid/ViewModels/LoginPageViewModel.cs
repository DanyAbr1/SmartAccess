using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using SmartAccess.Droid.Helpers;
using SmartAccess.Droid.Models;
using SmartAccess.Droid.Services;
using Xamarin.Essentials;

namespace SmartAccess.Droid.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {


        private readonly INavigationService _navigationService;
        private readonly ApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registrarCommand;
        private DelegateCommand _restaurarPassCommand;
        private bool _encendido;
        private bool _pageVisble;

        public LoginPageViewModel(INavigationService navigationService)
        : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = new ApiService();
            _isEnabled = true;
            PageVisble = false;
            RemoveCache();

        }


        #region Propiedades

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));
        public DelegateCommand RegistrarCommand => _registrarCommand ?? (_registrarCommand = new DelegateCommand(RegisterUser));
        public DelegateCommand RestaurarPassCommand => _restaurarPassCommand ?? (_restaurarPassCommand = new DelegateCommand(OlvidePasswordPage));


        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool PageVisble
        {
            get => _pageVisble;
            set => SetProperty(ref _pageVisble, value);
        }
        #endregion

        #region Metodos
        private async void Login()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Información", "No se pudo conectar a internet por favor intente más tarde.", "Aceptar");

                return;
            }

            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Información", "Debe ingresar un usuario.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Información", "Debe ingresar una contraseña.", "Aceptar");
                return;
            }

            IsRunning = true;
            IsEnabled = false;



            var url = App.Current.Resources["UrlAPI"].ToString();

            var response = await _apiService.GetUsuarioByEmailAsync(url, "/api", "/User/authenticate", Email, Password);
            if (!response.IsSuccess)
            {

                IsRunning = false;
                IsEnabled = true;
                if (response.Message == "")
                {
                    response.Message = "No se pudo conectar con el Servidor por favor intente más tarde.";
                }

                var isValid = IsValidJson.IsValid(response.Message);
                if (isValid)
                {
                    var respuesta = JsonConvert.DeserializeObject<ResponseMessage>(response.Message);
                    await App.Current.MainPage.DisplayAlert("Información", respuesta.Message, "Aceptar");
                    Password = string.Empty;
                    return;
                }

                await App.Current.MainPage.DisplayAlert("Información", response.Message, "Aceptar");
                Password = string.Empty;
                return;
            }

            

            Preferences.Set("idUsuario", response.Result.Id);
            Preferences.Set("nombreCompleto", $"{ response.Result.Name} { response.Result.LastName}");

            Preferences.Set("email", Email);
            Preferences.Set("pass", Password);

            var parameter = new NavigationParameters();
            parameter.Add("Usuario", response.Result);


            var url2 = App.Current.Resources["UrlAPILock"].ToString();
            var response2 = await _apiService.GetSesionByEmailAsync(url2, "/session");
            if (!response2.IsSuccess)
            {
                var respuesta = JsonConvert.DeserializeObject<ResponseMessage>(response.Message);
                await App.Current.MainPage.DisplayAlert("Información", respuesta.Message, "Aceptar");
                Password = string.Empty;
                return;
            }

            Password = string.Empty;

            IsRunning = false;

            await _navigationService.NavigateAsync("/MenuPage/NavigationPage/GatewayPage", parameter);
        }


        private async void RegisterUser()
        {
            await _navigationService.NavigateAsync("NewUserPage");
        }

        private async void OlvidePasswordPage()
        {
            await _navigationService.NavigateAsync("ResetPasswordPage");

        }

        private void RemoveCache()
        {
            Preferences.Remove("Usuario");
            Preferences.Remove("idUsuario");
            Preferences.Remove("nombreCompleto");
            Preferences.Remove("AccesToken");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (!string.IsNullOrEmpty(Preferences.Get("email","")) && !string.IsNullOrEmpty(Preferences.Get("pass", "")))
            {                
                Email = Preferences.Get("email", "");
                Password = Preferences.Get("pass", "");
                Login();
                return;
            }

            PageVisble = true;
        }

        #endregion
    }
}
