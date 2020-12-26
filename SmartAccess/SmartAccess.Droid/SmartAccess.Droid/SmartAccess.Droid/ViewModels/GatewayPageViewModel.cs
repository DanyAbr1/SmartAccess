using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using SmartAccess.Droid.Helpers;
using SmartAccess.Droid.Models;
using SmartAccess.Droid.Services;
using System.Drawing;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartAccess.Droid.ViewModels
{
    public class GatewayPageViewModel : ViewModelBase
    {
        private DelegateCommand _lockUnluckCommand;
        private ApiService _apiService;
        private bool _isEnabled;
        private bool _isRunning;
        private bool _status;
        private string _image;
        private string _imageColor;

        public GatewayPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _apiService = new ApiService();
        }


        public DelegateCommand LockUnlockCommand => _lockUnluckCommand ?? (_lockUnluckCommand = new DelegateCommand(LockUnluckAsync));

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

        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public string ImageColor
        {
            get => _imageColor;
            set => SetProperty(ref _imageColor, value);
        }

        private async void getStatusAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Información", "No se pudo conectar a internet por favor intente más tarde.", "Aceptar");

                return;
            }


            IsRunning = true;
            IsEnabled = false;



            var url2 = App.Current.Resources["UrlAPILock"].ToString();

            var response = await _apiService.GetStatusLock(url2, "/remoteoperate", "/6D3AF4851A32417E90C9AAD31202ED8D/status");
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
                    return;
                }

                await App.Current.MainPage.DisplayAlert("Información", response.Message, "Aceptar");
                return;
            }

            GetStatus(response.Result.Status);

            IsRunning = false;
            IsEnabled = true;

        }

        private async Task UnlockAsycn()
        {

            IsRunning = true;
            IsEnabled = false;



            var url2 = App.Current.Resources["UrlAPILock"].ToString();

            var response = await _apiService.GetStatusLock(url2, "/remoteoperate", "/6D3AF4851A32417E90C9AAD31202ED8D/unlock");
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
                    return;
                }

                await App.Current.MainPage.DisplayAlert("Información", response.Message, "Aceptar");
                return;
            }

            GetStatus("kAugLockState_Unlocked");
            IsRunning = false;
            IsEnabled = true;
        }

        private async Task LockAsync()
        {

            IsRunning = true;
            IsEnabled = false;



            var url2 = App.Current.Resources["UrlAPILock"].ToString();

            var response = await _apiService.GetStatusLock(url2, "/remoteoperate", "/6D3AF4851A32417E90C9AAD31202ED8D/lock");
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
                    return;
                }

                await App.Current.MainPage.DisplayAlert("Información", response.Message, "Aceptar");
                return;
            }

            GetStatus("kAugLockState_Locked");
            IsRunning = false;
            IsEnabled = true;
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            getStatusAsync();
        }

        private async void LockUnluckAsync()
        {
            switch (_status)
            {
                case true:
                    await UnlockAsycn();
                    break;
                case false:
                    await LockAsync();
                    break;
            }
        }

        private void GetStatus(string Status)
        {
            switch (Status)
            {
                case "kAugLockState_Locked":
                    Image = "Locked.png";
                    _status = true;
                    ImageColor = "GreenYellow";
                    break;
                case "kAugLockState_Unlocked":
                    Image = "Unlocked.png";
                    _status = false;
                    ImageColor = "Red";
                    break;
            }
        }
    }
}
