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
            InitializeComponent();


            // Create a new instance of the EventsWindow class
            EventsWindow eventsWindow = new EventsWindow();
            eventsWindow.Show();

            presenter = new Presenter(this);
        }

        public void NewCalendarBtnClick(object sender, RoutedEventArgs e)
        {
            string input = NewCalendarTextBox.Text;
            NewCalendar(input);
            ChangeWindow();
        }

        public void NewCalendar(string filename)
        {
            presenter.InitlializeHomeCalendar(filename, true);
        }

        public void ChangeWindow()
        {
           CategoriesWindow categories = new CategoriesWindow(presenter);
           categories.Show();
           this.Close();
        }

    }
}