using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using eWarsztaty.Domain;
using eWarsztaty.Web.Infrastructure.Repositories;
using eWarsztaty.Web.Models.JsonModels;

namespace eWarsztaty.Web.Controllers
{
    public class CoursesController : ApiController
    {
        readonly CourseRepository _courseRepository = new CourseRepository();
        readonly FilesRepository _fileRepository = new FilesRepository();
        // GET api/courses
        public IEnumerable<CoursesJson> Get()
        {
            return _courseRepository.GetAllCourses();
        }

        // GET api/courses/5
        public CoursesJson Get(int id)
        {
            if (id > 0)
            {
                return _courseRepository.GetCourseById(id);
            }
            var newCourse = new CoursesJson();
            return newCourse;
        }

        // POST api/courses
        public IHttpActionResult Post([FromBody]CoursesJson course)
        {
            _courseRepository.SaveCourse(course);
            return Ok();
        }

        // PUT api/courses/5
        public IHttpActionResult Put(int id, [FromBody]CoursesJson course)
        {
            _courseRepository.SaveCourse(id, course);
            return Ok();
        }

        // DELETE api/courses/5
        public void Delete(int id)
        {
            _courseRepository.DeleteCourse(id);
        }

        [Route("api/courses/{id}/close")]
        [HttpGet]
        public IHttpActionResult CloseCourse(int id)
        {
            _courseRepository.CloseCourse(id);
            return Ok();
        }

        [HttpPost, Route("api/courses/{id}/upload")]
        public async Task<IHttpActionResult> Upload(int id)
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                if (file.Headers.ContentType != null && file.Headers.ContentLength > 0)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();
                    var extension = Path.GetExtension(fileName);
                    _fileRepository.SaveFile(buffer, fileName, extension, id);
                }
            }

            return Ok();
        }

        [HttpGet, Route("api/courses/{id}/download/{fileId}")]
        public HttpResponseMessage Download(int id, int fileId)
        {

            var file = _fileRepository.GetFile(fileId);
            if (file == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(file.File)
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = file.Nazwa
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

    }
}
