using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneProject
{
    public class Indexer
    {
        private const LuceneVersion version = LuceneVersion.LUCENE_48;

        /*private readonly string year = "2022";
        private readonly string yearterm = "2022-sp";

        private readonly string name = "1920s to Today 19thC Sp American Studies 20thC World from Midcentury 21st Century Dramaturgy History Judaism " +
            "Mathematical World Systems-Based Approach to the Operation of Livestock-Based Food Production System ABE Principles Bioenvironment " +
            "Biological Bioprocessing Abstract Linear Algebra Academic Presentation Skills Progress Accelerated Chemistry Lab Fundementals Algorithms " +
            "Computing Accounting Analysis Analytics Applications Accountancy Control Systems Financial Institutions Regulation Measurement Reporting Control " +
            "ACES Study Abroad Transfer Orientation ";*/
        private readonly string[] subject = { "AAS" , "ABE", "ACCY", "ACE", "ACES", "ADV", "AE", "AFAS", "AFRO", "AFST",
        "AGCM", "AGED", "AHS", "AIS", "ALEC", "ANSC", "ANTH", "ARAB", "ARCH", "ART", "ARTD", "ARTE", "ARTF", "ARTH", "ARTJ",
        "ARTS", "ASRM", "ASST", "ASTR", "ATMS", "BADM", "BCOG", "BCS", "BDI", "BIOC", "BIOE", "BIOL", "BIOP", "BSE", "BTW",
        "BUS", "CB", "CDB", "CEE", "CHBE", "CHEM", "CHIN", "CHLH", "CHP", "CI", "CIC", "CLCV", "CLE", "CMN", "CPSC", "CS",
        "CSE", "CW", "CWL", "CZCH", "DANC", "DTX", "EALC", "ECE", "ECON", "EDPR", "EDUC", "EIL", "ENG", "ENGL", "ENSU", "ENT",
        "ENVS", "EPOL", "EPSY", "ERAM", "ESE", "ESL", "EURO", "FAA", "FIN", "FLTE", "FR", "FSHN", "GC", "GEOG", "GEOL", "GER",
        "GLBL", "GMC", "GRK", "GRKM", "GS", "GSD", "GWS", "HDFS", "HEBR", "HIST", "HNDI", "HORT", "HT", "HUM", "IB", "IE", "IHLT",
        "INFO", "IS", "ITAL", "JAPN", "JOUR", "JS", "KIN", "KOR", "LA", "LAS", "LAST", "LAT", "LAW", "LCTL", "LEAD", "LER", "LING",
        "LLS", "MACS", "MATH", "MBA", "MCB", "MDIA", "MDVL", "ME", "MFST", "MICR", "MILS", "MIP", "MSE", "MUS", "MUSC", "MUSE",
        "NE", "NEUR", "NPRE", "NRES", "NS", "NUTR", "PATH", "PBIO", "PERS", "PHIL", "PHYS", "PLPA", "POL", "PORT", "PS", "PSM",
        "PSYC", "QUEC", "REES", "REHB", "REL", "RHET", "RMLG", "RSOC", "RST", "RUSS", "SAME", "SBC", "SCAN", "SE", "SHS", "SLAV",
        "SOC", "SOCW", "SPAN", "SPED", "STAT", "SWAH", "TAM", "TE", "THEA", "TMGT", "TRST", "TSM", "TURK", "UKR", "UP", "VCM",
        "VM", "WGGP", "WLOF", "WRIT", "YDSH"};
        private readonly string IndexPath;
        private readonly string DatasetFile;
        

        private TokenStream ts;
        private IOffsetAttribute offsetAtt;
        private ICharTermAttribute termAtt;

        public Indexer(string datasetfile, string indexPath)
        {
            this.IndexPath = indexPath;
            this.DatasetFile = datasetfile;
        }

        /// <summary>
        /// Reads all text from the file and creates a token stream.
        /// </summary>
        public void myTokenizer()
        {    
            var Analyzer = new StandardAnalyzer(version);
            ts = Analyzer.GetTokenStream("alltokens", File.ReadAllText(DatasetFile));
            offsetAtt = ts.AddAttribute<IOffsetAttribute>();
            termAtt = ts.AddAttribute<ICharTermAttribute>();
            Analyzer.Dispose();
        }

        
        /// <summary>
        /// Creates the index with all data
        /// </summary>
        public void CreateIndex() {
            using var dir = FSDirectory.Open(IndexPath);
            var Analyzer = new StandardAnalyzer(version);
            var IndexConfig = new IndexWriterConfig(version, Analyzer);
            using var writer = new IndexWriter(dir, IndexConfig);

            IndexConfig.OpenMode = OpenMode.CREATE_OR_APPEND;
            try
            {
                ts.Reset();
                
                while (ts.IncrementToken())
                {
                    var document = new Document();
                    Console.WriteLine("Added: "+ termAtt.ToString());
                   /* Console.WriteLine("token: " + ts.ReflectAsString(false));
                    Console.WriteLine("Term: " + termAtt.ToString());*/
                    /*Console.WriteLine("token start offset: " + offsetAtt.StartOffset);
                    Console.WriteLine("  token end offset: " + offsetAtt.EndOffset);*/

                    /*if (termAtt.ToString() == year)
                    {
                        document.Add(new TextField("Year", termAtt.ToString(), Field.Store.YES));
                    }
                    else if (yearterm.Contains(termAtt.ToString()))
                    {
                        document.Add(new TextField("Term", termAtt.ToString(), Field.Store.YES));
                    }
                    else if (subject.Contains(termAtt.ToString().ToUpper()))
                    {//saves subject code
                        document.Add(new StringField("Subject", termAtt.ToString().ToUpper(), Field.Store.YES));
                        Console.WriteLine("Before Saved: " + termAtt.ToString());
                        //saves subject number
                        ts.IncrementToken();
                        
                        Console.WriteLine("After saved: " + termAtt.ToString());
                        document.Add(new StringField("Number", termAtt.ToString(), Field.Store.YES));
                    }

                    else if ()
                    {

                    }*/
                    document.Add(new TextField("fieldname", termAtt.ToString(), Field.Store.YES));
                    
                    writer.AddDocument(document);
                    //writer.Flush(false, false);
                    writer.Commit();
                }
                
                ts.End();
            }
            finally
            {
                
                ts.Dispose();
            }
        }

        private bool checkSubject(string term)
        {
            return subject.Contains(term);
        }

    }
}
