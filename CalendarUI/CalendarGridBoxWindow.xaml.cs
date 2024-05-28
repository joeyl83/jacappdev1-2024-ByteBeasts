using Calendar;
using CalendarUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
            AddRightJustifiedColumn("Duration", "DurationInMinutes");
            AddRightJustifiedColumn("Busy Time", "BusyTime");
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
            AddRightJustifiedColumn("Busy Time", "TotalBusyTime");
            GridCalendarItems.ItemsSource=itemsByMonth;
            CheckSearch();
        }

        public void LoadByMonthAndCategory(List<Dictionary<string, object>> items)
        {
            GridCalendarItems.Columns.Clear();
            AddRightJustifiedColumn("Month", "[Month]");

            List<Category> categoryList = _presenter.GetCategoriesList();
            foreach(Category c in categoryList)
            {
                AddRightJustifiedColumn($"{c.Description}", $"[{c.Description}]");
            }
            AddRightJustifiedColumn("Total Busy Time", "[TotalBusyTime]");
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

            AddRightJustifiedColumn("Duration","DurationInMinutes");
            AddRightJustifiedColumn("Busy Time","BusyTime");
            GridCalendarItems.ItemsSource= calendarItems;
            CheckSearch();
        }

        //Method that accepts a CalendarItem Object to be selected in the datagrid.
        public void SelectCalendarItem(int eventId, bool isDeleting)
        {

            if (!isDeleting)
            {
                foreach (var item in GridCalendarItems.Items)
                {
                    CalendarItem calendarItem = item as CalendarItem; // eventId CalendarItem with the type of items in your DataGrid
                    if (calendarItem != null && calendarItem.EventID == eventId) // Replace EventId with the property name in your item class
                    {
                        GridCalendarItems.SelectedItem = calendarItem;
                        break;
                    }
                }
                return;
            }
            else
            {
                //for (int i = 0; i < GridCalendarItems.Items.Count; i++)
                //{
                //    CalendarItem calendarItem = GridCalendarItems.Items[i] as CalendarItem;
                //    if (calendarItem != null && calendarItem.EventID == eventId)
                //    {
                //        GridCalendarItems.SelectedItem = calendarItem;

                //        // Check if there is a next item
                //        if (i + 1 < GridCalendarItems.Items.Count)
                //        {
                //            // Get the next item
                //            CalendarItem nextItem = GridCalendarItems.Items[i + 1] as CalendarItem;

                //            // Now you can do something with nextItem...
                //        }

                //        break;
                //    }
                //}
                if (eventId >= 0 && eventId < GridCalendarItems.Items.Count)
                {
                    // Select the item at the specified index
                    GridCalendarItems.SelectedIndex = eventId;
                }
            }
        }


        private void AddRightJustifiedColumn(string header, string bindingPath)
        {
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = header;
            column.Binding = new Binding(bindingPath);
            System.Windows.Media.Color fontColor = _presenter.getFontColor();

            // Create a new Style for the cells
            Style cellStyle = new Style(typeof(DataGridCell));
            cellStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            cellStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(fontColor)));

            // Apply the Style to the column
            column.CellStyle = cellStyle;

            GridCalendarItems.Columns.Add(column);
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
            AddRightJustifiedColumn("Busy Time","TotalBusyTime");
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
                int selectedIndex = GridCalendarItems.SelectedIndex;

                // Delete the selected event
                _presenter.ProcessDeleteEvent(selectedEvent.EventID, selectedIndex);
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
                MessageBox.Show("Please input a valid search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (GridCalendarItems.Items.Count == 0)
            {
                MessageBox.Show("No Calendar Items To Search For.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(MonthCheckBox.IsChecked ?? true)
            {
                MessageBox.Show("Can't search while sorting by month.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(ByCategoryCheckBox.IsChecked ?? true)
            {
                MessageBox.Show("Can't search while sorting by category.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        else if (itemsList[i].ShortDescription.ToLower().Contains(search))
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
                            else if (itemsList[i].ShortDescription.ToLower().Contains(search))
                            {
                            
                                foundItem = itemsList[i];
                                break;
                            }
                        }
                    } 
                }
                else
                {
                    for (int i = 0; i < itemsList.Count; i++)
                    {
                        if (int.TryParse(search, out int duration))
                        {
                            if (itemsList[i].DurationInMinutes == duration)
                            {
                                foundItem = itemsList[i];
                                break;
                            }
                        }
                        else if (itemsList[i].ShortDescription.ToLower().Contains(search))
                        else if (itemsList[i].ShortDescription.ToLower().Contains(search))
                        {
                            foundItem = itemsList[i];
                            break;
                        }
                    }
                }
                if (foundItem != null)
                {
                    GridCalendarItems.SelectedItem = foundItem;
                    GridCalendarItems.ScrollIntoView(foundItem);
                }
                else
                {
                    MessageBox.Show("No results found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
    }
}
