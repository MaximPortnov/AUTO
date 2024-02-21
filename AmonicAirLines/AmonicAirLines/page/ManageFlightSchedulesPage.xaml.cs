using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AmonicAirLines.Classes;
using AmonicAirLines;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace AmonicAirLines.page
{

    /// <summary>
    /// Логика взаимодействия для ManageFlightSchedulesPage.xaml
    /// </summary>
    public partial class ManageFlightSchedulesPage : Page
    {
        List<Airports> AirportList = new List<Airports>();
        List<Flight> flights;
        public ManageFlightSchedulesPage()
        {
            InitializeComponent();
            foo();
        }
        async public void foo()
        {
            await LoadAirportsAsync();

        }
        private async Task<int> LoadAirportsAsync()
        {
            AirportList = await Airports.GetAirports();
            AirportList.Insert(0, new Airports() { Id = -1, CountryId = -1, Iatacode = "all", Name = "" });

            FromBox.ItemsSource = AirportList;
            FromBox.DisplayMemberPath = "Iatacode";
            ToBox.ItemsSource = AirportList;
            ToBox.DisplayMemberPath = "Iatacode";

            FromBox.SelectedValuePath = "Id";
            ToBox.SelectedValuePath = "Id";
            FromBox.SelectedIndex = 0;
            ToBox.SelectedIndex = 0;
            List<string> otherItems = new List<string> { "No", "Data-Time", "Flight Number" };
            SortByBox.ItemsSource = otherItems;
            SortByBox.SelectedIndex = 0;
            return 1;
        }
        private void SortBox(object sender, SelectionChangedEventArgs e)
        {
            string selectedValue = SortByBox.SelectedValue as string;

            if (selectedValue == "Data-Time")
            {
            }
            else if (selectedValue == "Flight Number")
            {
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true; // отмена ввода символа
                    return;
                }
            }
        }
        private void OutboundTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = OutboundTextBox.Text;

            // Если вводимый символ не цифра и не '/', и длина текста уже максимальна, то игнорируем ввод
            if ((!char.IsDigit(e.Text[0]) && e.Text[0] != '/') || text.Length >= 10)
            {
                e.Handled = true;
                return;
            }

            // Если вводимый символ '/' и в тексте уже есть 2 '/' или длина текста больше 9, то игнорируем ввод
            if (e.Text[0] == '/' && (text.Count(c => c == '/') >= 2 || text.Length >= 10))
            {
                e.Handled = true;
                return;
            }

            // Если вводим '/' и находимся на 2 или 5 позиции, то игнорируем ввод
            if (e.Text[0] == '/' && (text.Length == 2 || text.Length == 5))
            {
                e.Handled = true;
                return;
            }
        }

        private void OutboundTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = OutboundTextBox.Text;

            // Если текст короче 2 символов, то ничего не делаем
            if (text.Length < 2)
                return;

            // Если вставлен символ после 2 или 5 символа и он не '/', то добавляем '/'
            if (text.Length == 3 && text[2] != '/')
            {
                OutboundTextBox.Text = text.Insert(2, "/");
                OutboundTextBox.SelectionStart = 4;
            }
            else if (text.Length == 6 && text[5] != '/')
            {
                OutboundTextBox.Text = text.Insert(5, "/");
                OutboundTextBox.SelectionStart = 7;
            }

            // Если длина текста больше 2 и предыдущий введенный символ был '/', а текущий не '/', то перемещаем курсор в конец
            if (text.Length > 2 && text[text.Length - 2] == '/' && text[text.Length - 1] != '/')
            {
                OutboundTextBox.CaretIndex = text.Length;
            }
        }

        private void FromBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            loadDataGrid();
        }
        async private void loadDataGrid()
        {
            var from = FromBox.SelectedIndex;
            var to = ToBox.SelectedIndex;
            var outboundText = OutboundTextBox.Text;
            var flightNumber = FlightNumberTextBox.Text;
            string responseBody = "";
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/GetManageFlight?";
            if (from > 0)
            {
                apiUrl += $"from={AirportList[from].Iatacode}&";
            }
            if (to > 0)
            {
                apiUrl += $"to={AirportList[to].Iatacode}&";
            }
            if (!string.IsNullOrEmpty(outboundText))
            {
                string format = "dd/MM/yyyy"; // Определение формата
                DateTime date;
                bool success = DateTime.TryParseExact(outboundText, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                if (!success)
                {
                    return;
                }
                Console.WriteLine(success);
                apiUrl += $"outbound={date.ToString("yyyy-MM-dd")}&";
            }
            if (!string.IsNullOrEmpty(flightNumber))
            {
                apiUrl += $"flightNumber={flightNumber}";
            }

            //Console.WriteLine($"{from} {to} {OutboundText} {FlightNumber}");
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
            //Console.WriteLine(apiUrl);
            //Console.WriteLine(responseBody);
            if (string.IsNullOrEmpty(responseBody))
            {
                return;
            }

            flights = JsonConvert.DeserializeObject<List<Flight>>(responseBody);
            //Console.WriteLine(r[0].FirstClassPrice.ToString());
            string selectedValue = SortByBox.SelectedValue as string;

            if (selectedValue == "Data-Time")
            {
                flights.Sort((flight1, flight2) =>
                {
                    int dateComparison = flight1.Date.CompareTo(flight2.Date);
                    if (dateComparison != 0)
                        return dateComparison;
                    return flight1.Time.CompareTo(flight2.Time);
                });
            }
            else if (selectedValue == "Flight Number")
            {
                flights.Sort((flight1, flight2) => (int.Parse(flight1.FlightNumber)).CompareTo(int.Parse(flight2.FlightNumber)));
            }
            dataGrid.ItemsSource = flights;
            dataGrid.SelectedValuePath = "id";
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        async private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1)
            {
                return;
            }
            Console.WriteLine(dataGrid.SelectedValue);
            var t = new EditFlight((Flight)dataGrid.SelectedItem);
            t.ShowDialog();
            loadDataGrid();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var t = new ScheduleChanges();
            t.ShowDialog();
            loadDataGrid();
        }
    }
}
