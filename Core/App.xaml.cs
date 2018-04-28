using Xamarin.Forms;
using MomitKiller.Views;

namespace MomitKiller
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            /* 
             * If you create Settings.Development.cs Settings partial class 
             * like:
             * 
                public partial class Settings
                {
                    public Settings()
                    {
                        BaseUrl = "http://yourdevelopmentapi.url";
                        ApiKey = "YourDevelopmentApiKey";
                    }
                }
                
             * it will override Settings.cs settings
             */
            var dummy = new Settings();

            MainPage = new ThermostatView();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
