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
using Calendar;
namespace CalendarUI
{
    /// <summary>
    /// Interaction logic for CategoriesWindow.xaml
    /// </summary>
    public partial class CategoriesWindow : Window, CategoriesViewInterface
    {
        private readonly CategoriesPresenter _presenter;
        public CategoriesWindow()
        {
            InitializeComponent();
        }

        public void AddCategory()
        {
           Sucess.Visibility = Visibility.Visible;
        }

        public void ClearError()
        {
            throw new NotImplementedException();
        }

        public void RemoveCategory()
        {
            throw new NotImplementedException();
        }

        public void SetError()
        {
            throw new NotImplementedException();
        }

        private void Btn_AddCategory(object sender, RoutedEventArgs e)
        {
            Sucess.Visibility = Visibility.Collapsed;

        }
    }
}
