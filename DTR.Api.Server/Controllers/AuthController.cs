using DTR.Api.Server.Model.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DTR.Api.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] AuthRequest request)
    {

        //Any machine will request authorization to produce token. 
        //Client will send theire credentials and Api serve will autenticate. 
        //here is the logic for authentication providing machine secret and machine_id
        if (request.ClientId == "machine_id" && request.ClientSecret == "very_secret")
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, request.ClientId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PleaseWriteYourLongSecretKeyHere"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //This Credentials you can also find in the Program.cs 
            //Api server [Authorization] SecretController -> GetSecret()
            //will use this (token)details to compare if Details from program.cs is the same
            var token = new JwtSecurityToken(
                issuer: "ServerMachine",
                audience: "RequestingMachine",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        return Unauthorized();
    }

}
