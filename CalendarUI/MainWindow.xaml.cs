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
            InitializeComponent();          
            presenter = new Presenter(this);
            
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
           EventsWindow window= new EventsWindow(presenter);
           window.Show();
           this.Close();
        }

    }
}