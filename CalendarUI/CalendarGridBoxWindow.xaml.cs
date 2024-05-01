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
using System.Windows.Controls.Primitives;
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
            _presenter.LoadCategories(2);
        }

        public void LoadByACategory(List<CalendarItemsByCategory> itemsByCategory)
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

            AddColumn("Category", "Category");
            AddColumn("Description", "ShortDescription");
            AddColumn("Duration", "DurationInMinutes");
            AddColumn("Busy Time", "BusyTime");
            GridCalendarItems.ItemsSource = itemsByCategory[0].Items;
        }


        public void LoadByMonth(List<CalendarItemsByMonth> itemsByMonth)
        {
            GridCalendarItems.Columns.Clear();
            AddColumn("Month", "Month");
            AddColumn("Busy Time", "TotalBusyTime");
            GridCalendarItems.ItemsSource=itemsByMonth;
        }

        public void LoadByMonthAndCategory(List<Dictionary<string, object>> items)
        {
            GridCalendarItems.Columns.Clear();
            AddColumn("Month", "Month");
            AddColumn("Total Busy Time", "TotalBusyTime");
            for(int i = 0; i < items.Count; i++) 
            { 
                for(int j=0; j < items[i].Count; j++)
                {
                   
                }
            }
            AddColumn("Category", "Category");            
            GridCalendarItems.ItemsSource = items;
        }

        public void LoadCalendarItems(List<Calendar.CalendarItem> calendarItems)
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
        public void GroupByCategories(List<CalendarItemsByCategory> itemsByCategory)
        {
            GridCalendarItems.Columns.Clear();
            AddColumn("Category","Category");
            AddColumn("Total Busy Time","TotalBusyTime");
            GridCalendarItems.ItemsSource = itemsByCategory;
        }
        public void LoadCategories(List<Category> categories)
        {
            CategoryComboBox.Items.Clear();
            CategoryComboBox.DisplayMemberPath = "Description";
            CategoryComboBox.ItemsSource= categories;
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
            var selectedEvent = (CalendarItem)GridCalendarItems.SelectedItem;

            // Open the EventsWindow with the selected event
            var eventsWindow = new EventsWindow(_presenter, selectedEvent);
            eventsWindow.ShowDialog();
        }

        private void ChkBox_FilterByMonth(object sender, RoutedEventArgs e)
        {
            if(MonthCheckBox.IsChecked == true && ByCategoryCheckBox.IsChecked == true) 
            {
                _presenter.GetCalendarItemsByMonthAndCategory();
            }
            if(MonthCheckBox.IsChecked == true)
            {
                _presenter.GetCalendarItemsByMonth();
            }
            else
            {
                _presenter.GetCalendarItems();
            }
            
        }

        private void ChkBox_FilterByCategory(object sender, RoutedEventArgs e)
        {
            if (MonthCheckBox.IsChecked == true && ByCategoryCheckBox.IsChecked == true)
            {
                _presenter.GetCalendarItemsByMonthAndCategory();
            }
            if (ByCategoryCheckBox.IsChecked == true)
            {
                _presenter.GroupByCategory();
            }
            else
            {
                _presenter.GetCalendarItems();
            }
        }

        private void ChkBox_FilterByACategory(object sender, RoutedEventArgs e)
        {
            if (FilterByACategory.IsChecked == true && CategoryComboBox.SelectedItem != null)
            {
                Category category = CategoryComboBox.SelectedItem as Category;
                _presenter.FilterByACategory(category.Id);
            }
            else
            {
                _presenter.GetCalendarItems();
            }
        }
    }
}
