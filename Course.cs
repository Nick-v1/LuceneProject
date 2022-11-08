using System;

namespace LuceneProject
{
    public class Course
    {
        public Guid Guid { get; set; }
        public int Year { get; set; }
        public string Term { get; set; }
        public string Year_Term {get;set;}
        public string Subject { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Credit_Hours { get; set; }
        public string Section_Info { get; set; }
        public string Degree_Attributes { get; set; }
        public string Schedule_Information { get; set; }
        public int CRN { get; set; }
        public string Section { get; set; }
        public char Status_Code { get; set; }
        public char Part_of_Term { get; set; }
        public string Section_Title { get; set; }
        public string Section_Credit_Hours { get; set; }
        public char Section_Status { get; set; }
        public string Enrollment_Status { get; set; }
        public string Type { get; set; }
        public string Type_Code { get; set; }
        public string Start_Time { get; set; }
        public string End_Time { get; set; }
        public string Days_of_Week { get; set; }
        public string Room { get; set; }
        public string Building { get; set; }
        public string Instructors { get; set; }
    }
}