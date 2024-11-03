using System;
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
            MasterListView.ItemsSource = Data.MasterPolEntities.GetContext().Partners_import.ToList();
        }       
    }
}
