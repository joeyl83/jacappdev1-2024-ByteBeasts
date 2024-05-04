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
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            model = new HomeCalendar(directory + "/" + fileName + ".db", true);
            view.ChangeWindow();
        }

        /// <summary>
        /// Opens an existing homecalendar that will be used as the model. An error is shown in the file is invalid.
        /// </summary>
        /// <param name="filepath">The filepath of the homecalendar file.</param>
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
            List<string> list=new List<string>();
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

            // Add more views here :
        }

        private void defaultColors()
        {
            BackgroundColor = System.Windows.Media.Color.FromRgb(23, 25, 30);
            FontColor = System.Windows.Media.Color.FromRgb(255, 255, 255);
            BorderColor = System.Windows.Media.Color.FromRgb(255, 255, 255);
            ForegroundColor = System.Windows.Media.Color.FromRgb(46, 51, 59);
        }

    }
}
