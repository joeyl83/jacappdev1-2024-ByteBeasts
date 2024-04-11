using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;

namespace CalendarUI
{
    public class Presenter
    {
        private readonly ViewInterface view;
        private HomeCalendar model;
        public Presenter(ViewInterface v)
        {
            view = v;
        }

        public void InitlializeHomeCalendar(string databaseFilename, bool newDB)
        {
            model = new HomeCalendar(databaseFilename, newDB);
        }
    }
}
