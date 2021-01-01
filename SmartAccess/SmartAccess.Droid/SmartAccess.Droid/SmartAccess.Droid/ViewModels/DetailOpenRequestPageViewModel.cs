using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using SmartAccess.Droid;
using SmartAccess.Droid.Helpers;
using SmartAccess.Droid.Models;
using SmartAccess.Droid.Services;
using SmartAccess.Droid.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartAccess.ViewModels
{
    public class DetailOpenRequestPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _aceptarCommad;
        private DelegateCommand _cancelarCommand;
        private ApiService _apiService;
        private bool _isRunning;
        private OpenRequest _selectItem;
        private string _name;
        private string _message;


        public DetailOpenRequestPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = new ApiService();
        }

        public DelegateCommand AceptarCommand => _aceptarCommad ?? (_aceptarCommad = new DelegateCommand(ChangeStatusYes));


        public DelegateCommand CancelarCommand => _cancelarCommand ?? (_cancelarCommand = new DelegateCommand(ChangeStatusYesNo));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public OpenRequest RequestItem
        {
            get => _selectItem;
            set => SetProperty(ref _selectItem, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            RequestItem = (OpenRequest)parameters["Request"];
            Name = RequestItem.Name;
            Message = RequestItem.Message;
            base.OnNavigatedTo(parameters);
        }

        private async void ChangeStatusYes()
        {
            var statusLock = Preferences.Get("StatusLock", false);
            if (!statusLock)
            {
                await Prism.PrismApplicationBase.Current.MainPage.DisplayAlert("Información", "La cerradura no esta disponible. por favor intente luego", "Aceptar");
                return;
            }

            IsRunning = true;
            RequestItem.Status = "A";
            var url = Prism.PrismApplicationBase.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.SetStatus(url, "/api", "/OpenRequest/", RequestItem.Id, RequestItem);
            if (!response.IsSuccess)
            {
                IsRunning = false;
                if (response.Message == "")
                {
                    response.Message = "No se pudo conectar con el Servidor por favor intente más tarde.";
                }


                await Prism.PrismApplicationBase.Current.MainPage.DisplayAlert("Información", response.Message.ToString(), "Aceptar");
                await _navigationService.GoBackToRootAsync();
                return;
            }


            MainThread.BeginInvokeOnMainThread (async() =>
            {
                
                await LockOrUnlockAsync("lock");
                await Task.Delay(TimeSpan.FromSeconds(15));
                await LockOrUnlockAsync("unlock");
            });


            IsRunning = false;
            await _navigationService.GoBackToRootAsync();
        }

        private async void ChangeStatusYesNo()
        {
            IsRunning = true;
            RequestItem.Status = "C";
            var url = Prism.PrismApplicationBase.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.SetStatus(url, "/api", "/OpenRequest/", RequestItem.Id, RequestItem);
            if (!response.IsSuccess)
            {
                IsRunning = false;
                if (response.Message == "")
                {
                    response.Message = "No se pudo conectar con el Servidor por favor intente más tarde.";
                }


                await Prism.PrismApplicationBase.Current.MainPage.DisplayAlert("Información", response.Message.ToString(), "Aceptar");
                await _navigationService.GoBackToRootAsync();
                return;
            }

            IsRunning = false;
            await _navigationService.GoBackToRootAsync();
        }

        private async Task LockOrUnlockAsync(string action)
        {

            var url2 = App.Current.Resources["UrlAPILock"].ToString();

            var response = await _apiService.GetStatusLock(url2, "/remoteoperate", "/6D3AF4851A32417E90C9AAD31202ED8D/" + action);
            if (!response.IsSuccess)
            {

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
        }     
    }

}
