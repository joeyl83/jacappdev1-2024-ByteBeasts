using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Collections;
using System.Drawing;
using System.Data.Common;
using System.Data.SQLite;
using System.Data.SqlClient;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{
    // ====================================================================
    // CLASS: Events
    //        - A collection of Event items,
    //        - Read / write to file
    //        - etc
    // ====================================================================

    /// <summary>
    /// Manages a collection of event items. Reads and writes to files storing the events. Initialized with default values in the constructor, 
    /// but also from files with data containing various events when using the ReadFromFile method.
    /// </summary>
  
    public class Events
    {
        private SQLiteConnection _connection;

        public Events(SQLiteConnection connection, bool newDB = false)
        {
            _connection = connection;
        }

        /// <summary>
        /// Adds a new event with the specified values to the database's event table.
        /// </summary>
        /// <param name="date">The object representing the start date/time of the added event.</param>
        /// <param name="category">The category of the added event.</param>
        /// <param name="duration">The duration in minutes of the added event.</param>
        /// <param name="details">The details of the added event.</param>
        /// <example>
        /// 
        /// In this example, assume that the event database contains the following elements:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// </code>
        /// 
        /// <b>Adding a event to the database.</b>
        /// <code>
        /// <![CDATA[
        /// Events events = new Events(database.db);
        /// 
        /// events.Add(new DateTime(2024, 02, 06, 19, 50, 32), 5, 60, "History Exam";
        /// 
        /// Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", "Id", "Start date/time", "Category", "Duration", "Details"));
        /// foreach (Event e in events.List())
        /// {
        ///     Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", e.Id, e.StartDateTime, e.Category, e.DurationInMinutes, e.Details));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// 5  2024-02-06 7:50:32 PM     5          60         History Exam
        /// </code>
        /// </example>
        public void Add(DateTime date, int category, Double duration, String details)
        {
            SQLiteCommand cmd = new SQLiteCommand(_connection);
            cmd.CommandText = $"INSERT INTO events(StartDateTime,Details,DurationInMinutes,CategoryId) VALUES(@startdate,@details,@duration,@categoryId)";
            cmd.Parameters.AddWithValue("@startdate", date.ToString());
            cmd.Parameters.AddWithValue("@details", details);
            cmd.Parameters.AddWithValue("@duration", duration);
            cmd.Parameters.AddWithValue("@categoryId", category );
            cmd.ExecuteNonQuery();
            cmd.Dispose();

        }

        // ====================================================================
        // Delete Event
        // ====================================================================

        /// <summary>
        /// Removes the event from the database events table with the specified id. The passed id must be valid.
        /// </summary>
        /// <param name="Id">The id of the event that will be removed from the database.</param>
        /// <example>
        /// 
        /// In this example, assume that the events table contains the following elements:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// </code>
        /// 
        /// <b>Deleting an event from the database.</b>
        /// <code>
        /// <![CDATA[
        /// Events events = new Events(database.db);
        /// 
        /// events.Delete(3);
        /// 
        /// Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", "Id", "Start date/time", "Category", "Duration", "Details"));
        /// foreach (Event e in events.List())
        /// {
        ///     Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", e.Id, e.StartDateTime, e.Category, e.DurationInMinutes, e.Details));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// </code>
        /// </example>
        public void Delete(int Id)
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "DELETE FROM events WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong:{ex.Message}");
            }
        }



        // ====================================================================
        // Update Event
        // ====================================================================

        /// <summary>
        /// Updates the event from the database events table with the specified id. The passed id must be valid.
        /// </summary>
        /// <param name="Id">The id of the event that will be updated from the database.</param>
        /// <param name="date">The object representing the new start date/time for the event.</param>
        /// <param name="category">The new category for the event.</param>
        /// <param name="duration">The new duration in minutes for the event.</param>
        /// <param name="details">The new details for the event.</param>
        /// <example>
        /// 
        /// In this example, assume that the events table contains the following elements:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// </code>
        /// 
        /// <b>Updating an event from the database.</b>
        /// <code>
        /// <![CDATA[
        /// Events events = new Events(database.db);
        /// DateTime newDate = DateTime.Today;
        /// int newCat = 1;
        /// Double newDuration = 2;
        /// String newDetails = "New Event details!";
        /// int id = 1;
        /// 
        /// events.Update(id, newDate, newCat, newDuration, newDetails);
        /// 
        /// Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", "Id", "Start date/time", "Category", "Duration", "Details"));
        /// foreach (Event e in events.List())
        /// {
        ///     Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", e.Id, e.StartDateTime, e.Category, e.DurationInMinutes, e.Details));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2024-01-10 10:00:00 AM    1          2          New Event details!
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// </code>
        /// </example>
        public void Update(int  Id, DateTime date, int category, Double duration, String details)
        {
            try
            {
                
                SQLiteCommand cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "UPDATE events SET StartDateTime = @date, DurationInMinutes = @duration, Details = @details, CategoryId = @category WHERE Id=@id";
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@duration", duration);
                cmd.Parameters.AddWithValue("@details", details);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message}");
            }
        }


        // ====================================================================
        // Return list of Events
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================

        /// <summary>
        /// Generates a list of all stored events. The list is sorted by Id.
        /// </summary>
        /// <returns>The list of all stored events.</returns>
        /// <example>
        /// 
        /// In this example, assume that the event table in the database contains the following elements:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// </code>
        /// 
        /// <b>Getting the list of items.</b>
        /// <code>
        /// <![CDATA[
        /// Events events = new Events(Database.dbConnection, false);
        /// 
        /// List<Event> copy = events.List();
        /// 
        /// Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", "Id", "Start date/time", "Category", "Duration", "Details"));
        /// foreach (Event e in copy)
        /// {
        ///     Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", e.Id, e.StartDateTime, e.Category, e.DurationInMinutes, e.Details));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// </code>
        /// </example>
        public List<Event> List()
        {
            List<Event> newList = new List<Event>();

            SQLiteCommand cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT Id, StartDateTime, Details, DurationInMinutes, CategoryId FROM events ORDER BY Id;";
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                newList.Add(new Event(reader.GetInt32(0), DateTime.Parse(reader.GetString(1)), reader.GetInt32(4), reader.GetDouble(3), reader.GetString(2)));
            }

            cmd.Dispose();
            return newList;
        }
    }
}

