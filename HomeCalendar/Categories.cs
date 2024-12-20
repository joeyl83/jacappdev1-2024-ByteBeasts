﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Data.SQLite;
using System.Data.Common;
using System.Reflection.PortableExecutable;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{
    // ====================================================================
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to database
    //        - etc
    // ====================================================================

    /// <summary>
    /// Manages a collection of category items. Reads and writes to the database storing the categories. Initialized with the connection and whether it is a new or old database in the constructor.
    /// </summary>
    public class Categories
    {
        private SQLiteConnection _connection;

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

            if (newDB)
            {
                SetCategoriesToDefaults();
            }

        }

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================

        /// <summary>
        /// Gets the category with the specified ID from the database. Used to find the category object when you only have the ID. 
        /// If there is no category with the Id, an exception is thrown.
        /// </summary>
        /// <param name="i">The ID of the searched category.</param>
        /// <returns>The category object with the specified ID.</returns>
        /// <exception cref="Exception">Throws when there is no category with the specified ID in the database.</exception>
        /// <example>
        /// 
        /// In this example, assume that the category table in the database contains the following elements:
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
        /// Categories categories = new Categories(Database.dbConnection);
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
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "SELECT Id, Description, TypeId FROM categories WHERE Id = @id;";
                cmd.Parameters.AddWithValue("id", i);
                SQLiteDataReader reader = cmd.ExecuteReader();

                reader.Read();
                Category c = new Category(reader.GetInt32(0), reader.GetString(1), (Category.CategoryType)reader.GetInt32(2));
                return c;
            }
            catch (Exception ex)
            {
                throw new Exception("Error. Invalid Id: " + ex.Message);
            }

        }

        // ====================================================================
        // set categories to default
        // ====================================================================
        /// <summary>
        /// Sets default categories in the category database as well as filling the CategoryTypes table to default.
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
            // Add Defaults
            // ---------------------------------------------------------------
            try
            {


                SQLiteCommand cmd = new SQLiteCommand(_connection);
                AddCategoryTypes(cmd);
                cmd.CommandText = "DELETE FROM categories;";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

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
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: " + ex.Message);
            }
        }
        /// <summary>
        /// Adds all category types into the database.
        /// </summary>
        /// <param name="cmd">The command needed to execute the database queries.</param>
        public static void AddCategoryTypes(SQLiteCommand cmd)
        {
            foreach (string category in Enum.GetNames(typeof(Category.CategoryType)))
            {
                cmd.CommandText = "INSERT INTO categoryTypes(Description) VALUES('@categoryName')";
                cmd.Parameters.AddWithValue("categoryName", category);
                cmd.ExecuteNonQuery();
            }

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
            cmd.Dispose();
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
            try
            {


                SQLiteCommand cmd = new SQLiteCommand(_connection);
                cmd.CommandText = $"INSERT INTO categories(Description,TypeId) VALUES(@desc,@id)";
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@id", (int)type);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: " + ex.Message);
            }
        }

        // ====================================================================
        // Delete category
        // ====================================================================
        /// <summary>
        /// Removes the category from the database with the specified id, all events assocaciated with the category will also be deleted.
        /// The passed id must be the id of a category in the database.
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
                cmd.CommandText = "DELETE FROM events WHERE CategoryId=@id";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM categories WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong:{ex.Message}");
            }


        }

        /// <summary>
        /// Updates the specified category in the database with the new passed updated values.
        /// </summary>
        /// <param name="Id">The Id of the category that will be updated.</param>
        /// <param name="newDescription">The new description that will replace the old one.</param>
        /// <param name="newTypeId">The new type Id that will replace the old one.</param>
        /// <exception cref="Exception">Throws when the passed Id does not exist in the database.</exception>
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
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "UPDATE categories set Description = @description, TypeId = @typeId WHERE Id = @id;";
                cmd.Parameters.AddWithValue("description", newDescription);
                cmd.Parameters.AddWithValue("typeId", (int)newType);
                cmd.Parameters.AddWithValue("id", Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error. Invalid Id: " + ex.Message);
            }

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
            try
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
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: " + ex.Message);
            }
        }
    }
}

