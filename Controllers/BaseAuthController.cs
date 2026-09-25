using CandyOrg.Controllers.Extensions;
using Core.Auth.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CandyOrg.Controllers.Controllers;

[ApiController]
[Authorize]
public abstract class BaseAuthController : BaseController
{
    private const string XUserId = "X-User-Id";
    private IHeaderDictionary Headers => HttpContext.Request.Headers;
    private TokenPayload? _tokenPayload;

    private Guid? _userId;
    protected Guid UserId
    {
        get
        {
            if (_userId is null)
            {
                _userId = ResolveUserId();
            }

            return _userId.Value;
        }
    }

    private Guid ResolveUserId()
    {
        if (Guid.TryParse(Headers[XUserId], out var userId))
        {
            return userId;
        }

        if (_tokenPayload is null)
        {
            _tokenPayload = Headers.Authorization.ToString().GetPayload();
        }

        return _tokenPayload.UserId;
    }
}