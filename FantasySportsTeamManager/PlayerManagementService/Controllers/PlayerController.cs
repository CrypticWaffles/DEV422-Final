using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Data;
using PlayerManagementService.Model;
using PlayerManagementService.Model.PlayerManagementService.Model;

namespace PlayerManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        // Database context
        private readonly FantasySportsContext _context;

        /// <summary>
        /// Constructor for PlayerController
        /// </summary>
        /// <param name="context"></param>
        public PlayerController(FantasySportsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method to create a new player
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewPlayer([FromBody] PlayerCreationRequest request)
        {
            // Validate request
            if (request == null || string.IsNullOrWhiteSpace(request.PlayerName))
            {
                return BadRequest("Player data is incomplete.");
            }

            // Create new player instance
            Player newPlayer = new Player(
                request.PlayerName,
                request.Position
            );

            // Add new player to SQL Database
            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully created new player", Data = newPlayer });
        }

        /// <summary>
        /// Method to list all players
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> ListAllPlayers()
        {
            // Retrieve all players from the SQL Database
            var players = await _context.Players.ToListAsync();

            // Validate if any players exist
            if (players == null || !players.Any())
            {
                return Ok(new List<Player>());
            }
            return Ok(new { Message = "Successfully listed all players", Data = players });
        }

        /// <summary>
        /// Method to get a player by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayerById(int id)
        {
            // Find player in the SQL Database by ID
            var player = await _context.Players.FindAsync(id);

            // Validate player exists
            if (player == null)
            {
                return NotFound("There is no player that has this id.");
            }
            return Ok(new { Message = "Successfully got player by ID", Data = player });
        }

        /// <summary>
        /// Method to delete a player by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            // Find player in the SQL Database by ID
            var player = await _context.Players.FindAsync(id);
            // Validate player exists
            if (player == null)
            {
                return NotFound("There is no player that has this id.");
            }

            // Remove player from SQL
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully deleted player" });
        }

        /// <summary>
        /// Method to edit an existing player
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPlayer(int id, [FromBody] UpdatePlayerRequest request)
        {
            // Validate request
            if (request == null)
            {
                return BadRequest("Player data is null.");
            }

            // Find player in the SQL Database by ID
            var player = await _context.Players.FindAsync(id);

            // Validate player exists
            if (player == null)
            {
                return NotFound($"No player found with ID {id}.");
            }

            // Update player details using the method we kept in the model
            player.UpdateInfo(request.PlayerName, request.Position);

            // Commit changes to SQL
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Successfully edited player",
                Data = player
            });
        }

        /// <summary>
        /// Method to draft a player to a team
        /// </summary>
        [HttpPost("draft/{playerId}/team/{teamId}")]
        public async Task<IActionResult> DraftPlayerToTeam(int playerId, int teamId)
        {
            // Find player
            var player = await _context.Players.FindAsync(playerId);

            if (player == null)
            {
                return NotFound($"Player with ID {playerId} not found.");
            }

            // Update state
            player.DraftToTeam(teamId);

            // Save to SQL
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Successfully drafted player {playerId} to team {teamId}",
                Data = player
            });
        }

        /// <summary>
        /// Method to release a player from a team
        /// </summary>
        [HttpPost("release/{playerId}")]
        public async Task<IActionResult> ReleasePlayerFromTeam(int playerId)
        {
            // Find player
            var player = await _context.Players.FindAsync(playerId);

            if (player == null)
            {
                return NotFound($"Player with ID {playerId} not found.");
            }

            // Update state (sets TeamId to null)
            player.ReleaseFromTeam();

            // Save to SQL
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Successfully released player {playerId} from their team",
                Data = player
            });
        }
    }
}