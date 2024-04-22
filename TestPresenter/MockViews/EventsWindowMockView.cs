using CalendarUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TestPresenter.MockViews
{
    public class EventsWindowMockView : EventViewInterface
    {
        public bool calledAddEvent;
        public bool calledClearEventDetails;
        public bool calledLoadCategories;
        public bool calledShowError;
        public bool calledChangeBackgroundColor;
        public bool calledChangeFontColor;
        public bool calledChangeBorderColor;
        public bool calledChangeForegroundColor;
        public bool calledOpenAddCategoryWindow;
        public void AddEvent()
        {
            calledAddEvent = true;
        }

        public void ClearEventDetails()
        {
            calledClearEventDetails = true;
        }

        public void LoadCategories(List<string> categories)
        {
            calledLoadCategories = true;
        }

        public void ShowError(string error)
        {
            calledShowError = true;
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

        public void OpenAddCategoryWindow()
        {
            calledOpenAddCategoryWindow = true;
        }
    }
}
