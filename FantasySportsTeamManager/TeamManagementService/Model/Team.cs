using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeamManagementService.Model
{
    [Table("Teams")]
    public class Team
    {
        [Key]
        public int teamId {  get; set; }
        public required string teamName { get; set; }
        public DateTime createdDate { get; set; }
    }
}
