using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartAccess.Droid.Models;
using SmartAccess.Droid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartAccess.Droid.ViewModels
{
    public class NewUserPageViewModel : ViewModelBase
    {
        private ApiService _apiService;
        private bool _isRunning;
        private DelegateCommand _solicitudCommand;        
        private bool _isEnabled;
        private string _lastName;
        private string _name;
        private string _userName;        
        private string _dni;               
        private DateTime _birthDay;
        private string _password;
        private readonly INavigationService _navigationService;


        public NewUserPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _apiService = new ApiService();            
            _isEnabled = true;            
            _navigationService = navigationService;
        }
  


        #region Propiedades

        public DelegateCommand SolicitudCommand => _solicitudCommand ?? (_solicitudCommand = new DelegateCommand(EnviarSolicitud));


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


        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }



        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string DNI
        {
            get => _dni;
            set => SetProperty(ref _dni, value);
        }


        public DateTime BirthDay
        {
            get => _birthDay;
            set => SetProperty(ref _birthDay, value);
        }

        #endregion

        #region Metodos

        public async void EnviarSolicitud()
        {

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("Informacion", "No se pudo conectar a internet por favor intente más tarde.", "Aceptar");
                return;
            }

            var valid = await ValidaDatosAsync();
            if (!valid) return;
            var usuario = GeneraUsuario();

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();

            var response = await _apiService.SolicitudRegistro(url, "/api", "/User/AddUser", usuario);
            if (!response.IsSuccess)
            {

                IsRunning = false;
                IsEnabled = true;
                if (response.Message == "")
                {
                    response.Message = "No se pudo conectar con el servidor por favor intente más tarde.";
                }
                //var respuesta = JsonConvert.DeserializeObject<Response<object>>(response.Message);
                //var respuesta = JsonConvert.DeserializeObject<object>(response.Message);
                await App.Current.MainPage.DisplayAlert("Información", response.Message, "Aceptar");
                return;
            }

            IsRunning = false;
            IsEnabled = true;

            await App.Current.MainPage.DisplayAlert("Información", "El usuario fue Creado con éxito", "Aceptar");
            LimpiarComponentes();
            await _navigationService.NavigateAsync("LoginPage");
        }


        public async Task<bool> ValidaDatosAsync()
        {
            if (string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(LastName)
                || string.IsNullOrEmpty(DNI)
                || string.IsNullOrEmpty(Password)
                || BirthDay == null)
            {
                await App.Current.MainPage.DisplayAlert("Información", "Debe completar todos los campos para poder procesar la soliciúd.", "Aceptar");
                return false;
            }

            return true;

        }

        private User GeneraUsuario()
        {
            return new User
            {
                Name= Name,

                LastName= LastName,

                UserName= UserName,

                DNI= DNI,
                                
                BirthDate= BirthDay,

                Password= Password,
                

            };
        }

        private void LimpiarComponentes()
        {         
            Name = string.Empty;
            LastName = string.Empty;
            DNI = string.Empty;
            Password = string.Empty;         

        }
        #endregion

    }
}
