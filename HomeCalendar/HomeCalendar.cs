using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================


namespace Calendar
{
    // ====================================================================
    // CLASS: HomeCalendar
    //        - Combines a Categories Class and an Events Class
    //        - One File defines Category and Events File
    //        - etc
    // ====================================================================

    /// <summary>
    /// Manages a home calendar application, keeping track of <see cref="Events"/> and <see cref="Categories"/>. Loads <see cref="Event"/> and <see cref="Category"/> data from files using the <see cref="Events"/> and
    /// <see cref="Categories"/> classes. Contains methods to return <see cref="CalendarItem"/> lists with certain search conditions, as well as different ways of grouping.
    /// </summary>
    /// <example>
    /// 
    /// Brief example to demonstrate typical class usage:
    /// 
    /// <code>
    /// <![CDATA[
    /// 
    /// HomeCalendar calendar = new HomeCalendar(fileName);
    /// 
    /// //Deleting all vacations from the calendar
    /// List<CalendarItems> list = CalendarItems.GetCalendarItems(null, null, true, 9);
    /// 
    /// foreach(CalendarItem item in list)
    /// {
    ///     calendar.events.Delete(item.EventId)
    /// }
    /// 
    /// //Adding a new vacation to the calendar
    /// calendar.events.Add(new DateTime(2024, 05, 01), 9, 1440, "Bahamas trip");
    /// ]]>
    /// </code>
    /// </example>
    public class HomeCalendar
    {
        private string? _FileName;
        private string? _DirName;
        private Categories _categories;
        private Events _events;

        // ====================================================================
        // Properties
        // ===================================================================

        // Properties (location of files etc)

        /// <summary>
        /// Gets and sets the FileName.
        /// </summary>
        /// <value>Represents the file name of the file containing the category and event file names.</value>
        public String? FileName { get { return _FileName; } }

        /// <summary>
        /// Gets and sets the DirName
        /// </summary>
        /// <value>Represents the directory of the file containing the category and event file names.</value>
        public String? DirName { get { return _DirName; } }

        /// <summary>
        /// Gets the PathName.
        /// </summary>
        /// <value>Represents the full path of the file containing the category and event file names.</value>
        public String? PathName
        {
            get
            {
                if (_FileName != null && _DirName != null)
                {
                    return Path.GetFullPath(_DirName + "\\" + _FileName);
                }
                else
                {
                    return null;
                }
            }
        }

        // Properties (categories and events object)

        /// <summary>
        /// Gets and sets the categories.
        /// </summary>
        /// <value>Represents the list of categories in the home calendar.</value>
        public Categories categories { get { return _categories; } }

        /// <summary>
        /// Gets and sets the events.
        /// </summary>
        /// <value>Represents the list of events in the home calendar.</value>
        public Events events { get { return _events; } }


        // -------------------------------------------------------------------
        // Constructor (existing or new calendar )
        // -------------------------------------------------------------------

        /// <summary>
        /// Initializes a home calendar instance by reading events from XML file and categories from the database. 
        /// </summary>
        /// <param name="databaseFile">The database file containing the tables of data for categories and events.</param
        /// <param name="eventsXMLFile">The events file filed with events to will be loadedi nto the calendar.</param>
        /// <param name="newDB">Boolean value that distinguishes whether to create a new database file.</param>
        /// <example>
        /// 
        /// For this example, assume that the calendar file contains the following data:
        /// 
        /// <code>
        /// category-file.cats
        /// event-file.evts
        /// </code>
        /// 
        /// We will assume that those files/database are populated with valid categories and events. 
        /// <br></br>
        /// <b>Initializing the home calendar.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// 
        /// HomeCalendar homeCalendar = new HomeCalendar("./databaseFile.db", "./eventFile.evts");
        /// 
        /// Console.WriteLine("Categories:")
        /// foreach (Category c in homeCalendar.categories.List())
        /// {
        ///     Console.WriteLine(c.Description)
        /// }
        /// 
        /// Console.WriteLine("\nEvents:")
        /// foreach (Event e in homeCalendar.events.List())
        /// {
        ///     Console.WriteLine(e.Details)
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Categories:
        /// Non Standard
        /// School
        /// Work
        /// Fun
        /// Medical
        /// Sleep
        /// Working
        /// On call
        /// Canadian Holidays
        /// Vacation
        /// Wellness days
        /// Birthdays
        /// 
        /// Events:
        /// App Dev Homework
        /// Honolulu
        /// Honolulu
        /// On call security
        /// staff meeting
        /// New Year's
        /// Wendy's birthday
        /// Sprint retrospective
        /// </code>
        /// </example>


        public HomeCalendar(String databaseFile, String eventsXMLFile, bool newDB = false)
        {
            // if database exists, and user doesn't want a new database, open existing DB
            if (!newDB && File.Exists(databaseFile))
            {
                Database.existingDatabase(databaseFile);
            }

            // file did not exist, or user wants a new database, so open NEW DB
            else
            {
                Database.newDatabase(databaseFile);
                newDB = true;
            }

            // create the category object
            _categories = new Categories(Database.dbConnection, newDB);

            // create the _events course
            _events = new Events(Database.dbConnection, newDB);
            _events.ReadFromFile(eventsXMLFile);
        }



        #region OpenNewAndSave
        // ---------------------------------------------------------------
        // Read
        // Throws Exception if any problem reading this file
        // ---------------------------------------------------------------

