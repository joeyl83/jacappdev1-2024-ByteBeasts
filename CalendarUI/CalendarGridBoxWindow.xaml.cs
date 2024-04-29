using Calendar;
using CalendarUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
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

namespace CalendarUI
{
    /// <summary>
    /// Interaction logic for CalendarGridBoxWindow.xaml
    /// </summary>
    public partial class CalendarGridBoxWindow : Window, GridViewInterface
    {
        private Presenter _presenter;
        public CalendarGridBoxWindow(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.InitializeGridBoxView(this);
            _presenter.GetCalendarItems();
        }

        public void LoadByACategory(List<CalendarItemsByCategory> itemsByCategory)
        {
            throw new NotImplementedException();
        }


        public void LoadByMonth(List<CalendarItemsByMonth> itemsByMonth)
        {
            throw new NotImplementedException();
        }

        public void LoadByMonthAndCategory()
        {
            throw new NotImplementedException();
        }

        public void LoadCalendarItems(List<CalendarItem> calendarItems)
        {
   
            GridCalendarItems.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Start Date";
            column.Binding = new Binding("StartDateTime");
            column.Binding.StringFormat = "MM/dd/yy";
            GridCalendarItems.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Start Time";
            column.Binding = new Binding("StartDateTime");
            column.Binding.StringFormat = "0:H:mm:ss";
            GridCalendarItems.Columns.Add(column);

            AddColumn("Category","Category");
            AddColumn("Description","ShortDescription");
            AddColumn("Duration","DurationInMinutes");
            AddColumn("Busy Time","BusyTime");
            GridCalendarItems.ItemsSource= calendarItems;
        }
        public void AddColumn(string header,string property)
        {
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = header;
            column.Binding = new Binding(property);
            GridCalendarItems.Columns.Add(column);
        }
        public void LoadCategories()
        {
            throw new NotImplementedException();
        }

        public void LoadDates()
        {
            throw new NotImplementedException();
        }

        private void Btn_AddCategoryAndEvent(object sender, RoutedEventArgs e)
        {
            HomePage homePage = new HomePage(_presenter);
            homePage.ShowDialog();
        }

        private void Btn_AddEvent(object sender, RoutedEventArgs e)
        {
            OpenEventWindow();
        }

        private void Btn_AddCategory(object sender, RoutedEventArgs e)
        {
            OpenCategoryWindow();
        }

        private void Btn_Personalize(object sender, RoutedEventArgs e)
        {
            OpenPersonalizationWindow();
        }


        public void OpenCategoryWindow()
        {
            CategoriesWindow categories = new CategoriesWindow(_presenter);
            categories.ShowDialog();
        }

        public void OpenEventWindow()
        {
            EventsWindow events = new EventsWindow(_presenter);
            events.ShowDialog();
        }

        public void OpenPersonalizationWindow()
        {
            PersonalizationWindow personalization = new PersonalizationWindow(_presenter);
            personalization.ShowDialog();
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get the selected event
            var selectedEvent = (Event)GridCalendarItems.SelectedItem;

            // Open the EventsWindow with the selected event
            var eventsWindow = new EventsWindow(selectedEvent);
            eventsWindow.ShowDialog();
        }
    }
}
