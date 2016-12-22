﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Xamarin.Forms;

namespace Prototipo
{
    public class App : Application
    {
        public static RESTManager Manager { get; private set; }

        static UserDatabase database;
        public static bool IsLoggedIn { get; set; }
        public static string ApiKey { get; set; }
        public static bool IsDebuging { get; set; }

        public static UserDatabase Database
        {
            get
            {
                if (database == null) database = new UserDatabase();
                return database;
            }
        }
        public App()
        {
            Manager = new RESTManager(new RestService());
            IsDebuging = true;

            if (IsDebuging)
            {
                MainPage = new NavigationPage(new ProductsPage());
            }
            else
            {
                bool firstTime = string.IsNullOrEmpty(Helpers.Settings.GeneralSettings);
                if (firstTime)
                {
                    Helpers.Settings.GeneralSettings = "entered";
                    MainPage = new NavigationPage(new TutorialPage());
                }
                else if (Helpers.Settings.GeneralSettings.Equals("logged"))
                {
                    RestService.LoggedUser = Database.GetUsers().First();
                    Manager.Login(RestService.LoggedUser.email, RestService.LoggedUser.password);
                    MainPage = new NavigationPage(new ProfilePage(RestService.LoggedUser));
                }
                else
                {
                    if (Helpers.Settings.GeneralSettings.Equals("used"))
                        MainPage = new NavigationPage(new ProductsPage());
                    else
                    {
                        Helpers.Settings.GeneralSettings = "used";
                        MainPage = new NavigationPage(new StartPage());
                    }
                }
            }
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
