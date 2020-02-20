using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloblaLibraryCOVID19;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COVID19Application
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage
    {
        FTEs _FTE = new FTEs();
        List<Locations> _list;
        public EntryPage(FTEs FTE)
        {
            InitializeComponent();
            _FTE = FTE;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulatePickerLevels();
        }
        private async void PopulatePickerLevels()
        {
            var webApi = new API();
            _list = await webApi.GetLocationsAsync("locations");
            foreach (var item in _list.Select(x => x.LocationFloor).Distinct())
            {
                pLocationLevel.Items.Add(item.ToString());
            }
            
        }
        private void PopulatePickerName()
        {
            foreach (var item in _list.Where(x => x.LocationFloor == Byte.Parse(pLocationLevel.SelectedItem.ToString())))
            {
                pLocation.Items.Add(item.LocationName);
            }
        }

        private void pLocationLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            pLocation.Items.Clear();
            PopulatePickerName();
        }

        private async void btnSubmit_Clicked(object sender, EventArgs e)
        {
            var webApi = new API();
            var dateTime = (DPdate.Date + TPtime.Time).ToString("dd/MM/yyyy hh:mm:ss tt");
            var contactTracing = new ContactTracing()
            {
                RegisterDateTime = DateTime.ParseExact($"{dateTime}", "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                LocationID = _list.Where(x => x.LocationName == pLocation.SelectedItem.ToString()).Select(x => x.ID).FirstOrDefault(),
                Contact = _FTE.Contact,
                FTE_ID = _FTE.ID,
                FullName = _FTE.FullName,
                Email = _FTE.Email,
                Temp = Decimal.Parse(entryTemperature.Text)
            };
            var response = await webApi.PostAsync(contactTracing, "contacttracings/create");
            await DisplayAlert("Submit", $"{response}", "Ok");
            if (response == "Records saved!")
            {
                ClearFields();
            }
        }
        private void ClearFields()
        {
            pLocation.SelectedItem = null;
            pLocationLevel.SelectedItem = null;
            entryTemperature.Text = string.Empty;
        }
    }
}