using CalendarUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterTest.MockViews
{
    public class MainWindowMockView : ViewInterface
    {
        public bool calledChangeWindow;
        public bool calledNewCalendar;
        public bool calledOpenExistingCalendar;
        public bool calledShowError;
        public void ChangeWindow()
        {
            calledChangeWindow = true;
        }

        public void NewCalendar(string directory, string filename)
        {
            calledNewCalendar = true;
        }

        public void OpenExistingCalendar(string filepath)
        {
            calledOpenExistingCalendar = true;
        }

        public void ShowError(string message)
        {
            calledShowError = true;
        }
    }
}
