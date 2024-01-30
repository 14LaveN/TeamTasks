using TeamTasks.Micro.Identity.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TeamTasks.Micro.Identity.Controllers;

[ApiController]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class ApiBaseController : ControllerBase
{
    protected UserInfo CurrentUser => new(HttpContext.User);
}