        /// <summary>
        /// Reads the data from the calendar file in order to populate the categories and events of the current home calendar instance. An exception
        /// is thrown if there is any problem reading the file. If a null file name is passed, the method will try to read from a default file path.
        /// </summary>
        /// <param name="calendarFileName">The file path of the calendar file containing the category and event file names.</param>
        /// <exception cref="Exception">Throws when there is any problem when reading the file.</exception>
        /// <example>
        /// 
        /// For this example, assume that the calendar file contains the following data:
        /// 
        /// <code>
        /// category-file.cats
        /// event-file.evts
        /// </code>
        /// 
        /// We will assume that those files are populated with valid categories and events. 
        /// <br></br>
        /// <b>Loading data from the file to the instance.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar();
        /// homeCalendar.ReadFromFile("./calendar-file.calendar");
        /// 
        /// Console.WriteLine("Categories:")
        /// foreach (Category c in homeCalendar.categories.List())
        /// {
        ///     Console.WriteLine(c.Description)
        /// }
        /// 
        /// Console.WriteLine("\nEvents:")
        /// foreach (Event e in homeCalendar.events.List())
        /// {
        ///     Console.WriteLine(e.Details)
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Categories:
        /// Non Standard
        /// School
        /// Work
        /// Fun
        /// Medical
        /// Sleep
        /// Working
        /// On call
        /// Canadian Holidays
        /// Vacation
        /// Wellness days
        /// Birthdays
        /// 
        /// Events:
        /// App Dev Homework
        /// Honolulu
        /// Honolulu
        /// On call security
        /// staff meeting
        /// New Year's
        /// Wendy's birthday
        /// Sprint retrospective
        /// </code>
        /// </example>
        public void ReadFromFile(String? calendarFileName)
        {
            // ---------------------------------------------------------------
            // read the calendar file and process
            // ---------------------------------------------------------------
            try
            {
                // get filepath name (throws exception if it doesn't exist)
                calendarFileName = CalendarFiles.VerifyReadFromFileName(calendarFileName, "");

                // If file exists, read it
                string[] filenames = System.IO.File.ReadAllLines(calendarFileName);

                // ----------------------------------------------------------------
                // Save information about the calendar file
                // ----------------------------------------------------------------
                string? folder = Path.GetDirectoryName(calendarFileName);
                _FileName = Path.GetFileName(calendarFileName);

                // read the events and categories from their respective files
                _categories.ReadFromFile(folder + "\\" + filenames[0]);
                _events.ReadFromFile(folder + "\\" + filenames[1]);

                // Save information about calendar file
                _DirName = Path.GetDirectoryName(calendarFileName);
                _FileName = Path.GetFileName(calendarFileName);

            }

            // ----------------------------------------------------------------
            // throw new exception if we cannot get the info that we need
            // ----------------------------------------------------------------
            catch (Exception e)
            {
                throw new Exception("Could not read calendar info: \n" + e.Message);
            }

        }

        // ====================================================================
        // save to a file
        // saves the following files:
        //  filepath_events.evts  # events file
        //  filepath_categories.cats # categories files
        //  filepath # a file containing the names of the events and categories files.
        //  Throws exception if we cannot write to that file (ex: invalid dir, wrong permissions)
        // ====================================================================

