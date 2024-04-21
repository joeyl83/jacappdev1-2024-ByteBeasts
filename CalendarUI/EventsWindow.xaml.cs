using Calendar;
using CalendarUI.Interfaces;
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

namespace CalendarUI
{
    /// <summary>
    /// Interaction logic for EventsWindow.xaml
    /// </summary>
    public partial class EventsWindow : Window, EventViewInterface
    {
        private Presenter _presenter;
        public EventsWindow(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.InitializeEventView(this);
            _presenter.LoadCategories();
            LoadTimes();
        }

        public void AddEvent()
        {
            EventSuccess.Visibility = Visibility.Visible;
            ClearEventDetails();
        }

        public void LoadTimes()
        {
            for(int i = 0; i < 11; i++)
            {
                if (i == 0)
                {
                    StartTime.Items.Add($"12" + ":00AM");
                }
                StartTime.Items.Add($"{i+1}" + ":00AM");
            }
            for (int i = 0; i < 11; i++)
            {
                if (i == 0)
                {
                    StartTime.Items.Add($"12" + ":00PM");
                }
                StartTime.Items.Add($"{i + 1}" + ":00PM");
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
            if(string.IsNullOrWhiteSpace(StartDate.Text))
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
                string[] array = CatComboBox.Text.Split(':');
                int catId = Int32.Parse(array[1].Trim());
                array = StartTime.Text.Split(":");
                int hourStart = Int32.Parse(array[0].Trim());
                DateTime dateTime = (DateTime)StartDate.SelectedDate;             
                _presenter.ProcessAddEvent(dateTime.AddHours(hourStart), Double.Parse(Duration.Text), Details.Text, catId);
            }
          
        }

        public void LoadCategories(List<string> categories)
        {
            foreach (string type in categories)
            {
                CatComboBox.Items.Add(type);
            }
        }

        private void cancelEventButton_Click(object sender, RoutedEventArgs e)
        {
            ClearEventDetails();
        }

        void SetEvent(Event theEvent)
        {
            throw new NotImplementedException();
        }

        public void ChangeBackground(System.Windows.Media.Color color)
        {
            this.Background = new SolidColorBrush(color);
        }
        public void ChangeFontColor(System.Windows.Media.Color color)
        {
            this.Foreground = new SolidColorBrush(color);
        }
        public void ChangeBorderColor(System.Windows.Media.Color color)
        {
            this.BorderBrush = new SolidColorBrush(color);
        }


    }
}
