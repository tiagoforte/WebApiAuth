using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiToken.Security;

namespace WebApiToken.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly TokenOptions _tokenOptions;
        private readonly JsonSerializerSettings _serializerSettings;


        public AuthenticateController(IOptions<TokenOptions> jwtOptions)
        {
            _tokenOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_tokenOptions);




            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private static void ThrowIfInvalidOptions(TokenOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("O período deve ser maior que zero", nameof(TokenOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(TokenOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(TokenOptions.JtiGenerator));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("v1/authenticate")]
        public async Task<IActionResult> Post([FromBody] Authenticate command)
        {
            var email = "teste@teste.com.br";
            var password = "12345";
            var userName = "Admin";

            if (command.Email != email && command.Password != password)
                return BadRequest(new { message = "Usuário ou senha inválidos" });

            var identity = await GetClaimsIdentity(command);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, "Admin"),
                new Claim(JwtRegisteredClaimNames.NameId, "Admin"),
                new Claim(JwtRegisteredClaimNames.Email, command.Email),
                new Claim(JwtRegisteredClaimNames.Sid, command.Password),
                identity.FindFirst("ModernStore")
            };

            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                claims: claims.AsEnumerable(),
                notBefore: _tokenOptions.NotBefore,
                expires: _tokenOptions.Expiration,
                signingCredentials: _tokenOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_tokenOptions.ValidFor.TotalSeconds,
                user = new { username = userName }
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }


        private Task<ClaimsIdentity> GetClaimsIdentity(Authenticate user)
        {

            if (user == null)
                return Task.FromResult<ClaimsIdentity>(null);

            return Task.FromResult(new ClaimsIdentity(
                new GenericIdentity("tiago", "Token")
                ));
        }

    }
}
