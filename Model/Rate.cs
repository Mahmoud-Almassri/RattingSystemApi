using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RattingSystem.Model
{
    public class Rate
    {
        public Guid Id { get; set; }
        public int SessionRateDegree { get; set; }
        public int PresenteRaterDegree { get; set; }
        public string Comment { get; set; }
        [ForeignKey("Rate_Session")]
        public Guid Session_Id { get; set; }
        public Session Rate_Session { get; set; }
        [ForeignKey("Rate_User")]
        public string User_Id { get; set; }
        public IdentityUser Rate_User { get; set; }
    }
}