        /// <summary>
        /// Saves all of the needed home calendar data to files that can be loaded again. The event and category lists are saved to their own files,
        /// and the file names of those event and category files are saved to the calendar file. An exception is thrown if a file can't be written to.
        /// </summary>
        /// <param name="filepath">The file path where the files will be saved.</param>
        /// <example>
        /// 
        /// For this example, assume that the calendar file contains the following elements:
        /// 
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		        1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Year's                 1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendy's birthday           1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60
        /// </code>
        /// 
        /// <b>Save all data to their respective files.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./calendar-file.calendar");
        /// 
        /// homeCalendar.SaveToFile("./new-calendar-file.calendar");
        /// ]]>
        /// </code>
        /// 
        /// new-calendar-file.calendar content:
        /// <code>
        /// _categories.ctgs
        /// _events.evts
        /// </code>
        /// 
        /// <b>New category and event files are created with the appropriate data.</b>
        /// <br></br>
        /// 
        /// _categories.ctgs content:
        /// <code>
        /// <![CDATA[
        /// <Categories>
        /// <Category ID = "12" type="Event">Non Standard</Category>
        /// <Category ID = "1" type= "Event" > School </Category >
        /// <Category ID= "2" type= "Event" > Work </Category >
        /// <Category ID= "3" type= "Event" > Fun </Category >
        /// <Category ID= "4" type= "Event" > Medical </Category >
        /// <Category ID= "5" type= "Event" > Sleep </Category >
        /// <Category ID= "6" type= "Availability" > Working </Category >
        /// <Category ID= "7" type= "Availability" > On call</Category>
        /// <Category ID = "8" type= "Holiday" > Canadian Holidays</Category>
        /// <Category ID = "9" type= "AllDayEvent" > Vacation </Category >
        /// <Category ID= "10" type= "AllDayEvent" > Wellness days</Category>
        /// <Category ID = "11" type= "AllDayEvent" > Birthdays </Category >
        /// </Categories>
        /// ]]>
        /// </code>
        /// 
        /// _events.evts content:
        /// <code>
        /// <![CDATA[
        /// <Events>
        ///     <Event ID="1">
        ///         <StartDateTime>1/10/2018 10:00:00 AM</StartDateTime>
        ///         <Details>App Dev Homework</Details>
        ///         <DurationInMinutes>40</DurationInMinutes>
        ///         <Category>3</Category>
        ///     </Event>
        ///     <Event ID="2">
        ///         <StartDateTime>1/9/2020 12:00:00 AM</StartDateTime>
        ///         <Details>Honolulu</Details>
        ///         <DurationInMinutes>1440</DurationInMinutes>
        ///         <Category>9</Category>
        ///     </Event>
        ///     <Event ID="3">
        ///         <StartDateTime>1/10/2020 12:00:00 AM</StartDateTime>
        ///         <Details>Honolulu</Details>
        ///         <DurationInMinutes>1440</DurationInMinutes>
        ///         <Category>9</Category>
        ///     </Event>
        ///     <Event ID="4">
        ///         <StartDateTime>1/20/2020 11:00:00 AM</StartDateTime>
        ///         <Details>On call security</Details>
        ///         <DurationInMinutes>180</DurationInMinutes>
        ///         <Category>7</Category>
        ///     </Event>
        ///     <Event ID="5">
        ///         <StartDateTime>1/11/2018 7:30:00 PM</StartDateTime>
        ///         <Details>staff meeting</Details>
        ///         <DurationInMinutes>15</DurationInMinutes>
        ///         <Category>2</Category>
        ///     </Event>
        ///     <Event ID = "6" >
        ///         <StartDateTime>1/1/2020 12:00:00 AM</StartDateTime>
        ///         <Details>New Year's</Details>
        ///         <DurationInMinutes>1440</DurationInMinutes>
        ///         <Category>8</Category>
        ///     </Event>
        ///     <Event ID = "7" >
        ///         <StartDateTime>1/12/2020 12:00:00 AM</StartDateTime>
        ///         <Details>Wendy's birthday</Details>
        ///         <DurationInMinutes>1440</DurationInMinutes>
        ///         <Category>11</Category>
        ///     </Event>
        ///     <Event ID = "8" >
        ///         <StartDateTime>1/11/2018 10:15:00 AM</StartDateTime>
        ///         <Details>Sprint retrospective</Details>
        ///         <DurationInMinutes>60</DurationInMinutes>
        ///         <Category>2</Category>
        ///     </Event>
        /// </Events
        /// ]]>
        /// </code>
        /// </example>
        public void SaveToFile(String filepath)
        {

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if we can't write to the file)
            // ---------------------------------------------------------------
            filepath = CalendarFiles.VerifyWriteToFileName(filepath, "");

            String? path = Path.GetDirectoryName(Path.GetFullPath(filepath));
            String file = Path.GetFileNameWithoutExtension(filepath);
            String ext = Path.GetExtension(filepath);

            // ---------------------------------------------------------------
            // construct file names for events and categories
            // ---------------------------------------------------------------
            String eventpath = path + "\\" + file + "_events" + ".evts";
            String categorypath = path + "\\" + file + "_categories" + ".cats";

            // ---------------------------------------------------------------
            // save the events and categories into their own files
            // ---------------------------------------------------------------
            _events.SaveToFile(eventpath);
            _categories.SaveToFile(categorypath);

            // ---------------------------------------------------------------
            // save filenames of events and categories to calendar file
            // ---------------------------------------------------------------
            string[] files = { Path.GetFileName(categorypath), Path.GetFileName(eventpath) };
            System.IO.File.WriteAllLines(filepath, files);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = path;
            _FileName = Path.GetFileName(filepath);
        }
        #endregion OpenNewAndSave

        #region GetList



        // ============================================================================
        // Get all events list
        // ============================================================================

