using CalendarUI;
using CalendarUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPresenter.MockViews
{
    public class HomePageMockView : HomePageViewInterface
    {
        public bool calledOpenAddCategoryWindow;
        public bool calledAddEventWindow;
        public bool calledChangeBackgroundColor;
        public bool calledChangeFontColor;
        public bool calledChangeBorderColor;
        public bool calledChangeForegroundColor;
        public bool calledOpenPersonalizationWindow;

        public void OpenAddCategoryWindow()
        {
            calledOpenAddCategoryWindow = true;
        }

        public void OpenAddEventWindow()
        {
            calledAddEventWindow = true;
        }

        public void OpenPersonalizationWindow()
        {
            throw new NotImplementedException();
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
