using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TeamManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamManagementController : ControllerBase
    {
        private static readonly List<Team> _teams= new List<Team>();
        [HttpGet("teams")]
        public ActionResult<IEnumerable<Team>> ListAllTeams()
        {
            if (_teams == null || _teams.Count == 0)
            {
                return BadRequest("There are no teams to get. Create a team.");
            }
            return Ok(new
            {
                Message = "Successfully listed all teams",
                Data = _teams
            });
        }
        [HttpGet("graph")]
        public IActionResult GetGraph()
        {
            return Ok(new
            {
                Message = "Successfully retrieved Graph",
                Data = "WIP: GRAPH"
            });
        }
        [HttpGet("teams/{id}")]
        public ActionResult<Team> GetTeamById(int id)
        {
            var team = _teams.FirstOrDefault(x => x.teamId == id);
            if (_teams == null || _teams.Count == 0)
            {
                return BadRequest("There are no teams to get by id. Create a team.");
            }
            if (team == null)
            {
                return NotFound("There is no team that has this id.");
            }
            return Ok(new
            {
                Message = "Successfully got team by ID",
                Data = team
            });
        }
        [HttpPost("teams")]
        public IActionResult CreateNewTeam(Team team)
        {
            if (team == null)
            {
                return Unauthorized("Team's fields are null.");
            }
            team.createdDate = DateTime.Now;
            _teams.Add(team);
            team.teamId = _teams.Count;
            return Ok(new
            {
                Message = "Successfully created new team",
                Data = team
            });
        }
        [HttpPut("teams/{id}")]
        public IActionResult UpdateTeamDetails(int id,Team team)
        {
            var verifyteam = _teams.FirstOrDefault(x => x.teamId == id);
            if (_teams == null || _teams.Count == 0)
            {
                return BadRequest("There are no teams to update. Create a team.");
            }
            if(team == null)
            {
                return BadRequest("Team's fields are null");
            }
            if (verifyteam == null)
            {
                return NotFound("There is no team that has this id.");
            }
            verifyteam.teamName=team.teamName;
            return Ok(new
            {
                Message = "Successfully updated team details",
                Data = verifyteam
            });
        }
        [HttpDelete("teams/{id}")]
        public IActionResult DeleteTeam(int id)
        {
            var verifyteam = _teams.FirstOrDefault(x => x.teamId == id);
            if (_teams == null || _teams.Count == 0)
            {
                return BadRequest("There are no teams to delete. Create a team.");
            }
            if (verifyteam == null)
            {
                return NotFound("There is no team that has this id.");
            }
            _teams.RemoveAt(id-1);
            return Ok(new
            {
                Message = "Successfully deleted team",
                Data = verifyteam
            });
        }
    }
}
