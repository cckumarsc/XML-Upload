using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using XML_Upload.Models;
using XML_Upload.Upload;

namespace XML_Upload.Controllers
{
    public class XMLUploadController : ApiController
    {
        private readonly CsvParser _csv = new CsvParser();
        private readonly ApplicationDbContext _sctx;
        private readonly string Root;
        public StudentService _studentService;
        public XMLUploadController()
        {
            Root = HttpContext.Current.Server.MapPath("~/App_Data");
            _sctx = new ApplicationDbContext();
            _studentService = new StudentService();
        }

        [Route("Api/StudentDetail/Upload")]
        [HttpPost]
        public async Task<List<FieldVisitStExcel>> UploadStudentsSheetByAdminAsync()
        {
            var distinctStudents = new List<FieldVisitStExcel>();
            if (Request.Content.IsMimeMultipartContent())
            {
                var provider = new MultipartFormDataStreamProvider(Root);
                await Request.Content.ReadAsMultipartAsync(provider);
                distinctStudents = _studentService.UploadFieldVisitData(provider);
            }

            return distinctStudents;
        }

        [Route("Api/StudentDetails")]
        [HttpGet]
        public StudentDetailResponse GetStudentDetails()
        {
            StudentDetailResponse studentDetailResponse = _studentService.GetStudents();
            return studentDetailResponse;
        }
    }
}
