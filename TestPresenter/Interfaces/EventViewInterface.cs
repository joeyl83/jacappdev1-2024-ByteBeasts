using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;
using CalendarUI.Interfaces;

namespace CalendarUI
{
    public interface EventViewInterface: PersonalizationInterface
    {
        void ShowError(string error);
        void AddEvent();
        void ClearEventDetails();
        void LoadCategories(List<string> categories);

    }
 
}
