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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AmonicAirLines
{
    /// <summary>
    /// Логика взаимодействия для UserWindow1.xaml
    /// </summary>
    public partial class UserWindow1 : Window
    {
        private DispatcherTimer timer;

        public UserWindow1()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Обновление времени каждую секунду
            timer.Tick += Timer_Tick;
            timer.Start();
            timeTextBlock.Text = "Test text";
            timeTextBlock.Visibility = Visibility.Visible;

        }
        public void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(AppControle.obj.getNowTotalTime());
            timeTextBlock.Text = "общее время в системе" + AppControle.obj.getNowTotalTime().ToString(@"hh\:mm\:ss"); // Установка текущего времени в TextBlock
        }
    }
}
