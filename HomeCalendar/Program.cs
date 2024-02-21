using static Calendar.Category;

namespace Calendar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string CALENDAR_FILE_PATH = "./test.calendar";
            HomeCalendar homeCalendar = new HomeCalendar();
            homeCalendar.ReadFromFile(CALENDAR_FILE_PATH);

            //GetCalendarItems:

            //Prints all calendar items
            List<CalendarItem> calendarItemList = homeCalendar.GetCalendarItems(new DateTime(2019, 01, 01), new DateTime(2021, 01, 01), false, 8);
            Console.WriteLine("GetCalendarItems method: ");
            PrintCalendarItemsListTable(calendarItemList);

            //GetCalendarItemsByMonth:

            //Gets all calendar items, groups them by month and prints them all
            List<CalendarItemsByMonth> calendarItemListByMonth = homeCalendar.GetCalendarItemsByMonth(null, null, false, 1);
            Console.WriteLine("\nGetCalendarItemsByMonth method: ");
            PrintMonthlyCalendarItems(calendarItemListByMonth);

            //GetCalendarItemsByCategory:

            //Gets all calendar items, groups them by category and prints them all
            List<CalendarItemsByCategory> calendaryItemListByCategory = homeCalendar.GetCalendarItemsByCategory(null, null, false, 1);
            Console.WriteLine("\nGetCalendarItemsByCategory method: ");
            PrintCalendarItemsByCategory(calendaryItemListByCategory);

            //GetCalendarDictionaryByCategoryAndMonth:

            //Get all vacations in January 2020 and prints them
            List<Dictionary<string, object>> calendarDictionary = homeCalendar.GetCalendarDictionaryByCategoryAndMonth(null, null, false, 1);
            List<CalendarItem> birthdayListJan2020 = (List<CalendarItem>)calendarDictionary[1]["items:Vacation"];
            Console.WriteLine("\nGetCalendarDictionaryByCategoryAndMonth method: ");
            Console.WriteLine("Gets all items with the vacation category in the 01/2020 month");
            PrintCalendarItemsListTable(birthdayListJan2020);
            double totalBusyTime = (double)calendarDictionary[1]["Vacation"];
            Console.WriteLine($"Total duration in minutes for all items of the list: {totalBusyTime}");
        }

        static void PrintCalendarItemsListTable(List<CalendarItem> calendarItemList)
        {
            //Strings for the table
            string divider = "+--------------------+--------------------+--------------------+--------------------+---------------+---------------+";
            const string FORMAT = "|{0,-20}|{1,-20}|{2,-20}|{3,-20}|{4,-15}|{5,-15}|";

            //Header
            Console.WriteLine(divider);
            Console.WriteLine(FORMAT, "Short Description", "Category", "Duration In Minutes", "Busy Time", "Start Date", "Start Time");
            Console.WriteLine(divider);

            //Values
            foreach (CalendarItem calendarItem in calendarItemList)
            {
                Console.WriteLine(FORMAT, calendarItem.ShortDescription, calendarItem.Category, calendarItem.DurationInMinutes, calendarItem.BusyTime, calendarItem.StartDateTime.ToString("d"), calendarItem.StartDateTime.ToString("t"));
                Console.WriteLine(divider);
            }
            Console.WriteLine();
        }

        static void PrintMonthlyCalendarItems(List<CalendarItemsByMonth> calendarItemsByMonth)
        {
            //Prints a table for every month
            foreach (CalendarItemsByMonth calendarItemList in calendarItemsByMonth)
            {
                Console.WriteLine(calendarItemList.Month + ":");
                PrintCalendarItemsListTable(calendarItemList.Items);
            }
        }

        static void PrintCalendarItemsByCategory(List<CalendarItemsByCategory> calendarItemsByCategories)
        {
            //Prints a table for every category
            foreach (CalendarItemsByCategory calendarItemList in calendarItemsByCategories)
            {
                Console.WriteLine(calendarItemList.Category + ":");
                PrintCalendarItemsListTable(calendarItemList.Items);
            }
        }
    }

}