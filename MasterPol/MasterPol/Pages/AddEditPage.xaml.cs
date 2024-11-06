using MasterPol.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace MasterPol.Pages
{
    public partial class AddEditPage : Page
    {
        public string FlagAddorEdit = "default";
        private bool _isNew;
        public Data.PartnersImport _currentPartner = new Data.PartnersImport();
        public AddEditPage(Data.PartnersImport partner)
        {
            InitializeComponent();

            _isNew = partner == null;
            _currentPartner = _isNew ? new PartnersImport() : partner;

            DataContext = new
            {
                FlagAddorEdit = _isNew ? "add" : "edit",
                CurrentPartner = _currentPartner
            };

            if (partner != null)
            {
                _currentPartner = partner;
                FlagAddorEdit = "edit";
            }
            else
            {
                FlagAddorEdit = "add";
            }
            DataContext = _currentPartner;

            Init();
        }

        private void Init()
        {
            try
            {
                TypeComboBox.ItemsSource = Data.MasterPolEntities.GetContext().TypeOfPartner.ToList();

                if (FlagAddorEdit == "add")
                {
                    IdTextBox.Text = (Data.MasterPolEntities.GetContext().PartnersImport.Max(d => d.Id) + 1).ToString();
                    _currentPartner.Adress.IdIndex = 0;
                    _currentPartner.Adress.IdRegion = 0;
                    _currentPartner.Adress.IdCity = 0;
                    _currentPartner.Adress.IdStreet = 0;
                    _currentPartner.Adress.HouseNum = 0;
                }
                else if (FlagAddorEdit == "edit")
                {
                    LoadPartnerData(_currentPartner);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPartnerData(PartnersImport partner)
        {
            try
            {
                if (partner == null)
                {
                    Debug.WriteLine("partner is null");
                    return;
                }

                IdTextBox.Text = partner.Id.ToString();
                NameTextBox.Text = partner.PartnerName.Name;
                TypeComboBox.SelectedItem = Data.MasterPolEntities.GetContext().TypeOfPartner.Find(partner.Id);
                RatingTextBox.Text = partner.Reiting.ToString();

                if (partner.Adress == null)
                {
                    Debug.WriteLine("partner.Address is null");
                    _currentPartner.Adress = new Adress();
                }
                else
                {
                    _currentPartner.Adress = new Adress
                    {
                        IdIndex = partner.Adress.IdIndex,
                        IdRegion = partner.Adress.IdRegion,
                        IdCity = partner.Adress.IdCity,
                        IdStreet = partner.Adress.IdStreet,
                        HouseNum = partner.Adress.HouseNum
                    };
                }

                AdressTextBox.Text = $"{_currentPartner.Adress.IdIndex} {_currentPartner.Adress.IdRegion} {_currentPartner.Adress.IdCity}" +
                    $" {_currentPartner.Adress.IdStreet} {_currentPartner.Adress.HouseNum}";

                FIOTextBox.Text = partner.Directors.FIO;
                PhoneTextBox.Text = partner.PhoneOfPartner;
                EmailTextBox.Text = partner.EmailOfPartner;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine($"Exception: {ex.Message}");
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
                if (!ValidateInput())
                {
                    return;
                }

                using (var context = Data.MasterPolEntities.GetContext())
                {
                    if (_isNew)
                    {
                        AddPartner(context);
                    }
                    else
                    {
                        EditPartner(context);
                    }

                    var listViewPage = new ListViewPage();
                    Classes.Manager.MainFrame.Navigate(listViewPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Наименование не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                NameTextBox.Focus();
                isValid = false;
            }

            if (TypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип партнера!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                TypeComboBox.Focus();
                isValid = false;
            }

            if (!int.TryParse(RatingTextBox.Text, out int rating) || rating < 0)
            {
                MessageBox.Show("Рейтинг должен быть целым неотрицательным числом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                RatingTextBox.Focus();
                isValid = false;
            }

            if (string.IsNullOrEmpty(AdressTextBox.Text))
            {
                MessageBox.Show("Адрес не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                AdressTextBox.Focus();
                isValid = false;
            }

            if (string.IsNullOrEmpty(FIOTextBox.Text))
            {
                MessageBox.Show("ФИО не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FIOTextBox.Focus();
                isValid = false;
            }

            if (string.IsNullOrEmpty(PhoneTextBox.Text))
            {
                MessageBox.Show("Номер телефона не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                PhoneTextBox.Focus();
                isValid = false;
            }

            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                MessageBox.Show("Email не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                EmailTextBox.Focus();
                isValid = false;
            }

            return isValid;
        }

        private void AddPartner(MasterPolEntities context)
        {
            _currentPartner.Id = (context.PartnersImport.Max(d => d.Id) + 1);
            _currentPartner.PartnerName.Name = NameTextBox.Text;
            _currentPartner.TypeOfPartner.Id = (TypeComboBox.SelectedItem as TypeOfPartner).Id;
            _currentPartner.Reiting = int.Parse(RatingTextBox.Text);
            _currentPartner.Adress.IdRegion = int.Parse(AdressTextBox.Text.Split(' ')[0]);
            _currentPartner.Adress.IdCity = int.Parse(AdressTextBox.Text.Split(' ')[1]);
            _currentPartner.Adress.IdStreet = int.Parse(AdressTextBox.Text.Split(' ')[2]);
            _currentPartner.Adress.IdIndex = int.Parse(AdressTextBox.Text.Split(' ')[3]);
            _currentPartner.Adress.IdIndex = int.Parse(AdressTextBox.Text.Split(' ')[4]);
            _currentPartner.Directors = new Directors { FIO = FIOTextBox.Text };
            _currentPartner.PhoneOfPartner = PhoneTextBox.Text;
            _currentPartner.EmailOfPartner = EmailTextBox.Text;

            context.PartnersImport.Add(_currentPartner);
            context.SaveChanges();

            MessageBox.Show("Успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditPartner(MasterPolEntities context)
        {
            var partnerToUpdate = context.PartnersImport.Find(_currentPartner.Id);

            partnerToUpdate.PartnerName.Name = NameTextBox.Text;
            partnerToUpdate.TypeOfPartner.Id = (TypeComboBox.SelectedItem as TypeOfPartner).Id;
            partnerToUpdate.Reiting = int.Parse(RatingTextBox.Text);
            partnerToUpdate.Adress.IdIndex = int.Parse(AdressTextBox.Text.Split(' ')[0]);
            partnerToUpdate.Adress.IdRegion = int.Parse(AdressTextBox.Text.Split(' ')[1]);
            partnerToUpdate.Adress.IdRegion = int.Parse(AdressTextBox.Text.Split(' ')[2]);
            partnerToUpdate.Adress.IdRegion = int.Parse(AdressTextBox.Text.Split(' ')[3]);
            partnerToUpdate.Adress.IdRegion = int.Parse(AdressTextBox.Text.Split(' ')[4]);
            partnerToUpdate.Directors.FIO = FIOTextBox.Text;
            partnerToUpdate.PhoneOfPartner = PhoneTextBox.Text;
            partnerToUpdate.EmailOfPartner = EmailTextBox.Text;

            context.SaveChanges();

            MessageBox.Show("Успешно изменено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}