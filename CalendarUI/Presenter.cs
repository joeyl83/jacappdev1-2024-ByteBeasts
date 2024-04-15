using System;
using System.Collections.Generic;
using System.IO;
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

        public void NewHomeCalendar(string directory, string fileName)
        {
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            model = new HomeCalendar(directory + "/" + fileName + ".db", true);
        }

        public void OpenHomeCalendar(string filepath)
        {
            model = new HomeCalendar(filepath);
        }
    }
}
