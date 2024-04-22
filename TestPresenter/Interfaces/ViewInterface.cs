using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarUI.Interfaces
{
    public interface ViewInterface : PersonalizationInterface
    {
        void NewCalendar(string directory, string filename);
        void OpenExistingCalendar(string filepath);
        void ShowError(string message);
        void ChangeWindow();
    }
}
