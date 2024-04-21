using CalendarUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterTest.MockViews
{
    public class HomePageMockView : HomePageViewInterface
    {
        public bool calledOpenAddCategoryWindow;
        public bool calledAddEventWindow;
        public void OpenAddCategoryWindow()
        {
            calledOpenAddCategoryWindow = true;
        }

        public void OpenAddEventWindow()
        {
            calledAddEventWindow = true;
        }
    }
}
