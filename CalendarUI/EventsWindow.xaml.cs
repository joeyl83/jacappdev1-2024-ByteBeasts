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
            string[] array = CatComboBox.Text.Split(':');
            int catId = Int32.Parse(array[1].Trim());
            _presenter.ProcessAddEvent((DateTime)StartDate.SelectedDate, Double.Parse(Duration.Text), Details.Text,catId);
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
