using Newtonsoft.Json;
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

namespace AmonicAirLines.old
{

    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OfficeId { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Active { get; set; }
        public object Office { get; set; }
        public object Role { get; set; }
    }

    public class AnotherUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string UserRole { get; set; }
        public string EmailAddress { get; set; }
        public string Office { get; set; }
        public string HiddenData {  get; set; }
        public AnotherUser(User user, string Title)
        {
            Id = user.Id;
            int age = DateTime.Now.Year - user.Birthdate.Year;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Age = age;
            UserRole = user.RoleId == 1 ? "Admin" : "User";
            EmailAddress = user.Email;
            Office = Title;
            if (user.Active)
            {
                HiddenData = "0";
            } else
            {
                HiddenData = "1";
            }
        }
        public override string ToString()
        {
            return $"FirstName: {FirstName}, Last Name: {LastName}, Age: {Age}, User Role: {UserRole}, Email: {EmailAddress}, Office: {Office}";
        }
    }

    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        List<Office> OfficeList = new List<Office>();
        public AdminWindow()
        {
            InitializeComponent();
            foo();
        }
        async public void foo()
        {
            await LoadOfficesAsync();
            await fillComboBox();
        }
        private async Task<int> LoadOfficesAsync()
        {
            OfficeList = await Office.GetOffices();

            OfficeComboBox.ItemsSource = OfficeList;
            OfficeComboBox.DisplayMemberPath = "Title"; 

            OfficeComboBox.SelectedValuePath = "Id";
            return 1;
        }

        async public Task<int> fillComboBox()
        {
            
            int officeId = -1;
            if (OfficeComboBox.SelectedItem != null)
            {
                if (OfficeComboBox.SelectedItem is Office selectedOffice)
                {
                    officeId = selectedOffice.Id;
                }
            }
            Console.WriteLine(officeId);
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/UsersFromOffice?officeId={officeId}";
            string responseBody = "";
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(responseBody);
                    List<AnotherUser> users1 = new List<AnotherUser>();
                    foreach (var item in users)
                    {
                        users1.Add(new AnotherUser(item, OfficeList.Where(x => x.Id == item.OfficeId).SingleOrDefault().Title));
                        //Console.WriteLine(users1[users1.Count - 1].ToString());
                    }
                    //Console.WriteLine(users.Count);
                    dataGrid.ItemsSource = users1;
                    dataGrid.SelectedValuePath = "Id";
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
            }
            return 1;
        }

        private void OfficeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillComboBox();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OfficeComboBox.SelectedItem == null)
            {
                fillComboBox();
            }
            OfficeComboBox.SelectedItem = null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddUser window = new AddUser();
            Hide();
            window.ShowDialog();
            Show();
            fillComboBox();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AnotherUser user;
            if (dataGrid.SelectedItem != null)
            {
                user = dataGrid.SelectedItem as AnotherUser;
            } else
            {
                MessageBox.Show("Choose");
                return;
            }
            
            EditRole editRole = new EditRole();
            editRole.anotherUser = user;

            Hide();
            editRole.ShowDialog();
            Show();
            fillComboBox();
            
        }

        async private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            int userId = -1;
            if (dataGrid.SelectedItem != null)
            {
                if (dataGrid.SelectedItem is AnotherUser user)
                {
                    userId = user.Id;
                }
            } else
            {
                MessageBox.Show("Choose");
                return;
            }
            if (dataGrid.SelectedItems.Count > 1)
            {
                MessageBox.Show("выберите одного юзера");
                return;
            }
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/UserSwitch?userId={userId}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PutAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Успешно выполнен запрос.");
                    }
                    else
                    {
                        Console.WriteLine("Запрос завершился с ошибкой: " + response.StatusCode);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
                    return;
                }
            }
            fillComboBox();
        }
    }
}
