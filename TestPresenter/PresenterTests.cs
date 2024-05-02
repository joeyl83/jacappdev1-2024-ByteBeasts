using CalendarUI;
using TestPresenter.MockViews;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenterTest
{
    public class PresenterTests
    {
        [Fact]
        public void TestConstructor()
        {
            MainWindowMockView view = new MainWindowMockView();
            Presenter p = new Presenter(view);
            Assert.IsType<Presenter>(p);
        }

        [Fact]
        public void TestCreateNewHomeCalendarChangesWindow()
        {
            MainWindowMockView view = new MainWindowMockView();
            Presenter p = new Presenter(view);
            view.calledChangeWindow = false;

            p.NewHomeCalendar("calendars", "myCalendar.db");

            Assert.True(view.calledChangeWindow);
        }

        [Fact]
        public void TestOpenExistingHomeCalendarChangesWindow()
        {
            CreateTestCalendarFile();
            MainWindowMockView view = new MainWindowMockView();
            Presenter p = new Presenter(view);
            view.calledChangeWindow = false;

            p.OpenHomeCalendar("./testCalendar/calendar.db");

            Assert.True(view.calledChangeWindow);
        }

        [Fact]
        public void TestOpenHomeCalendarInvalidFileExtension()
        {
            MainWindowMockView view = new MainWindowMockView();
            Presenter p = new Presenter(view);
            view.calledChangeWindow = false;
            view.calledShowError = false;

            p.OpenHomeCalendar("./someFile.txt");

            Assert.False(view.calledChangeWindow);
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void TestAddCategory()
        {
            MainWindowMockView view = new MainWindowMockView();
            CategoriesWindowMockView categoriesView = new CategoriesWindowMockView();
            Presenter p = new Presenter(view);
            p.NewHomeCalendar("calendars", "myCalendar.db");
            p.InitializeCategoryView(categoriesView);
            categoriesView.calledAddCategory = false;

            p.ProcessAddCategory("National Holiday", Calendar.Category.CategoryType.Holiday);

            Assert.True(categoriesView.calledAddCategory);
        }

        [Fact]
        public void TestAddEvent()
        {
            MainWindowMockView view = new MainWindowMockView();
            EventsWindowMockView eventsView = new EventsWindowMockView();
            Presenter p = new Presenter(view);
            p.NewHomeCalendar("calendars", "myCalendar.db");
            p.InitializeEventView(eventsView);
            eventsView.calledAddEvent = false;

            p.ProcessAddEvent(DateTime.Now, 30, "This is a test event.", 1);

            Assert.True(eventsView.calledAddEvent);
        }

        [Fact]
        public void TestAddDuplicateEvent()
        {
            MainWindowMockView view = new MainWindowMockView();
            EventsWindowMockView eventsView = new EventsWindowMockView();
            Presenter p = new Presenter(view);
            p.NewHomeCalendar("calendars", "myCalendar.db");
            p.InitializeEventView(eventsView);
            eventsView.calledShowError = false;
            DateTime date = DateTime.Now;

            p.ProcessAddEvent(date, 30, "This is a test event.", 1);
            p.ProcessAddEvent(date, 30, "This is a test event.", 1);

            Assert.True(eventsView.calledShowError);
        }

        [Fact]
        public void TestLoadCategoryTypesInCategoryView()
        {
            MainWindowMockView view = new MainWindowMockView();
            CategoriesWindowMockView categoriesView = new CategoriesWindowMockView();
            Presenter p = new Presenter(view);
            p.NewHomeCalendar("calendars", "myCalendar.db");
            p.InitializeCategoryView(categoriesView);
            categoriesView.calledLoadCategoryTypes = false;

            p.LoadCategoryTypes();

            Assert.True(categoriesView.calledLoadCategoryTypes);
        }

        [Fact]
        public void TestLoadCategoriesInEventView()
        {
            MainWindowMockView view = new MainWindowMockView();
            EventsWindowMockView eventsView = new EventsWindowMockView();
            Presenter p = new Presenter(view);
            p.NewHomeCalendar("calendars", "myCalendar.db");
            p.InitializeEventView(eventsView);
            eventsView.calledLoadCategories = false;

            p.LoadCategories(1);

            Assert.True(eventsView.calledLoadCategories);
        }

        [Fact]
        public void TestChangeBackgroundColor()
        {
            MainWindowMockView view = new MainWindowMockView();
            CategoriesWindowMockView categoriesView = new CategoriesWindowMockView();
            EventsWindowMockView eventsView = new EventsWindowMockView();
            HomePageMockView homePageView = new HomePageMockView();
            PersonalizationWindowMockView personalizationView = new PersonalizationWindowMockView();
            Presenter p = new Presenter(view);

            p.NewHomeCalendar("calendars", "myCalendar.db");
            p.InitializeCategoryView(categoriesView);
            p.InitializeEventView(eventsView);
            p.InitializeHomePageView(homePageView);
            p.InitializePersonalizationWindow(personalizationView);

            view.calledChangeBackgroundColor = false;
            categoriesView.calledChangeBackgroundColor = false;
            eventsView.calledChangeBackgroundColor = false;
            homePageView.calledChangeBackgroundColor = false;
            personalizationView.calledChangeBackgroundColor = false;

            p.ProcessBackgroundColor(new System.Windows.Media.Color());

            Assert.True(view.calledChangeBackgroundColor);
            Assert.True(categoriesView.calledChangeBackgroundColor);
            Assert.True(eventsView.calledChangeBackgroundColor);
            Assert.True(homePageView.calledChangeBackgroundColor);
            Assert.True(personalizationView.calledChangeBackgroundColor);
        }

        [Fact]
        public void TestChangeFontColor()
        {
            MainWindowMockView view = new MainWindowMockView();
            CategoriesWindowMockView categoriesView = new CategoriesWindowMockView();
            EventsWindowMockView eventsView = new EventsWindowMockView();
            HomePageMockView homePageView = new HomePageMockView();
            PersonalizationWindowMockView personalizationView = new PersonalizationWindowMockView();
            Presenter p = new Presenter(view);

            p.NewHomeCalendar("calendars", "myCalendar.db");
            p.InitializeCategoryView(categoriesView);
            p.InitializeEventView(eventsView);
            p.InitializeHomePageView(homePageView);
            p.InitializePersonalizationWindow(personalizationView);

            view.calledChangeFontColor = false;
            categoriesView.calledChangeFontColor = false;
            eventsView.calledChangeFontColor = false;
            homePageView.calledChangeFontColor = false;
            personalizationView.calledChangeFontColor = false;

            p.ProcessFontColor(new System.Windows.Media.Color());

            Assert.True(view.calledChangeFontColor);
            Assert.True(categoriesView.calledChangeFontColor);
            Assert.True(eventsView.calledChangeFontColor);
            Assert.True(homePageView.calledChangeFontColor);
            Assert.True(personalizationView.calledChangeFontColor);
        }

        [Fact]
        public void TestChangeBorderColor()
        {
            MainWindowMockView view = new MainWindowMockView();
            CategoriesWindowMockView categoriesView = new CategoriesWindowMockView();
            EventsWindowMockView eventsView = new EventsWindowMockView();
            HomePageMockView homePageView = new HomePageMockView();
            PersonalizationWindowMockView personalizationView = new PersonalizationWindowMockView();
            Presenter p = new Presenter(view);

            p.NewHomeCalendar("calendars", "myCalendar.db");
            p.InitializeCategoryView(categoriesView);
            p.InitializeEventView(eventsView);
            p.InitializeHomePageView(homePageView);
            p.InitializePersonalizationWindow(personalizationView);

            view.calledChangeBorderColor = false;
            categoriesView.calledChangeBorderColor = false;
            eventsView.calledChangeBorderColor = false;
            homePageView.calledChangeBorderColor = false;
            personalizationView.calledChangeBorderColor = false;

            p.ProcessBorderColor(new System.Windows.Media.Color());

            Assert.True(view.calledChangeBorderColor);
            Assert.True(categoriesView.calledChangeBorderColor);
            Assert.True(eventsView.calledChangeBorderColor);
            Assert.True(homePageView.calledChangeBorderColor);
            Assert.True(personalizationView.calledChangeBorderColor);
        }

        [Fact]
        public void TestChangeForegroundColor()
        {
            MainWindowMockView view = new MainWindowMockView();
            CategoriesWindowMockView categoriesView = new CategoriesWindowMockView();
            EventsWindowMockView eventsView = new EventsWindowMockView();
            HomePageMockView homePageView = new HomePageMockView();
            PersonalizationWindowMockView personalizationView = new PersonalizationWindowMockView();
            Presenter p = new Presenter(view);

            p.NewHomeCalendar("calendars", "myCalendar.db");
            p.InitializeCategoryView(categoriesView);
            p.InitializeEventView(eventsView);
            p.InitializeHomePageView(homePageView);
            p.InitializePersonalizationWindow(personalizationView);

            view.calledChangeForegroundColor = false;
            categoriesView.calledChangeForegroundColor = false;
            eventsView.calledChangeForegroundColor = false;
            homePageView.calledChangeForegroundColor = false;
            personalizationView.calledChangeForegroundColor = false;

            p.ProcessForegroundColor(new System.Windows.Media.Color());

            Assert.True(view.calledChangeForegroundColor);
            Assert.True(categoriesView.calledChangeForegroundColor);
            Assert.True(eventsView.calledChangeForegroundColor);
            Assert.True(homePageView.calledChangeForegroundColor);
            Assert.True(personalizationView.calledChangeForegroundColor);
        }
        [Fact]

        public void TestGridViewGroupByCategories()
        {
            MainWindowMockView view = new MainWindowMockView();
            GridViewWindowMockView gridBoxView = new GridViewWindowMockView();
            Presenter p = new Presenter(view);
            p.InitializeGridBoxView(gridBoxView);

            p.GroupByCategory();

            Assert.True(gridBoxView.calledGroupByCategories);
        }
        [Fact]

        public void TestGridViewLoadByACategory()
        {
            MainWindowMockView view = new MainWindowMockView();
            GridViewWindowMockView gridBoxView = new GridViewWindowMockView();
            Presenter p = new Presenter(view);
            p.InitializeGridBoxView(gridBoxView);
            int catId = 1;

            p.FilterByACategory(catId);

            Assert.True(gridBoxView.calledLoadByACategory);
        }
        [Fact]
        public void TestGridViewLoadByMonth()
        {
            MainWindowMockView view = new MainWindowMockView();
            GridViewWindowMockView gridBoxView = new GridViewWindowMockView();
            Presenter p = new Presenter(view);
            p.InitializeGridBoxView(gridBoxView);

            p.GetCalendarItemsByMonth();

            Assert.True(gridBoxView.calledLoadByMonth);
        }
        [Fact]
        public void TestGridViewLoadByMonthAndCategory()
        {
            MainWindowMockView view = new MainWindowMockView();
            GridViewWindowMockView gridBoxView = new GridViewWindowMockView();
            Presenter p = new Presenter(view);
            p.InitializeGridBoxView(gridBoxView);

            p.GetCalendarItemsByMonthAndCategory();

            Assert.True(gridBoxView.calledLoadByMonthAndCategory);
        }
        [Fact]
        public void TestGridViewLoadCalendarItems()
        {
            MainWindowMockView view = new MainWindowMockView();
            GridViewWindowMockView gridBoxView = new GridViewWindowMockView();
            Presenter p = new Presenter(view);
            p.InitializeGridBoxView(gridBoxView);

            p.GetCalendarItems();

            Assert.True(gridBoxView.calledLoadCalendarItems);
        }
        [Fact]
        public void TestGridViewLoadCategories()
        {
            MainWindowMockView view = new MainWindowMockView();
            GridViewWindowMockView gridBoxView = new GridViewWindowMockView();
            Presenter p = new Presenter(view);
            p.InitializeGridBoxView(gridBoxView);

            p.LoadCategories(2);

            Assert.True(gridBoxView.calledLoadCategories);
        }
        private void CreateTestCalendarFile()
        {
            MainWindowMockView view = new MainWindowMockView();
            Presenter p = new Presenter(view);
            p.NewHomeCalendar("testCalendar", "calendar.db");
        }


    }
}
