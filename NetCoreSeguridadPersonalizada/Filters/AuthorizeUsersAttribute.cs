using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreSeguridadPersonalizada.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        //Este metodo es el que permitira impedir el acceso a los actions/controllers.
        //El filter simplemente se encarga de interceptar peticions y decicir que hacer.
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //El usuario que se ha validado en nuestra app estara dentro del context y en una propiedad llamada User.
            //Cualquier user esta compuesto por dos caracterisiticas:
            //1) Identity: El nombre del usuario y si es activo.
            //2) Principal: El rol del usuario.
            var user = context.HttpContext.User;
            //El fintro solamente preguntará si existe el user, y solo entra en accion sino existe.
            if(user.Identity.IsAuthenticated == false)
            {
                //lo llevamos al Log in sino se ha autenticado.
                //A la ruta debemos enviarle un controller y un action.
                RouteValueDictionary rutaLogIn = new RouteValueDictionary(new { controller = "Managed", action = "Login" });
                //Devolvemos la peticion al Log In.
                context.Result = new RedirectToRouteResult(rutaLogIn);
            }
        }
    }
}
