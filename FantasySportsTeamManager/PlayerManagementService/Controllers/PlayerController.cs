
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Data;
using PlayerManagementService.Model;
using System;

namespace PlayerManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly FantasySportsContext _context;

        public PlayerController(FantasySportsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPlayer([FromBody] PlayerCreationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.PlayerName))
                return BadRequest("Player data is incomplete.");

            var newPlayer = new Player(request.PlayerName, request.Position);
            _context.Players.Add(newPlayer);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully created new player", Data = newPlayer });
        }

        [HttpGet]
        public async Task<IActionResult> ListAllPlayers()
        {
            var players = await _context.Players.ToListAsync();
            return Ok(new { Message = "Successfully listed all players", Data = players });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound("There is no player that has this id.");
            return Ok(new { Message = "Successfully got player by ID", Data = player });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound("There is no player that has this id.");

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully deleted player" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPlayer(int id, [FromBody] UpdatePlayerRequest request)
        {
            if (request == null) return BadRequest("Player data is null.");

            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound($"No player found with ID {id}.");

            player.UpdateInfo(request.PlayerName, request.Position);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully edited player", Data = player });
        }

        [HttpPost("draft/{playerId}/team/{teamId}")]
        public async Task<IActionResult> DraftPlayerToTeam(int playerId, Guid teamId)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player == null) return NotFound($"Player with ID {playerId} not found.");

            player.DraftToTeam(teamId);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Successfully drafted player {playerId} to team {teamId}", Data = player });
        }

        [HttpPost("release/{playerId}")]
        public async Task<IActionResult> ReleasePlayerFromTeam(int playerId)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player == null) return NotFound($"Player with ID {playerId} not found.");

            player.ReleaseFromTeam();
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Successfully released player {playerId} from their team", Data = player });
        }
    }
}