using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Threading;

// ===================================================================
// Very important notes:
// ... To keep everything working smoothly, you should always
//     dispose of EVERY SQLiteCommand even if you recycle a 
//     SQLiteCommand variable later on.
//     EXAMPLE:
//            Database.newDatabase(GetSolutionDir() + "\\" + filename);
//            var cmd = new SQLiteCommand(Database.dbConnection);
//            cmd.CommandText = "INSERT INTO categoryTypes(Description) VALUES('Whatever')";
//            cmd.ExecuteNonQuery();
//            cmd.Dispose();
//
// ... also dispose of reader objects
//
// ... by default, SQLite does not impose Foreign Key Restraints
//     so to add these constraints, connect to SQLite something like this:
//            string cs = $"Data Source=abc.sqlite; Foreign Keys=1";
//            var con = new SQLiteConnection(cs);
//
// ===================================================================


namespace Calendar
{
    public class Database
    {

        public static SQLiteConnection dbConnection { get { return _connection; } }
        private static SQLiteConnection _connection;

        // ===================================================================
        // create and open a new database
        // ===================================================================
        /// <summary>
        /// Creates tables necessary for the home calendar API to function within a new database, i.e events,categories,categoryTypes.
        /// </summary>
        /// <param name="filename">The filename of the new database.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// public IsNewDatabase(string filename, bool newDB = true)
        ///     {
        ///        if(newDB)
        ///       {
        ///           Database.newDatabase(filename)
        ///       }
        ///     }
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public static void newDatabase(string filename)
        {

            // If there was a database open before, close it and release the lock
            CloseDatabaseAndReleaseFile();

            // your code
            string cs = @"data source=" + filename + @";Foreign Keys=1;";
            _connection = new SQLiteConnection(cs);
            _connection.Open();

            var cmd = new SQLiteCommand(Database._connection);

            cmd.CommandText = "DROP TABLE IF EXISTS events;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DROP TABLE IF EXISTS categories;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DROP TABLE IF EXISTS categoryTypes";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE categoryTypes(Id INTEGER PRIMARY KEY,
                                    Description TEXT);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE events(Id INTEGER PRIMARY KEY, 
                                    StartDateTime TEXT, Details TEXT, DurationInMinutes DOUBLE, CategoryId INTEGER, 
                                    FOREIGN KEY (CategoryId) REFERENCES categories(Id));";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE categories(Id INTEGER PRIMARY KEY, Description TEXT, TypeId INTEGER,
                                    FOREIGN KEY (TypeId) REFERENCES categoryTypes(Id));";
            cmd.ExecuteNonQuery();

            cmd.Dispose();
        }

        // ===================================================================
        // open an existing database
        // ===================================================================
        /// <summary>
        /// Establishes a connection and opens the existing database for use.
        /// </summary>
        /// <param name="filename">The filename of the existing database.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///     Database.existingDatabase(messyDB);
        ///     SQLiteConnection conn = Database.dbConnection;
        ///     Events events = new Events(conn, false);
        ///     events.Add(DateTime.Today,1,1,"Event details");
        /// 
        /// ]]>
        /// </code>
        /// </example>
        public static void existingDatabase(string filename)
        {

            CloseDatabaseAndReleaseFile();

            // your code
            string cs = @"data source=" + filename + @";Foreign Keys=1;";
            _connection = new SQLiteConnection(cs);
            _connection.Open();
        }

        // ===================================================================
        // close existing database, wait for garbage collector to
        // release the lock before continuing
        // ===================================================================
        /// <summary>
        /// Closes the existing database and waits for the garbage collector to release from the database file.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///     Database.existingDatabase(messyDB);
        ///     SQLiteConnection conn = Database.dbConnection;
        ///     Events events = new Events(conn, false);
        ///     events.Add(DateTime.Today,1,1,"Event details");
        ///     Database.CloseDatabaseAndReleaseFile()
        /// ]]>
        /// </code>
        /// </example>
        static public void CloseDatabaseAndReleaseFile()
        {
            if (Database.dbConnection != null)
            {
                // close the database connection
                Database.dbConnection.Close();


                // wait for the garbage collector to remove the
                // lock from the database file
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }

}
