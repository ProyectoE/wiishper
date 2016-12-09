﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Prototipo
{
    class RestService : IRestService
    {
        HttpClient client;

        dynamic sendContent;
        public static string userpass { get; set; }
        public List<User> Friends { get; private set; }
        public static List<Product> Products { get; private set; }
        public static User LoggedUser { get; set; }

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "82991d5c121abd6985314d7df5708260");
            Products = new List<Product>();
            Products.Add(new Product() { name = "producto 1", image = "profilepic.png" });
            sendContent = new JObject();
            sendContent.data = new JObject();
            sendContent.control = new JObject();
        }

        public async Task<List<User>> GetFriends()
        {
            Friends = new List<User>();

            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            dynamic sendContent = CreateMessage(Constants.SHOW_FRIENDS, data);
            List<User> people = new List<User>();
            PrepareClient();

            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if(response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    Debug.WriteLine(result);
                    Friends = JsonConvert.DeserializeObject<List<User>>(sr.data.ToString());
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                return null;
            }

            return Friends;
        }

        public async Task<List<Product>> GetProducts()
        {
            Products = new List<Product>();

            var uri = new Uri(Constants.RestURL);
            dynamic sendContent = CreateMessage(Constants.SHOW_RANDOM_PRODS, null);
            var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
            PrepareClient();
            try
            {
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    Debug.WriteLine(result);
                    Debug.WriteLine("********************************------------------------------------****************************");
                    Products = JsonConvert.DeserializeObject<List<Product>>(sr.data.ToString());
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("********************************------------------------------------****************************");
                Debug.WriteLine(ex);
                return new List<Product>();
            }

            return Products;
        }

        public async Task<string> SignUp(User user)
        {
            var uri = new Uri(Constants.RestURL);
            var data = JsonConvert.SerializeObject(user);
            dynamic sendContent = CreateMessage(Constants.SIGNUP, data);
            sendContent.data = JsonConvert.SerializeObject(user);
            userpass = user.password;
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    //App.Database.SaveUser(user);
                    return result;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<string> Update(User user)
        {
            var uri = new Uri(Constants.RestURL);
            var data = JsonConvert.SerializeObject(user);
            dynamic sendContent = CreateMessage(Constants.UPDATE_USER, data);
            sendContent.data = JsonConvert.SerializeObject(user);
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    LoggedUser = user;
                    //App.Database.SaveUser(user);
                    return result;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            Friends = new List<User>();

            var uri = new Uri(Constants.RestURL);

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    Friends = JsonConvert.DeserializeObject<List<User>>(sr.data.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
            }

            return Friends;
        }

        public async Task<User> Login(string username, string password)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.password = password;
            data.email = username;
            dynamic sendContent = CreateMessage(Constants.LOGIN, data);
            userpass = password;
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if(response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    LoggedUser = JsonConvert.DeserializeObject<User>(sr.data.ToString());
                    LoggedUser.password = userpass;
                    App.Database.SaveUser(LoggedUser);
                    Helpers.Settings.GeneralSettings = "logged";
                    return LoggedUser;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
            }

            return LoggedUser;
        }

        public async Task<List<User>> ShowPeople()
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            dynamic sendContent = CreateMessage(Constants.SEARCH_PEOPLE, data);
            List<User> people =  new List<User>();
            PrepareClient();
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    people = JsonConvert.DeserializeObject<List<User>>(sr.data.ToString());
                    return people;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> IsFriend(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.friendee = id;
            dynamic sendContent = CreateMessage(Constants.IS_FRIEND, data);
            PrepareClient();
            bool answer;
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    answer = (bool)sr.data;
                    return answer;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<string> AddFriend(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.friendee = id;
            dynamic sendContent = CreateMessage(Constants.ADD_FRIEND, data);
            PrepareClient();
            string answer;
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    answer = sr.data.ToString();
                    return answer;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return "FAIL";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                return "FAIL";
            }
        }

        public async Task<string> Unfriend(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.friendee = id;
            dynamic sendContent = CreateMessage(Constants.UNFRIEND, data);
            PrepareClient();
            string answer;
            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    answer = sr.data.ToString();
                    return answer;
                }
                else
                {
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    Debug.WriteLine(result);
                    return "FAIL";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                return "FAIL";
            }
        }

        public async Task<string> LikeProduct(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.idproducts = id;
            data.liked = 1;
            dynamic sendContent = CreateMessage(Constants.LIKE_PROD, data);
            PrepareClient();

            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, content);
                byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Debug.WriteLine(result);
                if(response.IsSuccessStatusCode)
                {
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    return sr.data.ToString();
                }
                else
                {
                    return "FAIL";
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
                return "FAIL";
            }
        }

        public async Task<string> RejectProduct(int id)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            data.idproducts = id;
            data.liked = 0;
            dynamic sendContent = CreateMessage(Constants.REJECT_PROD, data);
            PrepareClient();

            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, content);
                byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Debug.WriteLine(result);
                if (response.IsSuccessStatusCode)
                {
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    return sr.data.ToString();
                }
                else
                {
                    return "FAIL";
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
                return "FAIL";
            }
        }


        public async Task<List<Product>> ShowLikedProducts(int userid)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            if (userid >= 0)
                data.userid = userid;
            dynamic sendContent = CreateMessage(Constants.SHOW_LIKED_PRODS , data);
            PrepareClient();

            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, content);
                byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Debug.WriteLine(result);

                if (response.IsSuccessStatusCode)
                {
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(sr.data.ToString());
                    return products;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
                return null;
            }
        }


        public async Task<List<Product>> ShowRejectedProducts(int userid)
        {
            var uri = new Uri(Constants.RestURL);
            dynamic data = new JObject();
            if (userid >= 0)
                data.userid = userid;
            dynamic sendContent = CreateMessage(Constants.SHOW_REJECTED_PRODS , data);
            PrepareClient();

            try
            {
                var content = new StringContent(sendContent.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, content);
                byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                string result = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Debug.WriteLine(result);

                if (response.IsSuccessStatusCode)
                {
                    ServiceResult sr = JsonConvert.DeserializeObject<ServiceResult>(result);
                    List<Product> products = JsonConvert.DeserializeObject<List<Product>>(sr.data.ToString());
                    return products;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        private dynamic CreateMessage(int operation, dynamic data)
        {
            JObject message = new JObject();
            JObject control = new JObject();
            control.Add("operation", operation);
            message.Add("control", control);
            message.Add("data", data);

            return message;
        }

        private void PrepareClient()
        {
            if(LoggedUser != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", LoggedUser.apikey);
            else
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "11614066f1b101f695bf2479656da628");
        }

        public static bool IsUserLogged()
        {
            return LoggedUser != null;
        }
        
    }
}
