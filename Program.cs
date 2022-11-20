using System;
using System.IO;

namespace LuceneProject
{
    class Program
    {
        //Dataset path
        static readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Dataset\2022-sp.csv");
        static readonly string datasetFile = Path.GetFullPath(path);
        static string indexPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Index"));


        private void Run()
        {
            fileCheck(datasetFile);
            

            var index = new Indexer(datasetFile, indexPath);
            index.myTokenizer();
            index.CreateIndex();
            //index.CreateIndex();

            Console.WriteLine();

        }
        


        /// <summary>
        /// Checks if file exists
        /// </summary>
        /// <param name="dtfile">The data file</param>
        private void fileCheck(string dtfile)
        {
            if (File.Exists(dtfile))
                Console.WriteLine("Dataset file exists");
            else
                Console.WriteLine("Dataset file not found");
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