using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GloblaLibraryCOVID19;

namespace COVID19Application
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        FTEs _FTE = new FTEs();
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            entryUserID.Text = string.Empty;
            entryPassword.Text = string.Empty;
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            var webApi = new API();
            _FTE = await webApi.Login(entryUserID.Text, entryPassword.Text);
            if (_FTE == null)
            {
                await DisplayAlert("Login", "User credentials are invalid!", "Ok");
            }
            else
            {
                await DisplayAlert("Login", $"Welcome {_FTE.FullName}!", "Ok");
                await Navigation.PushAsync(new EntryPage(_FTE));
            }
        }
    }
}
