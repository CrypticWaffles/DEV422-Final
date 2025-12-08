using System.ComponentModel.DataAnnotations;
namespace TeamManagementService.Model
{
    public class Team
    {
        [Key]
        public int teamId {  get; set; }
        public required string teamName { get; set; }
        public DateTime createdDate { get; set; }
    }
}
