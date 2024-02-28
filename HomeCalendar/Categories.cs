using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Data.SQLite;
using System.Data.Common;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{
    // ====================================================================
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to file
    //        - etc
    // ====================================================================

    /// <summary>
    /// Manages a collection of category items. Reads and writes to files storing the categories. Initialized with default values in the constructor, 
    /// but also from files with data containing various categories when using the ReadFromFile method.
    /// </summary>
    public class Categories
    {
        private static String DefaultFileName = "calendarCategories.txt";
        private List<Category> _Categories = new List<Category>();
        private string? _FileName;
        private string? _DirName;
        private SQLiteConnection _connection;
        // ====================================================================
        // Properties
        // ====================================================================

        /// <summary>
        /// Gets and sets the FileName.
        /// </summary>
        /// <value>Represents the file name of the file containing the categories data in xml form.</value>
        public String? FileName { get { return _FileName; } }

        /// <summary>
        /// Gets and sets the DirName.
        /// </summary>
        /// <value>Represents the directory name where the file containing the categories data in xml form is located.</value>
        public String? DirName { get { return _DirName; } }

        // ====================================================================
        // Constructor
        // ====================================================================

        /// <summary>
        /// Initializes a Categories instance with a connection the database or with a default collection of categories.
        /// </summary>
        /// <param name="connection">The database connection that allows access to the database.</param>    
        /// <param name="newDB">Boolean value that tells whether it is a new database and sets default categories.</param>    
        /// <example>
        /// 
        /// For the example below, assume that the database has the following information:
        /// 
        /// <code>
        ///    Id         Type          Description
        ///    1          Event         School
        ///    2          Event         Personal
        ///    3          Event         VideoGames
        ///    4          Event         Medical
        ///    5          Event         Sleep
        ///    6          AllDayEvent   Vacation
        ///    7          AllDayEvent   Travel days
        ///    8          Holiday       Canadian Holidays
        ///    9          Holiday       US Holidays
        /// </code>
        /// 
        /// <b>Initializing a new instance.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories(Database.dbConnection);
        /// 
        /// Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", "Id", "Type", "Description"));
        /// foreach (Category c in categories.List())
        /// {
        ///     Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", c.Id, c.Type, c.Description));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        ///    Id         Type          Description
        ///    1          Event         School
        ///    2          Event         Personal
        ///    3          Event         VideoGames
        ///    4          Event         Medical
        ///    5          Event         Sleep
        ///    6          AllDayEvent   Vacation
        ///    7          AllDayEvent   Travel days
        ///    8          Holiday       Canadian Holidays
        ///    9          Holiday       US Holidays
        /// </code>
        /// </example>
        public Categories(SQLiteConnection connection, bool newDB = false)
        {
            _connection = connection;
 
            if(newDB)
            {
                SetCategoriesToDefaults();
            }

        }

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================

        /// <summary>
        /// Gets the category with the specified ID from the stored list of categories. Used to find the category object when you only have the ID. 
        /// If there is no category at the searched index, an exception is thrown.
        /// </summary>
        /// <param name="i">The ID of the searched category.</param>
        /// <returns>The category object with the specified ID.</returns>
        /// <exception cref="Exception">Throws when there is no category object with the specified ID in the list.</exception>
        /// <example>
        /// 
        /// In this example, assume that the category list of the instance contains the following elements:
        /// 
        /// <code>
        ///    Id    Type         Description
        ///    1     Event        School
        ///    2     Holiday      Canadian Holidays
        ///    3     Event        Vacation
        ///    4     Event        Wellness Day
        /// </code>
        /// 
        /// <b>Getting the category object that has the id 3.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories();
        /// categories.ReadFromFile("./category-file.cats");
        /// 
        /// Category c = categories.GetCategoryFromId(3);
        /// Console.WriteLine(c.Description);
        /// ]]>
        /// </code>
        /// 
        /// Output:
        /// <code>
        /// Vacation
        /// </code>
        /// </example>
        public Category GetCategoryFromId(int i)
        {
            Category? c = _Categories.Find(x => x.Id == i);
            if (c == null)
            {
                throw new Exception("Cannot find category with id " + i.ToString());
            }
            return c;
        }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================

        /// <summary>
        /// Verifies the file path and populates the current instance using data found in the file. If the passed file path is not specified,
        /// it reads in the AppData file. To call the method without specifying the file path, pass in null.
        /// </summary>
        /// <param name="filepath">The path of the xml file with data representing a list of categories.</param>
        /// <example>
        /// 
        /// In this example, assume that the category file contains the following elements:
        /// 
        /// <code>
        ///    Id    Type         Description
        ///    1     Event        School
        ///    2     Holiday      Canadian Holiday
        ///    3     Event        Vacation
        ///    4     Event        Wellness Day
        /// </code>
        /// 
        /// <b>Populating the list of categories from the file.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories();
        /// categories.ReadFromFile("./category-file.cats");
        /// 
        /// Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", "Id", "Type", "Description"));
        /// foreach (Category c in categories.List())
        /// {
        ///     Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", c.Id, c.Type, c.Description));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Id         Type       Description
        /// 1          Event      School
        /// 2          Holiday    Canadian Holiday
        /// 3          Event      Vacation
        /// 4          Event      Wellness Day
        /// </code>
        /// </example>
        public void ReadFromFile(String? filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current categories,
            // ---------------------------------------------------------------
            _Categories.Clear();

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
            // If file exists, read it
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================

        /// <summary>
        /// Verifies the file path and saves the categories of the current instance as xml data to the file. If the passed file path is not specified,
        /// it saves in the AppData file. To call the method without specifying the file path, pass in null. This data can be loaded back using the 
        /// ReadFromFile method.
        /// </summary>
        /// <param name="filepath">The path of the xml file where the data of the current instance will be converted and saved.</param>
        /// <example>
        /// 
        /// In this example, assume that the category list of the Categories instance contains the default values:
        /// 
        /// <code>
        ///    Id         Type          Description
        ///    1          Event         School
        ///    2          Event         Personal
        ///    3          Event         VideoGames
        ///    4          Event         Medical
        ///    5          Event         Sleep
        ///    6          AllDayEvent   Vacation
        ///    7          AllDayEvent   Travel days
        ///    8          Holiday       Canadian Holidays
        ///    9          Holiday       US Holidays
        /// </code>
        /// 
        /// <b>Saving the categories in the instance to a file.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories();
        /// categories.SaveToFile("./category-file.cats");
        /// ]]>
        /// </code>
        /// 
        /// category-file.cats content:
        /// <code>
        /// <![CDATA[
        /// <Categories>
        /// <Category ID = "1" type="Event">School</Category>
        /// <Category ID = "2" type="Event">Personal</Category>
        /// <Category ID = "3" type="Event">VideoGames</Category>
        /// <Category ID = "4" type="Event">Medical</Category>
        /// <Category ID = "5" type="Event">Sleep</Category>
        /// <Category ID = "6" type="AllDayEvent">Vacation</Category>
        /// <Category ID = "7" type="AllDayEvent">Travel days</Category>
        /// <Category ID = "8" type= "Holiday" > Canadian Holidays</Category>
        /// <Category ID = "9" type= "Holiday" > US Holidays</Category>
        /// </Categories>
        /// ]]>
        /// </code>
        /// </example>
        public void SaveToFile(String? filepath = null)
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
        // set categories to default
        // ====================================================================
        /// <summary>
        /// Sets default categories to the category list of the current instance.
        /// </summary>
        /// <example>
        /// 
        /// For the example below, assume that the default categories are the following elements:
        /// 
        /// <code>
        ///    Id         Type          Description
        ///    1          Event         School
        ///    2          Event         Personal
        ///    3          Event         VideoGames
        ///    4          Event         Medical
        ///    5          Event         Sleep
        ///    6          AllDayEvent   Vacation
        ///    7          AllDayEvent   Travel days
        ///    8          Holiday       Canadian Holidays
        ///    9          Holiday       US Holidays
        /// </code>
        /// 
        /// <b>Setting the default values using the method.</b>
        /// 
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories();
        /// categories.SetCategoriesToDefaults();
        /// 
        /// Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", "Id", "Type", "Description"));
        /// foreach (Category c in categories.List())
        /// {
        ///     Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", c.Id, c.Type, c.Description));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        ///    Id         Type          Description
        ///    1          Event         School
        ///    2          Event         Personal
        ///    3          Event         VideoGames
        ///    4          Event         Medical
        ///    5          Event         Sleep
        ///    6          AllDayEvent   Vacation
        ///    7          AllDayEvent   Travel days
        ///    8          Holiday       Canadian Holidays
        ///    9          Holiday       US Holidays
        /// </code>
        /// </example>
        public void SetCategoriesToDefaults()
        {
            // ---------------------------------------------------------------
            // reset any current categories,
            // ---------------------------------------------------------------
            _Categories.Clear();

            // ---------------------------------------------------------------
            // Add Defaults
            // ---------------------------------------------------------------
            Add("School", Category.CategoryType.Event);
            Add("Personal", Category.CategoryType.Event);
            Add("VideoGames", Category.CategoryType.Event);
            Add("Medical", Category.CategoryType.Event);
            Add("Sleep", Category.CategoryType.Event);
            Add("Vacation", Category.CategoryType.AllDayEvent);
            Add("Travel days", Category.CategoryType.AllDayEvent);
            Add("Canadian Holidays", Category.CategoryType.Holiday);
            Add("US Holidays", Category.CategoryType.Holiday);
        }

        // ====================================================================
        // Add category
        // ====================================================================
        /// <summary>
        /// Adds a new category object with a specified description and type to the category database. 
        /// </summary>
        /// <param name="category">The category object which will be added to the database.</param>
        /// <example>
        /// 
        /// In this example, assume that the categories database contains the following elements:
        /// 
        /// <code>
        ///    Id    Type         Description
        ///    1     Event        School
        ///    2     Holiday      Canadian Holiday
        ///    3     Event        Vacation
        ///    4     Event        Wellness Day
        /// </code>
        /// 
        /// <b>Adding a category to the database.</b>
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories(conn,true);
        /// 
        /// Category category = new Category("Birthday", Category.CategoryType.Event);
        /// 
        /// categories.Add(category);
        /// 
        /// List<Category> categories = categories.List()
        /// 
        /// Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", "Id", "Type", "Description"));
        /// foreach (Category c in copy)
        /// {
        ///     Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", c.Id, c.Type, c.Description));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        ///    Id    Type         Description
        ///    1     Event        School
        ///    2     Holiday      Canadian Holiday
        ///    3     Event        Vacation
        ///    4     Event        Wellness Day
        ///    5     Event        Birthday
        /// </code>
        /// </example>
        private void Add(Category category)
        {
            SQLiteCommand cmd = new SQLiteCommand(_connection);
            cmd.CommandText = $"INSERT INTO categories(Description,TypeId) VALUES(${category.Description},${(int)category.Type})";
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds a new category with the specified description and type to the category database. 
        /// </summary>
        /// <param name="desc">The description of the category that will be added to the database.</param>
        /// <param name="type">The type of the category that will be added to the database.</param>
        /// <example>
        /// 
        /// In this example, assume that the categories database contains the following elements:
        /// 
        /// <code>
        ///    Id    Type         Description
        ///    1     Event        School
        ///    2     Holiday      Canadian Holiday
        ///    3     Event        Vacation
        ///    4     Event        Wellness Day
        /// </code>
        /// 
        /// <b>Adding a category to the database.</b>
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories(conn,true);
        /// 
        /// categories.Add("Birthday", Category.CategoryType.Event);
        /// 
        /// List<Category> categories = categories.List()
        /// 
        /// Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", "Id", "Type", "Description"));
        /// foreach (Category c in copy)
        /// {
        ///     Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", c.Id, c.Type, c.Description));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        ///    Id    Type         Description
        ///    1     Event        School
        ///    2     Holiday      Canadian Holiday
        ///    3     Event        Vacation
        ///    4     Event        Wellness Day
        ///    5     Event        Birthday
        /// </code>
        /// </example>
        public void Add(String desc, Category.CategoryType type)
        {
            SQLiteCommand cmd = new SQLiteCommand(_connection);
            cmd.CommandText = $"INSERT INTO categories(Description,TypeId) VALUES(@desc,@id)";
            cmd.Parameters.AddWithValue("@desc", desc);
            cmd.Parameters.AddWithValue("@id", (int)type);
            cmd.ExecuteNonQuery();

        }

        // ====================================================================
        // Delete category
        // ====================================================================
        /// <summary>
        /// Removes the category from the database of categories with the specified id. The passed id must be the id of a category in the database.
        /// </summary>
        /// <param name="Id">The id of the category that will be removed from the list.</param>
        /// <example>
        /// 
        /// In this example, assume that the categories database contains the following elements:
        /// 
        /// <code>
        ///    Id    Type         Description
        ///    1     Event        School
        ///    2     Holiday      Canadian Holiday
        ///    3     Event        Vacation
        ///    4     Event        Wellness Day
        /// </code>
        /// 
        /// <b>Deleting a category from the database.</b>
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories();
        /// 
        /// categories.Delete(3);
        /// 
        /// List<Category> categories = categories.List()
        /// 
        /// Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", "Id", "Type", "Description"));
        /// foreach (Category c in copy)
        /// {
        ///     Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", c.Id, c.Type, c.Description));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        ///    Id    Type         Description
        ///    1     Event        School
        ///    2     Holiday      Canadian Holiday
        ///    4     Event        Wellness Day
        /// </code>
        /// </example>
        public void Delete(int Id)
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "DELETE FROM categories WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)  
            { 
                Console.WriteLine($"Something went wrong:{ex.Message}");
            }
             

        }

        /// <summary>
        /// Updates the specified category in the database with the new passed updated values.
        /// </summary>
        /// <param name="Id">The Id of the category that will be updated.</param>
        /// <param name="newDescription">The new description that will replace the old one.</param>
        /// <param name="newTypeId">The new type Id that will replace the old one.</param>
        /// <example>
        /// 
        /// In this example, assume that the category table in the database contains the following elements:
        /// 
        /// <code>
        ///    Id    TypeId       Description
        ///    1     1            School
        ///    2     4            Canadian Holiday
        ///    3     1            Vacation
        ///    4     1            Wellness Day
        /// </code>
        /// 
        /// <b>Updating a row in the database.</b>
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories(Database.dbConnection, false);
        /// 
        /// categories.Update(1, "National Holiday", 4);
        /// 
        /// Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", "Id", "Type", "Description"));
        /// foreach (Category c in copy)
        /// {
        ///     Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", c.Id, c.Type, c.Description));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        ///    Id    Type         Description
        ///    1     Holiday      National Holiday
        ///    5     Event        Birthday
        ///    3     Event        Vacation
        ///    4     Event        Wellness Day
        /// </code>
        /// </example>
        public void UpdateProperties(int Id, string newDescription, Category.CategoryType newType)
        {
            SQLiteCommand cmd = new SQLiteCommand(_connection);
            cmd.CommandText = "UPDATE categories set Description = @description, TypeId = @typeId WHERE Id = @id;";
            cmd.Parameters.AddWithValue("description", newDescription);
            cmd.Parameters.AddWithValue("typeId", (int)newType);
            cmd.Parameters.AddWithValue("id", Id);
            cmd.ExecuteNonQuery();
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================

        /// <summary>
        /// Generates a list of category objects from the database and returns it.
        /// </summary>
        /// <returns>The category list.</returns>
        /// <example>
        /// 
        /// In this example, assume that the category table in the database contains the following elements:
        /// 
        /// <code>
        ///    Id    TypeId       Description
        ///    1     1            School
        ///    2     4            Canadian Holiday
        ///    3     1            Vacation
        ///    4     1            Wellness Day
        /// </code>
        /// 
        /// <b>Getting the list of items.</b>
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories(Database.dbConnection, false);
        /// 
        /// List<Category> copy = categories.List();
        /// 
        /// Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", "Id", "Type", "Description"));
        /// foreach (Category c in copy)
        /// {
        ///     Console.WriteLine(string.Format("{0, -10} {1,-10} {2,-10}", c.Id, c.Type, c.Description));
        /// }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        ///    Id    Type         Description
        ///    1     Event        School
        ///    5     Event        Birthday
        ///    3     Event        Vacation
        ///    4     Event        Wellness Day
        /// </code>
        /// </example>
        public List<Category> List()
        {
            List<Category> newList = new List<Category>();

            SQLiteCommand cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT Id, Description, TypeId FROM categories ORDER BY Id;";
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                newList.Add(new Category(reader.GetInt32(0), reader.GetString(1), (Category.CategoryType)reader.GetInt32(2)));
            }

            cmd.Dispose();
            return newList;
        }

        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {

            // ---------------------------------------------------------------
            // read the categories from the xml file, and add to this instance
            // ---------------------------------------------------------------
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                foreach (XmlNode category in doc.DocumentElement.ChildNodes)
                {
                    String id = (((XmlElement)category).GetAttributeNode("ID")).InnerText;
                    String typestring = (((XmlElement)category).GetAttributeNode("type")).InnerText;
                    String desc = ((XmlElement)category).InnerText;

                    Category.CategoryType type;
                    switch (typestring.ToLower())
                    {
                        case "event":
                            type = Category.CategoryType.Event;
                            break;
                        case "alldayevent":
                            type = Category.CategoryType.AllDayEvent;
                            break;
                        case "holiday":
                            type = Category.CategoryType.Holiday;
                            break;
                        case "availability":
                            type = Category.CategoryType.Availability;
                            break;
                        default:
                            type = Category.CategoryType.Event;
                            break;
                    }
                    this.Add(new Category(int.Parse(id), desc, type));
                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadXMLFile: Reading XML " + e.Message);
            }

        }


        // ====================================================================
        // write all categories in our list to XML file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            try
            {
                // create top level element of categories
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Categories></Categories>");

                // foreach Category, create an new xml element
                foreach (Category cat in _Categories)
                {
                    XmlElement ele = doc.CreateElement("Category");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = cat.Id.ToString();
                    ele.SetAttributeNode(attr);
                    XmlAttribute type = doc.CreateAttribute("type");
                    type.Value = cat.Type.ToString();
                    ele.SetAttributeNode(type);

                    XmlText text = doc.CreateTextNode(cat.Description);
                    doc.DocumentElement.AppendChild(ele);
                    doc.DocumentElement.LastChild.AppendChild(text);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("_WriteXMLFile: Reading XML " + e.Message);
            }

        }

    }
}

