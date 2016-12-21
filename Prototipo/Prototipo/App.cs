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

        private void LoadUsers()
        {
            Random r = new Random();
            for(int i=0; i<10; i++)
            {
                Taste t = new Taste { idproducts = r.Next(10000), inter_date=new DateTime(), liked= r.Next(1) == 0 };
                Database.SaveProduct(t);
            }
            //string[] names = { "Andrés", "Felipe", "Cristian", "Mónica", "Juan", "Renzo", "Jonathan", "Miguel", "Jeny", "Ginna" };
            //string[] surnames = { "Mejía", "Perry", "Hincapié", "Saravia", "Sesana", "Álvarez", "Acosta", "Beltrán", "Barrera" };
            //Random r = new Random();
            //for(int i=0; i<15; i++)
            //{
            //    string name = names[r.Next(0, names.Length)];
            //    string surname = surnames[r.Next(0, surnames.Length)];
            //    User user = new User()
            //    {
            //        name = name,
            //        surname = surname,
            //        username = name + "_" + surname,
            //        email = name.Substring(0, 1) + "." + surname + "@" + "mill.com.co",
            //        birthdate = new DateTime(r.Next(1977, 1996), r.Next(1, 13), r.Next(1, 29)),
            //        entrydate = new DateTime(r.Next(2015, 2017), r.Next(1, 13), r.Next(1, 29))
            //    };
            //    Database.SaveUser(user);
            //    Debug.WriteLine(user);
            //}      
        }

        private void UpdateUsers()
        {
            List<User> users = Database.GetUsers().ToList();
            foreach(User user in users)
            {
                user.profilepic = "profilepic.png";
                Database.SaveUser(user);
            }
        }
    }
}
