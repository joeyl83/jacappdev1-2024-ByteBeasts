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
    // CLASS: CalendarItem
    //        A single calendar item, includes a Category and an Event
    // ====================================================================

    /// <summary>
    /// Represents a single calendar item with a category and an event. CalendarItems kept track of by the HomeCalendar to organize the calendar for the user.
    /// </summary>
    public class CalendarItem
    {
        /// <summary>
        /// Gets and sets the CategoryId.
        /// </summary>
        /// <value>Represents the category id of the CalendarItem.</
        public int CategoryID { get; set; }

        /// <summary>
        /// Gets and sets the EventID.
        /// </summary>
        /// <value>Represents the event id of the CalendarItem.</value>
        public int EventID { get; set; }

        /// <summary>
        /// Gets and sets the StartDateTime.
        /// </summary>
        /// <value>Represents the starting date and time of the calendar item.</value>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets and sets the Category.
        /// </summary>
        /// <value>Represents the category of the calendar item as a string.</value>
        public String? Category { get; set; }

        /// <summary>
        /// Gets and sets the ShortDescription.
        /// </summary>
        /// <value>Represents the short description of the calendar item as a string.</value>
        public String? ShortDescription { get; set; }

        /// <summary>
        /// Gets and sets the DurationInMinutes.
        /// </summary>
        /// <value>Represents the duration in minutes that a calendar item will last.</value>
        public Double DurationInMinutes { get; set; }

        /// <summary>
        /// Gets and sets the BusyTime.
        /// </summary>
        /// <value>Represents the amount of estimated time that a calendar item should take to complete in minutes.</value>
        public Double BusyTime { get; set; }

    }

    /// <summary>
    /// Contains a list of CalendarItems with the represented month and total busy time. Used to store data so that it can be accessed for a specific month.
    /// </summary>
    public class CalendarItemsByMonth
    {
        /// <summary>
        /// Gets and sets the Month.
        /// </summary>
        /// <value>Represents the month that the group of calendar items belongs to.</value>
        public String? Month { get; set; }

        /// <summary>
        /// Gets and sets the Items.
        /// </summary>
        /// <value>Represents the list of calendar items that all belong to a given month.</value>
        public List<CalendarItem>? Items { get; set; }

        /// <summary>
        /// Gets and sets the TotalBusyTime.
        /// </summary>
        /// <value>Represents the total amount of busy time of every calendar item in the list.</value>
        public Double TotalBusyTime { get; set; }
    }


    /// <summary>
    /// Contains a list of CalendarItems with the represented category and total busy time. Used to store data so that it can be accessed for a specific category.
    /// </summary>
    public class CalendarItemsByCategory
    {
        /// <summary>
        /// Gets and sets the Category.
        /// </summary>
        /// <value>Represents the category that the group of calendar items belongs to.</value>
        public String? Category { get; set; }

        /// <summary>
        /// Gets and sets the Items.
        /// </summary>
        /// <value>Represents the list of calendar items that all belong to a given category.</value>
        public List<CalendarItem>? Items { get; set; }

        /// <summary>
        /// Gets and sets the TotalBusyTime
        /// </summary>
        /// <value>Represents the total amount of busy time of every calendar item in the list.</value>
        public Double TotalBusyTime { get; set; }

    }


}
