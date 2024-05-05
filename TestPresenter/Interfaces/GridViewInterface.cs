using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CalendarUI
{
    public interface GridViewInterface
    {
        public void GroupByCategories(List<CalendarItemsByCategory> itemsByCategory);

        public void LoadByACategory(List<CalendarItemsByCategory> itemsByCategory);

        public void LoadByMonth(List<CalendarItemsByMonth> itemsByMonth);

        public void LoadByMonthAndCategory(List<Dictionary<string, object>> items);

        public void LoadCalendarItems(List<Calendar.CalendarItem> calendarItems);

        public void LoadCategories(List<Category> categories);

        public void ChangeBackground(System.Windows.Media.Color color);

        public void ChangeForeground(System.Windows.Media.Color color);

        public void ChangeBackgroundColor(System.Windows.Media.Color color);

        public void ChangeForegroundColor(System.Windows.Media.Color color);

        public void ChangeFontColor(System.Windows.Media.Color color);

        public void ChangeBorderColor(System.Windows.Media.Color color);

       
    }
}
