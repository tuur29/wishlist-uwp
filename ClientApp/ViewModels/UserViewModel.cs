﻿using ClientApp.Models;
using ClientApp.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels {
    public class UserViewModel : NotificationBase {

        User user;

        public UserViewModel() {

            user = new User();
            _SelectedSubscriptionIndex = -1;
            _SelectedOwnedIndex = -1;

            foreach (var list in user.SubscribedLists) {
                var nl = ListViewModel.FromList(list);
                nl.PropertyChanged += List_OnNotifyPropertyChanged;
                _Subscriptions.Add(nl);
            }

            foreach (var list in user.OwningLists) {
                var nl = ListViewModel.FromList(list);
                nl.PropertyChanged += List_OnNotifyPropertyChanged;
                _Owned.Add(nl);
            }

        }

        // Subscriptions

        List<ListViewModel> _Subscriptions = new List<ListViewModel>();
        public List<ListViewModel> Subscriptions {
            get { return _Subscriptions; }
            set { SetProperty(ref _Subscriptions, value); }
        }
        
        public ListViewModel SelectedSubscription {
            get { return (_SelectedSubscriptionIndex >= 0) ? _Subscriptions[_SelectedSubscriptionIndex] : null; }
        }

        // used explicitly by ListDetailPage to grab content for a specific list
        public List GetSubscriptionById(int id) {
            foreach (var sub in _Subscriptions) {
                if (sub.ListId == id) { return sub; }
            }
            throw new IndexOutOfRangeException();
        }

        // _SelectedIndex is used internally when navigating into lists in the details view, when navigating back
        // This index remembers where the user left off and helps them scroll through all subscribed lists to where they were
        int _SelectedSubscriptionIndex;

        public int SelectedSubscriptionIndex {
            get { return _SelectedSubscriptionIndex; }
            set {
                if (SetProperty(ref _SelectedSubscriptionIndex, value)) { RaisePropertyChanged(nameof(SelectedSubscription)); }
            }
        }

        public void AddSubscription() {
            var list = new ListViewModel();
            list.PropertyChanged += List_OnNotifyPropertyChanged;
            Subscriptions.Add(list);
            user.RegisterSubscription(list);
            SelectedSubscriptionIndex = Subscriptions.IndexOf(list);
        }

        public void RemoveSubscription() {
            if (SelectedSubscriptionIndex != -1) {
                var sub = Subscriptions[SelectedSubscriptionIndex];
                Subscriptions.RemoveAt(SelectedSubscriptionIndex);
                user.RemoveSubscription(sub);
            }
        }

        // Owned lists

        List<ListViewModel> _Owned = new List<ListViewModel>();
        public List<ListViewModel> Owned {
            get { return _Owned; }
            set { SetProperty(ref _Owned, value); }
        }

        public ListViewModel SelectedOwned {
            get { return (_SelectedOwnedIndex >= 0) ? _Owned[_SelectedOwnedIndex] : null; }
        }

        // used explicitly by ListDetailPage to grab content for a specific list
        public List GetOwnedById(int id) {
            foreach (var list in _Owned) {
                if (list.ListId == id) { return list; }
            }
            throw new IndexOutOfRangeException();
        }

        // _SelectedIndex is used internally when navigating into lists in the details view, when navigating back
        // This index remembers where the user left off and helps them scroll through all subscribed lists to where they were
        int _SelectedOwnedIndex;

        public int SelectedOwnedIndex {
            get { return _SelectedOwnedIndex; }
            set {
                if (SetProperty(ref _SelectedOwnedIndex, value)) { RaisePropertyChanged(nameof(SelectedOwned)); }
            }
        }

        public void AddOwned() {
            var list = new ListViewModel();
            list.PropertyChanged += List_OnNotifyPropertyChanged;
            Owned.Add(list);
            user.RegisterOwned(list);
            SelectedOwnedIndex = Owned.IndexOf(list);
        }

        public void RemoveOwned() {
            if (SelectedOwnedIndex != -1) {
                var list = Owned[SelectedOwnedIndex];
                Owned.RemoveAt(SelectedOwnedIndex);
                user.RemoveOwned(list);
            }
        }

        // Other

        void List_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e) {
            user.Update((ListViewModel)sender);

        }

    }
}
