using CalendarUI;
using CalendarUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPresenter.MockViews
{
    public class MainWindowMockView : ViewInterface
    {
        public bool calledChangeWindow;
        public bool calledNewCalendar;
        public bool calledOpenExistingCalendar;
        public bool calledShowError;
        public bool calledChangeBackgroundColor;
        public bool calledChangeFontColor;
        public bool calledChangeBorderColor;
        public bool calledChangeForegroundColor;

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
