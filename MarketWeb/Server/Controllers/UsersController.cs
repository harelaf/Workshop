using MarketProject.Service;
using MarketWeb.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MarketWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MarketAPI _marketApi;
        private ILogger _logger;

        public UsersController(MarketAPI marketApi, ILogger logger)
        {
            _marketApi = marketApi;
            _logger = logger;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/login")]
        public Response<string> Login(string token, string username, string password)
        {
            _logger.LogInformation("Login called.");
            Response<string> response = _marketApi.Login(token, username, password);
            return response;
        }
        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("/enter")]
        public Response<string> EnterSystem()
        {
            _logger.LogInformation("EnterSystem called.");
            Response<string> response = _marketApi.EnterSystem();
            return response;
        }
    }
}
