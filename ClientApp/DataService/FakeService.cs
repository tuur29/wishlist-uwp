﻿using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ClientApp.DataService {

    public class FakeService : IDataService {

        public static String Name = "Fake Data Service";
        public static String JWTToken = "";

        public bool IsLoggedIn() {
            return JWTToken != "";
        }

        public dynamic Login(string email, string password) {
            Debug.WriteLine("GET /login/ for JWT Token with email " + email);
            JWTToken = "temp";
            return JWTToken;
        }

        public dynamic Register(string email, string password) {
            Debug.WriteLine("GET /register/ for JWT Token with email " + email);
            JWTToken = "temp";
            return JWTToken;
        }

        public void Logout() {
            Debug.WriteLine("Logout");
            JWTToken = "";
        }

        public List<List> GetSubscribedLists() {
            Debug.WriteLine("GET for Subscribed Lists.");

            return new List<List>() {
                    new List() { Name="John Locke's Birthday Wishes", OwnerUserId="John Locke's Mama"},
                    new List() { Name="Jessica's Maternity List", OwnerUserId="Jessica" },
                    new List() { Name="Babyborrel van den Sep De Laet", OwnerUserId="Senne De Laet"}
                };
        }

        public List<List> GetOwnedLists() {
            Debug.WriteLine("GET for Owned Lists.");

            return new List<List>() {
                    new List() { Name="Tutti and Frutti Baby Shower", OwnerUserId="Desmond Tutu", Color=Color.FromArgb(255, 247, 34, 176) },
                    new List() { Name="Lala in Nanaland", OwnerUserId="Zazu"}
                };
        }

        // probably not gonna work? We called observable in een observable maar let's see I guess
        public List<Item> GetListItems(List list) {
            Debug.WriteLine("GET items for list with name " + list.Name);

            var items = new List<Item>() {
                new Item() {ProductName = "Baby Wipes", List = list},
                new Item() {ProductName = "Baby shampoo", List = list, CheckedByUserId = "temp"},
                new Item() {ProductName = "Packet of Cigarettes", List = list, ItemPriceUsd = 5.55}
            };

            if (list.ListId == 0 || list.ListId == 4)
                items.Add(new Item() { ProductName = "Pacifiers", List = list });

            return items;
        }

        public void Write(List list) {
            Debug.WriteLine("POST List with name " + list.Name);
        }
        
        public void Delete(List list) {
            Debug.WriteLine("POST List with name DELETE" + list.Name);
        }

        private List<Item> GenerateSomeItems(List list) {
            List<Item> randItems = new List<Item>();
            Random rnd = new Random();

            string[] items = new string[] { "toys", "window wipers", "lullaby", "flashlight", "dentures", "visual studio licence", "box cutters", "tissues" };

            for (int i = 0; i < rnd.Next(2, 6); i++) {
                randItems.Add(new Item() { ProductName = items[rnd.Next(0, 4)], List = list });
            }

            return randItems;
        }

    }
}
