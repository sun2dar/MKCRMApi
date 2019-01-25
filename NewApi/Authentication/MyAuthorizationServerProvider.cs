using Microsoft.Owin.Security.OAuth;
using CCARE.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace NewApi
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); // 
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            string username = context.UserName;
            string password = context.Password;

            //if (context.UserName == "admin" && context.Password == "admin")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            //    identity.AddClaim(new Claim("username", "admin"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "Sourav Mondal"));
            //    context.Validated(identity);
            //}
            //else if (context.UserName == "user" && context.Password == "user")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            //    identity.AddClaim(new Claim("username", "user"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "Suresh Sha"));
            //    context.Validated(identity);
            //}

            //Query authentication run here
            UserService userService = new UserService();
            MyResult<CCARE.Models.SystemUser> result = new MyResult<CCARE.Models.SystemUser>();
            result = userService.authenticate(context.UserName, context.Password);

            if (result.code == 1)
            {
                identity.AddClaim(new Claim("username", username));
                identity.AddClaim(new Claim(ClaimTypes.Name, result.model.FullName));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", result.message);
                return;
            }
        }
    }
}