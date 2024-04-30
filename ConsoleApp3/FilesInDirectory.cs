using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class ScanFilesInDirectory
    {
        string directoryPath;
        public ScanFilesInDirectory(string directoryPath ) {
            this.directoryPath = directoryPath; 
            ReadFilesInDirectory(directoryPath);
        }
        static void ReadFilesInDirectory(string directoryPath)
        {
            try
            {
                string[] files = Directory.GetFiles(directoryPath, "*.cs");
                Console.WriteLine(directoryPath);

                foreach (string file in files)
                {
                    SCode obj = new SCode(file);
                }
                // create a recursive function to call all subpaths 
                string[] subDirectories = Directory.GetDirectories(directoryPath);
                foreach (string subDirectory in subDirectories)
                {
                    ReadFilesInDirectory(subDirectory);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }
    }
}
