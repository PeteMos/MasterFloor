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
using System.Windows.Threading;

namespace MasterPol.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();

                if (string.IsNullOrEmpty(LoginTextBox.Text))
                {
                    errors.AppendLine("Заполните логин!");
                }

                if (string.IsNullOrEmpty(PasswordBox.Password))
                {
                    errors.AppendLine("Заполните пароль!");
                }

                if (Data.MasterPolEntities.GetContext().Directors
                    .Any(d => d.Login == LoginTextBox.Text && d.Password == PasswordBox.Password))
                {
                    Classes.Manager.MainFrame.Navigate(new Pages.ListViewPage());
                    MessageBox.Show("Успех!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Некорректный логин/пароль", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
