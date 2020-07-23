using GSU.Museum.Shared.Services;
using GSU.Museum.Shared.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace GSU.Museum
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(Navigation);
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            if(App.Settings == null)
            {
                App.Settings = await DependencyService.Get<CachingService>().ReadSettings();
                DependencyService.Get<LocalizationService>().Localize();
            }
            var viewModel = BindingContext as MainPageViewModel;
            await viewModel.GetHalls();
        }
    }
}
