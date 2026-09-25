using CandyOrg.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CandyOrg.Controllers.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult Problem<T>(Result<T> result)
    {
        var statusCode = result.Code switch
        {
            ErrorCode.Forbidden => StatusCodes.Status403Forbidden,
            ErrorCode.NotFound => StatusCodes.Status404NotFound,
            ErrorCode.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorCode.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };
        if (statusCode == StatusCodes.Status400BadRequest && result.Value is not null)
        {
            return new ObjectResult(result.Value) { StatusCode = statusCode };
        }

        if (result.Error is null)
        {
            return StatusCode(statusCode);
        }

        return new ObjectResult(new { ErrorMessage = result.Error.Message }) { StatusCode = statusCode };
    }

    protected IActionResult OkOrProblem<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        
        return Problem(result);
    }
}