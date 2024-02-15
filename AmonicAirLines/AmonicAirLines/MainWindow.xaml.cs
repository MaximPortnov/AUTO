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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using AmonicAirLines.page;
using System.Diagnostics;

namespace AmonicAirLines
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private static Dictionary<string, int> loginAttempts = new Dictionary<string, int>();
        private static Dictionary<string, DateTime> lockedUsers = new Dictionary<string, DateTime>();

        public MainWindow()
        {
            InitializeComponent();
            if (Debugger.IsAttached)
            {
                mainFrame.Navigate(new page.Debug());
            }
            else
            {
                mainFrame.Navigate(new page.StartPage());
            }

        }

        


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Вызов метода при открытии окна
            Console.WriteLine("load");
            AppControle.loadObj();
            //AppControle.obj.printSession();
            if (AppControle.obj.lastSessionIsCrash())
            {
                CrashDialogWindow crashDialogWindow = new CrashDialogWindow();
                crashDialogWindow.ShowDialog();
                //MessageBoxResult res = MessageBox.Show("краш произошел из за системной ошибки?", "краш", MessageBoxButton.YesNo);
                AppControle.obj.setReasonCrash(crashDialogWindow.Result == "System", crashDialogWindow.CrashDescription);


            }
            AppControle.obj.printSession();

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Вызов метода при закрытии окна
            Console.WriteLine("cloas");
            AppControle.saveObj();
        }


    }
}
