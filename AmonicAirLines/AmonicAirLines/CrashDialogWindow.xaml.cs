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

namespace AmonicAirLines
{
    /// <summary>
    /// Логика взаимодействия для CrashDialogWindow.xaml
    /// </summary>
    public partial class CrashDialogWindow : Window
    {
        public string Result { get; private set; }
        public string CrashDescription { get; private set; }

        public CrashDialogWindow()
        {
            InitializeComponent();
        }

        private void SystemButton_Click(object sender, RoutedEventArgs e)
        {
            Result = "System";
            CrashDescription = CrashDescriptionTextBox.Text;
            Close(); // Закрываем окно после выбора
        }

        private void SoftwareButton_Click(object sender, RoutedEventArgs e)
        {
            Result = "Software";
            CrashDescription = CrashDescriptionTextBox.Text;
            Close(); // Закрываем окно после выбора
        }
    }
}
