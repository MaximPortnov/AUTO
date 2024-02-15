﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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

namespace AmonicAirLines.page
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private static Dictionary<string, int> loginAttempts = new Dictionary<string, int>();
        private static Dictionary<string, DateTime> lockedUsers = new Dictionary<string, DateTime>();

        public StartPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Text;

            if (lockedUsers.ContainsKey(username) && lockedUsers[username] > DateTime.Now)
            {
                // Пользователь заблокирован, показать сообщение о времени ожидания
                TimeSpan remainingTime = lockedUsers[username] - DateTime.Now;
                MessageBox.Show($"Пользователь заблокирован. Повторите попытку через {remainingTime.TotalSeconds} секунд.");
                return;
            }

            string responseBody = "False";

            string hashedPassword = "";
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                hashedPassword = sb.ToString();
            }
            Console.WriteLine(hashedPassword);

            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/CheckUsers?login={username}&password={hashedPassword}";

            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
            }

            if (responseBody == "user")
            {
                loginAttempts[username] = 0;
                if (lockedUsers.ContainsKey(username))
                {
                    lockedUsers.Remove(username);
                }
                NavigationService.Navigate(new UserMainPage());
                //UserWindow window = new UserWindow();
                //this.Hide();
                //window.ShowDialog();
                //this.Show();

            }
            if (responseBody == "admin")
            {
                loginAttempts[username] = 0;
                if (lockedUsers.ContainsKey(username))
                {
                    lockedUsers.Remove(username);
                }
                NavigationService.Navigate(new AdminMainPage());
                //AdminWindow window = new AdminWindow();
                //this.Hide();
                //window.ShowDialog();
                //this.Show();

            }
            else if (responseBody == "block")
            {
                MessageBox.Show("Вы заблокированы!");
            }
            else if (responseBody == "false")
            {
                if (!loginAttempts.ContainsKey(username))
                {
                    loginAttempts[username] = 0;
                }
                loginAttempts[username]++;
                if (loginAttempts[username] >= 3)
                {
                    lockedUsers[username] = DateTime.Now.AddSeconds(10);
                    MessageBox.Show("Превышен лимит попыток входа. Пользователь заблокирован на 10 секунд.");
                    return;
                }
                MessageBox.Show("Неправильное имя пользователя или пароль.");
            }
            else if (responseBody == "error")
            {
                MessageBox.Show("Ошибка входа");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
