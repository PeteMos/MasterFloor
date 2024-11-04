using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MasterPol.Data;

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
            var partners = Data.MasterPolEntities.GetContext().Partners_import.ToList();

            var partnerDiscounts = from partner in partners
                                   join product in Data.MasterPolEntities.GetContext().Partner_products_import
                                   on partner.Id equals product.IdPartnerName
                                   group product by partner into g
                                   select new
                                   {
                                       Partner = g.Key,
                                       Discount = CalculateDiscount(g.Sum(p => p.CountOfProduction))
                                   };

            MasterListView.ItemsSource = partnerDiscounts.Select(pd => new
            {
                Partner = pd.Partner, // Передаем весь объект партнера
                pd.Discount
            }).ToList();
        }

        private int CalculateDiscount(decimal totalCount)
        {
            if (totalCount < 10000) return 0;
            if (totalCount >= 10000 && totalCount < 50000) return 5;
            if (totalCount >= 50000 && totalCount < 300000) return 10;
            return 15; // totalCount >= 300000
        }
    }
}
