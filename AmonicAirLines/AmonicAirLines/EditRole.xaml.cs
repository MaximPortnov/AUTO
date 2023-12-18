using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace AmonicAirLines
{
    
    /// <summary>
    /// Логика взаимодействия для EditRole.xaml
    /// </summary>
    public partial class EditRole : Window
    {
        public AnotherUser anotherUser { get; set; }
        List<Office> offices;
        public EditRole()
        {
            InitializeComponent();
            init();
        }
        async void init()
        {
            await LoadOfficesAsync();
            EmailTextBox.Text = anotherUser.EmailAddress;
            FirstNameTextBox.Text = anotherUser.FirstName;
            LastNameTextBox.Text = anotherUser.LastName;
            OfficeComboBox.SelectedItem = offices.Where(x => x.Title == anotherUser.Office).SingleOrDefault();
            if (anotherUser.UserRole == "User")
            {
                UserRadioButton.IsChecked = true;
                AdminRadioButton.IsChecked = false;
            } else
            {
                UserRadioButton.IsChecked = false;
                AdminRadioButton.IsChecked = true;
            }
        }
        private async Task<int> LoadOfficesAsync()
        {
            offices = await Office.GetOffices();

            // Привязываем список офисов к ComboBox
            OfficeComboBox.ItemsSource = offices;
            OfficeComboBox.DisplayMemberPath = "Title"; // Отображаем поле Title

            // Необходимо настроить, чтобы при выборе элемента отображалось значение ID
            OfficeComboBox.SelectedValuePath = "Id";
            return 1;
        }
        async private void Applybtn_Click(object sender, RoutedEventArgs e)
        {
            string emailAddress = EmailTextBox.Text;
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            int officeId = 0;
            if (OfficeComboBox.SelectedItem != null)
            {
                // Преобразование выбранного объекта к типу Office
                if (OfficeComboBox.SelectedItem is Office selectedOffice)
                {
                    officeId = selectedOffice.Id; // Получение Id офиса
                }
            }
            int role = (UserRadioButton.IsChecked == true) ? 2 : 1;

            // Создаем объект для представления данных
            var data = new
            {
                Id = anotherUser.Id,
                EmailAddress = emailAddress,
                FirstName = firstName,
                LastName = lastName,
                OfficeId = officeId,
                RoleId = role
            };

            // Преобразуем данные в формат JSON
            string jsonData = JsonConvert.SerializeObject(data);

            // Теперь jsonData содержит данные в формате JSON, которые можно отправить куда-либо
            //Console.WriteLine(jsonData);
            // Вместо Console.WriteLine(jsonData) можно использовать отправку данных по сети или сохранение в файл, например
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/UserEdit";
            Console.WriteLine(jsonData);
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("Запрос выполнен успешно.");
                else
                    Console.WriteLine($"Ошибка при выполнении запроса: {response.StatusCode}");
            }
            Close();
        }

        private void Cancelbtn1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
