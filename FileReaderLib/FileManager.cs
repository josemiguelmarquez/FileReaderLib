using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public static void ReadFile(string filePath, int? encrptKey)
        {
            if (File.Exists(filePath) && (filePath.EndsWith(".xml") || filePath.EndsWith(".txt")))
            {
                // Not encrypted
                if (encrptKey == null)
                {
                    string contents = File.ReadAllText(filePath);
                    //Console.Write("File Content: " + contents + "\n");
                }
                else
                {
                    // Decrypt data and read
                    string decFilePath = DecryptTextFile(filePath, (int) encrptKey);

                    string contents = File.ReadAllText(decFilePath);
                    //Console.Write("File Content: " + contents + "\n");
                }


            }
        }

        /// <summary>
        /// Method to create an encrypted file from the one located in filepath
        /// </summary>
        /// <param name="filePath">file path of the text file (.txt)</param>
        /// <param name="key">factor that changes the encryption</param>
        public static void EncryptTextFile(string filePath, int key)
        {
            if (File.Exists(filePath) && filePath.EndsWith(".txt"))
            {
                // Get the file name
                string fileName = Path.GetFileName(filePath);

                // Create the result File
                var resultFile = new FileStream(AppContext.BaseDirectory + "(E)" + fileName, FileMode.OpenOrCreate, FileAccess.Write);
                using (StreamWriter streamWriter = new StreamWriter(resultFile))
                {

                    // Read the file and encrypt it -- according to the factor
                    var stream = File.OpenRead(filePath);
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!sr.EndOfStream)
                        {
                            // Write the crypted data to the new stream
                            streamWriter.WriteLine(Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(sr.ReadLine()), new byte[key], DataProtectionScope.LocalMachine)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to create a decrypted file from the one located in filePath
        /// </summary>
        /// <param name="filePath">file path of the encrypted text data</param>
        /// <param name="key">factor used for the encryptation</param>
        /// <returns>the path of the new decrypted data</returns>
        private static string DecryptTextFile(string filePath, int key)
        {
            if (File.Exists(filePath) && filePath.EndsWith(".txt"))
            {
                // Get the file name
                string fileName = Path.GetFileName(filePath);
                string newFileName = AppContext.BaseDirectory + fileName.Substring(3);

                // Create the result File
                var resultFile = new FileStream(newFileName, FileMode.OpenOrCreate, FileAccess.Write);
                using (StreamWriter streamWriter = new StreamWriter(resultFile))
                {

                    // Read the file and dencrypt it -- according to the factor
                    var stream = File.OpenRead(filePath);
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!sr.EndOfStream)
                        {
                            // Write the decrypted data to the new stream
                            streamWriter.WriteLine(Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(sr.ReadLine()), new byte[key], DataProtectionScope.LocalMachine)));
                        }
                    }
                }

                return newFileName;
            }

            return string.Empty;
        
        }
    }
}
