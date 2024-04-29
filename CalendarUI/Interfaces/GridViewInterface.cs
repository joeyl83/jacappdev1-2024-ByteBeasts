using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace CalendarUI.Interfaces
{
    public interface GridViewInterface
    {
        public void LoadCategories();

        public void LoadByACategory(List<CalendarItemsByCategory> itemsByCategory);

        public void LoadByMonth(List<CalendarItemsByMonth> itemsByMonth);

        public void LoadByMonthAndCategory();

        public void LoadCalendarItems(List<Calendar.CalendarItem> calendarItems);

        public void LoadDates();


        // Added by Lucas
        void OpenEventWindow();
        void OpenCategoryWindow();
        void OpenPersonalizationWindow();
    }
}
