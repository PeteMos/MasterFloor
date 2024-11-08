﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MasterPol.Pages
{
    public partial class ListViewPage : Page
    {
        public ListViewPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var partners = Data.MasterPolEntities.GetContext().PartnersImport.ToList();

            var partnerDiscounts = (from partner in partners
                                    join product in Data.MasterPolEntities.GetContext().PartnerProductsImport
                                    on partner.Id equals product.IdPartnerName
                                    group product by partner into g
                                    select new
                                    {
                                        Partner = g.Key,
                                        Discount = CalculateDiscount(g.Sum(p => p.CountOfProduction))
                                    }).ToList();

            MasterListView.ItemsSource = partnerDiscounts;
        }

        private int CalculateDiscount(int totalCount)
        {
            if (totalCount < 10000) return 0;
            if (totalCount >= 10000 && totalCount < 50000) return 5;
            if (totalCount >= 50000 && totalCount < 300000) return 10;
            return 15;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.AddEditPage(null));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.AddEditPage(null));
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.HistoryPage());
        }
    }
}
