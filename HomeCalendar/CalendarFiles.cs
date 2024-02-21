using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Calendar
{

    /// <summary>
    /// Manages calendar files to see if they can be accessed or written to. Contains default paths that are used if needed.
    /// The files contain data that is used to create the calendar.
    /// </summary>
    public class CalendarFiles 
    {
        private static String DefaultSavePath = @"Calendar\";
        private static String DefaultAppData = @"%USERPROFILE%\AppData\Local\";

        // ====================================================================
        // verify that the name of the file, or set the default file, and 
        // is it readable?
        // throws System.IO.FileNotFoundException if file does not exist
        // ====================================================================

        /// <summary>
        /// Verifies the name of the file or sets the default file and checks if the file is readable. The method is used when generating the lists of calendar items
        /// from a file, verifying if the file is readable and setting a default file path if the passed file path is null. If the file doesn't exist an exception is thrown.
        /// </summary>
        /// <param name="FilePath">The path of the file that will be verified.</param>
        /// <param name="DefaultFileName">The default file name that is set if the specified file path isn't specified.</param>
        /// <returns>The specified filepath if it is specified, otherwise the created default file path.</returns>
        /// <exception cref="FileNotFoundException">Throws when the specified file does not exist.</exception>
        /// <example>
        /// 
        /// For the example below, assume that the passed file path is valid:
        /// 
        /// <code>
        /// <![CDATA[
        /// string filePath = "~\\Documents\\file.txt";
        /// string verifiedPath = CalendarFiles.VerifyReadFromFileName(filePath, "");
        /// Console.WriteLine(verifiedPath);
        /// ]]>
        /// </code>
        /// 
        /// Output:
        /// <code>
        /// ~\Documents\file.txt
        /// </code>
        /// 
        /// For this example, assume that we want to use the default file path:
        /// 
        /// <code>
        /// <![CDATA[
        /// string verifiedPath = CalendarFiles.VerifyReadFromFileName(null, "default-name.txt");
        /// Console.WriteLine(verifiedPath);
        /// ]]>
        /// </code>
        /// 
        /// Output:
        /// <code>
        /// C:\Users\user\AppData\Local\Calendar\default-name.txt
        /// </code>
        /// </example>
        public static String VerifyReadFromFileName(String? FilePath, String DefaultFileName)
        {

            // ---------------------------------------------------------------
            // if file path is not defined, use the default one in AppData
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does FilePath exist?
            // ---------------------------------------------------------------
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("ReadFromFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ----------------------------------------------------------------
            // valid path
            // ----------------------------------------------------------------
            return FilePath;

        }

        // ====================================================================
        // verify that the name of the file, or set the default file, and 
        // is it writable
        // ====================================================================

        /// <summary>
        /// Verifies the path of the file or sets the default path and checks if the file is writable with a valid directory path. The method is used when 
        /// generating the lists of calendar items as data to save in a file. If the directory doesn't exist or the file can't be written to an exception is thrown.
        /// </summary>
        /// <param name="FilePath">The path of the file that will be verified.</param>
        /// <param name="DefaultFileName">The default file name that is set if the specified file path isn't specified.</param>
        /// <returns>The specified filepath if it is valid and is writable, otherwise the created default file path.</returns>
        /// <exception cref="Exception">Throws when the specified file path is invalid.</exception>
        /// <example>
        /// 
        /// For the example below, assume that the passed file path is valid:
        /// 
        /// <code>
        /// <![CDATA[
        /// string filePath = "~\\Documents\\file.txt";
        /// string verifiedPath = CalendarFiles.VerifyWriteToFileName(filePath, "");
        /// Console.WriteLine(verifiedFilePath);
        /// ]]>
        /// </code>
        /// 
        /// Output:
        /// <code>
        /// ~\\Documents\\file.txt
        /// </code>
        /// 
        /// For this example, assume that we want to use the default file path:
        /// 
        /// <code>
        /// <![CDATA[
        /// string verifiedPath = CalendarFiles.VerifyWriteToFileName(null, "default-name.txt");
        /// Console.WriteLine(verifiedPath);
        /// ]]>
        /// </code>
        /// 
        /// Output:
        /// <code>
        /// C:\Users\user\AppData\Local\Calendar\default-name.txt
        /// </code>
        /// </example>
        public static String VerifyWriteToFileName(String? FilePath, String DefaultFileName)
        {
            // ---------------------------------------------------------------
            // if the directory for the path was not specified, then use standard application data
            // directory
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                // create the default appdata directory if it does not already exist
                String tmp = Environment.ExpandEnvironmentVariables(DefaultAppData);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                // create the default Calendar directory in the appdirectory if it does not already exist
                tmp = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does directory where you want to save the file exist?
            // ... this is possible if the user is specifying the file path
            // ---------------------------------------------------------------
            String? folder = Path.GetDirectoryName(FilePath);
            String delme = Path.GetFullPath(FilePath);
            if (!Directory.Exists(folder))
            {
                throw new Exception("SaveToFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ---------------------------------------------------------------
            // can we write to it?
            // ---------------------------------------------------------------
            if (File.Exists(FilePath))
            {
                FileAttributes fileAttr = File.GetAttributes(FilePath);
                if ((fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new Exception("SaveToFileException:  FilePath(" + FilePath + ") is read only");
                }
            }

            // ---------------------------------------------------------------
            // valid file path
            // ---------------------------------------------------------------
            return FilePath;

        }



    }
}
