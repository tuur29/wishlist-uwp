﻿using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Newtonsoft.Json;
using HttpClient = System.Net.Http.HttpClient;

namespace ClientApp.DataService {
    public class RealService {
        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static String Name = "Real Data Service";

        public static String JWTToken = localSettings.Values.ContainsKey("JWTToken") ? localSettings.Values["JWTToken"].ToString() : "";

        // ACCOUNT

        public static bool validJWT(string obj) {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(obj) as JwtSecurityToken;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool IsLoggedIn {
            get { return JWTToken != ""; }
        }

        public static dynamic Login(string email, string password) {
            Debug.WriteLine("GET /login/ for JWT Token with email " + email);

            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("Password", password),
            });

            var response = "";
            Task task = Task.Run(async () => {
                var res = await httpClient.PostAsync(new Uri(App.BaseUri + "Account/Login"), content);
                response = await res.Content.ReadAsStringAsync();
            });
            task.Wait();

            var obj = JsonConvert.DeserializeObject<object>(response);
            if (validJWT(obj.ToString()))
                localSettings.Values["JWTToken"] = obj.ToString();
            
            return obj;
        }

        public static dynamic Register(string email, string password) {
            Debug.WriteLine("GET /register/ for JWT Token with email "+ email);

            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("Password", password),
            });

            var response = "";
            Task task = Task.Run(async () => {
                var res = await httpClient.PostAsync(new Uri(App.BaseUri + "Account/Register"), content);
                response = await res.Content.ReadAsStringAsync();
            });
            task.Wait();

            var obj = JsonConvert.DeserializeObject<object>(response);
            Debug.WriteLine(obj);
            return obj;
        }

        public static void Logout() {
            Debug.WriteLine("Logout");
            JWTToken = "";
        }

        // LISTS

        public static List<List> GetSubscribedLists() {
            Debug.WriteLine("GET for Subscribed Lists.");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            var response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(App.BaseUri + "Users/Subscriptions")); // sends GET request
            });
            task.Wait();
            // TODO: convert int representation of color to Color
            return JsonConvert.DeserializeObject<List<List>>(response);
        }

        public static List<List> GetOwnedLists() {
            Debug.WriteLine("GET for Owned Lists.");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            var response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(App.BaseUri + "Users/Lists")); // sends GET request
            });
            task.Wait();
            // TODO: convert int representation of color to Color
            return JsonConvert.DeserializeObject<List<List>>(response);
        }

        // probably not gonna work? We called observable in een observable maar let's see I guess
        public static List<Item> GetListItems(List list) {
            Debug.WriteLine("GET items for list with name " + list.Name);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            string response = "";
            Task task = Task.Run(async () => {
                response = await httpClient.GetStringAsync(new Uri(App.BaseUri + "Lists/"+list.ListId)); // sends GET request
            });
            task.Wait();
            Debug.WriteLine("resp: "+response);
            return JsonConvert.DeserializeObject<List>(response).Items;
        }

        public static void Write(List list) {
            Debug.WriteLine("POST List with name " + list.Name);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);

            StringContent content = new StringContent(
                JsonConvert.SerializeObject(list),
                System.Text.Encoding.UTF8
            );
            httpClient.PostAsync(new Uri(App.BaseUri + "Lists/"+list.ListId), content);
        }
        
        public static void Delete(List list) {
            Debug.WriteLine("DELETE List with name " + list.Name);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWTToken);
            httpClient.DeleteAsync(new Uri(App.BaseUri + "Lists/" + list.ListId));
        }

    }
}
