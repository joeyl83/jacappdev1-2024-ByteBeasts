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
        private readonly Presenter _presenter;
        public CategoriesWindow(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.InitializeCategoryView(this);
            LoadComboBox();
        }

        public void LoadComboBox()
        {
            int count = 0;
            foreach (string category in Enum.GetNames(typeof(Category.CategoryType)))
            {
                count++;
                CategoryType.Items.Add($"{category}:{count}");
            }
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

        public void SetError(string message)
        {
            ErrorMessage.Text = message;
        }

        private void Btn_AddCategory(object sender, RoutedEventArgs e)
        {
            int typenumber = 0;
            string[] array = CategoryType.Text.Split(':');
            typenumber = Int32.Parse(array[1].Trim());
            Category.CategoryType type = (Category.CategoryType)typenumber;
            Sucess.Visibility = Visibility.Collapsed;
            _presenter.ProcessAddCategory(CategoryName.Text,type);
        }
    }
}
