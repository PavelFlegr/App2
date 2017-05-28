using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyLocations : ContentPage
    {
        public MyLocations()
        {
            
            InitializeComponent();
            LoadLocations();
        }

        void LoadLocations()
        {
            var locList = new ObservableCollection<LocationListItemVM>();
            foreach(Location loc in LocationDB.GetLocationList())
            {
                LocationListItemVM locVM = new LocationListItemVM(loc);
                locList.Add(locVM);
                locVM.Removed += (o, e) => locList.Remove(locVM);
            }
            locations.ItemsSource = locList;
        }

        private void Map_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new Map(), this);
            Navigation.PopAsync();
        }

        private void locations_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.InsertPageBefore(new Map(((LocationListItemVM)e.Item).location), this);
            Navigation.PopAsync();
        }
    }
}
