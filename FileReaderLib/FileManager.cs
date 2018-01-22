using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReaderLib
{
    /// <summary>
    /// File manager Object that holds the logic to Read the files
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Reads all text files (.txt) that are in the same directory as the library
        /// </summary>
        public static void ReadFiles()
        {
            foreach (string file in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".txt") || s.EndsWith(".xml")))
            {
                string contents = File.ReadAllText(file);
                //Console.Write("File Content: " + contents + "\n");
            }
        }

        /// <summary>
        /// Reads text file (.txt) pointed in the the filePath parameter
        /// </summary>
        public static void ReadFiles(string filePath)
        {
            if (filePath.EndsWith(".xml") || filePath.EndsWith(".txt"))
            {
                string contents = File.ReadAllText(filePath);
                //Console.Write("File Content: " + contents + "\n");
            }
        }
    }
}