        /// <summary>
        /// Retrieves a list of calendar items from the category and event lists of the current home calendar instance. The list will only have items
        /// with a start date/time that is in between the specified range. To not have a date range, the date values can be set to null. Additionally, 
        /// if the filter flag is enabled, the list will only contain items with the specified category id. 
        /// </summary>
        /// <param name="Start">The start date/time of the range. No start date/time if null.</param>
        /// <param name="End">The end date/time of the range. No end date/time if null.</param>
        /// <param name="FilterFlag">True if the category id will be filtered; false otherwise.</param>
        /// <param name="CategoryID">The specified category id that will be filtered, only if the filter flag is true.</param>
        /// <returns>The list of calendar items that respects the specified conditions.</returns>
        /// <example>
        /// 
        /// For all examples below, assume the calendar file contains the following elements:
        /// 
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		        1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Year's                 1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendy's birthday           1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60
        /// </code>
        /// 
        /// <b>Gettign a list of all calendar items.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./calendar-file.calendar");
        /// 
        /// List<CalendarItems> calendarItems = homeCalendar.GetCalendarItems(null, null, false, 0);
        /// 
        /// //Print important values
        /// foreach (CalendarItem c in calendarItems)
        /// {
        ///        Console.WriteLine(
        ///          String.Format("{0} {1,-20}  {2,8} {3,12}",
        ///             c.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),
        ///             c.ShortDescription,
        ///             c.DurationInMinutes, c.BusyTime));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2018/Jan/10/10/00 App Dev Homework            40           40
        /// 2018/Jan/11/10/15 Sprint retrospective        60          100
        /// 2018/Jan/11/19/30 staff meeting               15          115
        /// 2020/Jan/01/00/00 New Year's                1440         1555
        /// 2020/Jan/09/00/00 Honolulu                  1440         2995
        /// 2020/Jan/10/00/00 Honolulu                  1440         4435
        /// 2020/Jan/12/00/00 Wendy's birthday          1440         5875
        /// 2020/Jan/20/11/00 On call security           180         6055
        /// </code>
        /// 
        /// <b>Getting a list of calendar items with a certain category id using the filter flag.</b>
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./calendar-file.calendar");
        /// 
        /// List<CalendarItem> calendarItems = homeCalendar.GetCalendarItems(null, null, true, 9);
        /// 
        /// //Print important values
        /// foreach (CalendarItem c in calendarItems)
        /// {
        ///        Console.WriteLine(
        ///          String.Format("{0} {1,-20}  {2,8} {3,12}",
        ///             c.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),
        ///             c.ShortDescription,
        ///             c.DurationInMinutes, c.BusyTime));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2020/Jan/09/00/00 Honolulu                  1440         2995
        /// 2020/Jan/10/00/00 Honolulu                  1440         4435
        /// </code>
        /// 
        /// <b>Getting calendar items between a certain date range.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./calendar-file.calendar");
        /// 
        /// List<CalendarItem> calendarItems = homeCalendar.GetCalendarItems(new DateTime(2019, 01, 01), new DateTime(2021, 01, 01), false, 0);
        /// 
        /// //Print important values
        /// foreach (CalendarItem c in calendarItems)
        /// {
        ///        Console.WriteLine(
        ///          String.Format("{0} {1,-20}  {2,8} {3,12}",
        ///             c.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),
        ///             c.ShortDescription,
        ///             c.DurationInMinutes, c.BusyTime));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2020/Jan/01/00/00 New Year's                1440         1555
        /// 2020/Jan/09/00/00 Honolulu                  1440         2995
        /// 2020/Jan/10/00/00 Honolulu                  1440         4435
        /// 2020/Jan/12/00/00 Wendy's birthday          1440         5875
        /// 2020/Jan/20/11/00 On call security           180         6055
        /// </code>
        /// </example>
        public List<CalendarItem> GetCalendarItems(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // ------------------------------------------------------------------------
            // return joined list within time frame
            // ------------------------------------------------------------------------
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            var query =  from c in _categories.List()
                        join e in _events.List() on c.Id equals e.Category
                        where e.StartDateTime >= Start && e.StartDateTime <= End
                        select new { CatId = c.Id, EventId = e.Id, e.StartDateTime, Category = c.Description, e.Details, e.DurationInMinutes };

            // ------------------------------------------------------------------------
            // create a CalendarItem list with totals,
            // ------------------------------------------------------------------------
            List<CalendarItem> items = new List<CalendarItem>();
            Double totalBusyTime = 0;

            foreach (var e in query.OrderBy(q => q.StartDateTime))
            {
                // filter out unwanted categories if filter flag is on
                if (FilterFlag && CategoryID != e.CatId)
                {
                    continue;
                }

                // keep track of running totals
                totalBusyTime = totalBusyTime + e.DurationInMinutes;
                items.Add(new CalendarItem
                {
                    CategoryID = e.CatId,
                    EventID = e.EventId,
                    ShortDescription = e.Details,
                    StartDateTime = e.StartDateTime,
                    DurationInMinutes = e.DurationInMinutes,
                    Category = e.Category,
                    BusyTime = totalBusyTime
                });
            }

            return items;
        }

        // ============================================================================
        // Group all events month by month (sorted by year/month)
        // returns a list of CalendarItemsByMonth which is 
        // "year/month", list of calendar items, and totalBusyTime for that month
        // ============================================================================

