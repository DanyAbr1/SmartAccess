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
        private string _apellido;
        private string _nombre;
        private string _login;        
        private string _cedula;               
        private DateTime _fechaNacimiento;
        private string _contrasena;
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


        public string Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }

        public string Apellido
        {
            get => _apellido;
            set => SetProperty(ref _apellido, value);
        }



        public string Nombre2
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Contrasena
        {
            get => _contrasena;
            set => SetProperty(ref _contrasena, value);
        }

        public string Cedula
        {
            get => _cedula;
            set => SetProperty(ref _cedula, value);
        }


        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set => SetProperty(ref _fechaNacimiento, value);
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
            if (string.IsNullOrEmpty(Nombre)
                || string.IsNullOrEmpty(Apellido)
                || string.IsNullOrEmpty(Cedula)
                || string.IsNullOrEmpty(Contrasena)
                || FechaNacimiento == null)
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
                Name= Nombre,

                LastName= Apellido,

                UserName= Nombre2,

                DNI= Cedula,
                                
                BirthDate= FechaNacimiento,

                Password= Contrasena,
                

            };
        }

        private void LimpiarComponentes()
        {         
            Nombre = string.Empty;
            Apellido = string.Empty;
            Cedula = string.Empty;
            Contrasena = string.Empty;         

        }
        #endregion

    }
}
