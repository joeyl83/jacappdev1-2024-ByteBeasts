using Calendar;
using CalendarUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
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
using CalendarItem = Calendar.CalendarItem;

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
            LoadPersonalization();
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
            CheckSearch();
        }

        public void CheckSearch()
        {
            if (GridCalendarItems.Items.Count == 0)
            {
                SearchInput.Visibility = Visibility.Collapsed;
            }
            else
            {
                SearchInput.Visibility = Visibility.Visible;
            }
        }
        public void LoadByMonth(List<CalendarItemsByMonth> itemsByMonth)
        {
            GridCalendarItems.Columns.Clear();
            AddColumn("Month", "Month");
            AddColumn("Busy Time", "TotalBusyTime");
            GridCalendarItems.ItemsSource=itemsByMonth;
            CheckSearch();
        }

        public void LoadByMonthAndCategory(List<Dictionary<string, object>> items)
        {
            GridCalendarItems.Columns.Clear();
            AddColumn("Month", "[Month]");

            List<Category> categoryList = _presenter.GetCategoriesList();
            foreach(Category c in categoryList)
            {
                AddColumn($"{c.Description}", $"[{c.Description}]");
            }
            AddColumn("Total Busy Time", "[TotalBusyTime]");
            GridCalendarItems.ItemsSource = items;
            CheckSearch();
        }

        public void LoadCalendarItems(List<Calendar.CalendarItem> calendarItems)
        {
            GridCalendarItems.ItemsSource = null;
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
            CheckSearch();
        }

        public void ModifiedFiltersEvent(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDateElement.SelectedDate;
            DateTime? endDate = EndDateElement.SelectedDate;
            
            _presenter.ProcessFilters(startDate, endDate, MonthCheckBox.IsChecked, ByCategoryCheckBox.IsChecked, CategoryComboBox.SelectedItem, FilterByACategory.IsChecked);
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
            AddColumn("Busy Time","TotalBusyTime");
            GridCalendarItems.ItemsSource = itemsByCategory;
            CheckSearch();
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


        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Get the selected event
                var selectedEvent = (CalendarItem)GridCalendarItems.SelectedItem;

                // Open the EventsWindow with the selected event
                var eventsWindow = new EventsWindow(_presenter, selectedEvent);
                eventsWindow.ShowDialog();
            }
            catch
            {
                //display error message
                MessageBox.Show("Cannot perform this action.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

 
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the selected event
                var selectedEvent = (CalendarItem)GridCalendarItems.SelectedItem;

                // Delete the selected event
                _presenter.ProcessDeleteEvent(selectedEvent.EventID);
            }
            catch
            {
                //display error message
                MessageBox.Show("Cannot perform this action.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

                if (child is Grid grid1)
                {
                    foreach (var child2 in grid1.Children)
                    {
                        if (child2 is GroupBox groupBox)
                        {
                            groupBox.Foreground = new SolidColorBrush(color);

                            if (groupBox.Content is Panel panel1)
                            {
                                foreach (var child3 in panel1.Children)
                                {
                                    if (child3 is CheckBox check)
                                    {
                                        check.Foreground = new SolidColorBrush(color);
                                    }
                                }
                            }
                        }

                        if (child2 is Panel panel2)
                        {
                            foreach (var child3 in panel2.Children)
                            {
                                if (child3 is Button button)
                                {
                                    button.Foreground = new SolidColorBrush(color);
                                }
                            }
                        }
                    }
                }

                if (child is DataGrid dataGrid)
                {
                    Style cellStyle = new Style(typeof(DataGridCell));
                    cellStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(color)));
                    GridCalendarItems.CellStyle = cellStyle;
                }
            }
        }

        public void ChangeBorderColor(System.Windows.Media.Color color)
        {
            this.BorderBrush = new SolidColorBrush(color);

            foreach (var child in mainGrid.Children)
            {

                if (child is Grid grid1)
                {
                    foreach (var child2 in grid1.Children)
                    {
                        if (child2 is GroupBox groupBox)
                        {
                            groupBox.BorderBrush = new SolidColorBrush(color);
                        }

                        if (child2 is Panel panel)
                        {
                            foreach (var child3 in panel.Children)
                            {
                                if (child3 is Button button)
                                {
                                    button.BorderBrush = new SolidColorBrush(color);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ChangeForegroundColor(System.Windows.Media.Color color)
        {
            foreach (var child in mainGrid.Children)
            {
                if (child is Grid grid1)
                {
                    foreach (var child2 in grid1.Children)
                    {
                        if (child2 is GroupBox groupBox)
                        {
                            groupBox.Background = new SolidColorBrush(color);
                        }

                        if (child2 is Panel panel)
                        {
                            foreach (var child3 in panel.Children)
                            {
                                if (child3 is Button button)
                                {
                                    button.Background = new SolidColorBrush(color);
                                }
                            }
                        }
                    }
                }

                if (child is DataGrid dataGrid)
                {
                    dataGrid.Background = new SolidColorBrush(color);
                    Style rowStyle = new Style(typeof(DataGridRow));
                    rowStyle.Setters.Add(new Setter(DataGridRow.BackgroundProperty, new SolidColorBrush(color)));
                    GridCalendarItems.RowStyle = rowStyle;
                }
            }
        }

        public void LoadPersonalization()
        {
            ChangeBackground(Presenter.BackgroundColor);
            ChangeFontColor(Presenter.FontColor);
            ChangeBorderColor(Presenter.BorderColor);
            ChangeForegroundColor(Presenter.ForegroundColor);
        }

        private void Btn_Search(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(SearchInput.Text))
            {
                MessageBox.Show("Please input something to search for.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (GridCalendarItems.Items.Count == 0)
            {
                MessageBox.Show("No Calendar Items To Search For.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string search = SearchInput.Text.ToLower();
                List<CalendarItem> itemsList = new List<CalendarItem>();
                List<CalendarItem> newList = new List<CalendarItem>();
                CalendarItem foundItem = null;
                bool shouldContinue = true;
                foreach (var gridItem in GridCalendarItems.Items) 
                { 
                    CalendarItem calendarItem = gridItem as CalendarItem;

                    itemsList.Add(calendarItem);
                }

                if(GridCalendarItems.SelectedItem != null)
                {
                    int index = GridCalendarItems.SelectedIndex + 1;
                    for (int i = index; i < itemsList.Count; i++)
                    {
                        if(int.TryParse(search,out int duration))
                        {
                            if (itemsList[i].DurationInMinutes == duration)
                            {
                                foundItem = itemsList[i];
                                shouldContinue = false; 
                                break;
                            }
                        }
                        else if (itemsList[i].ShortDescription.ToLower() == search)
                        {
                            foundItem = itemsList[i];
                            shouldContinue = false;
                            break;
                        }
                    }
                    if(shouldContinue)
                    {
                        for (int i = 0; i < index; i++)
                        {
                            CalendarItem item = itemsList[i];
                            if (int.TryParse(search, out int duration))
                            {
                                if (itemsList[i].DurationInMinutes == duration)
                                {
                                    foundItem = itemsList[i];
                                    break;
                                }
                            }
                            else if (itemsList[i].ShortDescription.ToLower() == search)
                            {
                            
                                foundItem = itemsList[i];
                                break;
                            }
                        }
                    }
                    if(foundItem != null)
                    {
                        newList.Add(foundItem);
                        this.LoadCalendarItems(newList);
                    }
                    else
                    {
                        MessageBox.Show("No results found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }

            }
        }
    }
}
