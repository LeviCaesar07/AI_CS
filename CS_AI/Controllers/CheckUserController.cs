using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.DirectoryServices.AccountManagement;

namespace CS_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckUserController : Controller
    {
        private const string AD_SERVER = "10.254.210.12";
        private const string AD_USERNAME = "renaldo.reynard";
        private const string AD_PASSWORD = "04Feb2026!";

        private const string SEARCH_USERNAME = "prayogga.rio";

        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                using var context = new PrincipalContext(
                    ContextType.Domain,
                    AD_SERVER,
                    AD_USERNAME,
                    AD_PASSWORD
                );

                var user = UserPrincipal.FindByIdentity(
                    context,
                    IdentityType.SamAccountName,
                    SEARCH_USERNAME
                );

                if (user == null)
                {
                    return NotFound(new
                    {
                        Found = false,
                        Message = $"User '{SEARCH_USERNAME}' tidak ditemukan"
                    });
                }

                return Ok(new
                {
                    Found = true,
                    Username = user.SamAccountName ?? "",
                    DisplayName = user.DisplayName ?? "",
                    DistinguishedName = user.DistinguishedName ?? ""
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Found = false,
                    Error = ex.Message
                });
            }
        }
    }
}
