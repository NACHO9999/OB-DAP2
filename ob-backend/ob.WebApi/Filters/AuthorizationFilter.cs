using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ob.IBusinessLogic;
using ob.Domain;
using Microsoft.AspNetCore.Http;

namespace ob.WebApi.Filters;

public class AuthorizationFilter : Attribute, IAuthorizationFilter
{
    public Type [] RoleNeeded { get; set; }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
        Guid token = Guid.Empty;

        if (string.IsNullOrEmpty(authorizationHeader) || !Guid.TryParse(authorizationHeader, out token))
        {
            // Si asigno un result se corta la ejecucion de la request y ya devuelve la response
            context.Result = new ObjectResult(new { Message = "Authorization header is missing" })
            {
                StatusCode = 401
            };
        }

        var sessionManager = GetSessionService(context);
        var currentUser = sessionManager.GetCurrentUser(token);

        if (currentUser == null)
        {
            // Si asigno un result se corta la ejecucion de la request y ya devuelve la response
            context.Result = new ObjectResult(new { Message = "Not authenticated" })
            {
                StatusCode = 401
            };
        }
        bool hasPermissions = false;

        foreach (Type rol in RoleNeeded)
        {
            if (rol == currentUser.GetType())
            {
                hasPermissions = true;
            }
        }

        if (!hasPermissions)
        {
            context.Result = new ContentResult()
            {
                StatusCode = 403,
                Content = "You are not authorized to use this functionality."
            };
        }
    }

    protected ISessionService GetSessionService(AuthorizationFilterContext context)
    {
        var sessionManagerObject = context.HttpContext.RequestServices.GetService(typeof(ISessionService));
        var sessionService = sessionManagerObject as ISessionService;

        return sessionService;
    }
}