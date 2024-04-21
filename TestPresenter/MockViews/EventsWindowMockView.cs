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
    }
}
