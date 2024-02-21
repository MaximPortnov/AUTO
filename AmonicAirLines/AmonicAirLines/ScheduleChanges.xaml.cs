using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace AmonicAirLines
{
    /// <summary>
    /// Логика взаимодействия для ScheduleChanges.xaml
    /// </summary>
    public partial class ScheduleChanges : Window
    {
        public ScheduleChanges()
        {
            InitializeComponent();
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "csv file (*.csv)|*.csv";
            var t = openFileDialog.ShowDialog();
            Console.WriteLine((t.HasValue && !t.Value).ToString() + " " + (t.HasValue).ToString() + " " + (!t.Value).ToString());
            if (t.HasValue && !t.Value)
                return;
            // получаем выбранный файл
            string filename = openFileDialog.FileName;
            PathTextBox.Text = filename;
            // читаем файл в строку
            string fileText = File.ReadAllText(filename);
            List<string> lines = new List<string>(fileText.Split(new char[] { '\n', }));
            //List<List<string>>
            int suc = 0;
            int dup = 0;
            int mis = 0;
            foreach (string line in lines)
            {
                var t1 = new List<string>(line.Split(new char[] { ',' }));
                
                Console.WriteLine(line + " " + t1[0] + " " + t1.Count);
                if (t1.Count != 9)
                {
                    mis++;
                }
                else
                {
                    do
                    {
                        DateTime date;
                        TimeSpan time;
                        int flightNumber;
                        int aircraftID;
                        double economyPrice;
                        bool confirmed;
                        bool r;
                        r = DateTime.TryParseExact(t1[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                        if (!r) { mis++; break; }
                        r = TimeSpan.TryParseExact(t1[2], @"hh\:mm", null, out time);
                        if (!r) { mis++; break; }
                        r = int.TryParse(t1[3], out flightNumber);
                        if (!r) { mis++; break; }
                        if (t1[4].Length != 3 || t1[5].Length != 3) { mis++; break; }
                        r = int.TryParse(t1[6], out aircraftID);
                        if (!r) { mis++; break; }
                        r = double.TryParse(t1[7].Replace('.', ','), out economyPrice);
                        if (!r) { mis++; break; }
                        if (t1[8] == "OK\r")
                            confirmed = true;
                        else if (t1[8] == "CANCELED\r")
                            confirmed = false;
                        else { mis++; break; }

                        if (t1[0] == "ADD")
                        {
                            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/AddFlight?date={date.ToString("yyyy-MM-dd")}&time={t1[2]}&flightNumber={flightNumber}&from={t1[4]}&to={t1[5]}&aircraftID={aircraftID}&economyPrice={economyPrice}&confirmed={confirmed.ToString().ToLower()}";
                            Console.WriteLine(apiUrl);
                            HttpClient client = new HttpClient();
                            try
                            {
                                var responseMessage = await client.PostAsync(apiUrl, null);
                                if (responseMessage.IsSuccessStatusCode)
                                {
                                    suc++;
                                }
                                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Conflict)
                                {
                                    dup++;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
                                mis++; break;
                            }
                        }
                        else if (t1[0] == "EDIT")
                        {
                            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/PutFl?date={date.ToString("yyyy-MM-dd")}&time={t1[2]}&flightNumber={flightNumber}&from={t1[4]}&to={t1[5]}&aircraftID={aircraftID}&economyPrice={economyPrice}&confirmed={confirmed.ToString().ToLower()}";
                            Console.WriteLine(apiUrl);
                            HttpClient client = new HttpClient();
                            try
                            {
                                var responseMessage = await client.PutAsync(apiUrl, null);
                                if (responseMessage.IsSuccessStatusCode)
                                {
                                    suc++;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
                                mis++; break;
                            }
                        }
                        else
                        {
                            mis++; break;
                        }

                    } while (false);
                }
                sucLB.Content = $"Successful Changes Applied: {suc}";
                dupLB.Content = $"Duplicate Records Discarded: {dup}";
                misLB.Content = $"Record with missing flieds discarded: {mis}";
            }
            MessageBox.Show("Файл открыт");
        }
    }
}
