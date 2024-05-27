using Calendar;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CalendarUI
{
    /// <summary>
    /// Interaction logic for EventsWindow.xaml
    /// </summary>
    public partial class EventsWindow : Window, EventViewInterface
    {
        private Presenter _presenter;
        private CalendarItem _calendarItem;
        public EventsWindow(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.InitializeEventView(this);
            _presenter.LoadCategories(1);
            LoadTimes();

            LoadPersonalization();
            AddEventConfirm.Visibility = Visibility.Visible;
        }

        public EventsWindow(Presenter presenter, CalendarItem selectedEvent)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.InitializeEventView(this);
            _presenter.LoadCategories(1);
            LoadTimes();

            LoadPersonalization();
            EditEventConfirm.Visibility = Visibility.Visible;

            _calendarItem = selectedEvent;

            PrefillEventData();
        }

        public void AddEvent()
        {
            EventSuccess.Visibility = Visibility.Visible;
            ClearEventDetails();
        }

        public void LoadTimes()
        {

           //Loop to add all hours from 1 - 24 (french time)
           for (int i = 0; i < 24; i++)
            {
                StartTime.Items.Add($"{i}" + ":00 ");
            }
        }
        public void ClearEventDetails()
        {
            StartDate.SelectedDate = null;
            Duration.Clear();
            Details.Clear();
            StartTime.SelectedIndex = -1;
            CatComboBox.SelectedIndex = -1;
        }

        public void ShowError(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void addEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StartDate.Text))
            {
                ShowError("Fields empty.");
            }
            else if (string.IsNullOrWhiteSpace(CatComboBox.Text))
            {
                ShowError("Fields empty.");
            }
            else if (string.IsNullOrWhiteSpace(Duration.Text))
            {
                ShowError("Fields empty.");
            }
            else if (string.IsNullOrWhiteSpace(Details.Text))
            {
                ShowError("Fields empty.");
            }
            else if (!int.TryParse(Duration.Text, out int result))
            {
                ShowError("Duration must be an integer.");
            }
            else
            {
                Category category = CatComboBox.SelectedItem as Category;
                string[] array = StartTime.Text.Split(":");
                int hourStart = Int32.Parse(array[0].Trim());
                DateTime dateTime = (DateTime)StartDate.SelectedDate;             
                _presenter.ProcessAddEvent(dateTime.AddHours(hourStart), Double.Parse(Duration.Text), Details.Text, category.Id);
            }

        }

        public void LoadCategories(List<Category> categories)
        {
            CatComboBox.Items.Clear();
            CatComboBox.DisplayMemberPath = "Description";
            CatComboBox.ItemsSource = categories;
        }

        private void cancelEventButton_Click(object sender, RoutedEventArgs e)
        {
            ClearEventDetails();
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
                if (child is Button button)
                {
                    button.Foreground = new SolidColorBrush(color);
                }
            }
        }
        public void ChangeBorderColor(System.Windows.Media.Color color)
        {
            this.BorderBrush = new SolidColorBrush(color);

            foreach (var child in mainGrid.Children)
            {
                if (child is Button button)
                {
                    button.BorderBrush = new SolidColorBrush(color);
                }

                if (child is GroupBox groupBox)
                {
                    groupBox.BorderBrush = new SolidColorBrush(color);

                    if (groupBox.Content is Button buttonInGroupBox)
                    {
                        buttonInGroupBox.BorderBrush = new SolidColorBrush(color);
                    }

                    else if (groupBox.Content is Panel panel)
                    {
                        foreach (var child2 in panel.Children)
                        {
                            if (child2 is Button button2)
                            {
                                button2.BorderBrush = new SolidColorBrush(color);
                            }
                            if (child2 is TextBox textBox)
                            {
                                textBox.BorderBrush = new SolidColorBrush(color);
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
                if (child is GroupBox groupBox)
                {
                    groupBox.Background = new SolidColorBrush(color);

                    if (groupBox.Content is Button buttonInGroupBox)
                    {
                        buttonInGroupBox.Background = new SolidColorBrush(color);
                    }
                }

                if (child is Button button)
                {
                    button.Background = new SolidColorBrush(color);
                }
            }
        }

        public void OpenAddCategoryWindow()
        {
            CategoriesWindow categories = new CategoriesWindow(_presenter);
            categories.Show();
        }

        public void AddCategoryBtn(object sender, RoutedEventArgs e)
        {
            OpenAddCategoryWindow();
        }

        private void CatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PrefillEventData()
        {
            if (_calendarItem != null)
            {
                StartDate.SelectedDate = _calendarItem.StartDateTime;
                Duration.Text = _calendarItem.DurationInMinutes.ToString();
                Details.Text = _calendarItem.ShortDescription;
                CatComboBox.SelectedIndex = (_calendarItem.CategoryID) - 1;

                int startTime = _calendarItem.StartDateTime.Hour;
                StartTime.SelectedIndex = startTime;


            }
        }


        private void EditEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (_calendarItem != null)
            {
                if (string.IsNullOrWhiteSpace(StartDate.Text))
                {
                    ShowError("Fields empty.");
                }
                else if (string.IsNullOrWhiteSpace(CatComboBox.Text))
                {
                    ShowError("Fields empty.");
                }
                else if (string.IsNullOrWhiteSpace(Duration.Text))
                {
                    ShowError("Fields empty.");
                }
                else if (string.IsNullOrWhiteSpace(Details.Text))
                {
                    ShowError("Fields empty.");
                }
                else
                {
                    Category category = CatComboBox.SelectedItem as Category;
                    string[] array = StartTime.Text.Split(":");
                    int hourStart = Int32.Parse(array[0].Trim());
                    DateTime dateTime = (DateTime)StartDate.SelectedDate;
                    int catId = category.Id;
                    _presenter.ProcessEditEvent(_calendarItem.EventID, dateTime.AddHours(hourStart), Double.Parse(Duration.Text), Details.Text, catId, _calendarItem);
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

    }




}
