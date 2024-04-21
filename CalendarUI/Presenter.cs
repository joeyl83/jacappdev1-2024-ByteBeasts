using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;
using CalendarUI.Interfaces;

namespace CalendarUI
{
    public class Presenter
    {
        private readonly ViewInterface view;
        private CategoriesViewInterface categoryView;
        private HomePageViewInterface homePageView;
        private EventViewInterface eventView;
        private PersonalizationInterface personalizationView;
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
            view.ChangeWindow();
        }

        public void OpenHomeCalendar(string filepath)
        {
            string extension = Path.GetExtension(filepath);
            if(extension == ".db")
            {
                model = new HomeCalendar(filepath);
                view.ChangeWindow();
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
        public void LoadCategoryTypes()
        {
            List<string> list=new List<string>();
            int count = 0;
            foreach (string categoryType in Enum.GetNames(typeof(Category.CategoryType)))
            {
                count++;
                list.Add($"{categoryType}:{count}");
            }
            categoryView.LoadCategoryTypes(list);
        }
        public void LoadCategories()
        {
            List<string> list = new List<string>();
            int count = 0;
            foreach (Category category in model.categories.List())
            {
                count++;
                list.Add($"{category.Description}:{count}");
            }
            eventView.LoadCategories(list);
        }
        public void InitializeEventView(EventViewInterface view)
        {
            eventView = view;
        }

        public void InitializeHomePageView(HomePageViewInterface view)
        {
            homePageView = view;
        }

        public void InitializePersonalizationWindow(PersonalizationInterface view)
        {
            personalizationView = view;
        }

        public void ProcessBackgroundColor(System.Windows.Media.Color color)
        {
            personalizationView?.ChangeBackground(color);
            eventView?.ChangeBackground(color);
            categoryView?.ChangeBackground(color);
            homePageView?.ChangeBackground(color);

            // Add more views here :
        }

        public void ProcessFontColor(System.Windows.Media.Color color)
        {
            personalizationView?.ChangeFontColor(color);
            eventView?.ChangeFontColor(color);
            categoryView?.ChangeFontColor(color);
            homePageView?.ChangeFontColor(color);

            // Add more views here :
        }

        public void ProcessBorderColor(System.Windows.Media.Color color)
        {
            personalizationView?.ChangeBorderColor(color);
            eventView?.ChangeBorderColor(color);
            categoryView?.ChangeBorderColor(color);
            homePageView?.ChangeBorderColor(color);

            // Add more views here :
        }

    }
}
