﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Plugin.Toasts;

namespace Prototipo
{
    public partial class SignUp : ContentPage
    {
        IToastNotificator notificator;
        public SignUp()
        {
            InitializeComponent();
            if(Device.OS == TargetPlatform.iOS && Device.Idiom == TargetIdiom.Tablet)
            {
                ScrollView view = new ScrollView { Orientation = ScrollOrientation.Vertical };
                view.Content = MainLayout;
                Content = view;
            }
            NavigationPage.SetHasNavigationBar(this, false);
            notificator = DependencyService.Get<IToastNotificator>();
        }

        private async void OnSignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FormPage());
            
        }

        private async void OnLogin(object sender, EventArgs e)
        {
            User user = await App.Manager.Login(username.Text, password.Text);
            if (user != null)
            {
                List<Taste> tastes = App.Database.GetProducts().ToList();
                foreach(Taste t in tastes)
                {
                    if (t.liked)
                        App.Manager.LikeProduct(t.idproducts);
                    else
                        App.Manager.RejectProduct(t.idproducts);
                }
                App.Database.CleanProducts();
                Navigation.InsertPageBefore(new ProfilePage(user), this);
                await Navigation.PopAsync();
            }
            else
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Correo o contraseña incorrectos", TimeSpan.FromSeconds(2));
            }
        }

        private async void OnFbSignUp(object sender, EventArgs e)
        {
            await notificator.Notify(ToastNotificationType.Info, "Wiishper", "Estamos trabajando en esta funcionalidad", TimeSpan.FromSeconds(2));
        }

        private async void OnTwitterSignUp(object sender, EventArgs e)
        {
            await notificator.Notify(ToastNotificationType.Info, "Wiishper", "Estamos trabajando en esta funcionalidad", TimeSpan.FromSeconds(2));
        }

        private async void OnExit(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new ProductsPage(), this);
            await Navigation.PopAsync();
        }

        private async void OnGoogleSignUp(object sender, EventArgs e)
        {
            await notificator.Notify(ToastNotificationType.Info, "Wiishper", "Estamos trabajando en esta funcionalidad", TimeSpan.FromSeconds(2));
        }
    }
}
