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
        private static String DefaultFileName = "calendar.txt";
        private List<Event> _Events = new List<Event>();
        private string _FileName;
        private string _DirName;
        private SQLiteConnection _connection;
        // ====================================================================
        // Properties
        // ====================================================================

        /// <summary>
        /// Gets and sets the FileName.
        /// </summary>
        /// <value>Represents the file name of the file containing the events data in xml form.</value>
        public String FileName { get { return _FileName; } }

        /// <summary>
        /// Gets and sets the DirName
        /// </summary>
        /// <value>Represents the directory name where the file containing the events data in xml form is located.</value>
        public String DirName { get { return _DirName; } }

        public Events(SQLiteConnection connection, bool newDB = false)
        {
            _connection = connection;
        }
        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================

        /// <summary>
        /// Verifies the file path and populates the current instance using data found in the passed file path. If the passed file path is not specified,
        /// it reads in the AppData file. To call the method without specifying the file path, pass in null.
        /// </summary>
        /// <param name="filepath">The path of the xml file with data representing a list of events.</param>
        /// <example>
        /// 
        /// In this example, assume that the event file contains the following elements:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// 5  2018-01-11 7:30:00 PM     2          15         staff meeting
        /// 6  2020-01-01 12:00:00 AM    8          1440       New Year's
        /// 7  2020-01-12 12:00:00 AM    11         1440       Wendy's birthday
        /// 8  2018-01-11 10:15:00 AM    2          60         Sprint retrospective
        /// </code>
        /// 
        /// <b>Populating the list of events from the file.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// Events events = new Events();
        /// events.ReadFromFile("./event-file.evts");
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
        /// 5  2018-01-11 7:30:00 PM     2          15         staff meeting
        /// 6  2020-01-01 12:00:00 AM    8          1440       New Year's
        /// 7  2020-01-12 12:00:00 AM    11         1440       Wendy's birthday
        /// 8  2018-01-11 10:15:00 AM    2          60         Sprint retrospective
        /// </code>
        /// </example>
        public void ReadFromFile(String filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current Events,
            // so clear out any old definitions
            // ---------------------------------------------------------------
            _Events.Clear();

            // ---------------------------------------------------------------
            // reset default dir/filename to null 
            // ... filepath may not be valid, 
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = CalendarFiles.VerifyReadFromFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // read the Events from the xml file
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use?
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);


        }

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================

        /// <summary>
        /// Verifies the file path and saves the events of the current instance as xml data to the file. If the passed file path is not specified,
        /// it saves in the AppData file. To call the method without specifying the file path, pass in null. This data can be loaded back using the 
        /// ReadFromFile method.
        /// </summary>
        /// <param name="filepath">The path of the xml file where the data of the current instance will be converted and saved.</param>
        /// <example>
        /// 
        /// In this example, assume that the event file contains the following elements:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// </code>
        /// 
        /// <b>Save the contents to a new file.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// Events events = new Events();
        /// events.ReadFromFile("./event-file.evts");
        /// 
        /// events.SaveToFile("./new-events-file.evts");
        /// ]]>
        /// </code>
        /// 
        /// new-events-file.evts content:
        /// <code>
        /// <![CDATA[
        /// <Events>
        ///     <Event ID="1">
        ///         <StartDateTime>1/10/2018 10:00:00 AM</StartDateTime>
        ///         <Details>App Dev Homework</Details>
        ///         <DurationInMinutes>40</DurationInMinutes>
        ///         <Category>3</Category>
        ///     </Event>
        ///     <Event ID = "2">
        ///         <StartDateTime>1/9/2020 12:00:00 AM</StartDateTime>
        ///         <Details>Honolulu</Details>
        ///         <DurationInMinutes>1440</DurationInMinutes>
        ///         <Category>9</Category>
        ///     </Event>
        ///     <Event ID = "3">
        ///         <StartDateTime>1/10/ 2020 12:00:00 AM</StartDateTime>
        ///         <Details>Honolulu</Details>
        ///         <DurationInMinutes>1440</DurationInMinutes>
        ///         <Category>9</Category>
        ///     </Event>
        /// </Events
        /// ]]>
        /// </code>
        /// </example>
        public void SaveToFile(String filepath = null)
        {
            // ---------------------------------------------------------------
            // if file path not specified, set to last read file
            // ---------------------------------------------------------------
            if (filepath == null && DirName != null && FileName != null)
            {
                filepath = DirName + "\\" + FileName;
            }

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = CalendarFiles.VerifyWriteToFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // save as XML
            // ---------------------------------------------------------------
            _WriteXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }



        // ====================================================================
        // Add Event
        // ====================================================================
        private void Add(Event exp)
        {
            _Events.Add(exp);
        }

        /// <summary>
        /// Adds a new event with the specified values to the event list of the current instance. Automatically finds the next
        /// id that the event should have.
        /// </summary>
        /// <param name="date">The object representing the start date/time of the added event.</param>
        /// <param name="category">The category of the added event.</param>
        /// <param name="duration">The duration in minutes of the added event.</param>
        /// <param name="details">The details of the added event.</param>
        /// <example>
        /// 
        /// In this example, assume that the event file contains the following elements:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// </code>
        /// 
        /// <b>Adding a event to the list.</b>
        /// <code>
        /// <![CDATA[
        /// Events events = new Events();
        /// events.ReadFromFile("./event-file.evts");
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
        /// Removes the event from the list of events with the specified id. The passed id must be the id of a category in the list.
        /// </summary>
        /// <param name="Id">The id of the event that will be removed from the list.</param>
        /// <example>
        /// 
        /// In this example, assume that the event file contains the following elements:
        /// 
        /// <code>
        /// Id Start date/time           Category   Duration   Details
        /// 1  2018-01-10 10:00:00 AM    3          40         App Dev Homework
        /// 2  2020-01-09 12:00:00 AM    9          1440       Honolulu
        /// 3  2020-01-10 12:00:00 AM    9          1440       Honolulu
        /// 4  2020-01-20 11:00:00 AM    7          180        On call security
        /// </code>
        /// 
        /// <b>Deleting an event from the list.</b>
        /// <code>
        /// <![CDATA[
        /// Events events = new Events();
        /// events.ReadFromFile("./event-file.evts");
        /// 
        /// categories.Delete(3);
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


        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {


            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                // Loop over each Event
                foreach (XmlNode Event in doc.DocumentElement.ChildNodes)
                {
                    // set default Event parameters
                    int id = int.Parse((((XmlElement)Event).GetAttributeNode("ID")).InnerText);
                    String description = "";
                    DateTime date = DateTime.Parse("2000-01-01");
                    int category = 0;
                    Double DurationInMinutes = 0.0;

                    // get Event parameters
                    foreach (XmlNode info in Event.ChildNodes)
                    {
                        switch (info.Name)
                        {
                            case "StartDateTime":
                                date = DateTime.Parse(info.InnerText);
                                break;
                            case "DurationInMinutes":
                                DurationInMinutes = Double.Parse(info.InnerText);
                                break;
                            case "Details":
                                description = info.InnerText;
                                break;
                            case "Category":
                                category = int.Parse(info.InnerText);
                                break;
                        }
                    }

                    // have all info for Event, so create new one
                    this.Add(new Event(id, date, category, DurationInMinutes, description));

                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadFromFileException: Reading XML " + e.Message);
            }
        }


        // ====================================================================
        // write to an XML file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            // ---------------------------------------------------------------
            // loop over all categories and write them out as XML
            // ---------------------------------------------------------------
            try
            {
                // create top level element of Events
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Events></Events>");

                // foreach Category, create an new xml element
                foreach (Event exp in _Events)
                {
                    // main element 'Event' with attribute ID
                    XmlElement ele = doc.CreateElement("Event");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = exp.Id.ToString();
                    ele.SetAttributeNode(attr);
                    doc.DocumentElement.AppendChild(ele);

                    // child attributes (date, description, DurationInMinutes, category)
                    XmlElement d = doc.CreateElement("StartDateTime");
                    XmlText dText = doc.CreateTextNode(exp.StartDateTime.ToString("M\\/d\\/yyyy h:mm:ss tt"));
                    ele.AppendChild(d);
                    d.AppendChild(dText);

                    XmlElement de = doc.CreateElement("Details");
                    XmlText deText = doc.CreateTextNode(exp.Details);
                    ele.AppendChild(de);
                    de.AppendChild(deText);

                    XmlElement a = doc.CreateElement("DurationInMinutes");
                    XmlText aText = doc.CreateTextNode(exp.DurationInMinutes.ToString());
                    ele.AppendChild(a);
                    a.AppendChild(aText);

                    XmlElement c = doc.CreateElement("Category");
                    XmlText cText = doc.CreateTextNode(exp.Category.ToString());
                    ele.AppendChild(c);
                    c.AppendChild(cText);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("SaveToFileException: Reading XML " + e.Message);
            }
        }

    }
}

