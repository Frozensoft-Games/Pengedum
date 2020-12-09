using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Extensions
{
    public class FileManagerExtension
    {
        // Helper method to delete a directory
        public static void DeleteDirectory(string directoryPath)
        {
            Directory.Delete(directoryPath, true);
        }

        // Helper method to create a file
        public static void CreateFile(string filePath)
        {
            File.Create(filePath);
        }

        // Helper method to create a directory
        public static void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Helper method to move a directory and it's content
        public static void MoveDirectory(string oldDirectoryPath, string newDirectoryPath)
        {
            Directory.Move(oldDirectoryPath, newDirectoryPath);
        }

        // Helper method to get directories in a directory
        public static string[] GetDirectories(string directoryPath)
        {
            return Directory.GetDirectories(directoryPath);
        }

        // Helper method to get files in a directory
        public static string[] GetFiles(string directoryPath)
        {
            return Directory.GetFiles(directoryPath);
        }
    }
}
