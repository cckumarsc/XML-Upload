using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XML_Upload.Models;

namespace XML_Upload.Upload
{
    public class StudentService
    {
        private readonly CsvParser _csv = new CsvParser();
        private readonly ApplicationDbContext _sctx;
        public StudentService()
        {
            _sctx = new ApplicationDbContext();
        }
        public List<FieldVisitStExcel> UploadFieldVisitData(MultipartFormDataStreamProvider provider)
        {
            List<FieldVisitStExcel> distinctStudents;
            var file = provider.FileData.FirstOrDefault();
            if (file == null) throw new InvalidOperationException("Server could not read uploaded excel file");
            var userFileName = file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            var student = new StudentsUploadDetails();
            var colsDictionary = student.GetColumnsDictionary();

            using (TextReader reader = File.OpenText(file.LocalFileName))
            {
                var parseline = _csv.Parse(reader);
                var colsCount = parseline.GetUpperBound(0);
                var valueDictionary = colsDictionary.Keys.ToDictionary(str => str,
                    str => Array.FindIndex(parseline[0], c => c == colsDictionary[str]));
                distinctStudents = student.FindRowsFromExcel(parseline, valueDictionary);
            }

            return distinctStudents;
        }
        public StudentDetailResponse GetStudents()
        {
            var studentDetailResponse = new StudentDetailResponse();
            var studentDetailList = new List<FieldVisitStExcel>();
            try
            {
                var students = _sctx.FieldVisitStExcel.ToList();
                foreach (var student in students)
                {
                    var studentDetails = new FieldVisitStExcel();
                    studentDetails.Id = student.Id;
                    studentDetails.CollegeFullName = student.CollegeFullName;
                    studentDetails.CollegeShortName = student.CollegeShortName;
                    studentDetails.DateandTime = student.DateandTime;
                    studentDetails.Time = student.Time;
                    studentDetails.Course = student.Course;
                    studentDetails.Discussion = student.Discussion;
                    studentDetails.Email = student.Email;
                    studentDetails.FacebookURL = student.FacebookURL;
                    studentDetails.Imagination = student.Imagination;
                    studentDetails.MobileNo = student.MobileNo;
                    studentDetails.Name = student.Name;
                    studentDetails.IsIntrestedInFirstMeet = student.IsIntrestedInFirstMeet;
                    studentDetails.Replyed = student.Replyed;
                    studentDetails.Year = student.Year;
                    studentDetails.ContactAgain = student.IsContactAgain;
                    studentDetails.LKTM = student.LKTM;
                    studentDetailList.Add(studentDetails);
                }
            }
            catch (Exception)
            {
            }

            studentDetailResponse.Students = studentDetailList;
            return studentDetailResponse;
        }
    }
}
