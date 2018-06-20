﻿using ClientApp.Models;
using ClientApp.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClientApp.DataService {

    public class FakeService : IDataService {

        public static readonly string Name = "Fake Data Service";
        private string JWTToken = "skippinglogin";

        // ACCOUNT

        public bool IsLoggedIn() {
            return JWTToken != "";
        }        

        public dynamic Login(LoginViewModel vm) {
            JWTToken = "temp";
            return JObject.FromObject( new { success = true, data = new {user = GetCurrentUser(), token = JWTToken} });
        }

        public dynamic Register(RegisterViewModel vm) {
            JWTToken = "temp";
            return JObject.FromObject(new { success = true, data = new { user = GetCurrentUser(), token = JWTToken } });
        }

        public void Logout() {
            Debug.WriteLine("Logout");
            JWTToken = "";
        }

        public dynamic ChangePassword(ChangePasswordViewModel vm) {
            return JObject.FromObject(new { success = true });
        }

        public dynamic EditAccount(EditAccountViewModel vm) {
            return JObject.FromObject(new { success = true });
        }

        public dynamic ForgotPassword(ForgotPasswordViewModel vm) {
            return JObject.FromObject(new { success = true });
        }

        public dynamic ResetPassword(ResetPasswordViewModel vm) {
            return JObject.FromObject(new { success = true });
        }

        // USER

        public User GetCurrentUser() {
            return new User() {
                FirstName = "Peter",
                LastName = "Petersson",
                Email = "peter@domain.com",
                Id = "4875185"
            };
        }

        public User GetUser(string id) {
            return new User() {
                FirstName = "Jan",
                LastName = "Janssens",
                Email = "jan@domain.com",
                Id = "54674654"
            };
        }

        public List<List> GetOwnedLists() {
            return new List<List> {
                new List { ListId=0, Name="Verjaardag Jan", OwnerUser=GetCurrentUser(), Description="Voor op het feestje af te geven", Deadline=new DateTime(2018,12,31) },
                new List { ListId=1, Name="Babyborrel Charlotte", OwnerUser=GetCurrentUser(), Description="Een jonge spruit!", Deadline=new DateTime(2019,05,12) },
                new List { ListId=2, Name="Trouw", OwnerUser=GetCurrentUser(), Description="Al gepasseerd eigenlijk", Deadline=new DateTime(2018,01,01) }
            };
        }

        public List<List> GetSubscribedLists() {
            return new List<List> {
                new List { ListId=0, Name="Verjaardag Jan", OwnerUser=GetUser(""), Description="Voor op het feestje af te geven", Deadline=new DateTime(2018,12,31) },
                new List { ListId=1, Name="Babyborrel Charlotte", OwnerUser=GetUser(""), Description="Een jonge spruit!", Deadline=new DateTime(2019,05,12) },
                new List { ListId=2, Name="Trouw", OwnerUser=GetUser(""), Description="Al gepasseerd eigenlijk", Deadline=new DateTime(2018,01,01) }
            };
        }

        public List GetList(int id) {
                List list = new List { ListId = 0, Name = "Verjaardag Jan", OwnerUser = GetUser(""), Description = "Voor op het feestje af te geven", Deadline = new DateTime(2018, 12, 31) };
            return list;
        }

        public dynamic NewList(List list) {
            return JObject.FromObject(new { success = true });
        }

        public List<Item> GetListItems(List list) {
            return new List<Item> {
                new Item { ItemId=0, ProductName="Playstation",  CheckedByUser=GetCurrentUser() },
                new Item { ItemId=1, ProductName="Tent", ItemPriceUsd=19.99, CheckedByUser=GetUser("") },
                new Item { ItemId=2, ProductName="Ovenschotel", ItemPriceUsd=9.99 },
                new Item { ItemId=3, ProductName="Barbies" }
            };
        }

        public dynamic EditList(List list) {
            return JObject.FromObject(new { success = true });
        }

        public void SendInvitations(List list) {
        }

        public void DeleteList(List list) {
        }

        public void MarkItem(Item item) {
        }

        public void UnMarkItem(Item item) {
        }

        public dynamic NewItem(Item item) {
            return JObject.FromObject(new { success = true });
        }

        public dynamic EditItem(Item item) {
            return JObject.FromObject(new { success = true });
        }

        public void DeleteItem(Item item) {
        }

        public List<Notification> GetNotifications() {
            return new List<Notification> {
                new Notification() { OwnerUser = GetCurrentUser(), Type = NotificationType.DeadlineReminder, SubjectList = GetList(1), NotificationId = 0, IsUnread=true },
                new Notification() { OwnerUser = GetCurrentUser(), Type = NotificationType.JoinRequest, SubjectUser = GetUser(""), NotificationId = 1, IsUnread=true },
                new Notification() { OwnerUser = GetCurrentUser(), Type = NotificationType.ListInvitation, SubjectList = GetList(2), NotificationId = 2, IsUnread=false },
                new Notification() { OwnerUser = GetCurrentUser(), Type = NotificationType.ListInvitation, SubjectList = GetList(1), NotificationId = 3, IsUnread=true },
                new Notification() { OwnerUser = GetCurrentUser(), Type = NotificationType.ListInvitation, SubjectList = GetList(0), NotificationId = 4, IsUnread=false }
            };
        }

        public void MarkAllNotificationsAsRead() {
        }

        public void MarkNotificationAsRead(Notification notification) {
        }

    }
}
