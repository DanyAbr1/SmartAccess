using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using SmartAccess.Droid.Models;
using SmartAccess.Droid.Services;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartAccess.Droid.ViewModels
{
    public class ChatPageViewModel : ViewModelBase
    {


        private readonly INavigationService _navigationService;

        HubConnection _hubConnection;

        private readonly ApiService _apiService;
        private DelegateCommand _sendMessageCommand;
        private string _Name;
        private string _writteMessage;
        private bool _isRunning;
        private User _usuario;
        private ObservableCollection<Chat> _Conversation;
        private bool connected;
        private string _userColor;

        public ChatPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _Conversation = new ObservableCollection<Chat>();
            _apiService = new ApiService();

        }

        public DelegateCommand SendMessageCommand => _sendMessageCommand ?? (_sendMessageCommand = new DelegateCommand(SendedMessage));


        public ObservableCollection<Chat> Conversation
        {
            get => _Conversation;
            set => SetProperty(ref _Conversation, value);
        }



        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        public string WritteMessage
        {
            get => _writteMessage;
            set => SetProperty(ref _writteMessage, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string UserColor 
        { 
            get =>_userColor; 
            set => SetProperty(ref _userColor,value); 
        }


        private async Task GetMessages()
        {
            IsRunning = true;

            var url = Prism.PrismApplicationBase.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.GetChats(url, "/api", "/Chat/GetMessage");
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


            //response.Result.ForEach(x => Conversation.Add(x));            
            if (response.Result.Count > 0)
            {
                for (int i = 0; i < response.Result.Count; i++)
                {
                    _
                    if(response.Result[i].Id == _usuario.Id)
                    {

                    }
                }
            }

            IsRunning = false;

        }
        private async void SendedMessage()
        {
            var msj = new Chat
            {
                Name = _usuario.Name,
                Message = WritteMessage
            };

            WritteMessage = string.Empty;


            var url = Prism.PrismApplicationBase.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.NewMessage(url, "/api", "/Chat/NewMessage", msj);
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

        }


        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            _usuario = (User)parameters["Usuario"];
            await GetMessages();
            await ConectionHub();
        }


        private async Task ConectionHub()
        {
            var uri = $"{App.AzureBackendUrl}/hubs/messages";
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(uri, options =>
                    {
                        options.HttpMessageHandlerFactory = factory => new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                        };
                    }).Build();

                if (!connected)
                    await _hubConnection.StartAsync();

                connected = true;

            }
            catch (Exception ex)
            {
            }


            _hubConnection.On<string>("NewMessage", (chat) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        var newChat = JsonConvert.DeserializeObject<Chat>(chat);
                        var index = Conversation.Count;
                        Conversation.Insert(index, newChat);

                    }
                    catch (Exception ex)
                    {
                    }
                });
            });

        }

    }
}
