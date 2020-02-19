using System;
using System.Collections.Generic;
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
        long _FTEID = 0;
        List<Locations> _list;
        public EntryPage(long FTEID)
        {
            InitializeComponent();
            _FTEID = FTEID;
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

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {

        }
    }
}