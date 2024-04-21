using CalendarUI;
using System;
using System.Collections.Generic;
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
    }
}
