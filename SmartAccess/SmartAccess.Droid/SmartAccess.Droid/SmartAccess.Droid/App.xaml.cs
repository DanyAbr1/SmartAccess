using Prism;
using Prism.Ioc;
using SmartAccess.Droid.ViewModels;
using SmartAccess.Droid.Views;
using Xamarin.Essentials;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace SmartAccess.Droid
{
    public partial class App
    {

        public static string AzureBackendUrl =
           DeviceInfo.Platform == DevicePlatform.Android ? "https://smartaccessapi.azurewebsites.net" : "http://localhost:5000";
        public static bool UseMockDataStore = false;
        

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<MenuPage, MenuPageViewModel>();
            containerRegistry.RegisterForNavigation<GatewayPage, GatewayPageViewModel>();
            containerRegistry.RegisterForNavigation<ChatPage, ChatPageViewModel>();
            containerRegistry.RegisterForNavigation<NewUserPage, NewUserPageViewModel>();
            containerRegistry.RegisterForNavigation<ResetPasswordPage, ResetPasswordPageViewModel>();
        }
    }
}
