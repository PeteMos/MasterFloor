using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterPol.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        public string FlagAddorEdit = "default";
        public Data.PartnersImport _currentpartner = new Data.PartnersImport();
        public AddEditPage(Data.PartnersImport partner)
        {
            InitializeComponent();

            if (partner != null)
            {
                _currentpartner = partner;
                FlagAddorEdit = "edit";
            }
            else
            {
                FlagAddorEdit = "add";
            }
            DataContext = _currentpartner;

            Init();
        }
        public void Init()
        {
            try
            {
                TypeComboBox.ItemsSource = Data.MasterPolEntities.GetContext().TypeOfPartner.ToList();
                if (FlagAddorEdit == "add")
                {
                    IdTextBox.Visibility = Visibility.Hidden;
                    IdLabel.Visibility = Visibility.Hidden;

                    TypeComboBox.SelectedItem = null;
                    NameTextBox.Text = string.Empty;
                    RatingTextBox.Text = string.Empty;
                    AdressTextBox.Text = string.Empty;
                    FIOTextBox.Text = string.Empty;
                    PhoneTextBox.Text = string.Empty;
                    EmailTextBox.Text = string.Empty;
                    IdTextBox.Text = Data.MasterPolEntities.GetContext().ProductsImport.Max(d => d.Id + 1).ToString();
                }
                else if (FlagAddorEdit == "edit")
                {
                    IdTextBox.Visibility = Visibility.Hidden;
                    IdLabel.Visibility = Visibility.Hidden;

                    TypeComboBox.SelectedItem = null;
                    NameTextBox.Text = _currentpartner.PartnerName.Name.ToString();
                    RatingTextBox.Text = _currentpartner.Reiting.ToString();
                    AdressTextBox.Text = _currentpartner.Adress.ToString();
                    FIOTextBox.Text = _currentpartner.Directors.FIO.ToString();
                    PhoneTextBox.Text = _currentpartner.PhoneOfPartner.ToString();
                    EmailTextBox.Text = _currentpartner.EmailOfPartner.ToString();
                    IdTextBox.Text = Data.MasterPolEntities.GetContext().PartnersImport.Max(p => p.Id + 1).ToString();
                    TypeComboBox.SelectedItem = Data.MasterPolEntities.GetContext().TypeOfPartner.Where(d => d.Id == _currentpartner.Id).FirstOrDefault();
                }
            }
            catch (Exception)
            {

            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.ListViewPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrEmpty(NameTextBox.Text))
                {
                    errors.AppendLine("Заполните наименование!");
                }
                if (TypeComboBox.SelectedItem == null)
                {
                    errors.AppendLine("Выберите тип партнера!");
                }
                if (string.IsNullOrEmpty(RatingTextBox.Text))
                {
                    errors.AppendLine("Заполните рейтинг!");
                }

                if (string.IsNullOrEmpty(AdressTextBox.Text))
                {
                    errors.AppendLine("Заполните адрес!");
                }
                if (string.IsNullOrEmpty(FIOTextBox.Text))
                {
                    errors.AppendLine("Заполните ФИО!");
                }
                if (string.IsNullOrEmpty(PhoneTextBox.Text))
                {
                    errors.AppendLine("Заполните номер телефона!");
                }

                if (string.IsNullOrEmpty(EmailTextBox.Text))
                {
                    errors.AppendLine("Заполните Email!");
                }

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                var selectedCategory = TypeComboBox.SelectedItem as Data.TypeOfPartner;
                _currentpartner.Id = Data.MasterPolEntities.GetContext().TypeOfPartner.Where(p => p.Id == selectedCategory.Id).FirstOrDefault().Id;
                _currentpartner.Reiting = Convert.ToInt32(RatingTextBox.Text);
                _currentpartner.PhoneOfPartner = PhoneTextBox.Text;
                _currentpartner.EmailOfPartner = EmailTextBox.Text;


                var searchDirector = (from item in Data.MasterPolEntities.GetContext().Directors
                                      where item.FIO == FIOTextBox.Text
                                      select item).FirstOrDefault();
                if (searchDirector != null)
                {
                    _currentpartner.Id = searchDirector.Id;
                }
                else
                {
                    Data.Directors directors = new Data.Directors()
                    {
                        FIO = FIOTextBox.Text
                    };
                    Data.MasterPolEntities.GetContext().Directors.Add(directors);
                    Data.MasterPolEntities.GetContext().SaveChanges();
                    _currentpartner.Id = directors.Id;
                }

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                var searchPartnerName = (from item in Data.MasterPolEntities.GetContext().PartnerName
                                         where item.Name == NameTextBox.Text
                                         select item).FirstOrDefault();
                if (searchPartnerName != null)
                {
                    _currentpartner.Id = searchPartnerName.Id;
                }
                else
                {
                    Data.PartnerName partnerName = new Data.PartnerName()
                    {
                        Name = NameTextBox.Text
                    };
                    Data.MasterPolEntities.GetContext().PartnerName.Add(partnerName);
                    Data.MasterPolEntities.GetContext().SaveChanges();
                    _currentpartner.Id = partnerName.Id;
                }



                if (FlagAddorEdit == "add")
                {
                    Data.MasterPolEntities.GetContext().PartnersImport.Add(_currentpartner);
                    Data.MasterPolEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (FlagAddorEdit == "edit")
                {
                    Data.MasterPolEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно изменено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }


            catch (Exception)
            {

            }
        }
    }
}
