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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AmonicAirLines.Classes;


namespace AmonicAirLines
{
    /// <summary>
    /// Логика взаимодействия для EditFlight.xaml
    /// </summary>
    public partial class EditFlight : Window
    {
        public Flight fl;
        public EditFlight(Flight f)
        {
            InitializeComponent();
            fl = f;
            FromLB.Content = "From: " + fl.From;
            ToLB.Content = "To: " + fl.To;
            AircraftLB.Content = "Aircraft: " + fl.Aircraft;
            //Console.WriteLine(fl.Date.ToString("dd/MM/yyyy"));
            DateTextBox.Text = fl.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            TimeTextBox.Text = fl.Time.ToString(@"hh\:mm");
            EconomyPriceTextBox.Text = fl.EconomyPrice.ToString();
        }
        private void DateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = DateTextBox.Text;

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
        private void DateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = DateTextBox.Text;

            // Если текст короче 2 символов, то ничего не делаем
            if (text.Length < 2)
                return;

            // Если вставлен символ после 2 или 5 символа и он не '/', то добавляем '/'
            if (text.Length == 3 && text[2] != '/')
            {
                DateTextBox.Text = text.Insert(2, "/");
                DateTextBox.SelectionStart = 4;
            }
            else if (text.Length == 6 && text[5] != '/')
            {
                DateTextBox.Text = text.Insert(5, "/");
                DateTextBox.SelectionStart = 7;
            }

            // Если длина текста больше 2 и предыдущий введенный символ был '/', а текущий не '/', то перемещаем курсор в конец
            if (text.Length > 2 && text[text.Length - 2] == '/' && text[text.Length - 1] != '/')
            {
                DateTextBox.CaretIndex = text.Length;
            }
        }
        private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли вводимый символ цифрой или символом ':'
            if (!char.IsDigit(e.Text[0]) && e.Text[0] != ':')
            {
                e.Handled = true;
                return;
            }

            // Получаем текущий текст в TextBox
            string text = TimeTextBox.Text;

            // Проверяем, чтобы вводимый символ не был вторым символом ':' при пустом тексте в TextBox
            if (text.Length == 0 && e.Text[0] == ':')
            {
                e.Handled = true;
                return;
            }

            // Проверяем, чтобы вводимый символ не был третьим символом ':' при наличии двух символов ':' и вводимый символ не был вторым символом ':'
            if ((text.Length == 2 && text[1] == ':' && e.Text[0] == ':') || (text.Length == 2 && text[0] != ':' && e.Text[0] == ':'))
            {
                e.Handled = true;
                return;
            }

            // Проверяем, чтобы вводимый символ не был шестым символом, если предыдущий символ ':'
            if (text.Length == 5 && text[4] == ':' && e.Text[0] == ':')
            {
                e.Handled = true;
                return;
            }
        }
        private void TimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = TimeTextBox.Text;

            // Если текст короче 2 символов, то ничего не делаем
            if (text.Length < 2)
                return;

            // Если вставлен символ после 2 символа и он не ':', то добавляем ':'
            if (text.Length == 2 && text[1] != ':')
            {
                TimeTextBox.Text = text.Insert(2, ":");
                TimeTextBox.SelectionStart = 3;
            }


        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            string date = DateTextBox.Text;
            string time = TimeTextBox.Text;
            string economy = EconomyPriceTextBox.Text;
            DateTime dateTemp;
            bool s = DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTemp);
            if (!s)
            {
                return;
            }
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/PutFlight?id={fl.id}&date={dateTemp.ToString("yyyy-MM-dd")}&time={time}&economyPrice={economy}&confirmed={fl.Confirmed.ToString().ToLower()}";
            Console.WriteLine(apiUrl);
            HttpClient client = new HttpClient();
            try
            {
                await client.PutAsync(apiUrl, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
                return;
            }
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
