using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XML_Upload.Models;

namespace XML_Upload.Upload
{
    public class StudentsUploadDetails
    {
        public Dictionary<string, string> GetColumnsDictionary()
        {
            var mappingDictionary = new Dictionary<string, string>
            {
                {"College", "College"},
                {"CollegeCode", "CollegeCode"},
                {"Date", "Date"},
                {"Time", "Time"},
                {"Course", "Course"},
                {"Email", "Email"},
                {"Interested during First Meeting", "Interested during First Meeting"},
                {"Discussion", "Discussion"},
                {"Cell", "Cell"},
                {"PersonName", "PersonName"},
                {"Replyed", "Replyed"},
                {"Year", "Year"},
                {"ContactAgain", "ContactAgain"},
                {"LKTM", "LKTM"},
                {"FacebookURL", "FacebookURL"},
                {"Dream", "Dream"}
            };
            return mappingDictionary;
        }


        public List<FieldVisitStExcel> FindRowsFromExcel(string[][] parseline, Dictionary<string, int> valueDictionary)
        {
            var distinctStudents = new List<FieldVisitStExcel>();
            var sctx = new ApplicationDbContext();
            var students = sctx.FieldVisitStExcel.ToList();
            try
            {
                var parselineData = parseline.Skip(1).Distinct();
                foreach (var eachStudent in parselineData)
                {
                    var studentDetails = new FieldVisitStExcel();
                    studentDetails.CollegeFullName = eachStudent[valueDictionary["College"]];
                    studentDetails.CollegeShortName = eachStudent[valueDictionary["CollegeCode"]];
                    studentDetails.DateandTime = eachStudent[valueDictionary["Date"]];
                    studentDetails.Time = eachStudent[valueDictionary["Time"]];
                    studentDetails.Course = eachStudent[valueDictionary["Course"]];
                    studentDetails.Discussion = eachStudent[valueDictionary["Discussion"]];
                    studentDetails.Email = eachStudent[valueDictionary["Email"]];
                    studentDetails.FacebookURL = eachStudent[valueDictionary["FacebookURL"]];
                    studentDetails.Imagination = eachStudent[valueDictionary["Dream"]];
                    studentDetails.MobileNo = eachStudent[valueDictionary["Cell"]];
                    studentDetails.Name = eachStudent[valueDictionary["PersonName"]];
                    studentDetails.IsIntrestedInFirstMeet =
                        eachStudent[valueDictionary["Interested during First Meeting"]];
                    studentDetails.Replyed = eachStudent[valueDictionary["Replyed"]];
                    studentDetails.Year = eachStudent[valueDictionary["Year"]];
                    studentDetails.ContactAgain = eachStudent[valueDictionary["ContactAgain"]];
                    studentDetails.LKTM = eachStudent[valueDictionary["LKTM"]];

                    if (!string.IsNullOrEmpty(studentDetails.Name))
                    {
                        distinctStudents.Add(studentDetails);
                        if (!students.Any(s => s.Name == studentDetails.Name) &&
                            !students.Any(s => s.CollegeFullName == studentDetails.CollegeFullName))
                            sctx.FieldVisitStExcel.Add(studentDetails);
                        else
                            studentDetails.Id = students
                                .Where(s => s.Name == studentDetails.Name &&
                                            s.CollegeFullName == studentDetails.CollegeFullName).Select(c => c.Id)
                                .FirstOrDefault();
                    }
                }

                sctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return distinctStudents;
        }
    }
}
