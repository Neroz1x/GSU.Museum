using GSU.Museum.Shared.Interfaces;
using GSU.Museum.Shared.ViewModels;
using PanCardView.Extensions;
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
            if(HallsCollectionView.SelectedItem == null)
            {
                var viewModel = BindingContext as MainPageViewModel;
                await viewModel.GetHalls();
            }
            else
            {
                HallsCollectionView.SelectedItem = null;
            }
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            var viewModel = BindingContext as MainPageViewModel;
            viewModel.Cancel();
        }
    }
}
