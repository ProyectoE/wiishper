﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Plugin.Toasts;

namespace Prototipo
{
    public partial class NotificationsPage : ContentPage
    {

        IToastNotificator notificator;
        public NotificationsPage()
        {
            InitializeComponent();
            notificator = DependencyService.Get<IToastNotificator>();
            NavigationPage.SetHasNavigationBar(this, false);
            notifications.ItemsSource = new List<Notification>() {
                new Notification() { subject="Juan", action="ingresó a wiishper", timestamp= new DateTime(2016, 11, 15) },
                new Notification() {subject="Andrés Felipe", action="agregó 5 productos a su wiishlist", timestamp=new DateTime(2016, 11, 16) } };
        }

        private async void OnNewsfeed(object sender, EventArgs e)
        {
            
        }

        private async void OnFriends(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new FriendsPage(), this);
            }
            await Navigation.PopAsync();
        }

        private async void OnProducts(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new ProductsPage(), this);
            await Navigation.PopAsync();
        }

        private async void OnActivity(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new ActivityPage(), this);
            }
            await Navigation.PopAsync();
        }

        private async void OnProfile(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new ProfilePage(RestService.LoggedUser), this);
            }
            await Navigation.PopAsync();
        }
    }

}
