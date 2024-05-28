using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ob.Domain;
using ob.IBusinessLogic;
namespace ob.WebApi.Controllers;

public abstract class BaseController : ControllerBase
{
    private ISessionService _sessionService;

    protected BaseController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    protected Guid GetAuthTokenFromHeader()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
        return Guid.Parse(authorizationHeader);
    }

    protected Usuario GetCurrentUser()
    {
        var token = GetAuthTokenFromHeader();
        return _sessionService.GetCurrentUser(token);
    }
}