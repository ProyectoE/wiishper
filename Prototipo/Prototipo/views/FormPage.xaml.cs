﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Plugin.Toasts;

namespace Prototipo
{
    public partial class FormPage : ContentPage
    {
        private User user;
        private IToastNotificator notificator;
        public FormPage(User user = null)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            notificator = DependencyService.Get<IToastNotificator>();
            if (user == null)
                user = new User();
            this.BindingContext = user;
        }

        private async void OnSave(object sender, EventArgs e)
        {
            user = (User)BindingContext;
         
            var response = await App.Manager.SignUp(user);

            if (response != null)
            {
                notificator.Notify(ToastNotificationType.Success, "Wiishper", "Bienvenido a wiishper", TimeSpan.FromSeconds(2));
                RestService.LoggedUser = user;
                await Navigation.PushAsync(new ProductsPage());
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ooops, ocurrió un error en el registro", TimeSpan.FromSeconds(2));
            }
        }

        private async void OnDismiss(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void OnGenderChanged(object sender, EventArgs e)
        {
            user = (User)BindingContext;
            user.gender = ((Switch)sender).IsToggled ? "F" : "M";
        }
    }
}
