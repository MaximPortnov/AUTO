using System;
using System.Collections.Generic;
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
using AmonicAirLines.Classes;

namespace AmonicAirLines.page
{
    /// <summary>
    /// Логика взаимодействия для BookingConfirmation.xaml
    /// </summary>
    public partial class BookingConfirmation : Page
    {
        List<Country> CountryList = new List<Country>();
        public BookingConfirmation()
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
            CountryList = await Country.GetCountry();

            CountryBox.ItemsSource = CountryList;
            CountryBox.DisplayMemberPath = "Name";

            CountryBox.SelectedValuePath = "Id";

            return 1;
        }

        private void OutboundTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = BirthdateBox.Text;

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
            string text = BirthdateBox.Text;

            // Если текст короче 2 символов, то ничего не делаем
            if (text.Length < 2)
                return;

            // Если вставлен символ после 2 или 5 символа и он не '/', то добавляем '/'
            if (text.Length == 3 && text[2] != '/')
            {
                BirthdateBox.Text = text.Insert(2, "/");
                BirthdateBox.SelectionStart = 4;
            }
            else if (text.Length == 6 && text[5] != '/')
            {
                BirthdateBox.Text = text.Insert(5, "/");
                BirthdateBox.SelectionStart = 7;
            }

            // Если длина текста больше 2 и предыдущий введенный символ был '/', а текущий не '/', то перемещаем курсор в конец
            if (text.Length > 2 && text[text.Length - 2] == '/' && text[text.Length - 1] != '/')
            {
                BirthdateBox.CaretIndex = text.Length;
            }
        }

    }
}
