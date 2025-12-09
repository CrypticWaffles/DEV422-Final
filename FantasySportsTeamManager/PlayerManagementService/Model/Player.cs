using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayerManagementService.Model
{
    [Table("Players")]
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        public string PlayerName { get; set; } 

        public string Position { get; set; }

        public int? TeamId { get; set; }

        public Player() { }

        public Player(string playerName, string position)
        {
            PlayerName = playerName;
            Position = position;
            TeamId = null; 
        }

        // --- Domain Methods ---

        public void DraftToTeam(int newTeamId)
        {
            TeamId = newTeamId;
        }

        public void ReleaseFromTeam()
        {
            TeamId = null;
        }

        public void UpdateInfo(string newName, string newPosition)
        {
            PlayerName = newName;
            Position = newPosition;
        }
    }

    namespace PlayerManagementService.Model
    {
        public class UpdatePlayerRequest
        {
            public string PlayerName { get; set; }
            public string Position { get; set; }
        }

        public class PlayerCreationRequest
        {
            public string PlayerName { get; set; }
            public string Position { get; set; }
        }
    }
}