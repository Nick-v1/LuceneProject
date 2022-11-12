using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Core;

namespace LuceneProject
{
    class Program
    {
        const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
        //Dataset path
        static readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Dataset\2022-sp.csv");
        static readonly string datasetFile = Path.GetFullPath(path);
        static string indexPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Index"));
        

        private void Run()
        {
            fileCheck(datasetFile);

            //All text from dataset
            //string text = File.ReadAllText(datasetFile);
            //string[] lines = File.ReadAllLines(datasetFilePath);
            var index = new Indexer(datasetFile);
            index.testing();

            

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
//Βήματα
/*1.Η ανάλυση θα βασίζεται στις μετρικές tf-idf  
2.Η αναζήτηση θα πρέπει να αγνοεί λέξεις που είναι πολύ κοινές και δεν συνεισφέρουν  στη σημασία του κειμένου (stop-words)  
3.Η αναζήτηση θα πρέπει να λαμβάνει υπόψη τους όρους που περιλαμβάνονται στο  κείμενο, αφού πρώτα προβεί σε αποκοπή των καταλήξεων (stemming).  
4.Η αναζήτηση θα μπορεί να κατευθυνθεί και σε άλλα πεδία πλην αυτών που  περιέχουν το κείμενο.  
5.Θα υπάρχει δυνατότητα να ταξινομηθούν τα αποτελέσματα, είτε με βάση κάποιο  άλλο πεδίο (πλην του κειμένου), είτε με βάση την σχετικοτητα του κειμένου τους με το  ερώτημα.  
6.Τα αποτελέσματα θα πρέπει να μπορούν να σελιδοποιηθούν με τρόπο  παραμετροποιήσιμο (τρέχουσα σελίδα, αριθμός αποτελεσμάτων ανά σελίδα)  
7.Η σχετικότητα εκάστου εκ των αποτελεσμάτων θα μπορεί να εμπεριέχεται στα  αποτελέσματα ως αριθμός 
8.Τα αποτελέσματα θα πρέπει να μπορούν να παρουσιάζουν επισημασμένες τις  εμφανίσεις των λέξεων που βρέθηκαν στο κείμενο  
9.Θα μπορούν να παρέχονται πληροφορίες για το ευρετήριο, όπως ο αριθμός  εγγράφων και οι κορυφαίοι σε εμφανίσεις όροι.*/