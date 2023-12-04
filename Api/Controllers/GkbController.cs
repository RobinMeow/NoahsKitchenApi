using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public abstract class GkbController : ControllerBase
{
    protected StatusCodeResult Status_500_Internal_Server_Error => StatusCode(StatusCodes.Status500InternalServerError);

    protected string CreateErrorMessage(string controllerName, string methodName)
    {
        return $"In {controllerName} On {methodName}";
    }
}