        /// <summary>
        /// Groups all events by month/year and stores them in a list of CalendarItemsByMonth. CalendarItemsByMonth is "year/month", list of calendar items, and 
        /// totalBusyTime for that month. The list will only have items with a start date/time that is in between the specified range. To not have
        /// a date range, the date values can be set to null. Additionally, if the filter flag is enabled, the list will only contain items with the 
        /// specified category id. 
        /// </summary>
        /// <param name="Start">The start date/time of the range. No start date/time if null.</param>
        /// <param name="End">The end date/time of the range. No end date/time if null.</param>
        /// <param name="FilterFlag">True if the category id will filtered; false otherwise.</param>
        /// <param name="CategoryID">The specified category id that will be filtered, only if the filter flag is true.</param>
        /// <returns>The list of CalendarItemsByMonth that respects the specified conditions.</returns>
        /// <example>
        /// 
        /// For all examples below, assume the calendar file contains the following elements:
        /// 
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		        1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Year's                 1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendy's birthday           1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60
        /// </code>
        /// 
        /// <b>Get all calendar items grouped by month.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./calendar-file.calendar");
        /// 
        /// List<CalendarItemsByMonth> calendarItemsByMonth = homeCalendar.GetCalendarItemsByMonth(null, null, false, 0);
        /// 
        /// //Print important values
        /// foreach (CalendarItemsByMonth month in calendarItemsByMonth)
        /// {
        ///     Console.WriteLine($"{month.Month}:");
        ///     foreach(CalendarItem c in month.Items)
        ///     {
        ///         Console.WriteLine(
        ///         String.Format("{0} {1,-20}  {2,8} {3,12}",
        ///         c.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),
        ///         c.ShortDescription,
        ///         c.DurationInMinutes, c.BusyTime));
        ///     }
        ///     Console.WriteLine();
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2018/01:
        /// 2018/Jan/10/10/00 App Dev Homework            40           40
        /// 2018/Jan/11/10/15 Sprint retrospective        60          100
        /// 2018/Jan/11/19/30 staff meeting               15          115
        /// 
        /// 2020/01:
        /// 2020/Jan/01/00/00 New Year's                1440         1555
        /// 2020/Jan/09/00/00 Honolulu                  1440         2995
        /// 2020/Jan/10/00/00 Honolulu                  1440         4435
        /// 2020/Jan/12/00/00 Wendy's birthday          1440         5875
        /// 2020/Jan/20/11/00 On call security           180         6055
        /// </code>
        /// 
        /// <b>Get all calendar items grouped by month with a specific id using the filter flag.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./calendar-file.calendar");
        /// 
        /// List<CalendarItemsByMonth> calendarItemsByMonth = homeCalendar.GetCalendarItemsByMonth(null, null, true, 9);
        /// 
        /// //Print important values
        /// foreach (CalendarItemsByMonth month in calendarItemsByMonth)
        /// {
        ///     Console.WriteLine($"{month.Month}:");
        ///     foreach(CalendarItem c in month.Items)
        ///     {
        ///         Console.WriteLine(
        ///         String.Format("{0} {1,-20}  {2,8} {3,12}",
        ///         c.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),
        ///         c.ShortDescription,
        ///         c.DurationInMinutes, c.BusyTime));
        ///     }
        ///     Console.WriteLine();
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2020/01:
        /// 2020/Jan/09/00/00 Honolulu                  1440         2995
        /// 2020/Jan/10/00/00 Honolulu                  1440         4435
        /// </code>
        /// 
        /// <b>Get all calendar items grouped by month in between a certain date range.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./calendar-file.calendar");
        /// 
        /// List<CalendarItemsByMonth> calendarItemsByMonth = homeCalendar.GetCalendarItemsByMonth(new DateTime(2019, 01, 01), new DateTime(2021, 01, 01), false, 0);
        /// 
        /// //Print important values
        /// foreach (CalendarItemsByMonth month in calendarItemsByMonth)
        /// {
        ///     Console.WriteLine($"{month.Month}:");
        ///     foreach(CalendarItem c in month.Items)
        ///     {
        ///         Console.WriteLine(
        ///         String.Format("{0} {1,-20}  {2,8} {3,12}",
        ///         c.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),
        ///         c.ShortDescription,
        ///         c.DurationInMinutes, c.BusyTime));
        ///     }
        ///     Console.WriteLine();
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2020/01:
        /// 2020/Jan/01/00/00 New Year's                1440         1555
        /// 2020/Jan/09/00/00 Honolulu                  1440         2995
        /// 2020/Jan/10/00/00 Honolulu                  1440         4435
        /// 2020/Jan/12/00/00 Wendy's birthday          1440         5875
        /// 2020/Jan/20/11/00 On call security           180         6055
        /// </code>
        /// </example>
        public List<CalendarItemsByMonth> GetCalendarItemsByMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<CalendarItem> items = GetCalendarItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by year/month
            // -----------------------------------------------------------------------
            var GroupedByMonth = items.GroupBy(c => c.StartDateTime.Year.ToString("D4") + "/" + c.StartDateTime.Month.ToString("D2"));

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<CalendarItemsByMonth>();
            foreach (var MonthGroup in GroupedByMonth)
            {
                // calculate totalBusyTime for this month, and create list of items
                double total = 0;
                var itemsList = new List<CalendarItem>();
                foreach (var item in MonthGroup)
                {
                    total = total + item.DurationInMinutes;
                    itemsList.Add(item);
                }

                // Add new CalendarItemsByMonth to our list
                summary.Add(new CalendarItemsByMonth
                {
                    Month = MonthGroup.Key,
                    Items = itemsList,
                    TotalBusyTime = total
                });
            }

            return summary;
        }

        // ============================================================================
        // Group all events by category (ordered by category name)
        // ============================================================================

