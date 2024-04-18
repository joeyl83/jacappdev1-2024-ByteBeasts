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
        }

        public void AddEvent()
        {
            throw new NotImplementedException();
        }

        public void ClearEventDetails()
        {
            throw new NotImplementedException();
        }

        public void SetEvent(Event theEvent)
        {
            throw new NotImplementedException();
        }

        public void ShowError(string error)
        {
            throw new NotImplementedException();
        }

        private void addEventButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
