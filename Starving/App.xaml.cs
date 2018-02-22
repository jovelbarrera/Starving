using Com.OneSignal;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Starving.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Starving
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");

            OneSignal.Current.StartInit(Application.Current.Resources["OneSignalAppId"].ToString()).EndInit();
        }

        protected override void OnStart()
        {
            base.OnStart();
#if !DEBUG
            AppCenter.Start("android=582d197b-91c6-480b-8c87-b215c66ee45c;" +
                            "ios=fc65c9bc-2512-4102-a3d5-c8389c860e8f;",
                   typeof(Analytics), typeof(Crashes));
#endif
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<PlaceDetailPage>();
        }
    }
}
