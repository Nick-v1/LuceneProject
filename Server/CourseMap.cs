using CsvHelper.Configuration;

namespace Server
{
    public class CourseMap : ClassMap<Course>
    {
        public CourseMap()
        {
            Map(course => course.Year).Name("Year");
            Map(course => course.Term).Name("Term");
            Map(course => course.YearTerm).Name("YearTerm");
            Map(course => course.Subject).Name("Subject");
            Map(course => course.Number).Name("Number");
            Map(course => course.Name).Name("Name");
            Map(course => course.Description).Name("Description");
            Map(course => course.CreditHours).Name("Credit Hours");
            Map(course => course.SectionInfo).Name("Section Info");
            Map(course => course.DegreeAttributes).Name("Degree Attributes"); //10
            Map(course => course.ScheduleInformation).Name("Schedule Information");
            Map(course => course.CRN).Name("CRN");
            Map(course => course.Section).Name("Section");
            Map(course => course.StatusCode).Name("Status Code");
            Map(course => course.PartofTerm).Name("Part of Term");
            Map(course => course.SectionTitle).Name("Section Title");
            Map(course => course.SectionCreditHours).Name("Section Credit Hours");
            Map(course => course.SectionStatus).Name("Section Status");
            Map(course => course.EnrollmentStatus).Name("Enrollment Status");
            Map(course => course.Type).Name("Type");
            Map(course => course.TypeCode).Name("Type Code");
            Map(course => course.StartTime).Name("Start Time");
            Map(course => course.EndTime).Name("End Time");
            Map(course => course.DaysofWeek).Name("Days of Week");
            Map(course => course.Room).Name("Room");
            Map(course => course.Building).Name("Building");
            Map(course => course.Instructors).Name("Instructors");
        }
    }
}
