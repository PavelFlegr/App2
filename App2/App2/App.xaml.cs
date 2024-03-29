﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using SQLite;

namespace App2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Page page;
            LocationDB.Init();
            if (LocationDB.GetLocations().Count > 0)
            {
                page = new App2.MyLocations();
            }
            else
            {
                page = new App2.Map();
            }

            MainPage = new NavigationPage(page);
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
