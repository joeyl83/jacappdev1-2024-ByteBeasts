using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarUI
{
    public interface ViewInterface
    {
        void NewCalendar(string directory, string filename);
        void ChangeWindow();
    }
}
