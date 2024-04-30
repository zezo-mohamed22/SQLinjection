using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class SCode
    {
        public List<int> linesStartedsqlInjection = new List<int> { };
        public List<int> injectedFunctions = new List<int> { };
        public List<int> Functions = new List<int> { };
        string FilePath; 
       public SCode(string FilePath)
        {
            this.FilePath = FilePath;   
            scanFiles();    
            countFunctions();
        }
        public void scanFiles()
        {
            try
            {
                string[] lines = File.ReadAllLines(FilePath);

                for (var i = 0; i < lines.Length; i++)
                {
                    if (ContainsSQLQuery(lines[i]))
                    {
                        Console.WriteLine(i);
                        linesStartedsqlInjection.Add(i + 1);
                        getfirstlineOffunction(lines);
                    }
                }
          //      getfirstlineOffunction(linesStartedsqlInjection);
                printFunction(lines);
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred while reading the file:");
                Console.WriteLine(e.Message);
            }
        }
        public bool ContainsSQLQuery(string program)
        {
            string pattern = @"(\bSELECT\b[\s\S]+?\bFROM\b[\s\S]+?\bWHERE\b[\s\S]+?=\s*['""](?:[^'""]+|''|['""]{2,})*['""])|(\bINSERT\b[\s\S]+?\bINTO\b[\s\S]+?\([\s\S]+?\)[\s\S]+?\bVALUES\b[\s\S]+?\([\s\S]*?['""](?:[^'""]+|''|['""]{2,})*['""])|(\bUPDATE\b[\s\S]+?\bSET\b[\s\S]+?=\s*['""](?:[^'""]+|''|['""]{2,})*['""])|(\bDELETE\b[\s\S]+?\bFROM\b[\s\S]+?\bWHERE\b[\s\S]+?=\s*['""](?:[^'""]+|''|['""]{2,})*['""])|(\bEXEC\b[\s\S]+?['""](?:[^'""]+|''|['""]{2,})*['""])|(\bsp_executesql\b[\s\S]+?['""](?:[^'""]+|''|['""]{2,})*['""])";
            return Regex.IsMatch(program, pattern, RegexOptions.IgnoreCase);
        }

        public int getlines()
        {
            int lines = linesStartedsqlInjection.Count;
            return lines;
        }

        void getfirstlineOffunction(string[] lines)
        {
            for (var i = 0; i < linesStartedsqlInjection.Count; i++)
            {
                for (int j = linesStartedsqlInjection[i] - 1; j >= 0; j--)
                {
                    if (isFunction(lines[j]))
                    {
                        Console.WriteLine(j);
                        injectedFunctions.Add(j);
                        break;
                    }
                }
            } 
        }
        public void print(string[] lines) {
            printFunction(lines);
        }

       public void printFunction(string[] lines)
        {
            int cnt = 1;
            char open = '{';
            char close = '}';

            string pattern = Regex.Escape(open.ToString());
            string patternclosed = Regex.Escape(close.ToString());

            for (int i = 0;i < injectedFunctions.Count; i++)
            {
                Console.WriteLine(lines[injectedFunctions[i]]);
                for (var j = injectedFunctions[i]+1; j < lines.Length; j++)
                {

                    int count = Regex.Matches(lines[j], pattern).Count;
                    int countclose = Regex.Matches(lines[j], patternclosed).Count;
                    cnt += count;
                    Console.WriteLine(cnt);

                    cnt -= countclose;
                    Console.WriteLine(lines[j]);
                    if(cnt == 0)
                    {
                        break;
                    }
                }
            }
        }
        public bool isFunction(string program)
        {
            string staticFunction = @"static.*?\(.*?\)";
            string publicFunction = @"public.*?\(.*?\)";
            string proctectedFunction = @"protected.*?\(.*?\)";
            string privateFunction = @"private.*?\(.*?\)";
            bool foundedPattern= Regex.IsMatch(program, staticFunction, RegexOptions.IgnoreCase);
            foundedPattern|= Regex.IsMatch(program, publicFunction, RegexOptions.IgnoreCase);
            foundedPattern|= Regex.IsMatch(program, proctectedFunction, RegexOptions.IgnoreCase);
            foundedPattern|= Regex.IsMatch(program, privateFunction, RegexOptions.IgnoreCase);
            return foundedPattern; 
        }
        void countFunctions()
        {
            string[] lines = File.ReadAllLines(FilePath);

            for (var i = 0; i < lines.Length; i++)
            {
                if (isFunction(lines[i]))
                {
                    Functions.Add(i + 1);
               //     getfirstlineOffunction(lines);
                }
            }
        }
        public int injectionsFunctions()
        {
            return injectedFunctions.Count;
        }
        public int numberFunctions()
        {
            return Functions.Count();
        }

    }
}