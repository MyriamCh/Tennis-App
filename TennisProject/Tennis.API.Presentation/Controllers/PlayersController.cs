using Microsoft.AspNetCore.Mvc;
using Tennis.API.Services.Interfaces;
using Tennis.API.Shared.Dtos;

namespace Tennis.API.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
        }

        /// <summary>
        /// Returns the list of players sorted by rank (best to worst).
        /// </summary>
        /// <response code="200">Returns the sorted list of players</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
        {
            try
            {
                var players = await _playerService.GetPlayersSortedByRankAsync();
                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new { message = $"An error occurred while retrieving players.", error = ex.Message });
            }
        }



        /// <summary>
        /// Returns details of a player by ID.
        /// </summary>
        /// <param name="id">The ID of the player.</param>
        /// <response code="200">Returns the player details</response>
        /// <response code="404">If the player is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}", Name = "GetPlayerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PlayerDto>> GetPlayerById(int id)
        {
            try
            {
                var player = await _playerService.GetPlayerByIdAsync(id);
                if (player == null)
                {
                    return NotFound(new { message = $"Player with ID {id} not found." });
                }

                return Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = $"An error occurred while retrieving player with ID {id}.", error = ex.Message });
            }
        }
        /// <summary>
        /// Returns global statistics: best country win ratio, average BMI, and median height.
        /// </summary>
        /// <response code="200">Returns the player statistics</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("statistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PlayerStatsDto>> GetPlayerStatistics()
        {
            try
            {
                var stats = await _playerService.GetStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving player statistics.", error = ex.Message });
            }
        }
    }
}
