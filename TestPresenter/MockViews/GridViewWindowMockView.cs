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
