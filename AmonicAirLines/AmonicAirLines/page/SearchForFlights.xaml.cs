using AmonicAirLines.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для SearchForFlights.xaml
    /// </summary>
    public partial class SearchForFlights : Page
    {
        class FlightForBooking
        {
            public List<int> Ids { get; set; } = new List<int>();
            public string From { get; set; }
            public string To { get; set; }
            public DateTime Date { get; set; }
            public TimeSpan Time { get; set; }
            public string FlightNumbers { get; set; }
            public decimal CabinPrice { get; set; }
            public int NumberOfStops { get; set; }
        }
        List<Airports> AirportList = new List<Airports>();
        List<Cabins> CabinList = new List<Cabins>();
        List<FlightSearch> outFlightSearch;
        List<FlightForBooking> outFlightForBookings;
        List<FlightSearch> retFlightSearch;
        List<FlightForBooking> retFlightForBookings;

        public SearchForFlights()
        {
            InitializeComponent();
            foo();

            returnRadioButton.Checked += returnRadioButton_Checked;
            oneWayRadioButton.Checked += oneWayRadioButton_Checked;
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key.ToString());
            if (e.Key == Key.D && Keyboard.Modifiers == ModifierKeys.Control)
            {
                FromBox.SelectedIndex = 2;
                ToBox.SelectedIndex = 1;
                CabinBox.SelectedIndex = 0;
                OutboundTextBox.Text = "12/12/2017";
                returnTextBox.Text = "20/12/2017";
                ThreeDays1CheckBox.IsChecked = true;
                ThreeDays2CheckBox.IsChecked = true;
                Button_Click(sender, e);
            }
        }
        async public void foo()
        {
            await LoadAirportsAsync();

        }
        private async Task<int> LoadAirportsAsync()
        {
            AirportList = await Airports.GetAirports();
            CabinList = await Cabins.GetCabinTypes();

            FromBox.ItemsSource = AirportList;
            FromBox.DisplayMemberPath = "Iatacode";
            FromBox.SelectedIndex = 0;
            ToBox.ItemsSource = AirportList;
            ToBox.DisplayMemberPath = "Iatacode";
            ToBox.SelectedIndex = 0;
            CabinBox.ItemsSource = CabinList;
            CabinBox.DisplayMemberPath = "Name";
            CabinBox.SelectedIndex = 0;
            FromBox.SelectedValuePath = "Id";
            ToBox.SelectedValuePath = "Id";
            CabinBox.SelectedValuePath = "Id";

            return 1;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        async private Task<string> f1(string apiUrl)
        {
            Console.WriteLine(apiUrl);
            string responseBody = "";
            HttpClient client = new HttpClient();
            try
            {
                
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                else { }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
            }
            return responseBody;
        }
        private void fillDataGrid(int from, int to, DateTime dateTime, bool threeDays,ref List<FlightSearch> flightSearch,ref List<FlightForBooking> flightForBookings, ref DataGrid dataGrid1)
        {
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/test?fromIata={AirportList[from].Iatacode}&toIata={AirportList[to].Iatacode}&date={dateTime.ToString("yyyy-MM-dd")}&threeDays={threeDays.ToString().ToLower()}";
            string responseBody = "";
            Task.Run( async () =>
            {
                responseBody = await f1(apiUrl);
            }).Wait();
            flightSearch = JsonConvert.DeserializeObject<List<FlightSearch>>(responseBody);
            flightForBookings = new List<FlightForBooking>();
            double coef = 1;
            if (CabinBox.SelectedIndex == 2)
            {
                coef = 1.35;
            }
            else if (CabinBox.SelectedIndex == 3)
            {
                coef = 1.35 * 1.30;
            }

            foreach (var i in flightSearch)
            {
                foreach (var j in i.FlightIds)
                {
                    Console.Write(j + " ");
                }
                Console.WriteLine();
                var temp = new FlightForBooking
                {
                    Ids = i.FlightIds,
                    From = AirportList[from].Iatacode,
                    To = AirportList[to].Iatacode,
                    Date = i.Dates[0],
                    Time = TimeSpan.Parse(i.Times[0]),
                    FlightNumbers = i.FlightNumbers.Replace(", ", " - "),
                    CabinPrice = (decimal)Math.Round(((double)(i.TotalCost)) * coef, 0),
                    NumberOfStops = i.Transfers - 1
                };
                flightForBookings.Add(temp);
            }
            dataGrid1.ItemsSource = flightForBookings;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FromBox.SelectedIndex <0 || ToBox.SelectedIndex <0)
            {
                MessageBox.Show("не все параметры выставлены");
                return;
            }
            int from = (int)FromBox.SelectedIndex;
            int to = (int)ToBox.SelectedIndex;
            int cabinType = (int)CabinBox.SelectedValue;
            bool ret = returnRadioButton.IsChecked == true;
            DateTime outDateTime;
            DateTime retDateTime;
            bool outThreeDays = ThreeDays1CheckBox.IsChecked == true;
            bool retThreeDays = ThreeDays2CheckBox.IsChecked == true;
            bool s;
            s = DateTime.TryParseExact(OutboundTextBox.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out outDateTime);
            if (!s)
            {
                MessageBox.Show("не все параметры выставлены");
                return;
            }
            fillDataGrid(from, to, outDateTime, outThreeDays, ref outFlightSearch, ref outFlightForBookings, ref outDataGrid);
            if (ret)
            {
                s = DateTime.TryParseExact(returnTextBox.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out retDateTime);
                if (!s)
                {
                    MessageBox.Show("не все параметры выставлены");
                    return;
                }
                fillDataGrid(to, from, retDateTime, retThreeDays, ref retFlightSearch, ref retFlightForBookings, ref retDataGrid);
            }
            // https://localhost:7139/test?fromIata={from}&toIata={to}&date={outDateTime}
        }

        private void oneWayRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            GridRow2.Visibility = Visibility.Collapsed;
            retDataGrid.Visibility = Visibility.Collapsed;
            rowDefinition2.Height = new GridLength(0);
            rowDefinition3.Height = new GridLength(0);
            returnTextBox.IsEnabled = false;

        }

        private void returnRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            GridRow2.Visibility = Visibility.Visible;
            retDataGrid.Visibility = Visibility.Visible;
            rowDefinition2.Height = new GridLength(25);
            rowDefinition3.Height = new GridLength(1, GridUnitType.Star);
            returnTextBox.IsEnabled = true;
        }

        private void date_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            string text = ((TextBox)sender).Text;

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

        private void date_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            string text = t.Text;

            // Если текст короче 2 символов, то ничего не делаем
            if (text.Length < 2)
                return;

            // Если вставлен символ после 2 или 5 символа и он не '/', то добавляем '/'
            if (text.Length == 3 && text[2] != '/')
            {
                t.Text = text.Insert(2, "/");
                t.SelectionStart = 4;
            }
            else if (text.Length == 6 && text[5] != '/')
            {
                t.Text = text.Insert(5, "/");
                t.SelectionStart = 7;
            }

            // Если длина текста больше 2 и предыдущий введенный символ был '/', а текущий не '/', то перемещаем курсор в конец
            if (text.Length > 2 && text[text.Length - 2] == '/' && text[text.Length - 1] != '/')
            {
                t.CaretIndex = text.Length;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int passengers;
            bool n = int.TryParse(PassengersTextBox.Text, out passengers);
            if (!n)
            {
                MessageBox.Show("не правильно введенно колличество пасажиров");
                return;
            }
            bool ret = returnRadioButton.IsChecked == true;
            object outFlight = outDataGrid.SelectedItem;
            object retFlight = retDataGrid.SelectedItem;
            if (outFlight == null || (ret && retFlight == null))
            {
                MessageBox.Show("не выбран рейс");
                return;
            }
            var t = new BookingConfirmation();
            t.ret = ret;
            t.outFlightSearch = outFlight as FlightSearch;
            if (ret)
            {
                t.retFlightSearch = retFlight as FlightSearch;
            }
            NavigationService.Navigate(t);
        }
    }
}
