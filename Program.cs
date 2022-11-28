using System;
using System.IO;
using System.Threading;

namespace LuceneProject
{
    class Program
    {
        //Dataset path
        static readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Dataset\2022-sp.csv");
        static readonly string datasetFile = Path.GetFullPath(path);
        static string indexPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Index"));
        static string indexPathFile = Path.GetFullPath(Path.Combine(indexPath, @"_0.cfs"));

        private void Run()
        {
            fileCheck(datasetFile);

            var index = new Indexer(datasetFile, indexPath);

            if (!File.Exists(indexPathFile)) {
                index.CreateIndex();
            }

            //index.SearchIndexMultiQuery(10,"Interdisciplinary introduction to the basic concepts and approaches");
            while (true)
            {
                Console.Write("Choose betweeen Advanced or General\nSearch Type: ");
                var searchType = Console.ReadLine().Trim().ToLower();

                if (searchType.Equals("advanced")) {
                    Console.Write("Header: (Choose between: Subject, Number, Name, Description" +
                    ", CRN, or Enrollment Status): ");
                    var header = Console.ReadLine();

                    Console.Write("\nQuery: ");
                    var query = Console.ReadLine();

                    Console.Write("Hits: ");
                    var TopResults = Console.ReadLine();

                    //search x top documents
                    index.SearchIndexSpecific(int.Parse(TopResults), query, header);

                    Console.Write("\nContinue searching? (y/n): ");
                    var continueSearch = Console.ReadKey().Key.ToString();
                    Console.WriteLine("\n");

                    if (continueSearch.Equals("N"))
                    {
                        Console.WriteLine("\n\n---------------------------------------------------------------------------------------------------------------\n\nExiting program...");
                        Thread.Sleep(2300);
                        Environment.Exit(1);
                    }
                    else
                        continue;
                }
                else if (searchType.Equals("general")) {
                    Console.Write("\nQuery: ");
                    var query = Console.ReadLine();

                    Console.Write("Hits: ");
                    var resultNum = Console.ReadLine();

                    index.SearchIndexMultiQuery(int.Parse(resultNum), query);

                    Console.Write("\nContinue searching? (y/n): ");
                    var continueSearch = Console.ReadKey().Key.ToString();
                    Console.WriteLine("\n");

                    if (continueSearch.Equals("N"))
                    {
                        Console.WriteLine("\n\n---------------------------------------------------------------------------------------------------------------\n\nExiting program...");
                        Thread.Sleep(2000);
                        Environment.Exit(1);
                    }
                }
            }

            //example call
            //index.SearchIndexExample();
        }

        /// <summary>
        /// Checks if file exists
        /// </summary>
        /// <param name="dtfile">The data file</param>
        private void fileCheck(string dtfile)
        {
            if (File.Exists(dtfile))
            {
                Console.WriteLine("Dataset file exists\n");
            }
            else
            {
                Console.WriteLine("Dataset file not found\n");
                Console.WriteLine("Exiting Program...");
                Thread.Sleep(1000);
                Environment.Exit(1);
            }
        }


        /// <summary>
        /// Entry point
        /// Initiates the app
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var self = new Program();
            self.Run();
        }
    }
}