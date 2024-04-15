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

        public void NewCalendar(string directory, string filename)
        {
            presenter.InitlializeHomeCalendar(directory, filename, true);
        }

        public void ChangeWindow()
        {
            throw new NotImplementedException();
        }
    }
}