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
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var indexPath = Path.Combine(basePath, "dataset");

            using var dir = FSDirectory.Open(indexPath);

            Console.WriteLine(dir);
        }













        static void Main(string[] args)
        {
            var self = new Program();
            self.Run();
        }
    }
}
