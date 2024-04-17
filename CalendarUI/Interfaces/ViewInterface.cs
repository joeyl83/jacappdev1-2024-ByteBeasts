using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;

namespace CalendarUI.Interfaces
{
    class ViewInterface
    {
        // This is the interface for the View class
        public interface IView : Interface1

        {
            void SetEvents(List<Event> events);
            void SetCategories(List<Category> categories);
            void SetEvent(Event theEvent);
            void SetCategory(Category theCategory);
            void SetDate(DateTime date);
            void SetDuration(double duration);
            void SetDetails(string details);
            void SetCategoryID(int categoryID);
            void SetEventID(int eventID);
            void SetError(string error);
            void ClearError();
            void ClearEvent();
            void ClearCategory();
            void ClearEvents();
            void ClearCategories();
            void ClearDate();
            void ClearDuration();
            void ClearDetails();
            void ClearCategoryID();
            void ClearEventID();
        }
    }
}
