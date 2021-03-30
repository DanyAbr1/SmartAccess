using Prism.Commands;
using Prism.Navigation;
using SmartAccess.Droid.Models;
using SmartAccess.Droid.Services;
using SmartAccess.Droid.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAccess.ViewModels
{
    public class OpenRequestPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        ApiService _apiService;
        private ObservableCollection<OpenRequest> _requestSource;
        private bool _isRunning;
        private DelegateCommand _aceptarCommad;
        private DelegateCommand _cancelarCommand;
        private OpenRequest _selectItem;
        private DelegateCommand _navigateToDetail;

        public OpenRequestPageViewModel(INavigationService navigationService)
        : base(navigationService)
        {
            _apiService = new ApiService();
            _navigationService = navigationService;
            _requestSource = new ObservableCollection<OpenRequest>();
        }

        public DelegateCommand NavigateToDetail => _navigateToDetail ?? (_navigateToDetail = new DelegateCommand(OpenDetail));
        public ObservableCollection<OpenRequest> RequestSource
        {
            get => _requestSource;
            set => SetProperty(ref _requestSource, value);
        }

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

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await GetOpenRequest();
        }

        private async Task GetOpenRequest()
        {
            IsRunning = true;
            RequestSource.Clear();

            var url = Prism.PrismApplicationBase.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.GetOpenRequest(url, "/api", "/OpenRequest/GetOpenRequest");
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


            var list = response.Result.ToList().Where(x => x.Status == "P");

            list.ToList().ForEach(x => RequestSource.Add(x));


            IsRunning = false;

        }


        private async void OpenDetail()
        {

            var parameter = new NavigationParameters();
            parameter.Add("Request", RequestItem);

            await _navigationService.NavigateAsync("DetailOpenRequestPage", parameter);

            //RequestSource.Remove(RequestSource.Where(i => i.Id == RequestItem.Id).FirstOrDefault());
          
        }

    }
}
