using CalendarUI;
using CalendarUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPresenter.MockViews
{
    public class CategoriesWindowMockView : CategoriesViewInterface
    {
        public bool calledAddCategory;
        public bool calledShowError;
        public bool calledLoadCategoryTypes;
        public bool calledChangeBackgroundColor;
        public bool calledChangeFontColor;
        public bool calledChangeBorderColor;
        public bool calledChangeForegroundColor;

        public void AddCategory()
        {
            calledAddCategory = true;
        }

        public void ShowError(string message)
        {
            calledShowError = true;
        }

        public void LoadCategoryTypes(List<string> categoryTypes)
        {
            calledLoadCategoryTypes = true;
        }

        public void ChangeBackground(System.Windows.Media.Color color)
        {
            calledChangeBackgroundColor = true;
        }

        public void ChangeFontColor(System.Windows.Media.Color color)
        {
            calledChangeFontColor = true;
        }

        public void ChangeBorderColor(System.Windows.Media.Color color)
        {
            calledChangeBorderColor = true;
        }

        public void ChangeForegroundColor(System.Windows.Media.Color color)
        {
            calledChangeForegroundColor = true;
        }
    }
}
