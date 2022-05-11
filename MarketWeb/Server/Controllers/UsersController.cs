using MarketProject.Service;
using MarketWeb.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MarketAPI _marketApi;

        public UsersController(MarketAPI marketApi)
        {
            _marketApi = marketApi;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("/login")]
        public async Task<ActionResult<string>> Login(string token, string username, string password)
        {
            Response<string> response = _marketApi.Login(token, username, password);
            if (response.ErrorOccured)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok(response.Value);
        }
        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("/enter")]
        public async Task<IActionResult> EnterSystem()
        {
            Response<string> response = _marketApi.EnterSystem();
            if (response.ErrorOccured)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok(response.Value);
        }
    }
}
