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
    // CLASS: Category
    //        - An individual category for Calendar program
    //        - Valid category types: Event,AllDayEvent,Holiday
    // ====================================================================
    /// <summary>
    /// Contains data of an individual category for the calendar program. Initialized with specified data or from a copy of another category instance.
    /// </summary>
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================

        /// <summary>
        /// Gets and sets the Id.
        /// </summary>
        /// <value>Represents the unique integer id of the category.</value>
        public int Id { get; }

        /// <summary>
        /// Gets and sets the Description.
        /// </summary>
        /// <value>Represents the description of the category as a string.</value>
        public String Description { get; }

        /// <summary>
        /// Gets and sets the Type.
        /// </summary>
        /// <value>Represents the type of category as an enum.</value>
        public CategoryType Type { get; }

        /// <summary>
        /// Represents a set of possible category types.
        /// </summary>
        public enum CategoryType
        {
            /// <summary>
            /// Represents a category that is some type of event.
            /// </summary>
            Event = 1,

            /// <summary>
            /// Represents a category that is an all day event.
            /// </summary>
            AllDayEvent,

            Availability,

            /// <summary>
            /// Represents a cateogry that is some type of holiday.
            /// </summary>
            Holiday,
        };

        // ====================================================================
        // Constructor
        // ====================================================================

        /// <summary>
        /// Initializes a category instance with the passed data values. The id and description are required and the default for the type is 'Event'.
        /// </summary>
        /// <param name="id">The id of the category.</param>
        /// <param name="description">The description of the category.</param>
        /// <param name="type">The category type.</param>
        /// <example>
        /// 
        /// Assume we want to create a category with an id of 1, a description "Exam" and a type Event:
        /// 
        /// <code>
        /// <![CDATA[
        /// Category category = new Category(1, "Exam", CategoryType.Event);
        /// 
        /// Console.WriteLine(category.Description);
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Exam
        /// </code>
        /// </example>

        public Category(int id, String description, CategoryType type = CategoryType.Event)
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================

        /// <summary>
        /// Initializes a new category instance with the exact same values as the passed category instance. Makes a new independant copy.
        /// </summary>
        /// <param name="category">The instance that will have its values copied to the initialized instance.</param>
        /// <example>
        /// 
        /// For this example, we will create a copy of the firstCategory as secondCategory:
        /// 
        /// <code>
        /// <![CDATA[
        /// Category firstCategory = new Category(1, "Exam", CategoryType.Event);
        /// Category secondCategory = new Category(firstCategory);
        /// 
        /// Console.WriteLine(secondCategory.Description);
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Exam
        /// </code>
        /// </example>
        public Category(Category category)
        {
            this.Id = category.Id;;
            this.Description = category.Description;
            this.Type = category.Type;
        }
        // ====================================================================
        // String version of object
        // ====================================================================

        /// <summary>
        /// Generates a string representation of the current category instance.
        /// </summary>
        /// <returns>The description of the current category instance.</returns>
        /// <example>
        /// 
        /// In this example, we will print the ToString() output of a category instance:
        /// 
        /// <code>
        /// <![CDATA[
        /// Category category = new Category(1, "Exam", CategoryType.Event);
        /// 
        /// Console.WriteLine(category.ToString());
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Exam
        /// </code>
        /// </example>
        public override string ToString()
        {
            return Description;
        }

    }
}

