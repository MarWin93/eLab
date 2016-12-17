using eWarsztaty.Web.Infrastructure.Repositories;
using eWarsztaty.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace eWarsztaty.Web.Controllers
{
    public class TopicsController : ApiController
    {
        readonly FilesRepository _fileRepository = new FilesRepository();
        readonly TopicRepository _topicRepository = new TopicRepository();
        // GET api/topics
        public IEnumerable<TopicsJson> Get()
        {
            
            return _topicRepository.GetAllTopics();
        }

        // GET api/topics/5
        public TopicsJson Get(int id)
        {
            if (id > 0)
            {
                return _topicRepository.GetTopicById(id);
            }
            var newTopic = new TopicsJson();
            return newTopic;
        }

        // POST api/topics
        public IHttpActionResult Post([FromBody]TopicsJson topic)
        {
            _topicRepository.SaveTopic(topic);
            return Ok();
        }

        // PUT api/topics/5
        public IHttpActionResult Put(int id, [FromBody]TopicsJson topic)
        {
            _topicRepository.SaveTopic(id, topic);
            return Ok();
        }

        // DELETE api/topics/5
        public void Delete(int id)
        {
            _topicRepository.DeleteTopic(id);
        }   

        [Route("api/topics/{id}/close")]
        [HttpGet]
        public IHttpActionResult CloseTopic(int id)
        {
            _topicRepository.CloseTopic(id);
            return Ok();
        }

        [HttpPost, Route("api/topics/{id}/upload")]
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

                    _fileRepository.SaveFile(buffer, fileName, extension, null, id);
                }
            }

            return Ok();
        }

        [HttpGet, Route("api/topics/{id}/download/{fileId}")]
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

        [HttpGet, Route("api/topics/{id}/show/{fileId}")]
        public HttpResponseMessage Show(int id, int fileId)
        {

            var file = _fileRepository.GetFile(fileId);
            if (file == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if(file.Rozszerzenie != ".pdf" && file.Rozszerzenie != ".PDF")
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(file.File)
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
                {
                    FileName = file.Nazwa
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/pdf");

            return result;
        }
    }
}
