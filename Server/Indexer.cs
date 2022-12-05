using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using CsvHelper;
using System.Globalization;
using System.IO;
using NLog;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class Indexer
    {
        private const LuceneVersion version = LuceneVersion.LUCENE_48;
        private Logger log = LogManager.GetCurrentClassLogger();

        private string IndexPath;
        private string IndexPathFile;
        private string DatasetFile;

        public Indexer() {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Dataset\2022-sp.csv");
            this.DatasetFile = Path.GetFullPath(path);
            this.IndexPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Index"));
            this.IndexPathFile = Path.GetFullPath(Path.Combine(IndexPath, @"_0.cfs"));
        }


        /// <summary>
        /// Use this method to create the index
        /// </summary>
        public string CreateIndex()
        {
            if (File.Exists(IndexPathFile)) {
                log.Info("Index exists!");
                return "Index already exists!";
            }
            log.Info("Creating Index...");

            var AllTextList = File.ReadAllLines(DatasetFile);
            using var dir = FSDirectory.Open(IndexPath);

            var Analyzer = new StandardAnalyzer(version);

            var IndexConfig = new IndexWriterConfig(version, Analyzer);
            using var writer = new IndexWriter(dir, IndexConfig);


            using var CsvReader = new StreamReader(DatasetFile);
            using var CsvContext = new CsvReader(CsvReader, CultureInfo.InvariantCulture);
            CsvContext.Context.RegisterClassMap<CourseMap>();
            var records = CsvContext.GetRecords<Course>();


            foreach (var course in records)
            {
                var doc = new Document();
                doc.Add(new TextField("Year", course.Year.ToString(), Field.Store.YES));
                doc.Add(new TextField("CRN", course.CRN.ToString(), Field.Store.YES)); //crn is unique id
                doc.Add(new TextField("Subject", course.Subject, Field.Store.YES)); //subject is student/work group
                doc.Add(new TextField("Term", course.Term, Field.Store.YES));
                doc.Add(new TextField("YearTerm", course.YearTerm.ToString(), Field.Store.YES));
                doc.Add(new TextField("Number", course.Number.ToString(), Field.Store.YES)); //number is course number
                doc.Add(new TextField("Name", course.Name, Field.Store.YES)); //name of course
                doc.Add(new TextField("Description", course.Description, Field.Store.YES));
                doc.Add(new TextField("Credit Hours", course.CreditHours, Field.Store.YES));
                doc.Add(new TextField("Section Info", course.SectionInfo, Field.Store.YES));
                doc.Add(new TextField("Degree Attributes", course.DegreeAttributes, Field.Store.YES));
                doc.Add(new TextField("Schedule Information", course.ScheduleInformation, Field.Store.YES));
                doc.Add(new TextField("Section", course.Section, Field.Store.YES));
                doc.Add(new TextField("Status Code", course.StatusCode.ToString(), Field.Store.NO)); //status code is always "A"
                doc.Add(new TextField("Part of Term", course.PartofTerm.ToString(), Field.Store.YES));
                doc.Add(new TextField("Section Title", course.SectionTitle, Field.Store.YES));
                doc.Add(new TextField("Section Credit Hours", course.SectionCreditHours, Field.Store.YES));
                doc.Add(new TextField("Section Status", course.SectionStatus.ToString(), Field.Store.YES)); //section status always "A"
                doc.Add(new TextField("Enrollment Status", course.EnrollmentStatus, Field.Store.YES)); //enrollment status
                doc.Add(new TextField("Type", course.Type, Field.Store.YES)); //type of lecture e.g: online, etc.
                doc.Add(new TextField("Type Code", course.TypeCode, Field.Store.YES));
                doc.Add(new TextField("Start Time", course.StartTime, Field.Store.YES));
                doc.Add(new TextField("End Time", course.EndTime, Field.Store.YES));
                doc.Add(new TextField("Days of Week", course.DaysofWeek, Field.Store.YES));
                doc.Add(new TextField("Room", course.Room, Field.Store.YES));
                doc.Add(new TextField("Building", course.Building, Field.Store.YES));
                doc.Add(new TextField("Instructors", course.Instructors, Field.Store.YES));
                writer.AddDocument(doc);
            }

            writer.Commit();
            log.Info("Index created!");
            return "Index has been successfully created";
        }

        /// <summary>
        /// Use this method to search the index
        /// Calls collector to gather results and then writes out.
        /// Average Score, Score of every hit, and some important results.
        /// </summary>
        /// <param name="hpp">how many hits</param>
        /// <param name="QueryToSearch">your query</param>
        /// <param name="header">the header/title of the field</param>
        public IEnumerable<Course> SearchIndexSpecific(int hpp, string QueryToSearch, string header)
        {
            int hitsPerPage = hpp;

            var Analyzer = new StandardAnalyzer(version);

            using var dir = FSDirectory.Open(IndexPath);
            using DirectoryReader ireader = DirectoryReader.Open(dir);

            IndexSearcher isearcher = new IndexSearcher(ireader);
            //query to parse
            Query query = new QueryParser(version, header, Analyzer).Parse(QueryToSearch);

            var collector = TopScoreDocCollector.Create(hitsPerPage, true);
            isearcher.Search(query, collector);
            ScoreDoc[] hits = collector.GetTopDocs().ScoreDocs;

            var courses = new List<Course>();

            float meanScoreSum = 0.0f;
            log.Info("Top Results\n-----------------------------------------------");
            foreach (var hit in hits)
            {
                int docId = hit.Doc;
                Document d = isearcher.Doc(docId);
                Course course = new Course();

                course.Building = d.Get("Building");
                course.CreditHours = d.Get("Credit Hours");
                course.CRN = int.Parse(d.Get("CRN"));
                course.DaysofWeek = d.Get("Days of Week");
                course.DegreeAttributes = d.Get("Degree Attributes");
                course.Description = d.Get("Description");
                course.EndTime = d.Get("End Time");
                course.EnrollmentStatus = d.Get("Enrollment Status");
                course.Instructors = d.Get("Instructors");
                course.Name = d.Get("Name");
                course.Number = int.Parse(d.Get("Number"));
                course.PartofTerm = d.Get("Part of Term");
                course.Room = d.Get("Room");
                course.ScheduleInformation = d.Get("Schedule Information");
                course.Section = d.Get("Section");
                course.SectionCreditHours = d.Get("Section Credit Hours");
                course.SectionInfo = d.Get("Section Status");
                course.SectionStatus = d.Get("Section Status");
                course.SectionTitle = d.Get("Section Title");
                course.StartTime = d.Get("Start Time");
                course.StatusCode = d.Get("Status Code");
                course.Subject = d.Get("Subject");
                course.Term = d.Get("Term");
                course.Type = d.Get("Type");
                course.TypeCode = d.Get("Type Code");
                course.Year = int.Parse(d.Get("Year"));
                course.YearTerm = d.Get("YearTerm");

                courses.Add(course);

                meanScoreSum += hit.Score;
                log.Info(docId + ". " + "CRN: " +d.Get("CRN") + ". Score: " + hit.Score);
            }
            
            log.Info($"\nTotal hits: {hits.Length}, Max Score: {isearcher.Search(query, hitsPerPage).MaxScore}");
            log.Info("Average relevance: " + meanScoreSum / hits.Length);
            return courses.ToArray();
        }

        /// <summary>
        /// Searching all index with the headers mentioned below.
        /// </summary>
        /// <param name="numHits">hits</param>
        /// <param name="question">query for lucene</param>
        /// <returns></returns>
        public IEnumerable<Course> SearchIndexMultiQuery(int numHits, string question)
        {
            var Analyzer = new StandardAnalyzer(version);

            using var dir = FSDirectory.Open(IndexPath);
            using DirectoryReader ireader = DirectoryReader.Open(dir);

            IndexSearcher isearcher = new IndexSearcher(ireader);

            var headers = new string[] { "Subject", "Number", "Name", "Description", "Enrollment Status" };

            Query query = new MultiFieldQueryParser(version, headers, Analyzer).Parse(question);


            var collector = TopScoreDocCollector.Create(numHits, true);
            isearcher.Search(query, collector);
            ScoreDoc[] hits = collector.GetTopDocs().ScoreDocs;

            var courses = new List<Course>();

            float meanScoreSum = 0.0f;
            Console.WriteLine("Top Results\n-----------------------------------------------");
            foreach (var hit in hits)
            {
                int docId = hit.Doc;
                Document d = isearcher.Doc(docId);
                Course course = new Course();

                course.Building = d.Get("Building");
                course.CreditHours = d.Get("Credit Hours");
                course.CRN = int.Parse(d.Get("CRN"));
                course.DaysofWeek = d.Get("Days of Week");
                course.DegreeAttributes = d.Get("Degree Attributes");
                course.Description = d.Get("Description");
                course.EndTime = d.Get("End Time");
                course.EnrollmentStatus = d.Get("Enrollment Status");
                course.Instructors = d.Get("Instructors");
                course.Name = d.Get("Name");
                course.Number = int.Parse(d.Get("Number"));
                course.PartofTerm = d.Get("Part of Term");
                course.Room = d.Get("Room");
                course.ScheduleInformation = d.Get("Schedule Information");
                course.Section = d.Get("Section");
                course.SectionCreditHours = d.Get("Section Credit Hours");
                course.SectionInfo = d.Get("Section Status");
                course.SectionStatus = d.Get("Section Status");
                course.SectionTitle = d.Get("Section Title");
                course.StartTime = d.Get("Start Time");
                course.StatusCode = d.Get("Status Code");
                course.Subject = d.Get("Subject");
                course.Term = d.Get("Term");
                course.Type = d.Get("Type");
                course.TypeCode = d.Get("Type Code");
                course.Year = int.Parse(d.Get("Year"));
                course.YearTerm = d.Get("YearTerm");

                courses.Add(course);

                meanScoreSum += hit.Score;
                log.Info(docId + ". " + "CRN: " + d.Get("CRN") + ". Score: "+hit.Score);
            }
            log.Info($"\nTotal hits: {hits.Length}, Max Score: {isearcher.Search(query, numHits).MaxScore}");
            log.Info("Average relevance: " + meanScoreSum / hits.Length);

            return courses.ToArray();
        }

        public void SearchIndexExample()
        {
            int hitsPerPage = 100;

            var Analyzer = new StandardAnalyzer(version);

            using var dir = FSDirectory.Open(IndexPath);
            using DirectoryReader ireader = DirectoryReader.Open(dir);

            IndexSearcher isearcher = new IndexSearcher(ireader);
            //query to parse. version, header to search, analyzer, query to search.
            Query query = new QueryParser(version, "Description", Analyzer).Parse("Programming");

            var collector = TopScoreDocCollector.Create(hitsPerPage, true);
            isearcher.Search(query, collector);
            ScoreDoc[] hits = collector.GetTopDocs().ScoreDocs;



            float meanScoreSum = 0.0f;
            Console.WriteLine("Top Results\n-------------------------------------------------------------------------");
            foreach (var hit in hits)
            {
                int docId = hit.Doc;
                Document d = isearcher.Doc(docId);

                Console.WriteLine("Doc Id: " + hit.Doc + ". Score: " + hit.Score + "\n___________________________________________");
                Console.WriteLine($"Subject code: {d.Get("Subject")}\nNumber: {d.Get("Number")}\nSubject name: {d.Get("Name")}\n" +
                    $"Credit Hours: {d.Get("Credit Hours")}\n" +
                    $"Enrollment Status: {d.Get("Enrollment Status")}\n" +
                    $"Type: {d.Get("Type")}\n" +
                    $"Type Code: {d.Get("Type Code")}\n" +
                    $"Start: {d.Get("Start Time")}\n" +
                    $"End: {d.Get("End Time")}\n");
                meanScoreSum += hit.Score;
            }
            Console.WriteLine($"\nTotal hits: {hits.Length}, Max Score: {isearcher.Search(query, hitsPerPage).MaxScore}");
            Console.WriteLine("Average relevance: " + meanScoreSum / hits.Length);
        }
    }
}
