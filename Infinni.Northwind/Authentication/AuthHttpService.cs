using System.Security.Claims;
using System.Threading.Tasks;

using InfinniPlatform.Http;

using Microsoft.AspNetCore.Identity;

namespace Infinni.Northwind.Authentication
{
    public class AuthHttpService : IHttpService
    {
        private readonly UserManager<CustomUser> _userManager;

        public AuthHttpService(UserManager<CustomUser> userManager)
        {
            _userManager = userManager;
        }

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Auth";
            builder.Post["/Create"] = CreateUser;
            builder.Get["/FindById"] = FindById;
            builder.Get["/FindByEmail"] = FindByEmail;
            builder.Get["/FindByName"] = FindByName;
            builder.Get["/Delete"] = Delete;
            builder.Get["/AddClaim"] = AddClaim;
        }

        private async Task<object> CreateUser(IHttpRequest httpRequest)
        {
            string email = httpRequest.Form.Email;
            string password = httpRequest.Form.Password;

            var appUser = new CustomUser
                          {
                              Age = 100,
                              UserName = email,
                              Email = email,
                              Id = "59142e50061ec276e470823f"
                          };
            var identityResult = await _userManager.CreateAsync(appUser, password);

            return identityResult;
        }

        private async Task<object> FindById(IHttpRequest httpRequest)
        {
            string id = httpRequest.Query.Id;

            var appUser = await _userManager.FindByIdAsync(id);

            return appUser;
        }

        private async Task<object> FindByEmail(IHttpRequest httpRequest)
        {
            string email = httpRequest.Query.Email;

            var appUser = await _userManager.FindByEmailAsync(email);

            return appUser;
        }


        private async Task<object> FindByName(IHttpRequest httpRequest)
        {
            string name = httpRequest.Query.UserName;

            var appUser = await _userManager.FindByNameAsync(name);

            return appUser;
        }

        private async Task<object> Delete(IHttpRequest httpRequest)
        {
            string name = httpRequest.Query.UserName;

            var appUser = await _userManager.FindByNameAsync(name);
            var identityResult = await _userManager.DeleteAsync(appUser);

            return identityResult;
        }

        private async Task<object> AddClaim(IHttpRequest httpRequest)
        {
            string name = httpRequest.Query.UserName;
            string claimType = httpRequest.Query.ClaimType;
            string claimValue = httpRequest.Query.ClaimValue;

            var appUser = await _userManager.FindByNameAsync(name);
            var identityResult = await _userManager.AddClaimAsync(appUser, new Claim(claimType, claimValue));

            return identityResult;
        }
    }
}