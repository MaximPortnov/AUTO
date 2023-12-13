using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace AmonicAirLines
{
    public class DataItem
    {
        public DateTime Date { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public string TimeSpentOnSystem { get; set; }
        public string UnsuccessfulLogoutReason { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private DispatcherTimer timer;
        public UserWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Обновление времени каждую секунду
            timer.Tick += Timer_Tick;
            timer.Start();
            timeTextBlock.Text = "общее время в системе: 00:00:00";
            crashTextBlock.Text = AppControle.obj.getCountCrashes().ToString();


            List<DataItem> dataItems = AppControle.obj.sessions
                .Take(AppControle.obj.sessions.Count - 1)
                .Select(session => new DataItem
            {
                Date = session.startTime.Date,
                LoginTime = session.startTime.TimeOfDay != TimeSpan.Zero ? session.startTime.ToString("HH:mm:ss") : "--",
                LogoutTime = session.endTime.TimeOfDay != TimeSpan.Zero ? session.endTime.ToString("HH:mm:ss") : "--",
                TimeSpentOnSystem = session.totalTime.ToString(),
                UnsuccessfulLogoutReason = session.crash ? session.crashDescription : string.Empty
            }).ToList();
            ActivitiesGrid.ItemsSource = dataItems;
        }
        public void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(AppControle.obj.getNowTotalTime());
            timeTextBlock.Text = "общее время в системе: " + AppControle.obj.getNowTotalTime().ToString(@"hh\:mm\:ss"); // Установка текущего времени в TextBlock
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
            timer.Stop();

        }

        private void Window_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
        }
    }
}
