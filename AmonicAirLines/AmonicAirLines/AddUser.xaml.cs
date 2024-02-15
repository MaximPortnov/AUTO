using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace AmonicAirLines
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
            LoadOfficesAsync();
        }
        private async void LoadOfficesAsync()
        {
            var offices = await Office.GetOffices();

            // Привязываем список офисов к ComboBox
            OfficeComboBox.ItemsSource = offices;
            OfficeComboBox.DisplayMemberPath = "Title"; // Отображаем поле Title

            // Необходимо настроить, чтобы при выборе элемента отображалось значение ID
            OfficeComboBox.SelectedValuePath = "Id";
        }

        private void Savebtn_Click(object sender, RoutedEventArgs e)
        {
            string emailAddress = Email.Text;
            if (emailAddress == null || emailAddress.Length == 0)
            {
                MessageBox.Show("нет почты");
                return;
            }
            string firstName = FirstNameTextBox.Text;
            if (firstName == null || firstName.Length == 0) {
                MessageBox.Show("нет имени юзера");
                return; 
            }
            string lastName = LastNameTextBox.Text;
            if (lastName == null || lastName.Length == 0)
            {
                MessageBox.Show("нет фамилии юзера");
                return;
            }
            int officeId = 0;
            if (OfficeComboBox.SelectedItem != null)
            {
                // Преобразование выбранного объекта к типу Office
                if (OfficeComboBox.SelectedItem is Office selectedOffice)
                {
                    officeId = selectedOffice.Id; // Получение Id офиса
                }
            }
            else
            {
                MessageBox.Show("не выбран офис");
                return;
            }
            string birthdate = BirthdateTextBox.Text;
            DateTime parsedDate;

            if (DateTime.TryParseExact(birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                string formattedDate = parsedDate.ToString("yyyy-MM-dd");
            }
            else
            {
                MessageBox.Show("Некорректный формат даты");
                return;
            }
            string password = PasswordTextBox.Text;
            if (lastName == null || lastName.Length == 0)
            {
                MessageBox.Show("введите пароль");
                return;
            }
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
            // Создание объекта для представления данных
            var userData = new
            {
                emailAddress = emailAddress,
                firstName = firstName,
                lastName = lastName,
                officeId = officeId,
                birthdate = birthdate,
                password = hashedPassword
            };

            string jsonData = JsonConvert.SerializeObject(userData);

            addUser(jsonData);
            Close();
        }
        async private void addUser(string json)
        {
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/Users";

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Запрос выполнен успешно.");
                else
                    Console.WriteLine($"Ошибка при выполнении запроса: {response.StatusCode}");
            }
        }

        private void Cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
