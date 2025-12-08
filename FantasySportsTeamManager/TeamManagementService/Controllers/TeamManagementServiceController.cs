using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlayerManagementService;
using TeamManagementService.Model;
using PlayerManagementService.Controllers;
using TeamManagementService.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace TeamManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamManagementServiceController : ControllerBase
    {
        private static readonly List<Team> _teams= new List<Team>();
        private readonly TeamManagementServiceContext context;
        public TeamManagementServiceController(TeamManagementServiceContext TeamManagementServiceContext)
        {
            this.context = TeamManagementServiceContext;
        }
        [HttpGet("teams")]
        public async Task<IActionResult> ListAllTeams()
        {
            return Ok(new
            {
                Message = "Successfully listed all teams",
                Data = await context.Teams.ToListAsync()+"\n"+_teams
            });
        }
        [HttpGet("graph")]
        public IActionResult GetGraph()
        {
            //PlayerController playerController = new PlayerController();
            //playerController.ListAllPlayers();
            return Ok(new
            {
                Message = "Successfully retrieved Graph",
                Data = "WIP: GRAPH"//playerController
            });
        }
        [HttpGet("teams/{id}")]
        public async Task<ActionResult> GetTeamById(int id)
        {
            var team = await context.Teams.FirstOrDefaultAsync(x => x.teamId == id); 
            var listteam = _teams.FirstOrDefault(x => x.teamId == id);
            if (_teams == null || _teams.Count == 0 || context.Teams.Count() == 0)
            {
                return BadRequest("There are no teams to get by id. Create a team.");
            }
            if (team == null || listteam == null)
            {
                return NotFound("There is no team that has this id.");
            }
            return Ok(new
            {
                Message = "Successfully got team by ID",
                Data = team+"\n"+listteam
            });
        }
        [HttpPost("teams")]
        public async Task<IActionResult> CreateNewTeam(Team team)
        {
            if (team == null)
            {
                return Unauthorized("Team's fields are null.");
            }
            team.createdDate = DateTime.Now;
            _teams.Add(team);
            team.teamId = _teams.Count;
            await context.Teams.AddAsync(team);
            team.teamId = context.Teams.Count();
            context.SaveChanges();
            return Ok(new
            {
                Message = "Successfully created new team",
                Data = team
            });
        }
        [HttpPut("teams/{id}")]
        public async Task<IActionResult> UpdateTeamDetails(int id,Team team)
        {
            var verifyteam = await context.Teams.FirstOrDefaultAsync(x => x.teamId == id);
            var listteam = _teams.FirstOrDefault(x => x.teamId == id);
            if (_teams == null || _teams.Count == 0 || context.Teams.Count() == 0)
            {
                return BadRequest("There are no teams to update. Create a team.");
            }
            if(team == null)
            {
                return BadRequest("Team's fields are null");
            }
            if (verifyteam == null || listteam == null)
            {
                return NotFound("There is no team that has this id.");
            }
            verifyteam.teamName=team.teamName;
            context.SaveChanges();
            return Ok(new
            {
                Message = "Successfully updated team details",
                Data = verifyteam
            });
        }
        [HttpDelete("teams/{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var verifyteam = await context.Teams.FirstOrDefaultAsync(x => x.teamId == id);
            if (_teams == null || _teams.Count == 0 || context.Teams.Count() == 0)
            {
                return BadRequest("There are no teams to delete. Create a team.");
            }
            if (verifyteam == null)
            {
                return NotFound("There is no team that has this id.");
            }
            _teams.RemoveAt(id - 1);
            context.Teams.Remove(verifyteam);
            context.SaveChanges();
            return Ok(new
            {
                Message = "Successfully deleted team",
                Data = verifyteam
            });
        }
    }
}
