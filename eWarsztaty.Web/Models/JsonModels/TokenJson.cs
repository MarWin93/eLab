using eWarsztaty.Domain;

namespace eWarsztaty.Web.Models.JsonModels
{
    public class TokenJson
    {
        public int Id  { get; set; }
        public string Username { get; set; }
        public  int RoleId { get; set; }
        public  string RoleName { get; set; }
    }
}