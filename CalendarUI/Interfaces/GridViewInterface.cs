using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace CalendarUI.Interfaces
{
    public interface GridViewInterface : PersonalizationInterface
    {
        public void GroupByCategories(List<CalendarItemsByCategory> itemsByCategory);

        public void LoadByACategory(List<CalendarItemsByCategory> itemsByCategory);

        public void LoadByMonth(List<CalendarItemsByMonth> itemsByMonth);

        public void LoadByMonthAndCategory(List<Dictionary<string, object>> items);

        public void LoadCalendarItems(List<Calendar.CalendarItem> calendarItems);

        // Added by Lucas
        void OpenEventWindow();
        void OpenCategoryWindow();
        void OpenPersonalizationWindow();

        public void LoadCategories(List<Category> categories);

    }
}
