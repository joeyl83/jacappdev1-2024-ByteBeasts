using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;
using CalendarUI;
using CalendarUI.Interfaces;
namespace TestPresenter.MockViews
{
    internal class GridViewWindowMockView : GridViewInterface
    {
        public bool calledGroupByCategories;
        public bool calledLoadByACategory;
        public bool calledLoadByMonth;
        public bool calledLoadByMonthAndCategory;
        public bool calledLoadCalendarItems;
        public bool calledLoadCategories;
        public bool calledChangeBackground;
        public bool calledChangeBackgroundColor;
        public bool calledChangeForeground;
        public bool calledChangeBorderColor;
        public bool calledChangeFontColor;
        public bool calledChangeForegroundColor;
        public void ChangeBackground(System.Windows.Media.Color color)
        {
            calledChangeBackgroundColor = true;
        }

        public void ChangeBackgroundColor(System.Windows.Media.Color color)
        {
            calledChangeBackgroundColor = true;
        }

        public void ChangeBorderColor(System.Windows.Media.Color color)
        {
            calledChangeBorderColor = true;
        }

        public void ChangeFontColor(System.Windows.Media.Color color)
        {
            calledChangeFontColor=true;
        }

        public void ChangeForeground(System.Windows.Media.Color color)
        {
           calledChangeForeground = true;
        }

        public void ChangeForegroundColor(System.Windows.Media.Color color)
        {
            calledChangeForegroundColor = true; 
        }

        public void GroupByCategories(List<CalendarItemsByCategory> itemsByCategory)
        {
           calledGroupByCategories = true;
        }

        public void LoadByACategory(List<CalendarItemsByCategory> itemsByCategory)
        {
           calledLoadByACategory = true;
        }

        public void LoadByMonth(List<CalendarItemsByMonth> itemsByMonth)
        {
           calledLoadByMonth = true;
        }

        public void LoadByMonthAndCategory(List<Dictionary<string, object>> items)
        {
           calledLoadByMonthAndCategory = true;
        }

        public void LoadCalendarItems(List<CalendarItem> calendarItems)
        {
            calledLoadCalendarItems = true;
        }

        public void LoadCategories(List<Category> categories)
        {
            calledLoadCategories = true;
        }
    }
}
