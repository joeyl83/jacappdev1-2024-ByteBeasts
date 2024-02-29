using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================


namespace Calendar
{
    // ====================================================================
    // CLASS: Event
    //        - An individual event for calendar program
    // ====================================================================

    /// <summary>
    /// Contains data on an individual event for the calendar program. Initialized with specified data or from a copy of another event instance.
    /// </summary>
    public class Event
    {
        // ====================================================================
        // Properties
        // ====================================================================

        /// <summary>
        /// Gets the Id.
        /// </summary>
        /// <value>Represents the unique integer id of the event.</value>
        public int Id { get; }

        /// <summary>
        /// Gets the StartDateTime.
        /// </summary>
        /// <value>Represents the starting date and time of the event as a datetime object.</value>
        public DateTime StartDateTime { get; }

        /// <summary>
        /// Gets the DurationInMinutes.
        /// </summary>
        /// <value>Represents the amount of minutes of the event.</value>
        public Double DurationInMinutes { get; set; }

        /// <summary>
        /// Gets the Details.
        /// </summary>
        /// <value>Represents the details of the event as a string.</value>
        public String Details { get; set; }

        /// <summary>
        /// Gets the Category.
        /// </summary>
        /// <value>Represents the category id that belongs to a category object.</value>
        public int Category { get; set; }

        // ====================================================================
        // Constructor
        //    NB: there is no verification the event category exists in the
        //        categories object
        // ====================================================================

        /// <summary>
        /// Initializes an event instance with the passed values.
        /// </summary>
        /// <param name="id">The id of the event.</param>
        /// <param name="date">The object representing the start date/time of the event.</param>
        /// <param name="category">The category id of the event.</param>
        /// <param name="duration">The duration in minutes of the event.</param>
        /// <param name="details">The details of the event.</param>
        /// <example>
        /// 
        /// In this example, assume we want to create a event with the following details:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2024-02-06 7:50:32 PM     5          60         History Exam
        /// </code>
        /// 
        /// <b>Create the instance.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// Event e = new Event(1, new DateTime(2024, 02, 06, 19, 50, 32), 5, 60, "History Exam");
        /// 
        /// Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", "Id", "Start date/time", "Category", "Duration", "Details"));
        /// Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", e.Id, e.StartDateTime, e.Category, e.DurationInMinutes, e.Details));
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2024-02-06 7:50:32 PM     5          60         History Exam
        /// </code>
        /// </example>
        public Event(int id, DateTime date, int category, Double duration, String details)
        {
            this.Id = id;
            this.StartDateTime = date;
            this.Category = category;
            this.DurationInMinutes = duration;
            this.Details = details;
        }

        // ====================================================================
        // Copy constructor - does a deep copy
        // ====================================================================

        /// <summary>
        /// Initializes a new event instance with the exact same values as the passed event instance. Makes a new independant copy.
        /// </summary>
        /// <param name="obj">The instance that will have its values copied to the initialized instance.</param>
        /// <example>
        /// 
        /// For this example, we will create a copy of firstEvent with the following attributes:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2024-02-06 7:50:32 PM     5          60         History Exam
        /// </code>
        /// 
        /// <b>Make a new instance with the old object.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// Event firstEvent = new Event(1, new DateTime(2024, 02, 06, 19, 50, 32), 5, 60, "History Exam");
        /// Event secondEvent = new Event(firstEvent);
        /// 
        /// Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", "Id", "Start date/time", "Category", "Duration", "Details"));
        /// Console.WriteLine(string.Format("{0, -2} {1,-25} {2,-10} {3,-10} {4,-10}", secondEvent.Id, secondEvent.StartDateTime, secondEvent.Category, secondEvent.DurationInMinutes, secondEvent.Details));
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2024-02-06 7:50:32 PM     5          60         History Exam
        /// </code>
        /// </example>
        public Event (Event obj)
        {
            this.Id = obj.Id;
            this.StartDateTime = obj.StartDateTime;
            this.Category = obj.Category;
            this.DurationInMinutes = obj.DurationInMinutes;
            this.Details = obj.Details;
           
        }
    }
}
