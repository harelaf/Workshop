
using MarketWeb.Service;
using MarketWeb.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace MarketWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private MarketAPI _marketApi;
        private ILogger<UsersController> _logger;

        public UsersController(MarketAPI marketApi, ILogger<UsersController> logger)
        {
            _marketApi = marketApi;
            _logger = logger;
        }


        [HttpPost("login")]
        public Response<string> Login( string token,  string username, string password)
        {
            _logger.LogInformation("Login called.");
            Response<string> response = _marketApi.Login(token, username, password);
            _logger.LogInformation("Returning {%s}.", response.Value);
            return response;
        }

        [HttpGet("entersystem")]
        public Response<string> EnterSystem()
        {
            _logger.LogInformation("EnterSystem called.");
            Response<string> response = _marketApi.EnterSystem();
            var responseAsJson = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                TypeNameHandling = TypeNameHandling.Auto
            });
            //var responseAsJson = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //});
            _logger.LogInformation("Returning: \n{%s}.", responseAsJson);
            return response;
        }
    }
}