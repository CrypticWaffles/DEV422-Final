namespace PlayerManagementService.Model
{
    public class Player
    {
        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDrafted { get; private set; } = false;
        public int TeamId { get; private set; } = -1;

        // Constructor
        public Player(int id, string firstName, string lastName, DateTime dob)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;

            IsDrafted = false;
            TeamId = -1;
        }

        // Methods to handle state changes
        // Draft the player to a team
        public void DraftToTeam(int newTeamId)
        {
            IsDrafted = true;
            TeamId = newTeamId;
        }

        // Release the player from the team
        public void ReleaseFromTeam()
        {
            IsDrafted = false;
            TeamId = -1;
        }
        public void UpdateInfo(string newFirstName, string newLastName, DateTime newDob)
        {
            FirstName = newFirstName;
            LastName = newLastName;
            DateOfBirth = newDob;
        }
    }

    public class PlayerCreationRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdatePlayerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}