using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NetCoreSeguridadPersonalizada.Controllers
{
    public class ManagedController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if(username.ToLower() == "admin" && password == "12345")
            {
                //Por medidas de seguridad se genera una Cookie cifrada que es para sabr si el user se ha validado en este explorador.
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role
                    );
                //Un Claim es informacion del usuario.
                Claim claimUsername = new Claim(ClaimTypes.Name, username);
                Claim claimRole = new Claim(ClaimTypes.Role, "USUARIO");
                identity.AddClaim(claimUsername);
                identity.AddClaim(claimRole);
                //Creamos un Usuario principal con esta identidad.
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                //Damos de alta al user en el sistema.
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.Now.AddMinutes(10)
                    });
                return RedirectToAction("Perfil", "Usuarios");
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas.";
                return View();
            }
        }
    }
}
