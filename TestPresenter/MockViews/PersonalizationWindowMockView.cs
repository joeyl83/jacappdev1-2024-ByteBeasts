using CalendarUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestPresenter.MockViews
{
    public class PersonalizationWindowMockView : PersonalizationInterface
    {
        public bool calledChangeBackgroundColor;
        public bool calledChangeFontColor;
        public bool calledChangeBorderColor;
        public bool calledChangeForegroundColor;

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
