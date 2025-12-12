using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamManagementService.Data;
using TeamManagementService.Model;

namespace TeamManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamManagementServiceController : ControllerBase
    {
        private readonly TeamManagementServiceContext _context;

        public TeamManagementServiceController(TeamManagementServiceContext context)
        {
            _context = context;
        }

        [HttpGet] // Changed from "teams" to match standard REST style
        public async Task<IActionResult> ListAllTeams()
        {
            var teams = await _context.Teams.ToListAsync();
            return Ok(new { Message = "Successfully listed all teams", Data = teams });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTeamById(Guid id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return NotFound("There is no team that has this id.");
            }
            return Ok(new { Message = "Successfully got team by ID", Data = team });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTeam([FromBody] Team team)
        {
            if (team == null) return BadRequest("Team data is null.");

            team.teamId = Guid.NewGuid();
            team.createdDate = DateTime.Now;

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully created new team", Data = team });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeamDetails(Guid id, [FromBody] Team team)
        {
            var existingTeam = await _context.Teams.FindAsync(id);
            if (existingTeam == null) return NotFound("Team not found.");

            // Update only the name
            existingTeam.teamName = team.teamName;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully updated team details", Data = existingTeam });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound("Team not found.");

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully deleted team" });
        }
    }
}