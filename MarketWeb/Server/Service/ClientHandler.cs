using MarketProject.Service;
using MarketWeb.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace MarketWeb.Server
{
    public class ClientHandler : HttpClientHandler
    {
        private MarketAPI _marketAPI;

        public ClientHandler(MarketAPI marketAPI)
        {
            _marketAPI = marketAPI;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // array in local storage for registered users
            var usersKey = "blazor-registration-login-example-users";
            var method = request.Method;
            var path = request.RequestUri.AbsolutePath;            

            return await handleRoute();

            async Task<HttpResponseMessage> handleRoute()
            {
                if (path == "/users/enter" && method == HttpMethod.Post)
                    return await enter();
                if (path == "/users/authenticate" && method == HttpMethod.Post)
                    return await authenticate();
                if (path == "/users/register" && method == HttpMethod.Post)
                    return await register();
                
                // pass through any requests not handled above
                return await base.SendAsync(request, cancellationToken);
            }

            // route functions

            async Task<HttpResponseMessage> enter()
            {
                var bodyJson = await request.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<bool>(bodyJson);

                Response<string> response = _marketAPI.EnterSystem();

                if (response.ErrorOccured)
                    return await error(response.ErrorMessage);

                return await ok(new
                {
                    Id = "",
                    Username = "",
                    FirstName = "",
                    LastName = "",
                    Token = "fake-jwt-token"
                });
            }

            async Task<HttpResponseMessage> authenticate()
            {
                var bodyJson = await request.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<LoginModel>(bodyJson);
                // Might wanna do async or something in MarketAPI
                Response<string> response = _marketAPI.Login(request.Headers.Authorization.Parameter, body.Username, body.Password);

                if (response.ErrorOccured)
                    return await error(response.ErrorMessage);

                return await ok(new {
                    Id = body.Username,
                    Username = body.Username,
                    FirstName = body.Username,
                    LastName = body.Username,
                    Token = response.Value
                });
            }

            async Task<HttpResponseMessage> register()
            {
                var bodyJson = await request.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<LoginModel>(bodyJson);

                Response response = _marketAPI.Register(request.Headers.Authorization.Parameter, body.Username, body.Password);

                if (response.ErrorOccured)
                    return await error(response.ErrorMessage);
                
                return await ok();
            }


            // helper functions

            async Task<HttpResponseMessage> ok(object body = null)
            {
                return await jsonResponse(HttpStatusCode.OK, body ?? new {});
            }

            async Task<HttpResponseMessage> error(string message)
            {
                return await jsonResponse(HttpStatusCode.BadRequest, new { message });
            }

            async Task<HttpResponseMessage> unauthorized()
            {
                return await jsonResponse(HttpStatusCode.Unauthorized, new { message = "Unauthorized" });
            }

            async Task<HttpResponseMessage> jsonResponse(HttpStatusCode statusCode, object content)
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json")
                };
                
                // delay to simulate real api call
                await Task.Delay(500);

                return response;
            }

            bool isLoggedIn()
            {
                return request.Headers.Authorization?.Parameter == "fake-jwt-token";
            } 

            int idFromPath()
            {
                return int.Parse(path.Split('/').Last());
            }
        }
    }
}