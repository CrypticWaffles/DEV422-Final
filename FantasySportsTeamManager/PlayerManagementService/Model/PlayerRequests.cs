
namespace PlayerManagementService.Model
{
    public class UpdatePlayerRequest
    {
        public string PlayerName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }

    public class PlayerCreationRequest
    {
        public string PlayerName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }
}
