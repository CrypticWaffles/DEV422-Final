using Microsoft.AspNetCore.Mvc;

namespace PlayerManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        // In-memory list to store players
        private static readonly List<Model.Player> _players = new List<Model.Player>();

        /// <summary>
        /// Method to create a new player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewPlayer([FromBody] Model.PlayerCreationRequest request)
        {
            // Validate fields
            if (request == null || string.IsNullOrWhiteSpace(request.FirstName))
            {
                return BadRequest("Player data is incomplete.");
            }

            // Generate ID
            // If list is empty, start at 1. Otherwise, find the max ID and add 1.
            int newId = _players.Any() ? _players.Max(p => p.Id) + 1 : 1;

            // Use the Constructor to build new Player object
            Model.Player newPlayer = new Model.Player(
                newId,
                request.FirstName,
                request.LastName,
                request.DateOfBirth
            );

            // Add to storage
            _players.Add(newPlayer);

            return Ok(new
            {
                Message = "Successfully created new player",
                Data = newPlayer
            });
        }

        /// <summary>
        /// Method to edit an existing player
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedPlayer"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult EditPlayer(int id, [FromBody] Model.UpdatePlayerRequest request)
        {
            // Validate
            if (request == null)
            {
                return BadRequest("Player data is null.");
            }

            // Find player with matching ID
            var player = _players.FirstOrDefault(x => x.Id == id);

            // validate player exists
            if (player == null)
            {
                return NotFound($"No player found with ID {id}.");
            }

            // Update player details
            player.UpdateInfo(request.FirstName, request.LastName, request.DateOfBirth);

            return Ok(new
            {
                Message = "Successfully edited player",
                Data = player
            });
        }

        /// <summary>
        /// Method to delete a player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeletePlayer(int id)
        {
            // Find player with matching ID
            var player = _players.FirstOrDefault(x => x.Id == id);
            // Check if players list is empty or player not found
            if (_players == null || _players.Count == 0)
            {
                return BadRequest("There are no players to delete. Create a player.");
            }
            if (player == null)
            {
                return NotFound("There is no player that has this id.");
            }
            // Remove player from the list
            _players.Remove(player);
            return Ok(new
            {
                Message = "Successfully deleted player",
            });
        }

        /// <summary>
        /// Method to list all players
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Model.Player>> ListAllPlayers()
        {
            // Check if players list is empty
            if (_players == null || _players.Count == 0)
            {
                return Ok(_players);
            }
            return Ok(new
            {
                Message = "Successfully listed all players",
                Data = _players
            });
        }

        // Graph??

        /// <summary>
        /// Method to get player by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Model.Player> GetPlayerById(int id)
        {
            // Find player with matching ID
            var player = _players.FirstOrDefault(x => x.Id == id);

            // Check if players list is empty or player not found
            if (_players == null || _players.Count == 0)
            {
                return BadRequest("There are no players to get by id. Create a player.");
            }
            if (player == null)
            {
                return NotFound("There is no player that has this id.");
            }
            return Ok(new
            {
                Message = "Successfully got player by ID",
                Data = player
            });
        }

        /// <summary>
        /// Method to draft a player to a team
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpPost("draft/{playerId}/team/{teamId}")]
        public IActionResult DraftPlayerToTeam(int playerId, int teamId)
        {
            // Find player with matching ID
            var player = _players.FirstOrDefault(x => x.Id == playerId);

            // Validate
            if (player == null)
            {
                return NotFound($"Player with ID {playerId} not found.");
            }

            player.DraftToTeam(teamId);

            return Ok(new
            {
                Message = $"Successfully drafted player {playerId} to team {teamId}",
                Data = player
            });
        }

        /// <summary>
        /// Method to release a player from a team
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpPost("release/{playerId}")]
        public IActionResult ReleasePlayerFromTeam(int playerId)
        {
            // Find player with matching ID
            var player = _players.FirstOrDefault(x => x.Id == playerId);

            // Validate
            if (player == null)
            {
                return NotFound($"Player with ID {playerId} not found.");
            }

            player.ReleaseFromTeam();

            return Ok(new
            {
                Message = $"Successfully released player {playerId} from their team",
                Data = player
            });
        }
    }
}
