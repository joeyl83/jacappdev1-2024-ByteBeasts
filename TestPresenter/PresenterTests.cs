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

            p.LoadCategories();

            Assert.True(eventsView.calledLoadCategories);
        }




        private void CreateTestCalendarFile()
        {
            MainWindowMockView view = new MainWindowMockView();
            Presenter p = new Presenter(view);
            p.NewHomeCalendar("testCalendar", "calendar.db");
        }
    }
}
