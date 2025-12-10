
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace PlayerManagementService.Model
{
    [Table("Players")]
    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; } // Matches UNIQUEIDENTIFIER (GUID)

        // Make required strings non-nullable (C# 11 'required' or default initializer)

        public string PlayerName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;


        // Undrafted = null; Drafted = Team GUID
        public Guid? TeamId { get; set; }

        public Player() { }

        public Player(string playerName, string position)
        {
            PlayerName = playerName;
            Position = position;
            TeamId = null;
        }

        // --- Domain Methods ---

        public void DraftToTeam(Guid newTeamId)
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
}
