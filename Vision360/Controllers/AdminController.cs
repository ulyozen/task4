using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vision360.Context.Repositories.Interfaces;
using Vision360.DTOs;
using Vision360.Mappers;

namespace Vision360.Controllers;

[Authorize]
public class AdminController(IAdminRepository repo) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] UserQueryDto dto, [FromHeader] RequestHeadersDto headers)
    {
        var users = (await repo.GetUsersAsync(dto.Search, dto.SortColumn, dto.SortDirection)).ToList();

        var model = dto.MapToModel(users);
        
        if (headers.RequestedWith == "XMLHttpRequest") return PartialView("_UsersTable", model);

        return View("Index", model);
    }
    
    [HttpPost]
    public async Task<IActionResult> BlockUser([FromBody] IEnumerable<string> userIds)
    {
        await repo.BlockUserAsync(userIds);

        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> UnblockUser([FromBody] IEnumerable<string> userIds)
    {
        await repo.UnBlockUserAsync(userIds);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser([FromBody] IEnumerable<string> userIds)
    {
        await repo.DeleteUserAsync(userIds);

        return Ok();
    }
}