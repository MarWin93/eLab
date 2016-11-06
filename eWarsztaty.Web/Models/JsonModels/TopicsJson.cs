namespace eWarsztaty.Web.Models.JsonModels
{
    public class TopicsJson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public bool IsArchived { get; set; }
      //  public string EnrollmentKey { get; set; }
    }
}