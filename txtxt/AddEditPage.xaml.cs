using MasterPol.Data;
using System;
using System.Collections.Generic;
using System.IO;
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
        private PartnersImport _currentpartner;
        private bool FlagAddorEdit;

        public AddEditPage(PartnersImport partner)
        {
            InitializeComponent();
            _currentpartner = partner;
            FlagAddorEdit = _currentpartner == null;
            Init();
        }

        public void Init()
        {
            IdTextBox.Visibility = Visibility.Hidden;
            IdLabel.Visibility = Visibility.Hidden;
            TypeComboBox.ItemsSource = Data.MasterFloorEntities.GetContext().TypeOfPartners.ToList();
            if (_currentpartner != null)
            {
                NameTextBox.Text = _currentpartner.PartnerName.PartnerName1;
                RatingTextBox.Text = _currentpartner.Reiting.ToString();
                IndexTextBox.Text = _currentpartner.Adress.Indexes.IndexOf.ToString();

                RegionTextBox.Text = _currentpartner.Adress.Regions.RegionOf;
                CityTextBox.Text = _currentpartner.Adress.Cities.CityOf;
                StreetTextBox.Text = _currentpartner.Adress.Streets.StreetOf; 

                HouseNumTextBox.Text = _currentpartner.Adress.HouseNum.ToString();
                FIOTextBox.Text = _currentpartner.Directors.FIO;
                PhoneTextBox.Text = _currentpartner.PhoneOfPartner;
                EmailTextBox.Text = _currentpartner.EmailOfPartner;
                IdTextBox.Text = _currentpartner.Id.ToString();

                TypeComboBox.SelectedItem = Data.MasterFloorEntities.GetContext().TypeOfPartners
                    .FirstOrDefault(d => d.Id == _currentpartner.Id);
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
                    errors.AppendLine("Заполните наименование!");
                if (TypeComboBox.SelectedItem == null)
                    errors.AppendLine("Выберите тип партнера!");
                if (string.IsNullOrEmpty(RatingTextBox.Text))
                    errors.AppendLine("Заполните рейтинг!");
                if (string.IsNullOrEmpty(IndexTextBox.Text))
                    errors.AppendLine("Заполните индекс!");
                if (string.IsNullOrEmpty(RegionTextBox.Text))
                    errors.AppendLine("Заполните регион!");
                if (string.IsNullOrEmpty(CityTextBox.Text))
                    errors.AppendLine("Заполните город!");
                if (string.IsNullOrEmpty(StreetTextBox.Text))
                    errors.AppendLine("Заполните улицу!");
                if (string.IsNullOrEmpty(HouseNumTextBox.Text))
                    errors.AppendLine("Заполните номер дома!");
                if (string.IsNullOrEmpty(FIOTextBox.Text))
                    errors.AppendLine("Заполните ФИО!");
                if (string.IsNullOrEmpty(PhoneTextBox.Text))
                    errors.AppendLine("Заполните номер телефона!");
                if (string.IsNullOrEmpty(EmailTextBox.Text))
                    errors.AppendLine("Заполните Email!");

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var selectedCategory = TypeComboBox.SelectedItem as Data.TypeOfPartners;
                if (selectedCategory == null)
                {
                    MessageBox.Show("Выберите тип партнера!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!FlagAddorEdit) // Если мы редактируем существующего партнера
                {
                    _currentpartner.PartnerName.PartnerName1 = NameTextBox.Text;
                    _currentpartner.Reiting = Convert.ToInt32(RatingTextBox.Text);
                    _currentpartner.PhoneOfPartner = PhoneTextBox.Text;
                    _currentpartner.EmailOfPartner = EmailTextBox.Text;
                    _currentpartner.Directors.FIO = FIOTextBox.Text;

                    // Обновляем индекс
                    _currentpartner.Adress.Indexes.IndexOf = Convert.ToInt32(IndexTextBox.Text);

                    // Работаем с адресом
                    if (_currentpartner.Adress.Regions == null)
                    {
                        _currentpartner.Adress.Regions = new Regions
                        {
                            RegionOf = RegionTextBox.Text
                        };
                    }
                    else
                    {
                        _currentpartner.Adress.Regions.RegionOf = RegionTextBox.Text;
                    }

                    if (_currentpartner.Adress.Cities == null)
                    {
                        _currentpartner.Adress.Cities = new Cities
                        {
                            CityOf = CityTextBox.Text
                        };
                    }
                    else
                    {
                        _currentpartner.Adress.Cities.CityOf = CityTextBox.Text;
                    }

                    if (_currentpartner.Adress.Streets == null)
                    {
                        _currentpartner.Adress.Streets = new Streets
                        {
                            StreetOf = StreetTextBox.Text
                        };
                    }
                    else
                    {
                        _currentpartner.Adress.Streets.StreetOf = StreetTextBox.Text;
                    }

                    _currentpartner.Adress.HouseNum = Convert.ToInt32(HouseNumTextBox.Text);

                    // Добавьте код для сохранения в базу данных
                    Data.MasterFloorEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно изменено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    var newPartner = new Data.PartnersImport
                    {
                        PartnerName = new Data.PartnerName { PartnerName1 = NameTextBox.Text },
                        Reiting = Convert.ToInt32(RatingTextBox.Text),
                        PhoneOfPartner = PhoneTextBox.Text,
                        EmailOfPartner = EmailTextBox.Text,
                        Adress = new Data.Adress
                        {
                            Indexes = new Data.Indexes { IndexOf = Convert.ToInt32(IndexTextBox.Text) },
                            HouseNum = Convert.ToInt32(HouseNumTextBox.Text)
                        }
                    };

                    var searchDirector = Data.MasterFloorEntities.GetContext().Directors
                        .FirstOrDefault(item => item.FIO == FIOTextBox.Text);
                    if (searchDirector != null)
                    {
                        newPartner.IdDirector = searchDirector.Id;
                    }
                    else
                    {
                        Data.Directors directors = new Data.Directors { FIO = FIOTextBox.Text };
                        Data.MasterFloorEntities.GetContext().Directors.Add(directors);
                        Data.MasterFloorEntities.GetContext().SaveChanges();
                        newPartner.IdDirector = directors.Id;
                    }

                    var searchPartnerName = Data.MasterFloorEntities.GetContext().PartnerName
                        .FirstOrDefault(item => item.PartnerName1 == NameTextBox.Text);
                    if (searchPartnerName != null)
                    {
                        newPartner.IdPartnerName = searchPartnerName.Id;
                    }
                    else
                    {
                        Data.PartnerName partnerName = new Data.PartnerName { PartnerName1 = NameTextBox.Text };
                        Data.MasterFloorEntities.GetContext().PartnerName.Add(partnerName);
                        Data.MasterFloorEntities.GetContext().SaveChanges();
                        newPartner.IdPartnerName = partnerName.Id;
                    }

                    var selectedType = Data.MasterFloorEntities.GetContext().TypeOfPartners
                        .FirstOrDefault(item => item.TypeOfPartner == TypeComboBox.Text);
                    if (selectedType != null)
                    {
                        newPartner.IdTypeOfPartner = selectedType.Id;
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, выберите тип партнера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    var searchRegion = Data.MasterFloorEntities.GetContext().Regions
                        .FirstOrDefault(item => item.RegionOf == RegionTextBox.Text);
                    if (searchRegion != null)
                    {
                        newPartner.Adress.IdRegion = searchRegion.Id;
                    }

                    var searchCity = Data.MasterFloorEntities.GetContext().Cities
                        .FirstOrDefault(item => item.CityOf == CityTextBox.Text);
                    if (searchCity != null)
                    {
                        newPartner.Adress.IdCity = searchCity.Id;
                    }

                    var searchStreet = Data.MasterFloorEntities.GetContext().Streets
                        .FirstOrDefault(item => item.StreetOf == StreetTextBox.Text);
                    if (searchStreet != null)
                    {
                        newPartner.Adress.IdStreet = searchStreet.Id;
                    }

                    Data.MasterFloorEntities.GetContext().PartnersImport.Add(newPartner);
                    Data.MasterFloorEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не получилось добавить(", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
