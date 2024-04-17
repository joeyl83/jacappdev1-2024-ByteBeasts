using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;

namespace CalendarUI.Interfaces
{
    internal interface EventViewInterface
    {
        void ShowError(string error);
        void AddEvent();
        void ClearEvent();
        void ClearEvents();
        void SetEvents(List<Event> events);
        void SetEvent(Event theEvent);
        void SetDate(DateTime date);
        void SetDuration(double duration);
        void SetDetails(string details);
        void SetCategoryID(int categoryID);
        void SetEventID(int eventID);
        void SetError(string error);
        void ClearError();
        void ClearDate();
        void ClearDuration();
        void ClearDetails();
        void ClearCategoryID();
        void ClearEventID();    
    }
}
