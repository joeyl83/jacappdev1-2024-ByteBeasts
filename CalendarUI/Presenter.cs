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
    /// <summary>
    /// The presenter layer of the calendar app. Manages the logic to decide what should be called in the view.
    /// </summary>
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

        /// <summary>
        /// Initializes the presenter instance with the first view that it needs to manage.
        /// </summary>
        /// <param name="v">The object that inherits the view interface, representing the view that needs to be managed.</param>
        public Presenter(ViewInterface v)
        {
            defaultColors();
            view = v;
        }

        /// <summary>
        /// Creates a new homecalendar in the specified directory and with the specified name. Used to create the model for MVP.
        /// </summary>
        /// <param name="directory">The name of the directory where the file will be saved in.</param>
        /// <param name="fileName">The name of the file where the homecalendar is saved.</param>
        public void NewHomeCalendar(string directory, string fileName)
        {
            if (string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(fileName))
            {
                view.ShowError("Directory name and file name cannot be empty.");
            }
            else
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                model = new HomeCalendar(directory + "/" + fileName + ".db", true);
                view.ChangeWindow();
            }
           
        }

        /// <summary>
        /// Opens an existing homecalendar that will be used as the model. An error is shown in the file is invalid.
        /// </summary>
        /// <param name="filepath">The filepath of the homecalendar file.</param>
        public void OpenHomeCalendar(string filepath)
        {
            string extension = Path.GetExtension(filepath);
            if (extension == ".db")
            {
                model = new HomeCalendar(filepath);
                view.ChangeWindow();
            }
            else
            {
                view.ShowError("Invalid file");
            }
        }

        /// <summary>
        /// Adds a category with the provided information to the model and updates the view accordingly.
        /// </summary>
        /// <param name="categoryName">The name of the category that is being added.</param>
        /// <param name="type">The type of the category that is being added.</param>
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

        /// <summary>
        /// Adds an event with the provided information to the model and updates the view accordingly.
        /// </summary>
        /// <param name="StartDateTime">The start date of the event.</param>
        /// <param name="DurationInMinutes">The duration in minutes of the event.</param>
        /// <param name="Details">The details explaining more information of the event.</param>
        /// <param name="CatId">The category ID of the event.</param>
        public void ProcessAddEvent(DateTime StartDateTime, double DurationInMinutes, string Details, int CatId)
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
                    model.events.Add(StartDateTime, CatId, DurationInMinutes, Details);
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

            gridView.LoadCalendarItems(model.GetCalendarItems(null, null, false, 1));
        }

        /// <summary>
        /// Edits an event using the provided information and updates the view accordingly.
        /// </summary>
        /// <param name="StartDateTime">The new start date of the event.</param>
        /// <param name="DurationInMinutes">The new duration in minutes of the event.</param>
        /// <param name="Details">The new details explaining more information of the event.</param>
        /// <param name="CatId">The new category ID of the event.</param>
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

            gridView.LoadCalendarItems(model.GetCalendarItems(null, null, false, 1));
        }

        /// <summary>
        /// Deletes an event with the provided ID. Sends an error to the view if needed.
        /// </summary>
        /// <param name="eventID">The ID of the event that is being deleted.</param>
        public void ProcessDeleteEvent(int eventID)
        {
            try
            {
                model.events.Delete(eventID);
            }
            catch (Exception ex)
            {
                eventView.ShowError(ex.Message);
            }

            gridView.LoadCalendarItems(model.GetCalendarItems(null, null, false, 1));
        }



        /// <summary>
        /// Sets the category view of the presenter instance so that it can be accessed and managed.
        /// </summary>
        /// <param name="view">The category view that the presenter will manage.</param>
        public void InitializeCategoryView(CategoriesViewInterface view)
        {
            categoryView = view;
        }

        /// <summary>
        /// Loads the category types from the category type enum into the view for the user to see.
        /// </summary>
        public void LoadCategoryTypes()
        {
            List<string> list = new List<string>();
            int count = 0;
            foreach (string categoryType in Enum.GetNames(typeof(Category.CategoryType)))
            {
                count++;
                list.Add($"{categoryType}:{count}");
            }
            categoryView.LoadCategoryTypes(list);
        }

        /// <summary>
        /// Loads all categories that exist in the homecalendar into the view for the user to see.
        /// </summary>
        /// <param name="check">Check to load categories to the event view or the grid view. 1 loads to the event view and 2 loads to the grid.</param>        
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

        /// <summary>
        /// Sets the event view of the presenter instance so that it can be accessed and managed.
        /// </summary>
        /// <param name="view">The event view that the presenter will manage.</param>
        public void InitializeEventView(EventViewInterface view)
        {
            eventView = view;
        }

        /// <summary>
        /// Sets the home page view of the presenter instance so that it can be accessed and managed.
        /// </summary>
        /// <param name="view">The home page view that the presenter will manage.</param>
        public void InitializeHomePageView(HomePageViewInterface view)
        {
            homePageView = view;
        }

        /// <summary>
        /// Sets the personalization view of the presenter instance so that it can be accessed and managed.
        /// </summary>
        /// <param name="view">The personalization view that the presenter will manage.</param>
        public void InitializePersonalizationWindow(PersonalizationInterface view)
        {
            personalizationView = view;
        }
        public void InitializeGridBoxView(GridViewInterface view)
        {
            gridView = view;
        }

        /// <summary>
        /// Processes a new background color for the theme of the application, and updates all of the views to adapt the change.
        /// </summary>
        /// <param name="color">The color that the background will be updated to.</param>
        public void ProcessBackgroundColor(System.Windows.Media.Color color)
        {
            BackgroundColor = color;
            view.ChangeBackground(color);
            personalizationView?.ChangeBackground(color);
            eventView?.ChangeBackground(color);
            categoryView?.ChangeBackground(color);
            homePageView?.ChangeBackground(color);
            gridView?.ChangeBackground(color);

            // Add more views here :
        }

        /// <summary>
        /// Processes a new font color for the theme of the application, and updates all of the views to adapt the change.
        /// </summary>
        /// <param name="color">The color that the font will be updated to in the views.</param>
        public void ProcessFontColor(System.Windows.Media.Color color)
        {
            FontColor = color;
            view.ChangeFontColor(color);
            personalizationView?.ChangeFontColor(color);
            eventView?.ChangeFontColor(color);
            categoryView?.ChangeFontColor(color);
            homePageView?.ChangeFontColor(color);
            gridView?.ChangeFontColor(color);

            // Add more views here :
        }

        /// <summary>
        /// Processes a new border color for the theme of the application, and updates all of the views to adapt the change.
        /// </summary>
        /// <param name="color">The color that the borders will be updated to.</param>
        public void ProcessBorderColor(System.Windows.Media.Color color)
        {
            BorderColor = color;
            view.ChangeBorderColor(color);
            personalizationView?.ChangeBorderColor(color);
            eventView?.ChangeBorderColor(color);
            categoryView?.ChangeBorderColor(color);
            homePageView?.ChangeBorderColor(color);
            gridView?.ChangeBorderColor(color);

            // Add more views here :
        }

        /// <summary>
        /// Processes a new foreground color for the theme of the application, and updates all of the views to adapt the change.
        /// </summary>
        /// <param name="color">The color that the foreground will be updated to.</param>
        public void ProcessForegroundColor(System.Windows.Media.Color color)
        {
            ForegroundColor = color;
            view.ChangeForegroundColor(color);
            personalizationView?.ChangeForegroundColor(color);
            eventView?.ChangeForegroundColor(color);
            categoryView?.ChangeForegroundColor(color);
            homePageView?.ChangeForegroundColor(color);
            gridView?.ChangeForegroundColor(color);

            // Add more views here :
        }

        private void defaultColors()
        {
            BackgroundColor = System.Windows.Media.Color.FromRgb(23, 25, 30);
            FontColor = System.Windows.Media.Color.FromRgb(255, 255, 255);
            BorderColor = System.Windows.Media.Color.FromRgb(255, 255, 255);
            ForegroundColor = System.Windows.Media.Color.FromRgb(46, 51, 59);
        }

        /// <summary>
        /// Gets the list of calendar items from the model and shows them in the view.
        /// </summary>
        public void GetCalendarItems()
        {
            List<CalendarItem> items = new List<CalendarItem>(model.GetCalendarItems(null, null, false, 1));
            gridView.LoadCalendarItems(items);
        }

        /// <summary>
        /// Processes all of the filters/grouping settings received and loads a list of calendar items using them to the view.
        /// </summary>
        /// <param name="startDate">The start date to filter the calendar items. Null if there is none selected.</param>
        /// <param name="endDate">The end date to filter the calendar items. Null if there is none selected.</param>
        /// <param name="groupByMonthSelection">True if the result should be grouped by month; false otherwise.</param>
        /// <param name="groupByCategorySelection">True if the result should be grouped by category; false otherwise.</param>
        /// <param name="selectedCategory">The category that will be searched for if the category filter flag is enabled.</param>
        /// <param name="filterFlagSelection">True if the result should be filtered by a categor; false otherwise.</param>
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
            else if (groupByMonth && groupByCategory)
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

        /// <summary>
        /// Gets a list of the categories from the model.
        /// </summary>
        /// <returns>A list of all categories in the homecalendar.</returns>
        public List<Category> GetCategoriesList()
        {
            return model.categories.List();
        }

        public System.Windows.Media.Color getFontColor()
        {
            return FontColor;
        }

    }
}
