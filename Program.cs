using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.IO;

namespace LuceneProject
{
    class Program
    {
        const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
        
        private void Run()
        {
            //Path
            string datasetFilePath = Path.GetFullPath(@"C:\Users\Nick\Source\Repos\Nick-v1\LuceneProject\Dataset\2022-sp.csv");
            //All text from dataset
            string text = File.ReadAllText(datasetFilePath);

            
        }












        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var self = new Program();
            self.Run();
        }
    }
}
