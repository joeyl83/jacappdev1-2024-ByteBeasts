using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Calendar;
using System.Data.SQLite;
using System.Diagnostics.Tracing;

namespace CalendarCodeTests
{
    [Collection("Sequential")]
    public class TestEvents
    {
        int numberOfEventsInFile = TestConstants.numberOfEventsInFile;
        int maxIDInEventFile = TestConstants.maxIDInEventFile;
        Event firstEventInFile = new Event(1, new DateTime(2021, 1, 10), 3, 40, "App Dev Homework");


        // ========================================================================
        [Fact]
        public void EventsMethod_Add()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn, false);
            DateTime date = DateTime.Today;
            int category = 1;
            Double duration = 1;
            String details = "Event details";

            // Act
            events.Add(date, category, duration, details);
            List<Event> eventsList = events.List();
            int sizeOfList = eventsList.Count;
            events.Delete(10);
            
            // Assert
            //Assert.Equal(theEvent.Id, 1);
            Assert.Equal(numberOfEventsInFile + 1, sizeOfList);
            Assert.Equal(details, eventsList[sizeOfList - 1].Details);
        }
        [Fact]
        public void EventsMethod_Delete()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn, false);
            int IdToDelete = 3;

            // Act
            events.Delete(IdToDelete);
            List<Event> eventsList = events.List();
            int sizeOfList = eventsList.Count;

            // Assert
            Assert.Equal(numberOfEventsInFile - 1, sizeOfList);
            Assert.False(eventsList.Exists(e => e.Id == IdToDelete), "correct Event item deleted");

        }

        [Fact]
        public void EventsMethod_Update()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn, true);
            DateTime newDate = DateTime.Today;
            int newCat = 1;
            Double newDuration = 2;
            String newDetails = "New Event details!";
            int id = 1;

            // Act
            events.Update(id, newDate, newCat, newDuration, newDetails);
            List<Event> eventList = events.List();
            Event theEvent = eventList[id - 1];


            // Assert 
            Assert.Equal(newDate, theEvent.StartDateTime);
            Assert.Equal(newCat, theEvent.Category);
            Assert.Equal(newDuration, theEvent.DurationInMinutes);
            Assert.Equal(newDetails, theEvent.Details);
        }
        [Fact]
        public void EventsMethod_Delete_InvalidIDDoesntCrash()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messyDB";
            System.IO.File.Copy(goodDB, messyDB, true);
            Database.existingDatabase(messyDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn, false);
            int IdToDelete = 9999;
            int sizeOfList = events.List().Count;

            // Act
            try
            {
                events.Delete(IdToDelete);
                Assert.Equal(sizeOfList, events.List().Count);
            }

            // Assert
            catch
            {
                Assert.True(false, "Invalid ID causes Delete to break");
            }
        }
        [Fact]
        public void EventsObject_New()
        {
            // Arrange

            // Act
            Events Events = new Events(Database.dbConnection);

            // Assert 
            Assert.IsType<Events>(Events);
        }

        [Fact]
        public void EventsMethod_List_ReturnsListOfEvents()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Events events = new Events(conn, false);

            // Act
            List<Event> list = events.List();

            // Assert
            Assert.Equal(numberOfEventsInFile, list.Count);
        }

        [Fact]
        public void EventsMethod_List_ModifyListDoesNotModifyEventsInstance()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(goodDB);
            SQLiteConnection conn = Database.dbConnection;
            Events Events = new Events(conn, false);
            List<Event> list = Events.List();

            // Act
            list[0].DurationInMinutes = list[0].DurationInMinutes + 21.03;

            // Assert
            Assert.NotEqual(list[0].DurationInMinutes, Events.List()[0].DurationInMinutes);

        }
    }
}

