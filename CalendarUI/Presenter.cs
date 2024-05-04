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
        public static System.Windows.Media.Color BorderColor { get; set; }
        public static System.Windows.Media.Color BackgroundColor { get; set; } 
        public static System.Windows.Media.Color ForegroundColor { get; set; }
        public static System.Windows.Media.Color FontColor { get; set; }

        private readonly ViewInterface view;
        private CategoriesViewInterface categoryView;
        private HomePageViewInterface homePageView;
        private EventViewInterface eventView;
        private PersonalizationInterface personalizationView;
        private GridViewInterface gridView;

        //details of the last added event:
        private DateTime lastStartDate;
        private double lastDuration;
        private string lastDetails;
        private int lastCatId;

        
        // private EventsViewInterface categoryView;
        private HomeCalendar model;
        public Presenter(ViewInterface v)
        {
            defaultColors();
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
                if(StartDateTime == lastStartDate && DurationInMinutes == lastDuration && Details == lastDetails && CatId == lastCatId)
                {
                    eventView.ShowError("Warning: the event that you are trying to add is identical as the previous one added.");
                    lastStartDate = new DateTime();
                    lastDuration = 0;
                    lastDetails = "";
                    lastCatId = 0;
                }
                else
                {
                    model.events.Add(StartDateTime, CatId, DurationInMinutes, Details);
                    eventView.AddEvent();
                    lastStartDate = StartDateTime;
                    lastDuration = DurationInMinutes;
                    lastDetails = Details;
                    lastCatId = CatId;
                }
            }
            catch(Exception ex)
            {
                eventView.ShowError(ex.Message);
            }
        }

        public void ProcessEditEvent(int eventID, DateTime StartDateTime, double DurationInMinutes, string Details, int CatId)
        {
            try
            {
                if (StartDateTime == lastStartDate && DurationInMinutes == lastDuration && Details == lastDetails && CatId == lastCatId)
                {
                    eventView.ShowError("Warning: the event that you are trying to add is identical as the previous one added.");
                    lastStartDate = new DateTime();
                    lastDuration = 0;
                    lastDetails = "";
                    lastCatId = 0;
                }
                else
                {
                    model.events.UpdateProperties(eventID, StartDateTime, CatId, DurationInMinutes, Details);
                    eventView.AddEvent();
                    lastStartDate = StartDateTime;
                    lastDuration = DurationInMinutes;
                    lastDetails = Details;
                    lastCatId = CatId;
                }
            }
            catch (Exception ex)
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
        public void LoadCategories(int check)
        {        
            if (check == 1)
            {
                eventView.LoadCategories(model.categories.List());
            }
            else if (check == 2)
            {
                gridView.LoadCategories(model.categories.List());
            }
           
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
        public void InitializeGridBoxView(GridViewInterface view)
        {
            gridView = view;
        }
        public void ProcessBackgroundColor(System.Windows.Media.Color color)
        {
            BackgroundColor = color;
            view.ChangeBackground(color);
            personalizationView?.ChangeBackground(color);
            eventView?.ChangeBackground(color);
            categoryView?.ChangeBackground(color);
            homePageView?.ChangeBackground(color);

            // Add more views here :
        }

        public void ProcessFontColor(System.Windows.Media.Color color)
        {
            FontColor = color;
            view.ChangeFontColor(color);
            personalizationView?.ChangeFontColor(color);
            eventView?.ChangeFontColor(color);
            categoryView?.ChangeFontColor(color);
            homePageView?.ChangeFontColor(color);

            // Add more views here :
        }

        public void ProcessBorderColor(System.Windows.Media.Color color)
        {
            BorderColor = color;
            view.ChangeBorderColor(color);
            personalizationView?.ChangeBorderColor(color);
            eventView?.ChangeBorderColor(color);
            categoryView?.ChangeBorderColor(color);
            homePageView?.ChangeBorderColor(color);

            // Add more views here :
        }

        public void ProcessForegroundColor(System.Windows.Media.Color color)
        {
            ForegroundColor = color;
            view.ChangeForegroundColor(color);
            personalizationView?.ChangeForegroundColor(color);
            eventView?.ChangeForegroundColor(color);
            categoryView?.ChangeForegroundColor(color);
            homePageView?.ChangeForegroundColor(color);

            // Add more views here :
        }

        private void defaultColors()
        {
            BackgroundColor = System.Windows.Media.Color.FromRgb(23, 25, 30);
            FontColor = System.Windows.Media.Color.FromRgb(255, 255, 255);
            BorderColor = System.Windows.Media.Color.FromRgb(255, 255, 255);
            ForegroundColor = System.Windows.Media.Color.FromRgb(46, 51, 59);
        }

        public void GetCalendarItems()
        {
            List<CalendarItem> items = new List<CalendarItem>(model.GetCalendarItems(null, null, false, 1));          
            gridView.LoadCalendarItems(items);        
        }
        public void ProcessFilters(DateTime? startDate, DateTime? endDate, bool? groupByMonthSelection, bool? groupByCategorySelection, object selectedCategory, bool? filterFlagSelection = false)
        {
            bool groupByMonth = false;
            bool groupByCategory = false;
            int categoryId = 1;
            bool filterFlag = false;

            if (filterFlagSelection == true && selectedCategory != null)
            {
                filterFlag = true;
                Category category = selectedCategory as Category;
                categoryId = category.Id;
            }

            if (groupByMonthSelection == true)
            {
                groupByMonth = true;
            }

            if (groupByCategorySelection == true)
            {
                groupByCategory = true;
            }

            if (!groupByMonth && !groupByCategory)
            {
                List<CalendarItem> items = model.GetCalendarItems(startDate, endDate, filterFlag, categoryId);
                gridView.LoadCalendarItems(items);
            }
            else if(groupByMonth && groupByCategory)
            {
                List<Dictionary<string, object>> itemDictionary = model.GetCalendarDictionaryByCategoryAndMonth(startDate, endDate, filterFlag, categoryId);
                gridView.LoadByMonthAndCategory(itemDictionary);
            }
            else if (groupByMonth)
            {
                List<CalendarItemsByMonth> itemsByMonth = model.GetCalendarItemsByMonth(startDate, endDate, filterFlag, categoryId);
                gridView.LoadByMonth(itemsByMonth);
            }
            else if (groupByCategory)
            {
                List<CalendarItemsByCategory> itemsByCategory = model.GetCalendarItemsByCategory(startDate, endDate, filterFlag, categoryId);
                gridView.GroupByCategories(itemsByCategory);
            }
            
        }

        public List<Category> GetCategoriesList()
        {
            return model.categories.List();
        }
    }
}
