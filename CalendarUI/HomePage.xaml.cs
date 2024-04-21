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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window, HomePageViewInterface
    {
        private readonly Presenter presenter;
        public HomePage(Presenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
            presenter.InitializeHomePageView(this);
        }

        public void AddEventBtnClick(object sender, RoutedEventArgs e)
        {
            OpenAddEventWindow();
        }
        public void AddCategoryBtnClick(object sender, RoutedEventArgs e)
        {
            OpenAddCategoryWindow();
        }

        public void OpenAddCategoryWindow()
        {
            CategoriesWindow categories = new CategoriesWindow(presenter);
            categories.Show();
        }

        public void OpenAddEventWindow()
        {
            EventsWindow events = new EventsWindow(presenter);
            events.Show();
        }
    }
}
