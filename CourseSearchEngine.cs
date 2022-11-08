using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;

namespace LuceneProject
{
    class CourseSearchEngine
    {
        private const LuceneVersion version = LuceneVersion.LUCENE_48;
        private readonly StandardAnalyzer _analyzer;
        private readonly RAMDirectory _directory;
        private readonly IndexWriterConfig _config;
        private readonly IndexWriter _writer;

        public CourseSearchEngine()
        {
            _analyzer = new StandardAnalyzer(version);
            _directory = new RAMDirectory();
            _config = new IndexWriterConfig(version, _analyzer);
            _writer = new IndexWriter(_directory, _config);

        }

        public void AddCoursesToIndex(IEnumerable<Course> courses) {
            foreach (var course in courses) {
                var document = new Document();
                document.Add(new StringField("Guid", course.Guid.ToString(), Field.Store.YES)); //id for index
                document.Add(new TextField("Year", course.Year.ToString(), Field.Store.YES));
                document.Add(new StringField("CRN", course.CRN.ToString(), Field.Store.YES)); //crn is unique id
                document.Add(new StringField("Subject", course.Subject, Field.Store.YES)); //subject is student/work group
                document.Add(new TextField("Term", course.Term, Field.Store.NO));
                document.Add(new TextField("YearTerm", course.Year_Term.ToString(), Field.Store.NO));
                document.Add(new StringField("Number", course.Number.ToString(), Field.Store.YES)); //number is course number
                document.Add(new TextField("Name", course.Name, Field.Store.YES)); //name of course
                document.Add(new TextField("Description", course.Description, Field.Store.YES));
                document.Add(new TextField("Credit Hours", course.Credit_Hours, Field.Store.YES));
                document.Add(new TextField("Section Info", course.Section_Info, Field.Store.YES));
                document.Add(new TextField("Degree Attributes", course.Degree_Attributes, Field.Store.YES));
                document.Add(new TextField("Schedule Information", course.Schedule_Information, Field.Store.YES));
                document.Add(new TextField("Section", course.Section, Field.Store.YES));
                document.Add(new TextField("Status Code", course.Status_Code.ToString(), Field.Store.NO)); //status code is always "A"
                document.Add(new TextField("Part of Term", course.Part_of_Term.ToString(), Field.Store.YES));
                document.Add(new TextField("Section Title", course.Section_Title, Field.Store.YES));
                document.Add(new TextField("Section Credit Hours", course.Section_Credit_Hours, Field.Store.YES));
                document.Add(new TextField("Section Status", course.Section_Status.ToString(), Field.Store.NO)); //section status always "A"
                document.Add(new TextField("Enrollment Status", course.Enrollment_Status, Field.Store.YES)); //enrollment status
                document.Add(new TextField("Type", course.Type, Field.Store.YES)); //type of lecture e.g: online, etc.
                document.Add(new StringField("Type Code", course.Type_Code, Field.Store.YES));
                document.Add(new TextField("Start Time", course.Start_Time, Field.Store.YES));
                document.Add(new TextField("End Time", course.End_Time, Field.Store.YES));
                document.Add(new TextField("Days of Week", course.Days_of_Week, Field.Store.YES));
                document.Add(new TextField("Room", course.Room, Field.Store.YES));
                document.Add(new TextField("Building", course.Building, Field.Store.YES));
                document.Add(new TextField("Instructors", course.Instructors, Field.Store.YES));
            }
            _writer.Commit();
        }

        public IEnumerable<Course> Search(string searchTerm) {
            var directoryReader = DirectoryReader.Open(_directory);
            var indexSearcher = new IndexSearcher(directoryReader);

            string[] fields = { "CRN", "Subject", "Number", "Type Code", "Name", "Enrollment Status", "Type", "Start Time", "Room", "Days of Week", "Building", "Instructors"};
            var queryParser = new MultiFieldQueryParser(version, fields, _analyzer);
            var query = queryParser.Parse(searchTerm);

            var hits = indexSearcher.Search(query, 20).ScoreDocs;

            var courses = new List<Course>();
            foreach (var hit in hits) {
                var document = indexSearcher.Doc(hit.Doc);
                courses.Add(new Course()
                {
                    Guid = new Guid(document.Get("Guid")),
                    Number = int.Parse(document.Get("Number")),
                    Name = document.Get("Name"),
                    CRN = int.Parse(document.Get("CRN")),
                    Enrollment_Status = document.Get("Enrollment Status"),
                    Type = document.Get("Type"),
                    Type_Code = document.Get("Type Code"),
                    Start_Time = document.Get("Start Time"),
                    End_Time = document.Get("End Time"),
                    Days_of_Week = document.Get("Days of Week"),
                    Room = document.Get("Room"),
                    Building = document.Get("Building"),
                    Instructors = document.Get("Instructors")
                });
            }
            return courses;
        }
    }
}