        /// <summary>
        /// Gets all categories within the database and uses the retrieved categories to get all calendar items within that category from the database. The user can
        /// pass filters; a start date time, end date time and a specified category id to find calendar items in. The specified category id will only
        /// be accounted for if the filter flag is true.
        /// </summary>
        /// <param name="Start">The start date/time of the range. No start date/time if null.</param>
        /// <param name="End">The end date/time of the range. No end date/time if null.</param>
        /// <param name="FilterFlag">True if the category id will filtered; false otherwise.</param>
        /// <param name="CategoryID">The specified category id that will be filtered, only if the filter flag is true.</param>
        /// <returns>The list of CalendarItemsByCategory that respects the specified conditions.</returns>
        /// <example>
        /// 
        /// For all examples below, assume the calendar database contains the following elements:
        /// 
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		        1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Year's                 1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendy's birthday           1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60
        /// </code>
        /// 
        /// <b>Get all calendar items grouped by category.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar(messyDB, inFile, false);
        /// 
        /// List<CalendarItemsByCategory> calendarItemsByCategory = homeCalendar.GetCalendarItemsByCategory(null, null, false, 0);
        /// 
        /// //Print important values
        /// foreach (CalendarItemsByCategory cat in calendarItemsByCategory)
        /// {
        ///     Console.WriteLine($"{cat.Category}:");
        ///     foreach(CalendarItem c in cat.Items)
        ///     {
        ///         Console.WriteLine(
        ///         String.Format("{0} {1,-20}  {2,8} {3,12}",
        ///         c.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),
        ///         c.ShortDescription,
        ///         c.DurationInMinutes, c.BusyTime));
        ///     }
        ///     Console.WriteLine();
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Birthdays:
        /// 2020-Jan.-12-00-00 Wendy's birthday          1440         5875
        ///
        /// Canadian Holidays:
        /// 2020-Jan.-01-00-00 New Year's                1440         1555
        ///
        /// Fun:
        /// 2018-Jan.-10-10-00 App Dev Homework            40           40
        ///
        /// On call:
        /// 2020-Jan.-20-11-00 On call security           180         6055
        ///
        /// Vacation:
        /// 2020-Jan.-09-00-00 Honolulu                  1440         2995
        /// 2020-Jan.-10-00-00 Honolulu                  1440         4435
        ///
        /// Work:
        /// 2018-Jan.-11-10-15 Sprint retrospective        60          100
        /// 2018-Jan.-11-19-30 staff meeting               15          115
        /// </code>
        /// 
        /// <b>Get all calendar items grouped by category with a specific id using the filter flag.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar(messyDB, inFile, false);
        /// 
        /// List<CalendarItemsByCategory> calendarItemsByCategory = homeCalendar.GetCalendarItemsByCategory(null, null, true, 9);
        /// 
        /// //Print important values
        /// foreach (CalendarItemsByCategory cat in calendarItemsByCategory)
        /// {
        ///     Console.WriteLine($"{cat.Category}:");
        ///     foreach(CalendarItem c in cat.Items)
        ///     {
        ///         Console.WriteLine(
        ///         String.Format("{0} {1,-20}  {2,8} {3,12}",
        ///         c.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),
        ///         c.ShortDescription,
        ///         c.DurationInMinutes, c.BusyTime));
        ///     }
        ///     Console.WriteLine();
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Vacation:
        /// 2020-Jan.-09-00-00 Honolulu                  1440         2995
        /// 2020-Jan.-10-00-00 Honolulu                  1440         4435
        /// </code>
        /// 
        /// <b>Get all calendar items grouped by category within a specified date range.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar(messyDB, inFile, false);
        /// 
        /// List<CalendarItemsByCategory> calendarItemsByCategory = homeCalendar.GetCalendarItemsByCategory(new DateTime(2019, 01, 01), new DateTime(2021, 01, 01), false, 0);
        /// 
        /// //Print important values
        /// foreach (CalendarItemsByCategory cat in calendarItemsByCategory)
        /// {
        ///     Console.WriteLine($"{cat.Category}:");
        ///     foreach(CalendarItem c in cat.Items)
        ///     {
        ///         Console.WriteLine(
        ///         String.Format("{0} {1,-20}  {2,8} {3,12}",
        ///         c.StartDateTime.ToString("yyyy/MMM/dd/HH/mm"),
        ///         c.ShortDescription,
        ///         c.DurationInMinutes, c.BusyTime));
        ///     }
        ///     Console.WriteLine();
        /// }
        /// ]]>
        /// </code>
        ///
        /// Sample output:
        /// <code>
        /// Birthdays:
        /// 2020-Jan.-12-00-00 Wendy's birthday          1440         5875
        ///
        /// Canadian Holidays:
        /// 2020-Jan.-01-00-00 New Year's                1440         1555
        ///
        /// On call:
        /// 2020-Jan.-20-11-00 On call security           180         6055
        ///
        /// Vacation:
        /// 2020-Jan.-09-00-00 Honolulu                  1440         2995
        /// 2020-Jan.-10-00-00 Honolulu                  1440         4435
        /// </code>
        /// </example>
        public List<CalendarItemsByCategory> GetCalendarItemsByCategory(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------


            List<Category> newList = new List<Category>();

            SQLiteCommand cmd = new SQLiteCommand(Database.dbConnection);

            cmd.CommandText = "SELECT Id, Description, TypeId FROM categories ORDER BY Description;";

            if (FilterFlag)
            {
                cmd.CommandText = "SELECT Id, Description, TypeId FROM categories WHERE Id=@CatId ORDER BY Description;";
                cmd.Parameters.AddWithValue("CatId", CategoryID);
            }
        
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                newList.Add(new Category(reader.GetInt32(0), reader.GetString(1), (Category.CategoryType)reader.GetInt32(2)));
            }

            cmd.Dispose();
        
            // -----------------------------------------------------------------------
            // Group by Category
            // -----------------------------------------------------------------------

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<CalendarItemsByCategory>();
            foreach (var CategoryGroup in newList)
            {
                List<CalendarItem> filteredItems = GetCalendarItems(Start, End, true,CategoryGroup.Id);
                if( filteredItems.Count != 0)
                {
                    // calculate totalBusyTime for this category, and create list of items
                    double total = 0;
                    var items = new List<CalendarItem>();
                    foreach (var item in filteredItems)
                    {
                        total = total + item.DurationInMinutes;
                        items.Add(item);
                    }

                    // Add new CalendarItemsByCategory to our list
                    summary.Add(new CalendarItemsByCategory
                    {
                        Category = CategoryGroup.Description,
                        Items = items,
                        TotalBusyTime = total
                    });
                }
                
            }

