using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;

namespace CalendarUI
{
    public interface EventViewInterface
    {
        void ShowError(string error);
        void AddEvent();
        void ClearEventDetails();
        void SetEvent(Event theEvent);
        void LoadCategories(string[] categories);
        
    }
}
