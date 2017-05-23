using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using PCLStorage;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyLocations : ContentPage
    {
        SQLiteConnection conn;
        public MyLocations()
        {
            conn = new SQLiteConnection(FileSystem.Current.LocalStorage.Path + "/db");
            InitializeComponent();
            LoadLocations();
        }

        void LoadLocations()
        {
            Helloo.ItemsSource = conn.Table<Location>().ToList();
        }

        private void Map_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new Map(), Navigation.NavigationStack[0]);
            Navigation.PopAsync();
        }
    }
}
