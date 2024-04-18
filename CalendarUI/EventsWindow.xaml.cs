using Calendar;
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
    public partial class EventsWindow : Window,EventViewInterface
    {
        private Presenter _presenter;
        public EventsWindow(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.InitializeEventView(this);
            _presenter.LoadCategories();
        }

        public void AddEvent()
        {
            EventSuccess.Visibility = Visibility.Visible;
            ClearEventDetails();
        }

        public void ClearEventDetails()
        {
            StartDate.SelectedDate = DateTime.MinValue;
            Duration.Clear();
            Details.Clear();
            CatComboBox.SelectedIndex = 0;
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
                DateTime startDate = (DateTime)StartDate.SelectedDate;
                startDate.TimeOfDay=
                _presenter.ProcessAddEvent((DateTime)StartDate.SelectedDate, Double.Parse(Duration.Text), Details.Text, catId);
            }
          
        }

        public void LoadCategories(List<string> categories)
        {
            foreach (string type in categories)
            {
                CatComboBox.Items.Add(type);
            }
        }
    }
}
