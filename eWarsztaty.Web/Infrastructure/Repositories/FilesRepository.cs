using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using eWarsztaty.Domain;
using eWarsztaty.Web.Models.JsonModels;

namespace eWarsztaty.Web.Infrastructure.Repositories
{
    public class FilesRepository
    {
        private eWarsztatyContext _db = new eWarsztatyContext();

        public void SaveFile(byte[] fileBytes, string fileName, string fileExtension, int? courseId = null, int? topicId = null)
        {
            var size = fileBytes.LongLength/1024;
            var file = new Plik {File = fileBytes, Nazwa = fileName, Rozszerzenie = fileExtension, CourseId = courseId, TopicId = topicId, Size = size.ToString()};
            _db.Pliki.Add(file);

            _db.SaveChanges();
        }

        public Plik GetFile(int fileId)
        {
            return _db.Pliki.FirstOrDefault(x => x.PlikId == fileId);
        }
    }
}