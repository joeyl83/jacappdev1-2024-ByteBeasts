using CalendarUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterTest.MockViews
{
    public class CategoriesWindowMockView : CategoriesViewInterface
    {
        public bool calledAddCategory;
        public bool calledShowError;
        public void AddCategory()
        {
            calledAddCategory = true;
        }

        public void ShowError(string message)
        {
            calledShowError = true;
        }
    }
}
