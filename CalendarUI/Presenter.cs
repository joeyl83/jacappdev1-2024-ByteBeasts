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
        private CategoriesViewInterface categoryView;
        private EventViewInterface eventView;
        // private EventsViewInterface categoryView;
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
            string extension = Path.GetExtension(filepath);
            if(extension == ".db")
            {
                model = new HomeCalendar(filepath);
            }
            else
            {
                view.ShowError("Invalid file");
            }
        }
        public void ProcessAddCategory(string categoryName, Category.CategoryType type)
        {
            try
            {
                model.categories.Add(categoryName, type);
                categoryView.AddCategory();
            }
            catch (Exception ex)
            {
                categoryView.ShowError(ex.Message);
            }
           
        }
        public void ProcessAddEvent(DateTime StartDateTime,double DurationInMinutes,string Details,int CatId)
        {
            try
            {
                model.events.Add(StartDateTime,CatId,DurationInMinutes, Details);
                eventView.AddEvent();
            }
            catch(Exception ex)
            {
                eventView.ShowError(ex.Message);
            }
        }
        public void InitializeCategoryView(CategoriesViewInterface view)
        {
            categoryView = view;
        }
        public void InitializeEventView(EventViewInterface view)
        {
            eventView = view;
        }
    }
}
