using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        // private EventsViewInterface categoryView;
        private HomeCalendar model;
        public Presenter(ViewInterface v)
        {
            view = v;
        }

        public void InitlializeHomeCalendar(string databaseFilename, bool newDB)
        {
            model = new HomeCalendar(databaseFilename + ".db", newDB);
        }
        public void ProcessAddCategory(string categoryName, Category.CategoryType type)
        {
            model.categories.Add(categoryName, type);
            categoryView.AddCategory();
        }
        public void InitializeCategoryView(CategoriesViewInterface view)
        {
            categoryView = view;
        }
    }
}