            return summary;
        }



        // ============================================================================
        // Group all events by category and Month
        // creates a list of Dictionary objects with:
        //          one dictionary object per month,
        //          and one dictionary object for the category total busy times
        // 
        // Each per month dictionary object has the following key value pairs:
        //           "Month", <name of month>
        //           "TotalBusyTime", <the total durations for the month>
        //             for each category for which there is an event in the month:
        //             "items:category", a List<CalendarItem>
        //             "category", the total busy time for that category for this month
        // The one dictionary for the category total busy times has the following key value pairs:
        //             for each category for which there is an event in ANY month:
        //             "category", the total busy time for that category for all the months
        // ============================================================================

        /// <summary>
        /// Groups all events by category and Month and stores them in a list of dictionary objects. The dictionary objects contain: one dictionary
        /// per month, and one dictionary object for the category total busy times. Each per month dictionary object has the following key value
        /// pairs: ("Month", name of month), ("TotalBusyTime", total durations for the month). For each category in that month: ("items:category",
        /// list of calendar items), (:category:, total busy time of that category in that month). The last dictionary contains for each category
        /// with an event in any month: ("category", total busy time for that category in all months). There will only be items with a start 
        /// date/time that is in between the specified range. To not have a date range, the date values can be set to null. Additionally, if the 
        /// filter flag is enabled, the list will only contain items with the specified category id. 
        /// </summary>
        /// <param name="Start">The start date/time of the range. No start date/time if null.</param>
        /// <param name="End">The end date/time of the range. No end date/time if null.</param>
        /// <param name="FilterFlag">True if the category id will filtered; false otherwise.</param>
        /// <param name="CategoryID">The specified category id that will be filtered, only if the filter flag is true.</param>
        /// <returns>The list of dictionaries with the specified events grouped by category and month.</returns>
        /// <example>
        /// 
        /// For all examples below, assume the calendar file contains the following elements:
        /// 
        /// <code>
        /// Cat_ID  Event_ID  StartDateTime           Details                 DurationInMinutes
        ///    3       1      1/10/2018 10:00:00 AM   App Dev Homework             40
        ///    9       2      1/9/2020 12:00:00 AM    Honolulu		        1440
        ///    9       3      1/10/2020 12:00:00 AM   Honolulu                   1440
        ///    7       4      1/20/2020 11:00:00 AM   On call security            180
        ///    2       5      1/11/2018 7:30:00 PM    staff meeting                15
        ///    8       6      1/1/2020 12:00:00 AM    New Year's                 1440
        ///   11       7      1/12/2020 12:00:00 AM   Wendy's birthday           1440
        ///    2       8      1/11/2018 10:15:00 AM   Sprint retrospective         60
        /// </code>
        /// 
        /// <b>Get all calendar items grouped by month and category.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./test.calendar");
        /// 
        /// List<Dictionary<string, object>> dictionaryList = homeCalendar.GetCalendarDictionaryByCategoryAndMonth(null, null, false, 0);
        /// 
        /// //Print important values
        /// foreach(Dictionary<string, object> dictionary in dictionaryList)
        /// {
        ///     for(int i = 0; i<dictionary.Count; i++)
        ///     {
        ///         if(i != 0 && i % 2 == 0 && dictionary.ElementAt(0).Value != "TOTALS")
        ///         {
        ///             Console.WriteLine(dictionary.ElementAt(i).Key + ":");
        ///             foreach(CalendarItem item in (List<CalendarItem>) dictionary.ElementAt(i).Value)
        ///             {
        ///                 Console.WriteLine(item.ShortDescription + ", " + item.StartDateTime);
        ///             }
        ///         }
        ///         else
        ///         {
        ///             Console.WriteLine(dictionary.ElementAt(i).Key + ": " + dictionary.ElementAt(i).Value);
        ///             Console.WriteLine();
        ///         }
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Month: 2018/01
        ///
        /// TotalBusyTime: 115
        ///
        /// items:Fun:
        /// App Dev Homework, 2018-01-10 10:00:00 AM
        /// Fun: 40
        ///
        /// items:Work:
        /// Sprint retrospective, 2018-01-11 10:15:00 AM
        /// staff meeting, 2018-01-11 7:30:00 PM
        /// Work: 75
        ///
        /// Month: 2020/01
        ///
        /// TotalBusyTime: 5940
        ///
        /// items:Birthdays:
        /// Wendy's birthday, 2020-01-12 12:00:00 AM
        /// Birthdays: 1440
        ///
        /// items:Canadian Holidays:
        /// New Year's, 2020-01-01 12:00:00 AM
        /// Canadian Holidays: 1440
        ///
        /// items:On call:
        /// On call security, 2020-01-20 11:00:00 AM
        /// On call: 180
        ///
        /// items:Vacation:
        /// Honolulu, 2020-01-09 12:00:00 AM
        /// Honolulu, 2020-01-10 12:00:00 AM
        /// Vacation: 2880
        ///
        /// Month: TOTALS
        ///
        /// Work: 75
        ///
        /// Fun: 40
        ///
        /// On call: 180
        ///
        /// Canadian Holidays: 1440
        ///
        /// Vacation: 2880
        ///
        /// Birthdays: 1440
        /// </code>
        /// 
        /// <b>Get all calendar items grouped by month and category with a specified id using the filter flag.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./test.calendar");
        /// 
        /// List<Dictionary<string, object>> dictionaryList = homeCalendar.GetCalendarDictionaryByCategoryAndMonth(null, null, true, 9);
        /// 
        /// //Print important values
        /// foreach(Dictionary<string, object> dictionary in dictionaryList)
        /// {
        ///     for(int i = 0; i<dictionary.Count; i++)
        ///     {
        ///         if(i != 0 && i % 2 == 0 && dictionary.ElementAt(0).Value != "TOTALS")
        ///         {
        ///             Console.WriteLine(dictionary.ElementAt(i).Key + ":");
        ///             foreach(CalendarItem item in (List<CalendarItem>) dictionary.ElementAt(i).Value)
        ///             {
        ///                 Console.WriteLine(item.ShortDescription + ", " + item.StartDateTime);
        ///             }
        ///         }
        ///         else
        ///         {
        ///             Console.WriteLine(dictionary.ElementAt(i).Key + ": " + dictionary.ElementAt(i).Value);
        ///             Console.WriteLine();
        ///         }
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Month: 2020/01
        ///
        /// TotalBusyTime: 2880
        /// 
        /// items:Vacation:
        /// Honolulu, 2020-01-09 12:00:00 AM
        /// Honolulu, 2020-01-10 12:00:00 AM
        /// Vacation: 2880
        /// 
        /// Month: TOTALS
        /// 
        /// Vacation: 2880
        /// </code>
        /// 
        /// <b>Get all calendar items grouped by month and category within a specified date range.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// HomeCalendar homeCalendar = new HomeCalendar("./test.calendar");
        /// 
        /// List<Dictionary<string, object>> dictionaryList = homeCalendar.GetCalendarDictionaryByCategoryAndMonth(new DateTime(2019, 01, 01), new DateTime(2021, 01, 01), false, 0);
        /// 
        /// //Print important values
        /// foreach(Dictionary<string, object> dictionary in dictionaryList)
        /// {
        ///     for(int i = 0; i<dictionary.Count; i++)
        ///     {
        ///         if(i != 0 && i % 2 == 0 && dictionary.ElementAt(0).Value != "TOTALS")
        ///         {
        ///             Console.WriteLine(dictionary.ElementAt(i).Key + ":");
        ///             foreach(CalendarItem item in (List<CalendarItem>) dictionary.ElementAt(i).Value)
        ///             {
        ///                 Console.WriteLine(item.ShortDescription + ", " + item.StartDateTime);
        ///             }
        ///         }
        ///         else
        ///         {
        ///             Console.WriteLine(dictionary.ElementAt(i).Key + ": " + dictionary.ElementAt(i).Value);
        ///             Console.WriteLine();
        ///         }
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Month: 2020/01
        ///
        /// TotalBusyTime: 5940
        ///
        /// items:Birthdays:
        /// Wendy's birthday, 2020-01-12 12:00:00 AM
        /// Birthdays: 1440
        ///
        /// items:Canadian Holidays:
        /// New Year's, 2020-01-01 12:00:00 AM
        /// Canadian Holidays: 1440
        ///
        /// items:On call:
        /// On call security, 2020-01-20 11:00:00 AM
        /// On call: 180
        ///
        /// items:Vacation:
        /// Honolulu, 2020-01-09 12:00:00 AM
        /// Honolulu, 2020-01-10 12:00:00 AM
        /// Vacation: 2880
        ///
        /// Month: TOTALS
        /// 
        /// On call: 180
        ///
        /// Canadian Holidays: 1440
        ///
        /// Vacation: 2880
        ///
        /// Birthdays: 1440
        /// </code>
        /// </example>
        public List<Dictionary<string,object>> GetCalendarDictionaryByCategoryAndMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items by month 
            // -----------------------------------------------------------------------
            List<CalendarItemsByMonth> GroupedByMonth = GetCalendarItemsByMonth(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // loop over each month
            // -----------------------------------------------------------------------
            var summary = new List<Dictionary<string, object>>();
            var totalBusyTimePerCategory = new Dictionary<String, Double>();

            foreach (var MonthGroup in GroupedByMonth)
            {
                // create record object for this month
                Dictionary<string, object> record = new Dictionary<string, object>();
                record["Month"] = MonthGroup.Month;
                record["TotalBusyTime"] = MonthGroup.TotalBusyTime;

                // break up the month items into categories
                var GroupedByCategory = MonthGroup.Items.GroupBy(c => c.Category);

                // -----------------------------------------------------------------------
                // loop over each category
                // -----------------------------------------------------------------------
                foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
                {

                    // calculate totals for the cat/month, and create list of items
                    double totalCategoryBusyTimeForThisMonth = 0;
                    var details = new List<CalendarItem>();

                    foreach (var item in CategoryGroup)
                    {
                        totalCategoryBusyTimeForThisMonth = totalCategoryBusyTimeForThisMonth + item.DurationInMinutes;
                        details.Add(item);
                    }

                    // add new properties and values to our record object
                    record["items:" + CategoryGroup.Key] = details;
                    record[CategoryGroup.Key] = totalCategoryBusyTimeForThisMonth;

                    // keep track of totals for each category
                    if (totalBusyTimePerCategory.TryGetValue(CategoryGroup.Key, out Double currentTotalBusyTimeForCategory))
                    {
                        totalBusyTimePerCategory[CategoryGroup.Key] = currentTotalBusyTimeForCategory + totalCategoryBusyTimeForThisMonth;
                    }
                    else
                    {
                        totalBusyTimePerCategory[CategoryGroup.Key] = totalCategoryBusyTimeForThisMonth;
                    }
                }

                // add record to collection
                summary.Add(record);
            }
            // ---------------------------------------------------------------------------
            // add final record which is the totals for each category
            // ---------------------------------------------------------------------------
            Dictionary<string, object> totalsRecord = new Dictionary<string, object>();
            totalsRecord["Month"] = "TOTALS";

            foreach (var cat in categories.List())
            {
                try
                {
                    totalsRecord.Add(cat.Description, totalBusyTimePerCategory[cat.Description]);
                }
                catch { }
            }
            summary.Add(totalsRecord);


            return summary;
        }




        #endregion GetList

    }
}
