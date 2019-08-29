﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using AppUIBasics.Data;
using System.Linq;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppUIBasics
{
    public sealed partial class NewControlsPage : ItemsPageBase
    {
        public NewControlsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var menuItem = NavigationRootPage.Current.NavigationView.MenuItems.Cast<Microsoft.UI.Xaml.Controls.NavigationViewItem>().First();
            menuItem.IsSelected = true;
            NavigationRootPage.Current.NavigationView.Header = menuItem.Content;
            Items = ControlInfoDataSource.Instance.Groups.SelectMany(g => g.Items.Where(i => i.BadgeString != null)).OrderBy(i => i.Title).ToList();
            itemsCVS.Source = FormatData();
        }

        private ObservableCollection<GroupInfoList> FormatData()
        {
            var query = from item in this.Items
                        group item by item.BadgeString into g
                        orderby g.Key
                        select new GroupInfoList(g) { Key = g.Key };

            ObservableCollection<GroupInfoList> groupList = new ObservableCollection<GroupInfoList>(query);

            foreach (var item in groupList)
            {
                switch (item.Key.ToString())
                {
                    case "New":
                        item.Title = "Recently Added Samples";
                        break;
                    case "Updated":
                        item.Title = "Recently Updated Samples";
                        break;
                    case "Preview":
                        item.Title = "Preview Samples";
                        break;
                }
            }

            return groupList;
        }

        protected override bool GetIsNarrowLayoutState()
        {
            return LayoutVisualStates.CurrentState == NarrowLayout;
        }
    }

    public class GroupInfoList : List<object>
    {
        public GroupInfoList(IEnumerable<object> items) : base(items)
        {
        }
        public object Key { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
            }
        }
    }
}
 