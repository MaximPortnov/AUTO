﻿using System;
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
            timeTextBlock.Visibility = Visibility.Visible;

        }
        public void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(AppControle.obj.getNowTotalTime());
            timeTextBlock.Text = "общее время в системе: " + AppControle.obj.getNowTotalTime().ToString(@"hh\:mm\:ss"); // Установка текущего времени в TextBlock
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {


            Window window = new MainWindow();
            this.Close();
            window.Show();
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
