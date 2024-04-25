using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Calendar;
using CalendarUI.Interfaces;

namespace CalendarUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        private readonly Presenter presenter;
        public MainWindow()
        {
            presenter = new Presenter(this);
            InitializeComponent();

            ChangeBackground(Presenter.BackgroundColor);
            ChangeFontColor(Presenter.FontColor);
            ChangeBorderColor(Presenter.BorderColor);
            ChangeForegroundColor(Presenter.ForegroundColor);
        }

        public void NewCalendarBtnClick(object sender, RoutedEventArgs e)
        {
            string calendarName = NewCalendarNameTextBox.Text;
            string calendarFolderName = NewCalendarFolderTextBox.Text;
            NewCalendar(calendarFolderName, calendarName);
        }

        public void OpenCalendarBtnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog.ShowDialog() == true)
            {
                string filepath = openFileDialog.FileName;
                OpenExistingCalendar(filepath);
            }

        }

        public void NewCalendar(string directory, string filename)
        {
            presenter.NewHomeCalendar(directory, filename);
        }

        public void OpenExistingCalendar(string filepath)
        {
            presenter.OpenHomeCalendar(filepath);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ChangeWindow()
        {
            //Remove this later.
            CalendarGridBoxWindow gridWindow = new CalendarGridBoxWindow(presenter);
            gridWindow.Show();
            this.Close();
        }

        public void ChangeBackground(System.Windows.Media.Color color)
        {
            this.Background = new SolidColorBrush(color);
        }

        public void ChangeFontColor(System.Windows.Media.Color color)
        {
            this.Foreground = new SolidColorBrush(color);

            foreach (var child in mainGrid.Children)
            {

                if (child is Button button2)
                {
                    button2.Foreground = new SolidColorBrush(color);
                }
            }
        }

        public void ChangeBorderColor(System.Windows.Media.Color color)
        {
            this.BorderBrush = new SolidColorBrush(color);
        }

        public void ChangeForegroundColor(System.Windows.Media.Color color)
        {
            foreach (var child in mainGrid.Children)
            {
                if (child is Button button2)
                {
                    button2.Background = new SolidColorBrush(color);
                }
            }
        }
    }
}
